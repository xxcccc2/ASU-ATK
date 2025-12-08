using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace TransportCompany
{
    public partial class DriverPayrollForm : Form
    {
        private string driverFIO;
        private DateTime startDate;
        private DateTime endDate;
        private DataGridView dataGridViewPayroll;
        private Label lblTotalEarnings;

        public DriverPayrollForm(string fio, DateTime start, DateTime end)
        {
            driverFIO = fio;
            startDate = start;
            endDate = end;
            InitializeComponent();
            SetupControls();
            LoadPayrollData();
        }

        private void SetupControls()
        {
            this.Text = $"Расчетный листок - {driverFIO}";
            this.BackColor = Color.FromArgb(46, 89, 132);
            this.ForeColor = Color.White;
            this.ClientSize = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            dataGridViewPayroll = new DataGridView
            {
                Location = new Point(12, 12),
                Size = new Size(560, 300),
                BackgroundColor = Color.FromArgb(46, 89, 132),
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                ColumnHeadersDefaultCellStyle = { BackColor = Color.FromArgb(30, 60, 90), ForeColor = Color.White }
            };
            dataGridViewPayroll.Columns.Add("Zone", "Зона");
            dataGridViewPayroll.Columns.Add("Shifts", "Количество смен");
            dataGridViewPayroll.Columns.Add("Rate", "Тариф (руб.)");
            dataGridViewPayroll.Columns.Add("Cost", "Стоимость (руб.)");
            dataGridViewPayroll.Columns["Zone"].Width = 100;
            dataGridViewPayroll.Columns["Shifts"].Width = 120;
            dataGridViewPayroll.Columns["Rate"].Width = 120;
            dataGridViewPayroll.Columns["Cost"].Width = 120;

            lblTotalEarnings = new Label
            {
                AutoSize = true,
                Location = new Point(12, 320),
                Text = "Итого заработок: -",
                ForeColor = Color.White
            };

            Button btnBack = new Button
            {
                BackColor = Color.FromArgb(46, 89, 132),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Location = new Point(500, 350),
                Name = "btnBack",
                Size = new Size(75, 23),
                TabIndex = 29,
                Text = "Назад",
                UseVisualStyleBackColor = false
            };
            btnBack.Click += (s, e) => this.Close();

            this.Controls.Add(dataGridViewPayroll);
            this.Controls.Add(lblTotalEarnings);
            this.Controls.Add(btnBack);
        }

        private void LoadPayrollData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT Zone, COUNT(*) as ShiftCount
                        FROM TransportRegistry 
                        WHERE FIO = @FIO 
                        AND Date BETWEEN @StartDate AND @EndDate
                        AND Zone IS NOT NULL
                        GROUP BY Zone
                        ORDER BY Zone";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FIO", driverFIO);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            decimal totalEarnings = 0;
                            dataGridViewPayroll.Rows.Clear();

                            while (reader.Read())
                            {
                                int zone = reader.GetInt32(0);
                                int shiftCount = reader.GetInt32(1);
                                decimal rate = CalculateSalaryForZone(zone);
                                decimal cost = rate * shiftCount;

                                dataGridViewPayroll.Rows.Add(zone, shiftCount, rate.ToString("F2"), cost.ToString("F2"));
                                totalEarnings += cost;
                            }

                            lblTotalEarnings.Text = $"Итого заработок: {totalEarnings:F2} руб.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}