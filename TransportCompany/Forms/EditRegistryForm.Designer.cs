namespace TransportCompany.Forms
{
    partial class EditRegistryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditRegistryForm));
            this.lblSelectRegistry = new System.Windows.Forms.Label();
            this.cmbRegistries = new System.Windows.Forms.ComboBox();
            this.dgvRegistryData = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();

            this.lblRecordCount = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegistryData)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSelectRegistry
            // 
            this.lblSelectRegistry.AutoSize = true;
            this.lblSelectRegistry.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSelectRegistry.ForeColor = System.Drawing.Color.White;
            this.lblSelectRegistry.Location = new System.Drawing.Point(12, 15);
            this.lblSelectRegistry.Name = "lblSelectRegistry";
            this.lblSelectRegistry.Size = new System.Drawing.Size(159, 23);
            this.lblSelectRegistry.TabIndex = 0;
            this.lblSelectRegistry.Text = "Выберите реестр:";
            // 
            // cmbRegistries
            // 
            this.cmbRegistries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegistries.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cmbRegistries.FormattingEnabled = true;
            this.cmbRegistries.Location = new System.Drawing.Point(152, 12);
            this.cmbRegistries.Name = "cmbRegistries";
            this.cmbRegistries.Size = new System.Drawing.Size(200, 29);
            this.cmbRegistries.TabIndex = 1;
            this.cmbRegistries.SelectedIndexChanged += new System.EventHandler(this.cmbRegistries_SelectedIndexChanged);
            // 
            // dgvRegistryData
            // 
            this.dgvRegistryData.AllowUserToAddRows = false;
            this.dgvRegistryData.AllowUserToDeleteRows = false;
            this.dgvRegistryData.BackgroundColor = System.Drawing.Color.White;
            this.dgvRegistryData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRegistryData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRegistryData.Location = new System.Drawing.Point(0, 50);
            this.dgvRegistryData.Name = "dgvRegistryData";
            this.dgvRegistryData.RowHeadersWidth = 51;
            this.dgvRegistryData.Size = new System.Drawing.Size(1200, 450);
            this.dgvRegistryData.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(12, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 35);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(150, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 35);
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            // 
            // lblRecordCount
            // 
            this.lblRecordCount.AutoSize = true;
            this.lblRecordCount.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblRecordCount.ForeColor = System.Drawing.Color.White;
            this.lblRecordCount.Location = new System.Drawing.Point(370, 15);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(93, 23);
            this.lblRecordCount.TabIndex = 6;
            this.lblRecordCount.Text = "Записей: 0";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.panel1.Controls.Add(this.lblSelectRegistry);
            this.panel1.Controls.Add(this.lblRecordCount);
            this.panel1.Controls.Add(this.cmbRegistries);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 50);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnBack);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 500);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1200, 60);
            this.panel2.TabIndex = 8;
            // 
            // EditRegistryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(89)))), ((int)(((byte)(132)))));
            this.ClientSize = new System.Drawing.Size(1200, 560);
            this.Controls.Add(this.dgvRegistryData);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EditRegistryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование Реестров";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegistryData)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lblSelectRegistry;
        private System.Windows.Forms.ComboBox cmbRegistries;
        private System.Windows.Forms.DataGridView dgvRegistryData;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnBack;

        private System.Windows.Forms.Label lblRecordCount;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}