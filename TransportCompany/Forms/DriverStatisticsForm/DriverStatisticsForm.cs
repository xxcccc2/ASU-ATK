using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace TransportCompany
{
    public partial class DriverStatisticsForm : Form
    {
        private Label lblMaxSalary, lblMinSalary, lblAvgSalary;
        private Label lblMaxTrips, lblMinTrips, lblAvgTrips;
        private Label lblTotalTrips, lblTotalSalary;
        private Label lblAvgRevenueWithVAT, lblAvgRevenueWithoutVAT;
        private Label lblMostFrequentZone, lblLeastFrequentZone;

        public DriverStatisticsForm()
        {
            InitializeComponent();
            SetupStatisticsLabels();
            LoadDrivers();
            SetDefaultPeriod();
            btnPayroll.Click += new EventHandler(btnPayroll_Click);
        }

        private void SetupStatisticsLabels()
        {
            int yPosition = 160;

            lblMaxSalary = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, yPosition),
                Text = "Максимальный заработок: -",
                ForeColor = System.Drawing.Color.White
            };
            this.Controls.Add(lblMaxSalary);
            yPosition += 25;

            lblMinSalary = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, yPosition),
                Text = "Минимальный заработок: -",
                ForeColor = System.Drawing.Color.White
            };
            this.Controls.Add(lblMinSalary);
            yPosition += 25;

            lblAvgSalary = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, yPosition),
                Text = "Средний заработок: -",
                ForeColor = System.Drawing.Color.White
            };
            this.Controls.Add(lblAvgSalary);
            yPosition += 25;

            lblMaxTrips = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, yPosition),
                Text = "Максимальное количество рейсов: -",
                ForeColor = System.Drawing.Color.White
            };
            this.Controls.Add(lblMaxTrips);
            yPosition += 25;

            lblMinTrips = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, yPosition),
                Text = "Минимальное количество рейсов: -",
                ForeColor = System.Drawing.Color.White
            };
            this.Controls.Add(lblMinTrips);
            yPosition += 25;

            lblAvgTrips = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, yPosition),
                Text = "Среднее количество рейсов: -",
                ForeColor = System.Drawing.Color.White
            };
            this.Controls.Add(lblAvgTrips);
            yPosition += 25;

            lblTotalTrips = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, yPosition),
                Text = "Общее количество рейсов: -",
                ForeColor = System.Drawing.Color.White
            };
            this.Controls.Add(lblTotalTrips);
            yPosition += 25;

            lblTotalSalary = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, yPosition),
                Text = "Общий заработок: -",
                ForeColor = System.Drawing.Color.White
            };
            this.Controls.Add(lblTotalSalary);
            yPosition += 25;

            lblAvgRevenueWithVAT = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, yPosition),
                Text = "Средняя выручка с НДС за рейс: -",
                ForeColor = System.Drawing.Color.White
            };
            this.Controls.Add(lblAvgRevenueWithVAT);
            yPosition += 25;

            lblAvgRevenueWithoutVAT = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, yPosition),
                Text = "Средняя выручка без НДС за рейс: -",
                ForeColor = System.Drawing.Color.White
            };
            this.Controls.Add(lblAvgRevenueWithoutVAT);
            yPosition += 25;

            lblMostFrequentZone = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, yPosition),
                Text = "Самая частая зона доставки: -",
                ForeColor = System.Drawing.Color.White
            };
            this.Controls.Add(lblMostFrequentZone);
            yPosition += 25;

            lblLeastFrequentZone = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, yPosition),
                Text = "Самая редкая зона доставки: -",
                ForeColor = System.Drawing.Color.White
            };
            this.Controls.Add(lblLeastFrequentZone);
        }

        private void LoadDrivers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT FIO FROM TransportRegistry WHERE FIO IS NOT NULL ORDER BY FIO";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string fio = reader["FIO"].ToString().Trim();
                                comboBoxDriverFIO.Items.Add(fio);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки списка водителей: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetDefaultPeriod()
        {
            DateTime today = DateTime.Today;
            dateTimePickerStart.Value = new DateTime(today.Year, today.Month, 1);
            dateTimePickerEnd.Value = today;
            radioButtonCurrentMonth.Checked = true;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (comboBoxDriverFIO.SelectedItem == null)
            {
                MessageBox.Show("Выберите водителя для анализа!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dateTimePickerStart.Value > dateTimePickerEnd.Value)
            {
                MessageBox.Show("Начальная дата не может быть позже конечной!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedDriver = comboBoxDriverFIO.SelectedItem.ToString();
            DateTime startDate = dateTimePickerStart.Value;
            DateTime endDate = dateTimePickerEnd.Value;

            CalculateAndDisplayStatistics(selectedDriver, startDate, endDate);
        }

        private void CalculateAndDisplayStatistics(string driverFIO, DateTime startDate, DateTime endDate)
        {
            using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
            {
                try
                {
                    connection.Open();
                    string queryTrips = @"
                        SELECT Date, Zone, TotalWithVAT, TotalWithoutVAT 
                        FROM TransportRegistry 
                        WHERE FIO = @FIO 
                        AND Date BETWEEN @StartDate AND @EndDate
                        AND Zone IS NOT NULL";
                    using (SqlCommand command = new SqlCommand(queryTrips, connection))
                    {
                        command.Parameters.AddWithValue("@FIO", driverFIO);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);

                        var tripsByMonth = new Dictionary<(int Year, int Month), List<(int Zone, decimal WithVAT, decimal WithoutVAT)>>();
                        var zones = new List<int>();
                        decimal totalWithVAT = 0, totalWithoutVAT = 0;
                        int totalTrips = 0;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime tripDate = reader.GetDateTime(0);
                                int zone = reader.GetInt32(1);
                                decimal withVAT = reader["TotalWithVAT"] != DBNull.Value ? reader.GetDecimal(2) : 0;
                                decimal withoutVAT = reader["TotalWithoutVAT"] != DBNull.Value ? reader.GetDecimal(3) : 0;

                                var monthKey = (Year: tripDate.Year, Month: tripDate.Month);
                                if (!tripsByMonth.ContainsKey(monthKey))
                                {
                                    tripsByMonth[monthKey] = new List<(int, decimal, decimal)>();
                                }
                                tripsByMonth[monthKey].Add((zone, withVAT, withoutVAT));
                                zones.Add(zone);
                                totalWithVAT += withVAT;
                                totalWithoutVAT += withoutVAT;
                                totalTrips++;
                            }
                        }

                        if (!tripsByMonth.Any())
                        {
                            MessageBox.Show("Нет данных за выбранный период.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearStatistics();
                            return;
                        }

                        var monthlySalaries = new List<decimal>();
                        decimal totalSalary = 0;

                        foreach (var month in tripsByMonth)
                        {
                            decimal monthSalary = month.Value.Sum(trip => CalculateSalaryForZone(trip.Zone));
                            monthlySalaries.Add(monthSalary);
                            totalSalary += monthSalary;
                        }

                        decimal maxSalary = monthlySalaries.Max();
                        decimal minSalary = monthlySalaries.Min();
                        decimal avgSalary = monthlySalaries.Average();

                        var monthlyTrips = tripsByMonth.Select(m => m.Value.Count).ToList();
                        int maxTrips = monthlyTrips.Max();
                        int minTrips = monthlyTrips.Min();
                        double avgTrips = monthlyTrips.Average();

                        decimal avgRevenueWithVAT = totalTrips > 0 ? totalWithVAT / totalTrips : 0;
                        decimal avgRevenueWithoutVAT = totalTrips > 0 ? totalWithoutVAT / totalTrips : 0;

                        string mostFrequentZone = "Нет данных", leastFrequentZone = "Нет данных";
                        if (zones.Any())
                        {
                            var zoneGroups = zones.GroupBy(z => z)
                                                  .OrderByDescending(g => g.Count())
                                                  .ThenBy(g => g.Key)
                                                  .ToList();
                            int maxZoneCount = zoneGroups.First().Count();
                            int minZoneCount = zoneGroups.Last().Count();

                            var mostFrequentZones = zoneGroups.Where(g => g.Count() == maxZoneCount)
                                                              .Select(g => g.Key.ToString());
                            mostFrequentZone = string.Join(", ", mostFrequentZones);

                            var leastFrequentZones = zoneGroups.Where(g => g.Count() == minZoneCount)
                                                               .Select(g => g.Key.ToString());
                            leastFrequentZone = string.Join(", ", leastFrequentZones);
                        }

                        lblMaxSalary.Text = $"Максимальный заработок: {maxSalary:F2} руб.";
                        lblMinSalary.Text = $"Минимальный заработок: {minSalary:F2} руб.";
                        lblAvgSalary.Text = $"Средний заработок: {avgSalary:F2} руб.";
                        lblMaxTrips.Text = $"Максимальное количество рейсов: {maxTrips}";
                        lblMinTrips.Text = $"Минимальное количество рейсов: {minTrips}";
                        lblAvgTrips.Text = $"Среднее количество рейсов: {avgTrips:F1}";
                        lblTotalTrips.Text = $"Общее количество рейсов: {totalTrips}";
                        lblTotalSalary.Text = $"Общий заработок: {totalSalary:F2} руб.";
                        lblAvgRevenueWithVAT.Text = $"Средняя выручка с НДС за рейс: {avgRevenueWithVAT:F2} руб.";
                        lblAvgRevenueWithoutVAT.Text = $"Средняя выручка без НДС за рейс: {avgRevenueWithoutVAT:F2} руб.";
                        lblMostFrequentZone.Text = $"Самая частая зона доставки: {mostFrequentZone}";
                        lblLeastFrequentZone.Text = $"Самая редкая зона доставки: {leastFrequentZone}";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка расчета статистики: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ClearStatistics();
                }
            }
        }

        private void ClearStatistics()
        {
            lblMaxSalary.Text = "Максимальный заработок: -";
            lblMinSalary.Text = "Минимальный заработок: -";
            lblAvgSalary.Text = "Средний заработок: -";
            lblMaxTrips.Text = "Максимальное количество рейсов: -";
            lblMinTrips.Text = "Минимальное количество рейсов: -";
            lblAvgTrips.Text = "Среднее количество рейсов: -";
            lblTotalTrips.Text = "Общее количество рейсов: -";
            lblTotalSalary.Text = "Общий заработок: -";
            lblAvgRevenueWithVAT.Text = "Средняя выручка с НДС за рейс: -";
            lblAvgRevenueWithoutVAT.Text = "Средняя выручка без НДС за рейс: -";
            lblMostFrequentZone.Text = "Самая частая зона доставки: -";
            lblLeastFrequentZone.Text = "Самая редкая зона доставки: -";
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

        private void radioButtonYear_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonYear.Checked)
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPayroll_Click(object sender, EventArgs e)
        {
            if (comboBoxDriverFIO.SelectedItem == null)
            {
                MessageBox.Show("Выберите водителя для расчета!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dateTimePickerStart.Value > dateTimePickerEnd.Value)
            {
                MessageBox.Show("Начальная дата не может быть позже конечной!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedDriver = comboBoxDriverFIO.SelectedItem.ToString();
            DateTime startDate = dateTimePickerStart.Value;
            DateTime endDate = dateTimePickerEnd.Value;

            DriverPayrollForm payrollForm = new DriverPayrollForm(selectedDriver, startDate, endDate);
            payrollForm.FormClosed += (s, args) => this.Show();
            payrollForm.Show();
            this.Hide();
        }
    }
}