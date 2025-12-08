namespace TransportCompany
{
    partial class VehicleDriverEarningsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VehicleDriverEarningsForm));
            this.comboBoxVehicleNumber = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxDriverFIO = new System.Windows.Forms.ComboBox();
            this.groupBoxDateFilter = new System.Windows.Forms.GroupBox();
            this.radioButtonCustomPeriod = new System.Windows.Forms.RadioButton();
            this.radioButtonYear = new System.Windows.Forms.RadioButton();
            this.radioButtonCurrentMonth = new System.Windows.Forms.RadioButton();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewEarnings = new System.Windows.Forms.DataGridView();
            this.btnBack = new System.Windows.Forms.Button();
            this.groupBoxDateFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEarnings)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxVehicleNumber
            // 
            this.comboBoxVehicleNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.comboBoxVehicleNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVehicleNumber.ForeColor = System.Drawing.Color.White;
            this.comboBoxVehicleNumber.FormattingEnabled = true;
            this.comboBoxVehicleNumber.Location = new System.Drawing.Point(12, 27);
            this.comboBoxVehicleNumber.Name = "comboBoxVehicleNumber";
            this.comboBoxVehicleNumber.Size = new System.Drawing.Size(200, 23);
            this.comboBoxVehicleNumber.TabIndex = 0;
            this.comboBoxVehicleNumber.SelectedIndexChanged += new System.EventHandler(this.comboBoxVehicleNumber_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Госномер ТС:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(218, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "ФИО:";
            // 
            // comboBoxDriverFIO
            // 
            this.comboBoxDriverFIO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.comboBoxDriverFIO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDriverFIO.ForeColor = System.Drawing.Color.White;
            this.comboBoxDriverFIO.FormattingEnabled = true;
            this.comboBoxDriverFIO.Location = new System.Drawing.Point(218, 27);
            this.comboBoxDriverFIO.Name = "comboBoxDriverFIO";
            this.comboBoxDriverFIO.Size = new System.Drawing.Size(200, 23);
            this.comboBoxDriverFIO.TabIndex = 3;
            this.comboBoxDriverFIO.SelectedIndexChanged += new System.EventHandler(this.comboBoxDriverFIO_SelectedIndexChanged);
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
            this.groupBoxDateFilter.TabIndex = 4;
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
            this.dateTimePickerEnd.ValueChanged += new System.EventHandler(this.dateTimePickerEnd_ValueChanged);
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
            this.dateTimePickerStart.ValueChanged += new System.EventHandler(this.dateTimePickerStart_ValueChanged);
            // 
            // dataGridViewEarnings
            // 
            this.dataGridViewEarnings.AllowUserToAddRows = false;
            this.dataGridViewEarnings.AllowUserToDeleteRows = false;
            this.dataGridViewEarnings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewEarnings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewEarnings.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewEarnings.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewEarnings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewEarnings.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewEarnings.Location = new System.Drawing.Point(12, 160);
            this.dataGridViewEarnings.Name = "dataGridViewEarnings";
            this.dataGridViewEarnings.ReadOnly = true;
            this.dataGridViewEarnings.RowHeadersVisible = false;
            this.dataGridViewEarnings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewEarnings.Size = new System.Drawing.Size(606, 300);
            this.dataGridViewEarnings.TabIndex = 5;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(543, 466);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 6;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // VehicleDriverEarningsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.ClientSize = new System.Drawing.Size(630, 500);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.dataGridViewEarnings);
            this.Controls.Add(this.groupBoxDateFilter);
            this.Controls.Add(this.comboBoxDriverFIO);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxVehicleNumber);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VehicleDriverEarningsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Заработок ТС и Водителей";
            this.Load += new System.EventHandler(this.VehicleDriverEarningsForm_Load);
            this.groupBoxDateFilter.ResumeLayout(false);
            this.groupBoxDateFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEarnings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ComboBox comboBoxVehicleNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxDriverFIO;
        private System.Windows.Forms.GroupBox groupBoxDateFilter;
        private System.Windows.Forms.RadioButton radioButtonCustomPeriod;
        private System.Windows.Forms.RadioButton radioButtonYear;
        private System.Windows.Forms.RadioButton radioButtonCurrentMonth;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.DataGridView dataGridViewEarnings;
        private System.Windows.Forms.Button btnBack;
    }
}