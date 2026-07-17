using System;
using System.Drawing;
using System.Windows.Forms;
using TransportCompany.Core.Models;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Dialogs
{
    /// <summary>Диалог добавления/редактирования полиса ОСАГО.</summary>
    public sealed class OsagoEditDialog : Form
    {
        private readonly TextBox _vehicleNumber = new TextBox();
        private readonly TextBox _policyNumber = new TextBox();
        private readonly DateTimePicker _startDate = new DateTimePicker();
        private readonly DateTimePicker _endDate = new DateTimePicker();

        public OsagoPolicy Policy { get; }

        public OsagoEditDialog(OsagoPolicy policy)
        {
            bool isNew = policy == null;
            Policy = policy ?? new OsagoPolicy
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1)
            };

            Text = isNew ? "Добавить ОСАГО" : "Редактировать ОСАГО";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(420, 296);
            BackColor = Color.White;
            Font = Fonts.Body;
            AutoScaleMode = AutoScaleMode.Font;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 16, 20, 12),
                ColumnCount = 1
            };

            foreach (Control input in new Control[] { _vehicleNumber, _policyNumber })
            {
                input.Width = 360;
                Styler.Input(input);
            }
            _startDate.Width = 360;
            _endDate.Width = 360;
            _startDate.Format = DateTimePickerFormat.Short;
            _endDate.Format = DateTimePickerFormat.Short;
            _startDate.Font = Fonts.Body;
            _endDate.Font = Fonts.Body;

            _vehicleNumber.Text = Policy.VehicleRegistrationNumber ?? "";
            _policyNumber.Text = Policy.PolicyNumber ?? "";
            if (!isNew)
            {
                _startDate.Value = Policy.StartDate;
                _endDate.Value = Policy.EndDate;
            }
            else
            {
                _startDate.Value = Policy.StartDate;
                _endDate.Value = Policy.EndDate;
            }

            layout.Controls.Add(FieldLabel("Гос. номер ТС"));
            layout.Controls.Add(_vehicleNumber);
            layout.Controls.Add(FieldLabel("Номер полиса"));
            layout.Controls.Add(_policyNumber);
            layout.Controls.Add(FieldLabel("Дата начала"));
            layout.Controls.Add(_startDate);
            layout.Controls.Add(FieldLabel("Дата окончания"));
            layout.Controls.Add(_endDate);

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
            if (string.IsNullOrWhiteSpace(_vehicleNumber.Text) || string.IsNullOrWhiteSpace(_policyNumber.Text))
            {
                UiNotify.Warning("Заполните гос. номер и номер полиса.");
                return;
            }

            if (_endDate.Value.Date < _startDate.Value.Date)
            {
                UiNotify.Warning("Дата окончания не может быть раньше даты начала.");
                return;
            }

            Policy.VehicleRegistrationNumber = _vehicleNumber.Text.Trim();
            Policy.PolicyNumber = _policyNumber.Text.Trim();
            Policy.StartDate = _startDate.Value.Date;
            Policy.EndDate = _endDate.Value.Date;
            DialogResult = DialogResult.OK;
        }
    }
}
