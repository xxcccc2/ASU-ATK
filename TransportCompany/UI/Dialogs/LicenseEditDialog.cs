using System;
using System.Drawing;
using System.Windows.Forms;
using TransportCompany.Core.Models;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Dialogs
{
    /// <summary>Диалог добавления/редактирования водительского удостоверения.</summary>
    public sealed class LicenseEditDialog : Form
    {
        private readonly TextBox _driverName = new TextBox();
        private readonly TextBox _licenseNumber = new TextBox();
        private readonly DateTimePicker _issueDate = new DateTimePicker();
        private readonly DateTimePicker _expiryDate = new DateTimePicker();

        public DriverLicenseRecord License { get; }

        public LicenseEditDialog(DriverLicenseRecord license)
        {
            bool isNew = license == null;
            License = license ?? new DriverLicenseRecord
            {
                IssueDate = DateTime.Today,
                ExpiryDate = DateTime.Today.AddYears(10)
            };

            Text = isNew ? "Добавить удостоверение" : "Редактировать удостоверение";
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

            foreach (Control input in new Control[] { _driverName, _licenseNumber })
            {
                input.Width = 360;
                Styler.Input(input);
            }
            _issueDate.Width = 360;
            _expiryDate.Width = 360;
            _issueDate.Format = DateTimePickerFormat.Short;
            _expiryDate.Format = DateTimePickerFormat.Short;
            _issueDate.Font = Fonts.Body;
            _expiryDate.Font = Fonts.Body;

            _driverName.Text = License.DriverFullName ?? "";
            _licenseNumber.Text = License.LicenseNumber ?? "";
            _issueDate.Value = License.IssueDate;
            _expiryDate.Value = License.ExpiryDate;

            layout.Controls.Add(FieldLabel("ФИО водителя"));
            layout.Controls.Add(_driverName);
            layout.Controls.Add(FieldLabel("Номер удостоверения"));
            layout.Controls.Add(_licenseNumber);
            layout.Controls.Add(FieldLabel("Дата выдачи"));
            layout.Controls.Add(_issueDate);
            layout.Controls.Add(FieldLabel("Действует до"));
            layout.Controls.Add(_expiryDate);

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
            if (string.IsNullOrWhiteSpace(_driverName.Text) || string.IsNullOrWhiteSpace(_licenseNumber.Text))
            {
                UiNotify.Warning("Заполните ФИО водителя и номер удостоверения.");
                return;
            }

            if (_expiryDate.Value.Date < _issueDate.Value.Date)
            {
                UiNotify.Warning("Дата окончания не может быть раньше даты выдачи.");
                return;
            }

            License.DriverFullName = _driverName.Text.Trim();
            License.LicenseNumber = _licenseNumber.Text.Trim();
            License.IssueDate = _issueDate.Value.Date;
            License.ExpiryDate = _expiryDate.Value.Date;
            DialogResult = DialogResult.OK;
        }
    }
}
