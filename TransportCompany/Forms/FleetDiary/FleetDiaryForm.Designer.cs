namespace TransportCompany
{
    partial class FleetDiaryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FleetDiaryForm));
            this.lblOSAGO = new System.Windows.Forms.Label();
            this.osagoGrid = new System.Windows.Forms.DataGridView();
            this.btnAddOSAGO = new System.Windows.Forms.Button();
            this.btnEditOSAGO = new System.Windows.Forms.Button();
            this.btnDeleteOSAGO = new System.Windows.Forms.Button();
            this.lblLicenses = new System.Windows.Forms.Label();
            this.licensesGrid = new System.Windows.Forms.DataGridView();
            this.btnAddLicense = new System.Windows.Forms.Button();
            this.btnEditLicense = new System.Windows.Forms.Button();
            this.btnDeleteLicense = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.osagoGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.licensesGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // lblOSAGO
            // 
            this.lblOSAGO.AutoSize = true;
            this.lblOSAGO.ForeColor = System.Drawing.Color.White;
            this.lblOSAGO.Location = new System.Drawing.Point(10, 10);
            this.lblOSAGO.Name = "lblOSAGO";
            this.lblOSAGO.Size = new System.Drawing.Size(113, 13);
            this.lblOSAGO.TabIndex = 0;
            this.lblOSAGO.Text = "ОСАГО на грузовики";
            // 
            // osagoGrid
            // 
            this.osagoGrid.AllowUserToAddRows = false;
            this.osagoGrid.BackgroundColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.osagoGrid.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.osagoGrid.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.osagoGrid.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.osagoGrid.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.osagoGrid.Location = new System.Drawing.Point(10, 40);
            this.osagoGrid.Name = "osagoGrid";
            this.osagoGrid.ReadOnly = true;
            this.osagoGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.osagoGrid.Size = new System.Drawing.Size(470, 250);
            this.osagoGrid.TabIndex = 1;
            // 
            // btnAddOSAGO
            // 
            this.btnAddOSAGO.BackColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.btnAddOSAGO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddOSAGO.ForeColor = System.Drawing.Color.White;
            this.btnAddOSAGO.Location = new System.Drawing.Point(10, 300);
            this.btnAddOSAGO.Name = "btnAddOSAGO";
            this.btnAddOSAGO.Size = new System.Drawing.Size(100, 30);
            this.btnAddOSAGO.TabIndex = 2;
            this.btnAddOSAGO.Text = "Добавить";
            this.btnAddOSAGO.UseVisualStyleBackColor = false;
            this.btnAddOSAGO.Click += new System.EventHandler(this.btnAddOSAGO_Click);
            // 
            // btnEditOSAGO
            // 
            this.btnEditOSAGO.BackColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.btnEditOSAGO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditOSAGO.ForeColor = System.Drawing.Color.White;
            this.btnEditOSAGO.Location = new System.Drawing.Point(120, 300);
            this.btnEditOSAGO.Name = "btnEditOSAGO";
            this.btnEditOSAGO.Size = new System.Drawing.Size(100, 30);
            this.btnEditOSAGO.TabIndex = 3;
            this.btnEditOSAGO.Text = "Редактировать";
            this.btnEditOSAGO.UseVisualStyleBackColor = false;
            this.btnEditOSAGO.Click += new System.EventHandler(this.btnEditOSAGO_Click);
            // 
            // btnDeleteOSAGO
            // 
            this.btnDeleteOSAGO.BackColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.btnDeleteOSAGO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteOSAGO.ForeColor = System.Drawing.Color.White;
            this.btnDeleteOSAGO.Location = new System.Drawing.Point(230, 300);
            this.btnDeleteOSAGO.Name = "btnDeleteOSAGO";
            this.btnDeleteOSAGO.Size = new System.Drawing.Size(100, 30);
            this.btnDeleteOSAGO.TabIndex = 4;
            this.btnDeleteOSAGO.Text = "Удалить";
            this.btnDeleteOSAGO.UseVisualStyleBackColor = false;
            this.btnDeleteOSAGO.Click += new System.EventHandler(this.btnDeleteOSAGO_Click);
            // 
            // lblLicenses
            // 
            this.lblLicenses.AutoSize = true;
            this.lblLicenses.ForeColor = System.Drawing.Color.White;
            this.lblLicenses.Location = new System.Drawing.Point(500, 10);
            this.lblLicenses.Name = "lblLicenses";
            this.lblLicenses.Size = new System.Drawing.Size(158, 13);
            this.lblLicenses.TabIndex = 5;
            this.lblLicenses.Text = "Водительские удостоверения";
            // 
            // licensesGrid
            // 
            this.licensesGrid.AllowUserToAddRows = false;
            this.licensesGrid.BackgroundColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.licensesGrid.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.licensesGrid.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.licensesGrid.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.licensesGrid.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.licensesGrid.Location = new System.Drawing.Point(500, 40);
            this.licensesGrid.Name = "licensesGrid";
            this.licensesGrid.ReadOnly = true;
            this.licensesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.licensesGrid.Size = new System.Drawing.Size(470, 250);
            this.licensesGrid.TabIndex = 6;
            // 
            // btnAddLicense
            // 
            this.btnAddLicense.BackColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.btnAddLicense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddLicense.ForeColor = System.Drawing.Color.White;
            this.btnAddLicense.Location = new System.Drawing.Point(500, 300);
            this.btnAddLicense.Name = "btnAddLicense";
            this.btnAddLicense.Size = new System.Drawing.Size(100, 30);
            this.btnAddLicense.TabIndex = 7;
            this.btnAddLicense.Text = "Добавить";
            this.btnAddLicense.UseVisualStyleBackColor = false;
            this.btnAddLicense.Click += new System.EventHandler(this.btnAddLicense_Click);
            // 
            // btnEditLicense
            // 
            this.btnEditLicense.BackColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.btnEditLicense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditLicense.ForeColor = System.Drawing.Color.White;
            this.btnEditLicense.Location = new System.Drawing.Point(610, 300);
            this.btnEditLicense.Name = "btnEditLicense";
            this.btnEditLicense.Size = new System.Drawing.Size(100, 30);
            this.btnEditLicense.TabIndex = 8;
            this.btnEditLicense.Text = "Редактировать";
            this.btnEditLicense.UseVisualStyleBackColor = false;
            this.btnEditLicense.Click += new System.EventHandler(this.btnEditLicense_Click);
            // 
            // btnDeleteLicense
            // 
            this.btnDeleteLicense.BackColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.btnDeleteLicense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteLicense.ForeColor = System.Drawing.Color.White;
            this.btnDeleteLicense.Location = new System.Drawing.Point(720, 300);
            this.btnDeleteLicense.Name = "btnDeleteLicense";
            this.btnDeleteLicense.Size = new System.Drawing.Size(100, 30);
            this.btnDeleteLicense.TabIndex = 9;
            this.btnDeleteLicense.Text = "Удалить";
            this.btnDeleteLicense.UseVisualStyleBackColor = false;
            this.btnDeleteLicense.Click += new System.EventHandler(this.btnDeleteLicense_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(10, 348);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(145, 61);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // FleetDiaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(46, 89, 132);
            this.ClientSize = new System.Drawing.Size(984, 421);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDeleteLicense);
            this.Controls.Add(this.btnEditLicense);
            this.Controls.Add(this.btnAddLicense);
            this.Controls.Add(this.licensesGrid);
            this.Controls.Add(this.lblLicenses);
            this.Controls.Add(this.btnDeleteOSAGO);
            this.Controls.Add(this.btnEditOSAGO);
            this.Controls.Add(this.btnAddOSAGO);
            this.Controls.Add(this.osagoGrid);
            this.Controls.Add(this.lblOSAGO);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FleetDiaryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Дневник автопарка и водителей";
            ((System.ComponentModel.ISupportInitialize)(this.osagoGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.licensesGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblOSAGO;
        private System.Windows.Forms.DataGridView osagoGrid;
        private System.Windows.Forms.Button btnAddOSAGO;
        private System.Windows.Forms.Button btnEditOSAGO;
        private System.Windows.Forms.Button btnDeleteOSAGO;
        private System.Windows.Forms.Label lblLicenses;
        private System.Windows.Forms.DataGridView licensesGrid;
        private System.Windows.Forms.Button btnAddLicense;
        private System.Windows.Forms.Button btnEditLicense;
        private System.Windows.Forms.Button btnDeleteLicense;
        private System.Windows.Forms.Button btnRefresh;
    }
}