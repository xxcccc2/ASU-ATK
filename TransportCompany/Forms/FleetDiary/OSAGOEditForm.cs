using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TransportCompany
{
    public partial class OSAGOEditForm : Form
    {
        private int? osagoId;

        public OSAGOEditForm(int? osagoId)
        {
            this.osagoId = osagoId;
            InitializeComponent();
            if (osagoId.HasValue)
            {
                Text = "Редактировать ОСАГО";
                LoadData();
            }
            else
            {
                Text = "Добавить ОСАГО";
            }
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(DB.ConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT VehicleRegistrationNumber, PolicyNumber, StartDate, EndDate FROM OSAGO WHERE OSAGOId = @OSAGOId", cn);
                    cmd.Parameters.AddWithValue("@OSAGOId", osagoId.Value);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtVehicleReg.Text = reader["VehicleRegistrationNumber"].ToString();
                        txtPolicyNumber.Text = reader["PolicyNumber"].ToString();
                        dtpStartDate.Value = Convert.ToDateTime(reader["StartDate"]);
                        dtpEndDate.Value = Convert.ToDateTime(reader["EndDate"]);
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
            if (string.IsNullOrWhiteSpace(txtVehicleReg.Text) || string.IsNullOrWhiteSpace(txtPolicyNumber.Text))
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(DB.ConnectionString))
                {
                    cn.Open();

                    // Проверяем, существует ли ТС в таблице Vehicles
                    SqlCommand checkVehicleCmd = new SqlCommand("SELECT COUNT(*) FROM Vehicles WHERE RegistrationNumber = @RegistrationNumber", cn);
                    checkVehicleCmd.Parameters.AddWithValue("@RegistrationNumber", txtVehicleReg.Text.Trim());
                    int vehicleExists = (int)checkVehicleCmd.ExecuteScalar();

                    // Если ТС не существует, добавляем его
                    if (vehicleExists == 0)
                    {
                        SqlCommand insertVehicleCmd = new SqlCommand("INSERT INTO Vehicles (RegistrationNumber) VALUES (@RegistrationNumber)", cn);
                        insertVehicleCmd.Parameters.AddWithValue("@RegistrationNumber", txtVehicleReg.Text.Trim());
                        insertVehicleCmd.ExecuteNonQuery();
                    }

                    // Сохраняем данные ОСАГО
                    string query = osagoId.HasValue
                        ? "UPDATE OSAGO SET VehicleRegistrationNumber = @VehicleReg, PolicyNumber = @Policy, StartDate = @StartDate, EndDate = @EndDate WHERE OSAGOId = @OSAGOId"
                        : "INSERT INTO OSAGO (VehicleRegistrationNumber, PolicyNumber, StartDate, EndDate) VALUES (@VehicleReg, @Policy, @StartDate, @EndDate)";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@VehicleReg", txtVehicleReg.Text.Trim());
                        cmd.Parameters.AddWithValue("@Policy", txtPolicyNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
                        cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);
                        if (osagoId.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@OSAGOId", osagoId.Value);
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