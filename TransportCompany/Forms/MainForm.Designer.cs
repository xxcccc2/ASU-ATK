namespace TransportCompany
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.exit = new System.Windows.Forms.Button();
            this.btnTO = new System.Windows.Forms.Button();
            this.FleetDiary = new System.Windows.Forms.Button();
            this.btnImportData = new System.Windows.Forms.Button();
            this.btnOpenReestrs = new System.Windows.Forms.Button();
            this.btnOpenEarningsForm = new System.Windows.Forms.Button();
            this.btnOpenComparisonForm = new System.Windows.Forms.Button();
            this.btnOpenZoneAnalysis = new System.Windows.Forms.Button();
            this.btnOpenDriverStatistics = new System.Windows.Forms.Button();
            this.btnEditRegistry = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // exit
            // 
            this.exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit.ForeColor = System.Drawing.Color.White;
            this.exit.Location = new System.Drawing.Point(210, 297);
            this.exit.Margin = new System.Windows.Forms.Padding(4);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(180, 40);
            this.exit.TabIndex = 5;
            this.exit.Tag = "exit";
            this.exit.Text = "Выход";
            this.exit.UseVisualStyleBackColor = false;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // btnTO
            // 
            this.btnTO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnTO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTO.ForeColor = System.Drawing.Color.White;
            this.btnTO.Location = new System.Drawing.Point(20, 20);
            this.btnTO.Name = "btnTO";
            this.btnTO.Size = new System.Drawing.Size(180, 40);
            this.btnTO.TabIndex = 18;
            this.btnTO.Text = "Техосмотр";
            this.btnTO.UseVisualStyleBackColor = false;
            this.btnTO.Click += new System.EventHandler(this.btnTO_Click);
            // 
            // FleetDiary
            // 
            this.FleetDiary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.FleetDiary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FleetDiary.ForeColor = System.Drawing.Color.White;
            this.FleetDiary.Location = new System.Drawing.Point(20, 70);
            this.FleetDiary.Margin = new System.Windows.Forms.Padding(4);
            this.FleetDiary.Name = "FleetDiary";
            this.FleetDiary.Size = new System.Drawing.Size(180, 40);
            this.FleetDiary.TabIndex = 19;
            this.FleetDiary.Tag = "stat";
            this.FleetDiary.Text = "Дневник Автопарка";
            this.FleetDiary.UseVisualStyleBackColor = false;
            this.FleetDiary.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnImportData
            // 
            this.btnImportData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnImportData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportData.ForeColor = System.Drawing.Color.White;
            this.btnImportData.Location = new System.Drawing.Point(20, 120);
            this.btnImportData.Margin = new System.Windows.Forms.Padding(4);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(180, 40);
            this.btnImportData.TabIndex = 21;
            this.btnImportData.Text = "Импорт Реестров";
            this.btnImportData.UseVisualStyleBackColor = false;
            this.btnImportData.Click += new System.EventHandler(this.btnImportData_Click);
            // 
            // btnOpenReestrs
            // 
            this.btnOpenReestrs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnOpenReestrs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenReestrs.ForeColor = System.Drawing.Color.White;
            this.btnOpenReestrs.Location = new System.Drawing.Point(210, 20);
            this.btnOpenReestrs.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenReestrs.Name = "btnOpenReestrs";
            this.btnOpenReestrs.Size = new System.Drawing.Size(180, 40);
            this.btnOpenReestrs.TabIndex = 22;
            this.btnOpenReestrs.Text = "Расчет ЗП";
            this.btnOpenReestrs.UseVisualStyleBackColor = false;
            this.btnOpenReestrs.Click += new System.EventHandler(this.btnOpenReestrs_Click);
            // 
            // btnOpenEarningsForm
            // 
            this.btnOpenEarningsForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnOpenEarningsForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenEarningsForm.ForeColor = System.Drawing.Color.White;
            this.btnOpenEarningsForm.Location = new System.Drawing.Point(210, 70);
            this.btnOpenEarningsForm.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenEarningsForm.Name = "btnOpenEarningsForm";
            this.btnOpenEarningsForm.Size = new System.Drawing.Size(180, 40);
            this.btnOpenEarningsForm.TabIndex = 23;
            this.btnOpenEarningsForm.Text = "Заработок ТС и Водителей";
            this.btnOpenEarningsForm.UseVisualStyleBackColor = false;
            this.btnOpenEarningsForm.Click += new System.EventHandler(this.btnOpenEarningsForm_Click);
            // 
            // btnOpenComparisonForm
            // 
            this.btnOpenComparisonForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnOpenComparisonForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenComparisonForm.ForeColor = System.Drawing.Color.White;
            this.btnOpenComparisonForm.Location = new System.Drawing.Point(210, 120);
            this.btnOpenComparisonForm.Name = "btnOpenComparisonForm";
            this.btnOpenComparisonForm.Size = new System.Drawing.Size(180, 40);
            this.btnOpenComparisonForm.TabIndex = 24;
            this.btnOpenComparisonForm.Text = "Сравнение ТС/Водителей";
            this.btnOpenComparisonForm.UseVisualStyleBackColor = false;
            this.btnOpenComparisonForm.Click += new System.EventHandler(this.btnOpenComparisonForm_Click);
            // 
            // btnOpenZoneAnalysis
            // 
            this.btnOpenZoneAnalysis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnOpenZoneAnalysis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenZoneAnalysis.ForeColor = System.Drawing.Color.White;
            this.btnOpenZoneAnalysis.Location = new System.Drawing.Point(20, 170);
            this.btnOpenZoneAnalysis.Name = "btnOpenZoneAnalysis";
            this.btnOpenZoneAnalysis.Size = new System.Drawing.Size(180, 40);
            this.btnOpenZoneAnalysis.TabIndex = 26;
            this.btnOpenZoneAnalysis.Text = "Анализ зон";
            this.btnOpenZoneAnalysis.UseVisualStyleBackColor = false;
            this.btnOpenZoneAnalysis.Click += new System.EventHandler(this.btnOpenZoneAnalysis_Click_1);
            // 
            // btnOpenDriverStatistics
            // 
            this.btnOpenDriverStatistics.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnOpenDriverStatistics.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenDriverStatistics.ForeColor = System.Drawing.Color.White;
            this.btnOpenDriverStatistics.Location = new System.Drawing.Point(210, 170);
            this.btnOpenDriverStatistics.Name = "btnOpenDriverStatistics";
            this.btnOpenDriverStatistics.Size = new System.Drawing.Size(180, 40);
            this.btnOpenDriverStatistics.TabIndex = 27;
            this.btnOpenDriverStatistics.Text = "Статистика водителей";
            this.btnOpenDriverStatistics.UseVisualStyleBackColor = false;
            this.btnOpenDriverStatistics.Click += new System.EventHandler(this.btnOpenDriverStatistics_Click);
            // 
            // btnEditRegistry
            // 
            this.btnEditRegistry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnEditRegistry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditRegistry.ForeColor = System.Drawing.Color.White;
            this.btnEditRegistry.Location = new System.Drawing.Point(20, 220);
            this.btnEditRegistry.Name = "btnEditRegistry";
            this.btnEditRegistry.Size = new System.Drawing.Size(180, 40);
            this.btnEditRegistry.TabIndex = 28;
            this.btnEditRegistry.Text = "Редактировать реестр";
            this.btnEditRegistry.UseVisualStyleBackColor = false;
            this.btnEditRegistry.Click += new System.EventHandler(this.btnEditRegistry_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Location = new System.Drawing.Point(210, 220);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(180, 40);
            this.btnSettings.TabIndex = 29;
            this.btnSettings.Text = "⚙ Настройки";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.ClientSize = new System.Drawing.Size(415, 350);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnEditRegistry);
            this.Controls.Add(this.btnOpenDriverStatistics);
            this.Controls.Add(this.btnOpenZoneAnalysis);
            this.Controls.Add(this.btnOpenComparisonForm);
            this.Controls.Add(this.btnOpenEarningsForm);
            this.Controls.Add(this.btnOpenReestrs);
            this.Controls.Add(this.btnImportData);
            this.Controls.Add(this.FleetDiary);
            this.Controls.Add(this.btnTO);
            this.Controls.Add(this.exit);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "АТК-Форум";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.Button btnTO;
        private System.Windows.Forms.Button FleetDiary;
        private System.Windows.Forms.Button btnImportData;
        private System.Windows.Forms.Button btnOpenReestrs;
        private System.Windows.Forms.Button btnOpenEarningsForm;
        private System.Windows.Forms.Button btnOpenComparisonForm;
        private System.Windows.Forms.Button btnOpenZoneAnalysis;
        private System.Windows.Forms.Button btnOpenDriverStatistics;
        private System.Windows.Forms.Button btnEditRegistry;
        private System.Windows.Forms.Button btnSettings;
    }
}