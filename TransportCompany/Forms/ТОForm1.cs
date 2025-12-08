using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TransportCompany.Forms
{
    public partial class ТОForm1 : Form
    {
        public ТОForm1()
        {
            InitializeComponent();
        }

        private void ТОForm1_Load(object sender, EventArgs e)
        {
            LoadTOData();
            dtpLastTO.Value = DateTime.Today;
        }

        private void LoadTOData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
                {
                    string query = "SELECT * FROM Техобслуживание ORDER BY [Дата последнего ТО] DESC";
                    DataTable table = new DataTable();

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;

                    // Настройка отображения столбцов
                    if (dataGridView1.Columns.Count > 0)
                    {
                        dataGridView1.Columns["id"].Visible = false;
                        dataGridView1.Columns["Номер машины"].HeaderText = "Номер машины";
                        dataGridView1.Columns["Дата последнего ТО"].HeaderText = "Дата ТО";
                        dataGridView1.Columns["Пробег (км)"].HeaderText = "Пробег (км)";
                        dataGridView1.Columns["Комментарий"].HeaderText = "Комментарий";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCarNumber.Text))
            {
                MessageBox.Show("Введите номер машины!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
                {
                    string query = @"INSERT INTO Техобслуживание 
                                   ([Номер машины], [Дата последнего ТО], [Пробег (км)], [Комментарий]) 
                                   VALUES (@CarNumber, @Date, @Mileage, @Comment)";

                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CarNumber", txtCarNumber.Text);
                        cmd.Parameters.AddWithValue("@Date", dtpLastTO.Value);
                        cmd.Parameters.AddWithValue("@Mileage", (int)nudMileage.Value);
                        cmd.Parameters.AddWithValue("@Comment", txtComment.Text);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Запись успешно добавлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtCarNumber.Clear();
                    nudMileage.Value = 0;
                    txtComment.Clear();
                    LoadTOData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTOData();
        }

        private void txtCarNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите запись для удаления!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
            string carNumber = dataGridView1.CurrentRow.Cells["Номер машины"].Value.ToString();
            string toDate = dataGridView1.CurrentRow.Cells["Дата последнего ТО"].Value.ToString();

            if (MessageBox.Show($"Удалить запись ТО для машины {carNumber} от {toDate}?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
                    {
                        string query = "DELETE FROM Техобслуживание WHERE id = @Id";

                        connection.Open();

                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Запись успешно удалена!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadTOData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении записи: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void CheckOverdueTO()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
                {
                    string query = @"
                        SELECT [Номер машины], [Дата последнего ТО] 
                        FROM Техобслуживание 
                        WHERE DATEADD(MONTH, 3, [Дата последнего ТО]) < GETDATE()";

                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                string message = "Следующие машины требуют ТО:\n\n";
                                while (reader.Read())
                                {
                                    string carNumber = reader["Номер машины"].ToString();
                                    DateTime lastTO = Convert.ToDateTime(reader["Дата последнего ТО"]);
                                    message += $"{carNumber} (последнее ТО: {lastTO:dd.MM.yyyy})\n";
                                }

                                MessageBox.Show(message, "Внимание! Требуется ТО",
                                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке ТО: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}