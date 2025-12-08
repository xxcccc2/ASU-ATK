namespace TransportCompany
{
    partial class DriverSalaryForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DriverSalaryForm));
            this.groupBoxPeriod = new System.Windows.Forms.GroupBox();
            this.radioButtonCustomPeriod = new System.Windows.Forms.RadioButton();
            this.radioButtonCurrentYear = new System.Windows.Forms.RadioButton();
            this.radioButtonCurrentMonth = new System.Windows.Forms.RadioButton();
            this.labelStartDate = new System.Windows.Forms.Label();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.groupBoxPeriod.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxPeriod
            // 
            this.groupBoxPeriod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.groupBoxPeriod.Controls.Add(this.radioButtonCustomPeriod);
            this.groupBoxPeriod.Controls.Add(this.radioButtonCurrentYear);
            this.groupBoxPeriod.Controls.Add(this.radioButtonCurrentMonth);
            this.groupBoxPeriod.Controls.Add(this.labelStartDate);
            this.groupBoxPeriod.Controls.Add(this.dateTimePickerStart);
            this.groupBoxPeriod.Controls.Add(this.labelEndDate);
            this.groupBoxPeriod.Controls.Add(this.dateTimePickerEnd);
            this.groupBoxPeriod.ForeColor = System.Drawing.Color.White;
            this.groupBoxPeriod.Location = new System.Drawing.Point(12, 12);
            this.groupBoxPeriod.Name = "groupBoxPeriod";
            this.groupBoxPeriod.Size = new System.Drawing.Size(528, 120);
            this.groupBoxPeriod.TabIndex = 2;
            this.groupBoxPeriod.TabStop = false;
            this.groupBoxPeriod.Text = "Период";
            // 
            // radioButtonCustomPeriod
            // 
            this.radioButtonCustomPeriod.AutoSize = true;
            this.radioButtonCustomPeriod.ForeColor = System.Drawing.Color.White;
            this.radioButtonCustomPeriod.Location = new System.Drawing.Point(6, 88);
            this.radioButtonCustomPeriod.Name = "radioButtonCustomPeriod";
            this.radioButtonCustomPeriod.Size = new System.Drawing.Size(109, 19);
            this.radioButtonCustomPeriod.TabIndex = 6;
            this.radioButtonCustomPeriod.Text = "Произвольный";
            this.radioButtonCustomPeriod.UseVisualStyleBackColor = false;
            this.radioButtonCustomPeriod.CheckedChanged += new System.EventHandler(this.radioButtonCustomPeriod_CheckedChanged);
            // 
            // radioButtonCurrentYear
            // 
            this.radioButtonCurrentYear.AutoSize = true;
            this.radioButtonCurrentYear.ForeColor = System.Drawing.Color.White;
            this.radioButtonCurrentYear.Location = new System.Drawing.Point(6, 65);
            this.radioButtonCurrentYear.Name = "radioButtonCurrentYear";
            this.radioButtonCurrentYear.Size = new System.Drawing.Size(95, 19);
            this.radioButtonCurrentYear.TabIndex = 5;
            this.radioButtonCurrentYear.Text = "Текущий год";
            this.radioButtonCurrentYear.UseVisualStyleBackColor = false;
            this.radioButtonCurrentYear.CheckedChanged += new System.EventHandler(this.radioButtonCurrentYear_CheckedChanged);
            // 
            // radioButtonCurrentMonth
            // 
            this.radioButtonCurrentMonth.AutoSize = true;
            this.radioButtonCurrentMonth.ForeColor = System.Drawing.Color.White;
            this.radioButtonCurrentMonth.Location = new System.Drawing.Point(6, 42);
            this.radioButtonCurrentMonth.Name = "radioButtonCurrentMonth";
            this.radioButtonCurrentMonth.Size = new System.Drawing.Size(111, 19);
            this.radioButtonCurrentMonth.TabIndex = 4;
            this.radioButtonCurrentMonth.Text = "Текущий месяц";
            this.radioButtonCurrentMonth.UseVisualStyleBackColor = false;
            this.radioButtonCurrentMonth.CheckedChanged += new System.EventHandler(this.radioButtonCurrentMonth_CheckedChanged);
            // 
            // labelStartDate
            // 
            this.labelStartDate.AutoSize = true;
            this.labelStartDate.ForeColor = System.Drawing.Color.White;
            this.labelStartDate.Location = new System.Drawing.Point(116, 42);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(96, 15);
            this.labelStartDate.TabIndex = 3;
            this.labelStartDate.Text = "Начальная дата:";
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.dateTimePickerStart.Enabled = false;
            this.dateTimePickerStart.ForeColor = System.Drawing.Color.White;
            this.dateTimePickerStart.Location = new System.Drawing.Point(218, 38);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(150, 23);
            this.dateTimePickerStart.TabIndex = 2;
            // 
            // labelEndDate
            // 
            this.labelEndDate.AutoSize = true;
            this.labelEndDate.ForeColor = System.Drawing.Color.White;
            this.labelEndDate.Location = new System.Drawing.Point(116, 68);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(89, 15);
            this.labelEndDate.TabIndex = 1;
            this.labelEndDate.Text = "Конечная дата:";
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.dateTimePickerEnd.Enabled = false;
            this.dateTimePickerEnd.ForeColor = System.Drawing.Color.White;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(218, 68);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(150, 23);
            this.dateTimePickerEnd.TabIndex = 0;
            // 
            // btnCalculate
            // 
            this.btnCalculate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnCalculate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalculate.ForeColor = System.Drawing.Color.White;
            this.btnCalculate.Location = new System.Drawing.Point(12, 138);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(100, 23);
            this.btnCalculate.TabIndex = 3;
            this.btnCalculate.Text = "Рассчитать";
            this.btnCalculate.UseVisualStyleBackColor = false;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.ForeColor = System.Drawing.Color.White;
            this.lblResult.Location = new System.Drawing.Point(12, 168);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(60, 15);
            this.lblResult.TabIndex = 4;
            this.lblResult.Text = "Результат";
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(118, 138);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 23);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // DriverSalaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.ClientSize = new System.Drawing.Size(552, 400);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.groupBoxPeriod);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DriverSalaryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Зарплата водителей";
            this.groupBoxPeriod.ResumeLayout(false);
            this.groupBoxPeriod.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.GroupBox groupBoxPeriod;
        private System.Windows.Forms.Label labelStartDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.Label labelEndDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.RadioButton radioButtonCustomPeriod;
        private System.Windows.Forms.RadioButton radioButtonCurrentYear;
        private System.Windows.Forms.RadioButton radioButtonCurrentMonth;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnBack;
    }

}