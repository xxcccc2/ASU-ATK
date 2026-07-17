using System;
using System.Drawing;
using System.Windows.Forms;
using TransportCompany.Core.Models;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Dialogs
{
    /// <summary>Диалог добавления/редактирования записи ТО.</summary>
    public sealed class MaintenanceEditDialog : Form
    {
        private readonly TextBox _carNumber = new TextBox();
        private readonly DateTimePicker _serviceDate = new DateTimePicker();
        private readonly NumericUpDown _mileage = new NumericUpDown();
        private readonly TextBox _comment = new TextBox();

        public MaintenanceRecord Record { get; }

        public MaintenanceEditDialog(MaintenanceRecord record)
        {
            bool isNew = record == null;
            Record = record ?? new MaintenanceRecord { LastServiceDate = DateTime.Today };

            Text = isNew ? "Добавить запись ТО" : "Редактировать запись ТО";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(420, 300);
            BackColor = Color.White;
            Font = Fonts.Body;
            AutoScaleMode = AutoScaleMode.Font;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 16, 20, 12),
                ColumnCount = 1,
                RowCount = 9
            };

            _carNumber.Width = 360;
            _serviceDate.Width = 360;
            _serviceDate.Format = DateTimePickerFormat.Short;
            _mileage.Width = 360;
            _mileage.Maximum = 10_000_000;
            _mileage.ThousandsSeparator = true;
            _comment.Width = 360;
            _comment.Multiline = true;
            _comment.Height = 52;

            Styler.Input(_carNumber);
            Styler.Input(_mileage);
            Styler.Input(_comment);
            _serviceDate.Font = Fonts.Body;

            _carNumber.Text = Record.CarNumber ?? "";
            if (Record.LastServiceDate > DateTimePicker.MinimumDateTime)
            {
                _serviceDate.Value = Record.LastServiceDate;
            }
            _mileage.Value = Math.Max(0, Math.Min(Record.MileageKm, (int)_mileage.Maximum));
            _comment.Text = Record.Comment ?? "";

            _carNumber.KeyPress += (s, e) =>
            {
                if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            layout.Controls.Add(FieldLabel("Номер машины"));
            layout.Controls.Add(_carNumber);
            layout.Controls.Add(FieldLabel("Дата ТО"));
            layout.Controls.Add(_serviceDate);
            layout.Controls.Add(FieldLabel("Пробег (км)"));
            layout.Controls.Add(_mileage);
            layout.Controls.Add(FieldLabel("Комментарий"));
            layout.Controls.Add(_comment);

            var buttons = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Bottom,
                Height = 52,
                Padding = new Padding(12, 10, 16, 8),
                BackColor = Palette.PageBack
            };

            Button save = Styler.PrimaryButton("Сохранить");
            Button cancel = Styler.SecondaryButton("Отмена");
            save.Click += OnSave;
            cancel.Click += (s, e) => DialogResult = DialogResult.Cancel;
            cancel.Margin = new Padding(8, 0, 0, 0);

            buttons.Controls.Add(save);
            buttons.Controls.Add(cancel);

            Controls.Add(layout);
            Controls.Add(buttons);
            AcceptButton = save;
            CancelButton = cancel;
        }

        private static Label FieldLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = Fonts.Small,
                ForeColor = Palette.TextSecondary,
                AutoSize = true,
                Margin = new Padding(0, 8, 0, 2)
            };
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_carNumber.Text))
            {
                UiNotify.Warning("Введите номер машины.");
                return;
            }

            Record.CarNumber = _carNumber.Text.Trim();
            Record.LastServiceDate = _serviceDate.Value.Date;
            Record.MileageKm = (int)_mileage.Value;
            Record.Comment = _comment.Text.Trim();
            DialogResult = DialogResult.OK;
        }
    }
}
