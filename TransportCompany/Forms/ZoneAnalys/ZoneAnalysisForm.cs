using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace TransportCompany
{
    public partial class ZoneAnalysisForm : Form
    {
        private List<DataGridView> monthTables;
        private List<Label> monthHeaders;
        private List<Label> totalTripsLabels;
        private List<Label> percentageLabels;

        public ZoneAnalysisForm()
        {
            InitializeComponent();
            monthTables = new List<DataGridView>();
            monthHeaders = new List<Label>();
            totalTripsLabels = new List<Label>();
            percentageLabels = new List<Label>();
            SetDefaultPeriod();
        }

        private void SetDefaultPeriod()
        {
            DateTime today = DateTime.Today;
            dateTimePickerStart.Value = new DateTime(today.Year, 1, 1);
            dateTimePickerEnd.Value = today;
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (dateTimePickerStart.Value > dateTimePickerEnd.Value)
            {
                MessageBox.Show("Начальная дата не может быть позже конечной!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ((dateTimePickerEnd.Value - dateTimePickerStart.Value).TotalDays > 366)
            {
                MessageBox.Show("Период не должен превышать 1 года!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AnalyzeZones();
        }

        private void AnalyzeZones()
        {
            DateTime startDate = dateTimePickerStart.Value;
            DateTime endDate = dateTimePickerEnd.Value;

            foreach (var table in monthTables)
            {
                this.Controls.Remove(table);
                table.Dispose();
            }
            foreach (var header in monthHeaders)
            {
                this.Controls.Remove(header);
                header.Dispose();
            }
            foreach (var label in totalTripsLabels)
            {
                this.Controls.Remove(label);
                label.Dispose();
            }
            foreach (var label in percentageLabels)
            {
                this.Controls.Remove(label);
                label.Dispose();
            }
            monthTables.Clear();
            monthHeaders.Clear();
            totalTripsLabels.Clear();
            percentageLabels.Clear();

            var zoneData = LoadZoneData(startDate, endDate);
            if (zoneData == null || !zoneData.Any())
            {
                MessageBox.Show("Данные за выбранный период отсутствуют.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SetupMonthTables(zoneData, startDate, endDate);
        }

        private Dictionary<(int Year, int Month), Dictionary<string, Dictionary<int, int>>> LoadZoneData(DateTime startDate, DateTime endDate)
        {
            var zoneData = new Dictionary<(int Year, int Month), Dictionary<string, Dictionary<int, int>>>();
            var totalTripsPerMonth = new Dictionary<(int Year, int Month), int>();

            using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT GosNumber, Zone, YEAR(Date) AS TripYear, MONTH(Date) AS TripMonth
                        FROM TransportRegistry 
                        WHERE Date BETWEEN @StartDate AND @EndDate
                        AND GosNumber IS NOT NULL AND GosNumber <> ''
                        AND Zone IS NOT NULL";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string gosNumber = reader["GosNumber"].ToString().Trim();
                                int zone = reader.GetInt32(reader.GetOrdinal("Zone"));
                                int year = reader.GetInt32(reader.GetOrdinal("TripYear"));
                                int month = reader.GetInt32(reader.GetOrdinal("TripMonth"));

                                var monthKey = (Year: year, Month: month);

                                if (!zoneData.ContainsKey(monthKey))
                                {
                                    zoneData[monthKey] = new Dictionary<string, Dictionary<int, int>>();
                                    totalTripsPerMonth[monthKey] = 0;
                                }

                                if (!zoneData[monthKey].ContainsKey(gosNumber))
                                {
                                    zoneData[monthKey][gosNumber] = new Dictionary<int, int>();
                                }

                                if (!zoneData[monthKey][gosNumber].ContainsKey(zone))
                                {
                                    zoneData[monthKey][gosNumber][zone] = 0;
                                }

                                zoneData[monthKey][gosNumber][zone]++;
                                totalTripsPerMonth[monthKey]++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            foreach (var monthKey in zoneData.Keys.ToList())
            {
                foreach (var gosNumber in zoneData[monthKey].Keys.ToList())
                {
                    for (int zone = 0; zone <= 10; zone++)
                    {
                        if (!zoneData[monthKey][gosNumber].ContainsKey(zone))
                        {
                            zoneData[monthKey][gosNumber][zone] = 0;
                        }
                    }
                }
            }

            var percentages = new Dictionary<(int Year, int Month), Dictionary<int, double>>();
            var zoneCounts = new Dictionary<(int Year, int Month), Dictionary<int, int>>(); // Исправлено: DictionaryView -> Dictionary<int, int>
            foreach (var monthKey in zoneData.Keys)
            {
                int totalTrips = totalTripsPerMonth[monthKey];
                var zoneTotals = new Dictionary<int, int>();

                for (int zone = 0; zone <= 10; zone++)
                {
                    zoneTotals[zone] = 0;
                    foreach (var gosNumber in zoneData[monthKey].Keys)
                    {
                        zoneTotals[zone] += zoneData[monthKey][gosNumber][zone];
                    }
                }

                percentages[monthKey] = new Dictionary<int, double>();
                zoneCounts[monthKey] = zoneTotals;
                for (int zone = 0; zone <= 10; zone++)
                {
                    double percentage = totalTrips > 0 ? (zoneTotals[zone] * 100.0 / totalTrips) : 0;
                    percentages[monthKey][zone] = percentage;
                }
            }

            this.Tag = new { Percentages = percentages, TotalTripsPerMonth = totalTripsPerMonth, ZoneCounts = zoneCounts };
            return zoneData;
        }

        private void SetupMonthTables(Dictionary<(int Year, int Month), Dictionary<string, Dictionary<int, int>>> zoneData, DateTime startDate, DateTime endDate)
        {
            var percentages = (this.Tag as dynamic).Percentages as Dictionary<(int Year, int Month), Dictionary<int, double>>;
            var totalTripsPerMonth = (this.Tag as dynamic).TotalTripsPerMonth as Dictionary<(int Year, int Month), int>;
            var zoneCounts = (this.Tag as dynamic).ZoneCounts as Dictionary<(int Year, int Month), Dictionary<int, int>>;
            var months = zoneData.Keys
                .Where(m => m.Year > startDate.Year || (m.Year == startDate.Year && m.Month >= startDate.Month))
                .Where(m => m.Year < endDate.Year || (m.Year == endDate.Year && m.Month <= endDate.Month))
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month)
                .ToList();

            int yPosition = 50;

            foreach (var (year, month) in months)
            {
                var monthKey = (Year: year, Month: month);
                string monthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

                Label monthHeader = new Label
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(12, yPosition - 20),
                    Text = $"Статистика по зонам за {monthName} {year}",
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold)
                };
                this.Controls.Add(monthHeader);
                monthHeaders.Add(monthHeader);

                int totalTrips = totalTripsPerMonth[monthKey];
                Label totalTripsLabel = new Label
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(12, yPosition),
                    Text = $"Общее количество рейсов: {totalTrips}"
                };
                this.Controls.Add(totalTripsLabel);
                totalTripsLabels.Add(totalTripsLabel);

                DataGridView dgv = new DataGridView
                {
                    Location = new System.Drawing.Point(12, yPosition + 20),
                    Size = new System.Drawing.Size(1200, 300),
                    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                    AllowUserToAddRows = false,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Horizontal
                };

                dgv.Columns.Add("ZoneColumn", "Зона");
                var gosNumbers = zoneData[monthKey].Keys.OrderBy(g => g).ToList();
                foreach (var gosNumber in gosNumbers)
                {
                    dgv.Columns.Add(gosNumber, gosNumber);
                }

                for (int zone = 0; zone <= 10; zone++)
                {
                    int rowIndex = dgv.Rows.Add();
                    dgv.Rows[rowIndex].Cells[0].Value = zone.ToString();
                    int colIndex = 1;
                    foreach (var gosNumber in gosNumbers)
                    {
                        int count = zoneData[monthKey][gosNumber][zone];
                        dgv.Rows[rowIndex].Cells[colIndex].Value = count.ToString();
                        colIndex++;
                    }
                }

                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgv.RowHeadersWidth = 50;

                this.Controls.Add(dgv);
                monthTables.Add(dgv);

                string percentageText = string.Join(" | ", Enumerable.Range(0, 11)
                    .Select(zone => $"{zone} зона: {percentages[monthKey][zone]:F1}% ({zoneCounts[monthKey][zone]} шт)"));
                Label lblPercentages = new Label
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(12, yPosition + 320),
                    Text = percentageText,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 8, System.Drawing.FontStyle.Bold)
                };
                this.Controls.Add(lblPercentages);
                percentageLabels.Add(lblPercentages);

                yPosition += 420;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnShowCharts_Click(object sender, EventArgs e)
        {
            var zoneData = LoadZoneData(dateTimePickerStart.Value, dateTimePickerEnd.Value);
            if (zoneData == null || !zoneData.Any())
            {
                MessageBox.Show("Нет данных для отображения графиков.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var percentages = (this.Tag as dynamic).Percentages as Dictionary<(int Year, int Month), Dictionary<int, double>>;
            var totalTripsPerMonth = (this.Tag as dynamic).TotalTripsPerMonth as Dictionary<(int Year, int Month), int>;
            var zoneCounts = (this.Tag as dynamic).ZoneCounts as Dictionary<(int Year, int Month), Dictionary<int, int>>;

            ChartAnalysisForm chartForm = new ChartAnalysisForm(percentages, totalTripsPerMonth, zoneCounts, dateTimePickerStart.Value, dateTimePickerEnd.Value);
            chartForm.ShowDialog();
        }
    }
}