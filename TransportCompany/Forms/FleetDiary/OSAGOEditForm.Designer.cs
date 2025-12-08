using System.Drawing;
using System.Windows.Forms;
using System;

namespace TransportCompany
{
    partial class OSAGOEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OSAGOEditForm));
            this.lblVehicleReg = new Label { Location = new Point(20, 20), Text = "Рег. номер авто:" };
            this.txtVehicleReg = new TextBox { Location = new Point(150, 20), Size = new Size(200, 20) };
            this.lblPolicyNumber = new Label { Location = new Point(20, 60), Text = "Номер полиса:" };
            this.txtPolicyNumber = new TextBox { Location = new Point(150, 60), Size = new Size(200, 20) };
            this.lblStartDate = new Label { Location = new Point(20, 100), Text = "Дата начала:" };
            this.dtpStartDate = new DateTimePicker { Location = new Point(150, 100), Size = new Size(200, 20), Format = DateTimePickerFormat.Short };
            this.lblEndDate = new Label { Location = new Point(20, 140), Text = "Дата окончания:" };
            this.dtpEndDate = new DateTimePicker { Location = new Point(150, 140), Size = new Size(200, 20), Format = DateTimePickerFormat.Short };
            this.btnSave = new Button { Location = new Point(150, 180), Size = new Size(100, 30), Text = "Сохранить" };
            this.btnCancel = new Button { Location = new Point(260, 180), Size = new Size(100, 30), Text = "Отмена" };
            this.SuspendLayout();

            // Настройка стилей для всех элементов
            Color backColor = Color.FromArgb(46, 89, 132);
            Color foreColor = Color.White;
            foreach (Control control in new Control[] { lblVehicleReg, txtVehicleReg, lblPolicyNumber, txtPolicyNumber,
                lblStartDate, dtpStartDate, lblEndDate, dtpEndDate, btnSave, btnCancel })
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
            this.Controls.AddRange(new Control[] { btnCancel, btnSave, dtpEndDate, lblEndDate, dtpStartDate, lblStartDate,
                txtPolicyNumber, lblPolicyNumber, txtVehicleReg, lblVehicleReg });
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OSAGOEditForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "ОСАГО Добавить";
            this.ResumeLayout(false);
        }

        private Label lblVehicleReg;
        private TextBox txtVehicleReg;
        private Label lblPolicyNumber;
        private TextBox txtPolicyNumber;
        private Label lblStartDate;
        private DateTimePicker dtpStartDate;
        private Label lblEndDate;
        private DateTimePicker dtpEndDate;
        private Button btnSave;
        private Button btnCancel;
    }
}