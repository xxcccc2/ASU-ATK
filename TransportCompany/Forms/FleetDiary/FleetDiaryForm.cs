using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace TransportCompany
{
    public partial class FleetDiaryForm : Form
    {
        public FleetDiaryForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            DataSet ds = new DataSet();
            DB.LoadData("SELECT OSAGOId, VehicleRegistrationNumber, PolicyNumber, StartDate, EndDate FROM OSAGO", ref ds, "OSAGO");
            osagoGrid.DataSource = ds.Tables["OSAGO"];
            osagoGrid.Columns["OSAGOId"].Visible = false;

            ds = new DataSet();
            DB.LoadData("SELECT LicenseId, DriverFullName, LicenseNumber, IssueDate, ExpiryDate FROM DriverLicenses", ref ds, "DriverLicenses");
            licensesGrid.DataSource = ds.Tables["DriverLicenses"];
            licensesGrid.Columns["LicenseId"].Visible = false;

            SetupColumnHeaders();
            HighlightExpiringRows();
        }

        private void SetupColumnHeaders()
        {
            // Настройка заголовков для таблицы ОСАГО
            if (osagoGrid.Columns["VehicleRegistrationNumber"] != null)
                osagoGrid.Columns["VehicleRegistrationNumber"].HeaderText = "Гос. номер ТС";
            if (osagoGrid.Columns["PolicyNumber"] != null)
                osagoGrid.Columns["PolicyNumber"].HeaderText = "Номер полиса";
            if (osagoGrid.Columns["StartDate"] != null)
                osagoGrid.Columns["StartDate"].HeaderText = "Дата начала";
            if (osagoGrid.Columns["EndDate"] != null)
                osagoGrid.Columns["EndDate"].HeaderText = "Дата окончания";

            // Настройка заголовков для таблицы водительских удостоверений
            if (licensesGrid.Columns["DriverFullName"] != null)
                licensesGrid.Columns["DriverFullName"].HeaderText = "ФИО водителя";
            if (licensesGrid.Columns["LicenseNumber"] != null)
                licensesGrid.Columns["LicenseNumber"].HeaderText = "Номер удостоверения";
            if (licensesGrid.Columns["IssueDate"] != null)
                licensesGrid.Columns["IssueDate"].HeaderText = "Дата выдачи";
            if (licensesGrid.Columns["ExpiryDate"] != null)
                licensesGrid.Columns["ExpiryDate"].HeaderText = "Дата истечения";
        }

        private void HighlightExpiringRows()
        {
            DateTime threshold = DateTime.Now.AddDays(30);
            foreach (DataGridViewRow row in osagoGrid.Rows)
            {
                DateTime endDate = Convert.ToDateTime(row.Cells["EndDate"].Value);
                if (endDate <= threshold && endDate >= DateTime.Now)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
            foreach (DataGridViewRow row in licensesGrid.Rows)
            {
                DateTime expiryDate = Convert.ToDateTime(row.Cells["ExpiryDate"].Value);
                if (expiryDate <= threshold && expiryDate >= DateTime.Now)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }

        private void CheckExpirations()
        {
            DataTable osagoExpiring = DB.GetExpiringOSAGO();
            DataTable licensesExpiring = DB.GetExpiringLicenses();
            string message = "";
            if (osagoExpiring.Rows.Count > 0)
            {
                message += "Истекают сроки ОСАГО:\n";
                foreach (DataRow row in osagoExpiring.Rows)
                {
                    message += $"- Автомобиль: {row["VehicleRegistrationNumber"]}, Полис: {row["PolicyNumber"]}, Истекает: {row["EndDate"]:dd.MM.yyyy}\n";
                }
            }
            if (licensesExpiring.Rows.Count > 0)
            {
                message += "\nИстекают сроки водительских удостоверений:\n";
                foreach (DataRow row in licensesExpiring.Rows)
                {
                    message += $"- Водитель: {row["DriverFullName"]}, Удостоверение: {row["LicenseNumber"]}, Истекает: {row["ExpiryDate"]:dd.MM.yyyy}\n";
                }
            }
            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "Уведомление об истекающих сроках", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAddOSAGO_Click(object sender, EventArgs e)
        {
            using (var form = new OSAGOEditForm(null))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void btnEditOSAGO_Click(object sender, EventArgs e)
        {
            if (osagoGrid.SelectedRows.Count > 0)
            {
                int osagoId = Convert.ToInt32(osagoGrid.SelectedRows[0].Cells["OSAGOId"].Value);
                using (var form = new OSAGOEditForm(osagoId))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDeleteOSAGO_Click(object sender, EventArgs e)
        {
            if (osagoGrid.SelectedRows.Count > 0)
            {
                int osagoId = Convert.ToInt32(osagoGrid.SelectedRows[0].Cells["OSAGOId"].Value);
                if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DB.Query("DELETE FROM OSAGO WHERE OSAGOId = @OSAGOId", new SqlParameter("@OSAGOId", osagoId));
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAddLicense_Click(object sender, EventArgs e)
        {
            using (var form = new LicenseEditForm(null))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void btnEditLicense_Click(object sender, EventArgs e)
        {
            if (licensesGrid.SelectedRows.Count > 0)
            {
                int licenseId = Convert.ToInt32(licensesGrid.SelectedRows[0].Cells["LicenseId"].Value);
                using (var form = new LicenseEditForm(licenseId))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDeleteLicense_Click(object sender, EventArgs e)
        {
            if (licensesGrid.SelectedRows.Count > 0)
            {
                int licenseId = Convert.ToInt32(licensesGrid.SelectedRows[0].Cells["LicenseId"].Value);
                if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DB.Query("DELETE FROM DriverLicenses WHERE LicenseId = @LicenseId", new SqlParameter("@LicenseId", licenseId));
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}