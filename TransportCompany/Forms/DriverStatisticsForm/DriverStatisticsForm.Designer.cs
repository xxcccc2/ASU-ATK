namespace TransportCompany
{
    partial class DriverStatisticsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DriverStatisticsForm));
            this.comboBoxDriverFIO = new System.Windows.Forms.ComboBox();
            this.labelDriver = new System.Windows.Forms.Label();
            this.groupBoxDateFilter = new System.Windows.Forms.GroupBox();
            this.radioButtonCustomPeriod = new System.Windows.Forms.RadioButton();
            this.radioButtonYear = new System.Windows.Forms.RadioButton();
            this.radioButtonCurrentMonth = new System.Windows.Forms.RadioButton();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnPayroll = new System.Windows.Forms.Button();
            this.groupBoxDateFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxDriverFIO
            // 
            this.comboBoxDriverFIO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.comboBoxDriverFIO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDriverFIO.ForeColor = System.Drawing.Color.White;
            this.comboBoxDriverFIO.FormattingEnabled = true;
            this.comboBoxDriverFIO.Location = new System.Drawing.Point(12, 27);
            this.comboBoxDriverFIO.Name = "comboBoxDriverFIO";
            this.comboBoxDriverFIO.Size = new System.Drawing.Size(200, 23);
            this.comboBoxDriverFIO.TabIndex = 0;
            // 
            // labelDriver
            // 
            this.labelDriver.AutoSize = true;
            this.labelDriver.ForeColor = System.Drawing.Color.White;
            this.labelDriver.Location = new System.Drawing.Point(12, 9);
            this.labelDriver.Name = "labelDriver";
            this.labelDriver.Size = new System.Drawing.Size(37, 15);
            this.labelDriver.TabIndex = 1;
            this.labelDriver.Text = "ФИО:";
            // 
            // groupBoxDateFilter
            // 
            this.groupBoxDateFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.groupBoxDateFilter.Controls.Add(this.radioButtonCustomPeriod);
            this.groupBoxDateFilter.Controls.Add(this.radioButtonYear);
            this.groupBoxDateFilter.Controls.Add(this.radioButtonCurrentMonth);
            this.groupBoxDateFilter.Controls.Add(this.dateTimePickerEnd);
            this.groupBoxDateFilter.Controls.Add(this.dateTimePickerStart);
            this.groupBoxDateFilter.ForeColor = System.Drawing.Color.White;
            this.groupBoxDateFilter.Location = new System.Drawing.Point(12, 54);
            this.groupBoxDateFilter.Name = "groupBoxDateFilter";
            this.groupBoxDateFilter.Size = new System.Drawing.Size(606, 100);
            this.groupBoxDateFilter.TabIndex = 2;
            this.groupBoxDateFilter.TabStop = false;
            this.groupBoxDateFilter.Text = "Период";
            // 
            // radioButtonCustomPeriod
            // 
            this.radioButtonCustomPeriod.AutoSize = true;
            this.radioButtonCustomPeriod.ForeColor = System.Drawing.Color.White;
            this.radioButtonCustomPeriod.Location = new System.Drawing.Point(6, 72);
            this.radioButtonCustomPeriod.Name = "radioButtonCustomPeriod";
            this.radioButtonCustomPeriod.Size = new System.Drawing.Size(134, 19);
            this.radioButtonCustomPeriod.TabIndex = 4;
            this.radioButtonCustomPeriod.TabStop = true;
            this.radioButtonCustomPeriod.Text = "Выбранный период";
            this.radioButtonCustomPeriod.UseVisualStyleBackColor = false;
            this.radioButtonCustomPeriod.CheckedChanged += new System.EventHandler(this.radioButtonCustomPeriod_CheckedChanged);
            // 
            // radioButtonYear
            // 
            this.radioButtonYear.AutoSize = true;
            this.radioButtonYear.ForeColor = System.Drawing.Color.White;
            this.radioButtonYear.Location = new System.Drawing.Point(6, 49);
            this.radioButtonYear.Name = "radioButtonYear";
            this.radioButtonYear.Size = new System.Drawing.Size(59, 19);
            this.radioButtonYear.TabIndex = 3;
            this.radioButtonYear.TabStop = true;
            this.radioButtonYear.Text = "За год";
            this.radioButtonYear.UseVisualStyleBackColor = false;
            this.radioButtonYear.CheckedChanged += new System.EventHandler(this.radioButtonYear_CheckedChanged);
            // 
            // radioButtonCurrentMonth
            // 
            this.radioButtonCurrentMonth.AutoSize = true;
            this.radioButtonCurrentMonth.ForeColor = System.Drawing.Color.White;
            this.radioButtonCurrentMonth.Location = new System.Drawing.Point(6, 26);
            this.radioButtonCurrentMonth.Name = "radioButtonCurrentMonth";
            this.radioButtonCurrentMonth.Size = new System.Drawing.Size(111, 19);
            this.radioButtonCurrentMonth.TabIndex = 2;
            this.radioButtonCurrentMonth.TabStop = true;
            this.radioButtonCurrentMonth.Text = "Текущий месяц";
            this.radioButtonCurrentMonth.UseVisualStyleBackColor = false;
            this.radioButtonCurrentMonth.CheckedChanged += new System.EventHandler(this.radioButtonCurrentMonth_CheckedChanged);
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.dateTimePickerEnd.Enabled = false;
            this.dateTimePickerEnd.ForeColor = System.Drawing.Color.White;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(250, 70);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(150, 23);
            this.dateTimePickerEnd.TabIndex = 1;
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.dateTimePickerStart.Enabled = false;
            this.dateTimePickerStart.ForeColor = System.Drawing.Color.White;
            this.dateTimePickerStart.Location = new System.Drawing.Point(250, 45);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(150, 23);
            this.dateTimePickerStart.TabIndex = 0;
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnAnalyze.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnalyze.ForeColor = System.Drawing.Color.White;
            this.btnAnalyze.Location = new System.Drawing.Point(468, 160);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(150, 23);
            this.btnAnalyze.TabIndex = 3;
            this.btnAnalyze.Text = "Рассчитать статистику";
            this.btnAnalyze.UseVisualStyleBackColor = false;
            this.btnAnalyze.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(543, 450);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnPayroll
            // 
            this.btnPayroll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnPayroll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayroll.ForeColor = System.Drawing.Color.White;
            this.btnPayroll.Location = new System.Drawing.Point(468, 200);
            this.btnPayroll.Name = "btnPayroll";
            this.btnPayroll.Size = new System.Drawing.Size(150, 23);
            this.btnPayroll.TabIndex = 28;
            this.btnPayroll.Text = "Расчетный листок";
            this.btnPayroll.UseVisualStyleBackColor = false;
            // 
            // DriverStatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.ClientSize = new System.Drawing.Size(630, 480);
            this.Controls.Add(this.btnPayroll);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.groupBoxDateFilter);
            this.Controls.Add(this.labelDriver);
            this.Controls.Add(this.comboBoxDriverFIO);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DriverStatisticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Персональная статистика водителей";
            this.groupBoxDateFilter.ResumeLayout(false);
            this.groupBoxDateFilter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ComboBox comboBoxDriverFIO;
        private System.Windows.Forms.Label labelDriver;
        private System.Windows.Forms.GroupBox groupBoxDateFilter;
        private System.Windows.Forms.RadioButton radioButtonCustomPeriod;
        private System.Windows.Forms.RadioButton radioButtonYear;
        private System.Windows.Forms.RadioButton radioButtonCurrentMonth;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnPayroll;
    }
}