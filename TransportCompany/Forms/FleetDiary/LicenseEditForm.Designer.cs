using System.Drawing;
using System.Windows.Forms;
using System;

namespace TransportCompany
{
    partial class LicenseEditForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseEditForm));
            this.lblDriverName = new Label { Location = new Point(20, 20), Text = "ФИО водителя:" };
            this.txtDriverName = new TextBox { Location = new Point(150, 20), Size = new Size(200, 20) };
            this.lblLicenseNumber = new Label { Location = new Point(20, 60), Text = "Номер удостоверения:" };
            this.txtLicenseNumber = new TextBox { Location = new Point(150, 60), Size = new Size(200, 20) };
            this.lblIssueDate = new Label { Location = new Point(20, 100), Text = "Дата выдачи:" };
            this.dtpIssueDate = new DateTimePicker { Location = new Point(150, 100), Size = new Size(200, 20), Format = DateTimePickerFormat.Short };
            this.lblExpiryDate = new Label { Location = new Point(20, 140), Text = "Дата окончания:" };
            this.dtpExpiryDate = new DateTimePicker { Location = new Point(150, 140), Size = new Size(200, 20), Format = DateTimePickerFormat.Short };
            this.btnSave = new Button { Location = new Point(150, 180), Size = new Size(100, 30), Text = "Сохранить" };
            this.btnCancel = new Button { Location = new Point(260, 180), Size = new Size(100, 30), Text = "Отмена" };
            this.SuspendLayout();

            // Настройка стилей для всех элементов
            Color backColor = Color.FromArgb(46, 89, 132);
            Color foreColor = Color.White;
            foreach (Control control in new Control[] { lblDriverName, txtDriverName, lblLicenseNumber, txtLicenseNumber,
                lblIssueDate, dtpIssueDate, lblExpiryDate, dtpExpiryDate, btnSave, btnCancel })
            {
                control.ForeColor = foreColor;
                if (control is TextBox || control is DateTimePicker || control is Button)
                    control.BackColor = backColor;
            }

            // Настройка кнопок
            foreach (Button btn in new Button[] { btnSave, btnCancel })
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.Click += btn == btnSave ? new EventHandler(btnSave_Click) : new EventHandler(btnCancel_Click);
            }

            // Настройка формы
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = backColor;
            this.ClientSize = new Size(384, 261);
            this.Controls.AddRange(new Control[] { btnCancel, btnSave, dtpExpiryDate, lblExpiryDate, dtpIssueDate,
                lblIssueDate, txtLicenseNumber, lblLicenseNumber, txtDriverName, lblDriverName });
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LicenseEditForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "В/У Добавить";
            this.ResumeLayout(false);
        }

        private Label lblDriverName;
        private TextBox txtDriverName;
        private Label lblLicenseNumber;
        private TextBox txtLicenseNumber;
        private Label lblIssueDate;
        private DateTimePicker dtpIssueDate;
        private Label lblExpiryDate;
        private DateTimePicker dtpExpiryDate;
        private Button btnSave;
        private Button btnCancel;
    }
}