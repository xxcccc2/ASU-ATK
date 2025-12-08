using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace TransportCompany
{
    public partial class DriverSalaryForm : Form
    {
        private DataGridView dgvSalaries; // Таблица для отображения зарплат

        public DriverSalaryForm()
        {
            InitializeComponent();
            SetDefaultPeriod();
            SetupDataGridView(); // Инициализируем таблицу
        }

        private void SetupDataGridView()
        {
            dgvSalaries = new DataGridView
            {
                Location = new System.Drawing.Point(12, 195),
                Size = new System.Drawing.Size(528, 200),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = System.Drawing.Color.FromArgb(46, 89, 132),
                ForeColor = System.Drawing.Color.White,
                ColumnHeadersDefaultCellStyle = { BackColor = System.Drawing.Color.FromArgb(46, 89, 132), ForeColor = System.Drawing.Color.White },
                DefaultCellStyle = { BackColor = System.Drawing.Color.FromArgb(46, 89, 132), ForeColor = System.Drawing.Color.White }
            };
            this.Controls.Add(dgvSalaries);
        }

        private void SetDefaultPeriod()
        {
            DateTime today = DateTime.Today;
            dateTimePickerStart.Value = new DateTime(today.Year, today.Month, 1);
            dateTimePickerEnd.Value = today;
            radioButtonCurrentMonth.Checked = true;
        }

        private void radioButtonCurrentMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCurrentMonth.Checked)
            {
                DateTime today = DateTime.Today;
                dateTimePickerStart.Value = new DateTime(today.Year, today.Month, 1);
                dateTimePickerEnd.Value = today;
                dateTimePickerStart.Enabled = false;
                dateTimePickerEnd.Enabled = false;
            }
        }

        private void radioButtonCurrentYear_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCurrentYear.Checked)
            {
                DateTime today = DateTime.Today;
                dateTimePickerStart.Value = new DateTime(today.Year, 1, 1);
                dateTimePickerEnd.Value = today;
                dateTimePickerStart.Enabled = false;
                dateTimePickerEnd.Enabled = false;
            }
        }

        private void radioButtonCustomPeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCustomPeriod.Checked)
            {
                dateTimePickerStart.Enabled = true;
                dateTimePickerEnd.Enabled = true;
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (dateTimePickerStart.Value > dateTimePickerEnd.Value)
            {
                MessageBox.Show("Начальная дата не может быть позже конечной!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime startDate = dateTimePickerStart.Value;
            DateTime endDate = dateTimePickerEnd.Value;

            CalculateAndDisplaySalaries(startDate, endDate);
        }

        private void CalculateAndDisplaySalaries(DateTime startDate, DateTime endDate)
        {
            using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                    SELECT FIO, Zone 
                    FROM TransportRegistry 
                    WHERE Date BETWEEN @StartDate AND @EndDate
                    AND FIO IS NOT NULL AND LEN(FIO) > 2 AND FIO NOT LIKE '%[0-9]%'
                    AND FIO NOT LIKE '%[/\\]%'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);

                        // Словарь для хранения зарплаты каждого водителя
                        var driverSalaries = new System.Collections.Generic.Dictionary<string, decimal>();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string fio = reader["FIO"].ToString().Trim();
                                int zone = reader.GetInt32(1);

                                if (!driverSalaries.ContainsKey(fio))
                                {
                                    driverSalaries[fio] = 0;
                                }

                                driverSalaries[fio] += CalculateSalaryForZone(zone);
                            }
                        }

                        // Настраиваем DataGridView
                        dgvSalaries.Columns.Clear();
                        dgvSalaries.Columns.Add("DriverColumn", "Водитель");
                        dgvSalaries.Columns.Add("SalaryColumn", "Зарплата (руб.)");

                        // Заполняем таблицу
                        foreach (var driver in driverSalaries.OrderBy(d => d.Key))
                        {
                            dgvSalaries.Rows.Add(driver.Key, driver.Value);
                        }

                        // Если нет данных
                        if (driverSalaries.Count == 0)
                        {
                            MessageBox.Show("Данные за выбранный период отсутствуют.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        lblResult.Text = $"Зарплата водителей за период с {startDate.ToShortDateString()} по {endDate.ToShortDateString()}";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка расчета зарплаты: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private decimal CalculateSalaryForZone(int zone)
        {
            switch (zone)
            {
                case 0:
                case 1: return 2700;
                case 2: return 3200;
                case 3: return 3600;
                case 4: return 4000;
                case 5: return 5000;
                case 6: return 6000;
                case 7: return 7000;
                case 8: return 8000;
                case 9: return 9000;
                case 10: return 10000;
                default: return 0;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Close();
        }
    }

}