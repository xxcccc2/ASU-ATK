namespace TransportCompany
{
    partial class ComparisonForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComparisonForm));
            this.comboBoxObject1 = new System.Windows.Forms.ComboBox();
            this.comboBoxObject2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxPeriod = new System.Windows.Forms.GroupBox();
            this.radioButtonCustomPeriod = new System.Windows.Forms.RadioButton();
            this.radioButtonYear = new System.Windows.Forms.RadioButton();
            this.radioButtonCurrentMonth = new System.Windows.Forms.RadioButton();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.btnCompare = new System.Windows.Forms.Button();
            this.chartTrips = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartRevenueWithVAT = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartRevenueWithoutVAT = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartSalary = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnBack = new System.Windows.Forms.Button();
            this.groupBoxPeriod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTrips)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenueWithVAT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenueWithoutVAT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSalary)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxObject1
            // 
            this.comboBoxObject1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxObject1.FormattingEnabled = true;
            this.comboBoxObject1.Location = new System.Drawing.Point(15, 32);
            this.comboBoxObject1.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxObject1.Name = "comboBoxObject1";
            this.comboBoxObject1.Size = new System.Drawing.Size(226, 21);
            this.comboBoxObject1.TabIndex = 0;
            // 
            // comboBoxObject2
            // 
            this.comboBoxObject2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxObject2.FormattingEnabled = true;
            this.comboBoxObject2.Location = new System.Drawing.Point(255, 32);
            this.comboBoxObject2.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxObject2.Name = "comboBoxObject2";
            this.comboBoxObject2.Size = new System.Drawing.Size(226, 21);
            this.comboBoxObject2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Первый объект:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(255, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Второй объект:";
            // 
            // groupBoxPeriod
            // 
            this.groupBoxPeriod.Controls.Add(this.radioButtonCustomPeriod);
            this.groupBoxPeriod.Controls.Add(this.radioButtonYear);
            this.groupBoxPeriod.Controls.Add(this.radioButtonCurrentMonth);
            this.groupBoxPeriod.Controls.Add(this.dateTimePickerEnd);
            this.groupBoxPeriod.Controls.Add(this.dateTimePickerStart);
            this.groupBoxPeriod.Location = new System.Drawing.Point(15, 65);
            this.groupBoxPeriod.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxPeriod.Name = "groupBoxPeriod";
            this.groupBoxPeriod.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxPeriod.Size = new System.Drawing.Size(465, 98);
            this.groupBoxPeriod.TabIndex = 4;
            this.groupBoxPeriod.TabStop = false;
            this.groupBoxPeriod.Text = "Период";
            // 
            // radioButtonCustomPeriod
            // 
            this.radioButtonCustomPeriod.AutoSize = true;
            this.radioButtonCustomPeriod.Location = new System.Drawing.Point(8, 65);
            this.radioButtonCustomPeriod.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonCustomPeriod.Name = "radioButtonCustomPeriod";
            this.radioButtonCustomPeriod.Size = new System.Drawing.Size(131, 17);
            this.radioButtonCustomPeriod.TabIndex = 4;
            this.radioButtonCustomPeriod.TabStop = true;
            this.radioButtonCustomPeriod.Text = "Выбранный период";
            this.radioButtonCustomPeriod.UseVisualStyleBackColor = true;
            this.radioButtonCustomPeriod.CheckedChanged += new System.EventHandler(this.radioButtonCustomPeriod_CheckedChanged);
            // 
            // radioButtonYear
            // 
            this.radioButtonYear.AutoSize = true;
            this.radioButtonYear.Location = new System.Drawing.Point(8, 41);
            this.radioButtonYear.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonYear.Name = "radioButtonYear";
            this.radioButtonYear.Size = new System.Drawing.Size(58, 17);
            this.radioButtonYear.TabIndex = 3;
            this.radioButtonYear.TabStop = true;
            this.radioButtonYear.Text = "За год";
            this.radioButtonYear.UseVisualStyleBackColor = true;
            this.radioButtonYear.CheckedChanged += new System.EventHandler(this.radioButtonYear_CheckedChanged);
            // 
            // radioButtonCurrentMonth
            // 
            this.radioButtonCurrentMonth.AutoSize = true;
            this.radioButtonCurrentMonth.Location = new System.Drawing.Point(8, 16);
            this.radioButtonCurrentMonth.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonCurrentMonth.Name = "radioButtonCurrentMonth";
            this.radioButtonCurrentMonth.Size = new System.Drawing.Size(106, 17);
            this.radioButtonCurrentMonth.TabIndex = 2;
            this.radioButtonCurrentMonth.TabStop = true;
            this.radioButtonCurrentMonth.Text = "Текущий месяц";
            this.radioButtonCurrentMonth.UseVisualStyleBackColor = true;
            this.radioButtonCurrentMonth.CheckedChanged += new System.EventHandler(this.radioButtonCurrentMonth_CheckedChanged);
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.Enabled = false;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(300, 65);
            this.dateTimePickerEnd.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(151, 22);
            this.dateTimePickerEnd.TabIndex = 1;
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Enabled = false;
            this.dateTimePickerStart.Location = new System.Drawing.Point(300, 41);
            this.dateTimePickerStart.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(151, 22);
            this.dateTimePickerStart.TabIndex = 0;
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(15, 171);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(2);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(112, 32);
            this.btnCompare.TabIndex = 5;
            this.btnCompare.Text = "Сравнить";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // chartTrips
            // 
            chartArea1.Name = "ChartArea1";
            this.chartTrips.ChartAreas.Add(chartArea1);
            this.chartTrips.Location = new System.Drawing.Point(15, 219);
            this.chartTrips.Margin = new System.Windows.Forms.Padding(2);
            this.chartTrips.Name = "chartTrips";
            this.chartTrips.Size = new System.Drawing.Size(435, 244);
            this.chartTrips.TabIndex = 6;
            this.chartTrips.Text = "Количество рейсов";
            // 
            // chartRevenueWithVAT
            // 
            chartArea2.Name = "ChartArea1";
            this.chartRevenueWithVAT.ChartAreas.Add(chartArea2);
            this.chartRevenueWithVAT.Location = new System.Drawing.Point(465, 479);
            this.chartRevenueWithVAT.Margin = new System.Windows.Forms.Padding(2);
            this.chartRevenueWithVAT.Name = "chartRevenueWithVAT";
            this.chartRevenueWithVAT.Size = new System.Drawing.Size(435, 244);
            this.chartRevenueWithVAT.TabIndex = 7;
            this.chartRevenueWithVAT.Text = "Выручка с НДС";
            // 
            // chartRevenueWithoutVAT
            // 
            chartArea3.Name = "ChartArea1";
            this.chartRevenueWithoutVAT.ChartAreas.Add(chartArea3);
            this.chartRevenueWithoutVAT.Location = new System.Drawing.Point(15, 479);
            this.chartRevenueWithoutVAT.Margin = new System.Windows.Forms.Padding(2);
            this.chartRevenueWithoutVAT.Name = "chartRevenueWithoutVAT";
            this.chartRevenueWithoutVAT.Size = new System.Drawing.Size(435, 244);
            this.chartRevenueWithoutVAT.TabIndex = 8;
            this.chartRevenueWithoutVAT.Text = "Выручка без НДС";
            // 
            // chartSalary
            // 
            chartArea4.Name = "ChartArea1";
            this.chartSalary.ChartAreas.Add(chartArea4);
            this.chartSalary.Location = new System.Drawing.Point(465, 219);
            this.chartSalary.Margin = new System.Windows.Forms.Padding(2);
            this.chartSalary.Name = "chartSalary";
            this.chartSalary.Size = new System.Drawing.Size(435, 244);
            this.chartSalary.TabIndex = 9;
            this.chartSalary.Text = "Зарплата";
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(825, 739);
            this.btnBack.Margin = new System.Windows.Forms.Padding(2);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 32);
            this.btnBack.TabIndex = 10;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // ComparisonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.ClientSize = new System.Drawing.Size(915, 780);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.chartSalary);
            this.Controls.Add(this.chartRevenueWithoutVAT);
            this.Controls.Add(this.chartRevenueWithVAT);
            this.Controls.Add(this.chartTrips);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.groupBoxPeriod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxObject2);
            this.Controls.Add(this.comboBoxObject1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ComparisonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сравнение ТС и Водителей";
            this.Load += new System.EventHandler(this.ComparisonForm_Load);
            this.groupBoxPeriod.ResumeLayout(false);
            this.groupBoxPeriod.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTrips)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenueWithVAT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenueWithoutVAT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSalary)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ComboBox comboBoxObject1;
        private System.Windows.Forms.ComboBox comboBoxObject2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxPeriod;
        private System.Windows.Forms.RadioButton radioButtonCustomPeriod;
        private System.Windows.Forms.RadioButton radioButtonYear;
        private System.Windows.Forms.RadioButton radioButtonCurrentMonth;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTrips;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenueWithVAT;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenueWithoutVAT;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSalary;
        private System.Windows.Forms.Button btnBack;
    }
}