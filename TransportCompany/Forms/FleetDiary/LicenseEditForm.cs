using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TransportCompany
{
    public partial class LicenseEditForm : Form
    {
        private int? licenseId;

        public LicenseEditForm(int? licenseId)
        {
            this.licenseId = licenseId;
            InitializeComponent();
            if (licenseId.HasValue)
            {
                Text = "Редактировать удостоверение";
                LoadData();
            }
            else
            {
                Text = "Добавить удостоверение";
            }
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(DB.ConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT DriverFullName, LicenseNumber, IssueDate, ExpiryDate FROM DriverLicenses WHERE LicenseId = @LicenseId", cn);
                    cmd.Parameters.AddWithValue("@LicenseId", licenseId.Value);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtDriverName.Text = reader["DriverFullName"].ToString();
                        txtLicenseNumber.Text = reader["LicenseNumber"].ToString();
                        dtpIssueDate.Value = Convert.ToDateTime(reader["IssueDate"]);
                        dtpExpiryDate.Value = Convert.ToDateTime(reader["ExpiryDate"]);
                    }
                    reader.Close();
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDriverName.Text) || string.IsNullOrWhiteSpace(txtLicenseNumber.Text))
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(DB.ConnectionString))
                {
                    cn.Open();

                    // Проверяем, существует ли водитель в таблице Drivers
                    SqlCommand checkDriverCmd = new SqlCommand("SELECT COUNT(*) FROM Drivers WHERE FullName = @FullName", cn);
                    checkDriverCmd.Parameters.AddWithValue("@FullName", txtDriverName.Text.Trim());
                    int driverExists = (int)checkDriverCmd.ExecuteScalar();

                    // Если водитель не существует, добавляем его
                    if (driverExists == 0)
                    {
                        SqlCommand insertDriverCmd = new SqlCommand("INSERT INTO Drivers (FullName) VALUES (@FullName)", cn);
                        insertDriverCmd.Parameters.AddWithValue("@FullName", txtDriverName.Text.Trim());
                        insertDriverCmd.ExecuteNonQuery();
                    }

                    // Сохраняем данные водительского удостоверения
                    string query = licenseId.HasValue
                        ? "UPDATE DriverLicenses SET DriverFullName = @DriverName, LicenseNumber = @LicenseNumber, IssueDate = @IssueDate, ExpiryDate = @ExpiryDate WHERE LicenseId = @LicenseId"
                        : "INSERT INTO DriverLicenses (DriverFullName, LicenseNumber, IssueDate, ExpiryDate) VALUES (@DriverName, @LicenseNumber, @IssueDate, @ExpiryDate)";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@DriverName", txtDriverName.Text.Trim());
                        cmd.Parameters.AddWithValue("@LicenseNumber", txtLicenseNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@IssueDate", dtpIssueDate.Value);
                        cmd.Parameters.AddWithValue("@ExpiryDate", dtpExpiryDate.Value);
                        if (licenseId.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@LicenseId", licenseId.Value);
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}