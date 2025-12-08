namespace TransportCompany.Forms
{
    partial class SettingsForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabZones = new System.Windows.Forms.TabPage();
            this.btnSaveZones = new System.Windows.Forms.Button();
            this.dgvZones = new System.Windows.Forms.DataGridView();
            this.colZoneId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpdatedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblZonesTitle = new System.Windows.Forms.Label();
            this.tabConnection = new System.Windows.Forms.TabPage();
            this.btnSaveConnection = new System.Windows.Forms.Button();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.lblConnectionTitle = new System.Windows.Forms.Label();
            this.lblCurrentConnection = new System.Windows.Forms.Label();
            this.txtOriginalConnectionString = new System.Windows.Forms.TextBox();
            this.tabHistory = new System.Windows.Forms.TabPage();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.colHistoryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistoryZone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistoryOldCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistoryNewCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHistoryChangedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblHistoryTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabZones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvZones)).BeginInit();
            this.tabConnection.SuspendLayout();
            this.tabHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabZones);
            this.tabControl.Controls.Add(this.tabConnection);
            this.tabControl.Controls.Add(this.tabHistory);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(560, 380);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabZones
            // 
            this.tabZones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabZones.Controls.Add(this.btnSaveZones);
            this.tabZones.Controls.Add(this.dgvZones);
            this.tabZones.Controls.Add(this.lblZonesTitle);
            this.tabZones.Location = new System.Drawing.Point(4, 26);
            this.tabZones.Name = "tabZones";
            this.tabZones.Padding = new System.Windows.Forms.Padding(3);
            this.tabZones.Size = new System.Drawing.Size(552, 350);
            this.tabZones.TabIndex = 0;
            this.tabZones.Text = "Стоимость зон";
            // 
            // btnSaveZones
            // 
            this.btnSaveZones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnSaveZones.Enabled = false;
            this.btnSaveZones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveZones.ForeColor = System.Drawing.Color.White;
            this.btnSaveZones.Location = new System.Drawing.Point(366, 310);
            this.btnSaveZones.Name = "btnSaveZones";
            this.btnSaveZones.Size = new System.Drawing.Size(180, 35);
            this.btnSaveZones.TabIndex = 2;
            this.btnSaveZones.Text = "Сохранить изменения";
            this.btnSaveZones.UseVisualStyleBackColor = false;
            this.btnSaveZones.Click += new System.EventHandler(this.btnSaveZones_Click);
            // 
            // dgvZones
            // 
            this.dgvZones.AllowUserToAddRows = false;
            this.dgvZones.AllowUserToDeleteRows = false;
            this.dgvZones.BackgroundColor = System.Drawing.Color.White;
            this.dgvZones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvZones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colZoneId,
            this.colCost,
            this.colUpdatedDate});
            this.dgvZones.Location = new System.Drawing.Point(6, 35);
            this.dgvZones.Name = "dgvZones";
            this.dgvZones.RowHeadersVisible = false;
            this.dgvZones.Size = new System.Drawing.Size(540, 270);
            this.dgvZones.TabIndex = 1;
            this.dgvZones.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvZones_CellValueChanged);
            // 
            // colZoneId
            // 
            this.colZoneId.HeaderText = "Зона";
            this.colZoneId.Name = "colZoneId";
            this.colZoneId.ReadOnly = true;
            this.colZoneId.Width = 80;
            // 
            // colCost
            // 
            this.colCost.HeaderText = "Стоимость (руб.)";
            this.colCost.Name = "colCost";
            this.colCost.Width = 150;
            // 
            // colUpdatedDate
            // 
            this.colUpdatedDate.HeaderText = "Последнее изменение";
            this.colUpdatedDate.Name = "colUpdatedDate";
            this.colUpdatedDate.ReadOnly = true;
            this.colUpdatedDate.Width = 200;
            // 
            // lblZonesTitle
            // 
            this.lblZonesTitle.AutoSize = true;
            this.lblZonesTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblZonesTitle.Location = new System.Drawing.Point(6, 10);
            this.lblZonesTitle.Name = "lblZonesTitle";
            this.lblZonesTitle.Size = new System.Drawing.Size(250, 19);
            this.lblZonesTitle.TabIndex = 0;
            this.lblZonesTitle.Text = "Настройка стоимости по зонам:";
            // 
            // tabConnection
            // 
            this.tabConnection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabConnection.Controls.Add(this.txtOriginalConnectionString);
            this.tabConnection.Controls.Add(this.lblCurrentConnection);
            this.tabConnection.Controls.Add(this.btnSaveConnection);
            this.tabConnection.Controls.Add(this.btnTestConnection);
            this.tabConnection.Controls.Add(this.txtConnectionString);
            this.tabConnection.Controls.Add(this.lblConnectionTitle);
            this.tabConnection.Location = new System.Drawing.Point(4, 26);
            this.tabConnection.Name = "tabConnection";
            this.tabConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tabConnection.Size = new System.Drawing.Size(552, 350);
            this.tabConnection.TabIndex = 1;
            this.tabConnection.Text = "Подключение к БД";
            // 
            // txtOriginalConnectionString
            // 
            this.txtOriginalConnectionString.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.txtOriginalConnectionString.Location = new System.Drawing.Point(6, 55);
            this.txtOriginalConnectionString.Multiline = true;
            this.txtOriginalConnectionString.Name = "txtOriginalConnectionString";
            this.txtOriginalConnectionString.ReadOnly = true;
            this.txtOriginalConnectionString.Size = new System.Drawing.Size(540, 60);
            this.txtOriginalConnectionString.TabIndex = 5;
            // 
            // lblCurrentConnection
            // 
            this.lblCurrentConnection.AutoSize = true;
            this.lblCurrentConnection.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCurrentConnection.Location = new System.Drawing.Point(6, 35);
            this.lblCurrentConnection.Name = "lblCurrentConnection";
            this.lblCurrentConnection.Size = new System.Drawing.Size(150, 15);
            this.lblCurrentConnection.TabIndex = 4;
            this.lblCurrentConnection.Text = "Текущая строка подключения:";
            // 
            // btnSaveConnection
            // 
            this.btnSaveConnection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnSaveConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveConnection.ForeColor = System.Drawing.Color.White;
            this.btnSaveConnection.Location = new System.Drawing.Point(366, 310);
            this.btnSaveConnection.Name = "btnSaveConnection";
            this.btnSaveConnection.Size = new System.Drawing.Size(180, 35);
            this.btnSaveConnection.TabIndex = 3;
            this.btnSaveConnection.Text = "Сохранить";
            this.btnSaveConnection.UseVisualStyleBackColor = false;
            this.btnSaveConnection.Click += new System.EventHandler(this.btnSaveConnection_Click);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnTestConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestConnection.ForeColor = System.Drawing.Color.White;
            this.btnTestConnection.Location = new System.Drawing.Point(180, 310);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(180, 35);
            this.btnTestConnection.TabIndex = 2;
            this.btnTestConnection.Text = "Проверить подключение";
            this.btnTestConnection.UseVisualStyleBackColor = false;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(6, 150);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(540, 100);
            this.txtConnectionString.TabIndex = 1;
            // 
            // lblConnectionTitle
            // 
            this.lblConnectionTitle.AutoSize = true;
            this.lblConnectionTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblConnectionTitle.Location = new System.Drawing.Point(6, 10);
            this.lblConnectionTitle.Name = "lblConnectionTitle";
            this.lblConnectionTitle.Size = new System.Drawing.Size(200, 19);
            this.lblConnectionTitle.TabIndex = 0;
            this.lblConnectionTitle.Text = "Настройка подключения к БД:";
            // 
            // lblNewConnection
            // 
            this.lblNewConnection = new System.Windows.Forms.Label();
            this.lblNewConnection.AutoSize = true;
            this.lblNewConnection.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNewConnection.Location = new System.Drawing.Point(6, 125);
            this.lblNewConnection.Name = "lblNewConnection";
            this.lblNewConnection.Size = new System.Drawing.Size(350, 15);
            this.lblNewConnection.TabIndex = 6;
            this.lblNewConnection.Text = "Новая строка подключения (измените Data Source для другого ПК):";
            this.tabConnection.Controls.Add(this.lblNewConnection);
            // 
            // tabHistory
            // 
            this.tabHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabHistory.Controls.Add(this.dgvHistory);
            this.tabHistory.Controls.Add(this.lblHistoryTitle);
            this.tabHistory.Location = new System.Drawing.Point(4, 26);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.Size = new System.Drawing.Size(552, 350);
            this.tabHistory.TabIndex = 2;
            this.tabHistory.Text = "История изменений";
            // 
            // dgvHistory
            // 
            this.dgvHistory.AllowUserToAddRows = false;
            this.dgvHistory.AllowUserToDeleteRows = false;
            this.dgvHistory.BackgroundColor = System.Drawing.Color.White;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHistoryDate,
            this.colHistoryZone,
            this.colHistoryOldCost,
            this.colHistoryNewCost,
            this.colHistoryChangedBy});
            this.dgvHistory.Location = new System.Drawing.Point(6, 35);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.RowHeadersVisible = false;
            this.dgvHistory.Size = new System.Drawing.Size(540, 310);
            this.dgvHistory.TabIndex = 1;
            // 
            // colHistoryDate
            // 
            this.colHistoryDate.HeaderText = "Дата изменения";
            this.colHistoryDate.Name = "colHistoryDate";
            this.colHistoryDate.ReadOnly = true;
            this.colHistoryDate.Width = 130;
            // 
            // colHistoryZone
            // 
            this.colHistoryZone.HeaderText = "Зона";
            this.colHistoryZone.Name = "colHistoryZone";
            this.colHistoryZone.ReadOnly = true;
            this.colHistoryZone.Width = 70;
            // 
            // colHistoryOldCost
            // 
            this.colHistoryOldCost.HeaderText = "Старая стоимость";
            this.colHistoryOldCost.Name = "colHistoryOldCost";
            this.colHistoryOldCost.ReadOnly = true;
            this.colHistoryOldCost.Width = 110;
            // 
            // colHistoryNewCost
            // 
            this.colHistoryNewCost.HeaderText = "Новая стоимость";
            this.colHistoryNewCost.Name = "colHistoryNewCost";
            this.colHistoryNewCost.ReadOnly = true;
            this.colHistoryNewCost.Width = 110;
            // 
            // colHistoryChangedBy
            // 
            this.colHistoryChangedBy.HeaderText = "Кем изменено";
            this.colHistoryChangedBy.Name = "colHistoryChangedBy";
            this.colHistoryChangedBy.ReadOnly = true;
            this.colHistoryChangedBy.Width = 110;
            // 
            // lblHistoryTitle
            // 
            this.lblHistoryTitle.AutoSize = true;
            this.lblHistoryTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblHistoryTitle.Location = new System.Drawing.Point(6, 10);
            this.lblHistoryTitle.Name = "lblHistoryTitle";
            this.lblHistoryTitle.Size = new System.Drawing.Size(280, 19);
            this.lblHistoryTitle.TabIndex = 0;
            this.lblHistoryTitle.Text = "История изменений стоимости зон:";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(392, 398);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(180, 35);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabZones.ResumeLayout(false);
            this.tabZones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvZones)).EndInit();
            this.tabConnection.ResumeLayout(false);
            this.tabConnection.PerformLayout();
            this.tabHistory.ResumeLayout(false);
            this.tabHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabZones;
        private System.Windows.Forms.TabPage tabConnection;
        private System.Windows.Forms.TabPage tabHistory;
        private System.Windows.Forms.DataGridView dgvZones;
        private System.Windows.Forms.DataGridViewTextBoxColumn colZoneId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpdatedDate;
        private System.Windows.Forms.Label lblZonesTitle;
        private System.Windows.Forms.Button btnSaveZones;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label lblConnectionTitle;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button btnSaveConnection;
        private System.Windows.Forms.Label lblCurrentConnection;
        private System.Windows.Forms.TextBox txtOriginalConnectionString;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistoryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistoryZone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistoryOldCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistoryNewCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHistoryChangedBy;
        private System.Windows.Forms.Label lblHistoryTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblNewConnection;
    }
}
