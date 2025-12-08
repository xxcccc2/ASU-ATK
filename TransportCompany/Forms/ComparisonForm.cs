using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace TransportCompany
{
    public partial class ComparisonForm : Form
    {
        private string logFilePath = "ComparisonLog.txt";

        public ComparisonForm()
        {
            InitializeComponent();
            LoadObjects();
            SetupDateFilters();
        }

        private void ComparisonForm_Load(object sender, EventArgs e)
        {
            radioButtonCurrentMonth.Checked = true;
        }

        private void LoadObjects()
        {
            try
            {
                LogMessage("Начало загрузки объектов для сравнения.");
                using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    LogMessage("Подключение к базе данных открыто.");

                    // Загрузка госномеров
                    string queryVehicles = "SELECT DISTINCT GosNumber FROM TransportRegistry WHERE GosNumber IS NOT NULL ORDER BY GosNumber";
                    using (SqlCommand command = new SqlCommand(queryVehicles, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string gosNumber = reader["GosNumber"].ToString();
                                comboBoxObject1.Items.Add($"ТС: {gosNumber}");
                                comboBoxObject2.Items.Add($"ТС: {gosNumber}");
                            }
                        }
                    }

                    // Загрузка ФИО водителей
                    string queryDrivers = "SELECT DISTINCT FIO FROM TransportRegistry WHERE FIO IS NOT NULL ORDER BY FIO";
                    using (SqlCommand command = new SqlCommand(queryDrivers, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string fio = reader["FIO"].ToString();
                                comboBoxObject1.Items.Add($"Водитель: {fio}");
                                comboBoxObject2.Items.Add($"Водитель: {fio}");
                            }
                        }
                    }
                }
                LogMessage("Объекты для сравнения успешно загружены.");
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка загрузки объектов: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Ошибка загрузки объектов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDateFilters()
        {
            try
            {
                LogMessage("Установка фильтров дат.");
                dateTimePickerStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dateTimePickerEnd.Value = DateTime.Now;
                LogMessage("Фильтры дат установлены.");
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка установки фильтров дат: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Ошибка установки фильтров дат: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            try
            {
                LogMessage("Начало сравнения объектов.");

                // Проверка выбора объектов
                if (comboBoxObject1.SelectedItem == null || comboBoxObject2.SelectedItem == null)
                {
                    MessageBox.Show("Выберите два объекта для сравнения.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string object1 = comboBoxObject1.SelectedItem.ToString();
                string object2 = comboBoxObject2.SelectedItem.ToString();

                // Проверка на одинаковые объекты
                if (object1 == object2)
                {
                    MessageBox.Show("Нельзя сравнивать одинаковые объекты.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Определяем тип сравнения
                bool isObject1Vehicle = object1.StartsWith("ТС:");
                bool isObject2Vehicle = object2.StartsWith("ТС:");
                string identifier1 = object1.Substring(object1.IndexOf(":") + 2).Trim();
                string identifier2 = object2.Substring(object2.IndexOf(":") + 2).Trim();

                // Определяем период
                DateTime startDate, endDate;
                if (radioButtonCurrentMonth.Checked)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    endDate = DateTime.Now;
                }
                else if (radioButtonYear.Checked)
                {
                    startDate = new DateTime(DateTime.Now.Year, 1, 1);
                    endDate = DateTime.Now;
                }
                else
                {
                    startDate = dateTimePickerStart.Value;
                    endDate = dateTimePickerEnd.Value;
                }

                // Получаем данные для первого объекта
                var data1 = GetComparisonData(isObject1Vehicle, identifier1, startDate, endDate);
                var data2 = GetComparisonData(isObject2Vehicle, identifier2, startDate, endDate);

                // Очищаем диаграммы
                chartTrips.Series.Clear();
                chartRevenueWithVAT.Series.Clear();
                chartRevenueWithoutVAT.Series.Clear();
                chartSalary.Series.Clear();

                // Настраиваем диаграммы
                // Преобразуем int в decimal для количества рейсов
                SetupChart(chartTrips, "Количество рейсов", new decimal[] { (decimal)data1.Item1, (decimal)data2.Item1 }, new[] { object1, object2 });
                SetupChart(chartRevenueWithVAT, "Выручка с НДС", new[] { data1.Item2, data2.Item2 }, new[] { object1, object2 });
                SetupChart(chartRevenueWithoutVAT, "Выручка без НДС", new[] { data1.Item3, data2.Item3 }, new[] { object1, object2 });
                SetupChart(chartSalary, "Зарплата", new[] { data1.Item4, data2.Item4 }, new[] { object1, object2 });

                LogMessage("Сравнение успешно выполнено.");
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка при сравнении: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Ошибка при сравнении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private (int, decimal, decimal, decimal) GetComparisonData(bool isVehicle, string identifier, DateTime startDate, DateTime endDate)
        {
            int trips = 0;
            decimal revenueWithVAT = 0;
            decimal revenueWithoutVAT = 0;
            decimal salary = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    string query;
                    if (isVehicle)
                    {
                        query = @"
                            SELECT 
                                COUNT(*) AS 'Количество рейсов',
                                SUM(TotalWithVAT) AS 'Сумма с НДС',
                                SUM(TotalWithoutVAT) AS 'Сумма без НДС',
                                SUM(CASE Zone
                                    WHEN 0 THEN 2700
                                    WHEN 1 THEN 2700
                                    WHEN 2 THEN 3200
                                    WHEN 3 THEN 3600
                                    WHEN 4 THEN 4000
                                    WHEN 5 THEN 5000
                                    WHEN 6 THEN 6000
                                    WHEN 7 THEN 7000
                                    WHEN 8 THEN 8000
                                    WHEN 9 THEN 9000
                                    WHEN 10 THEN 10000
                                    ELSE 0 END) AS 'Зарплата'
                            FROM TransportRegistry
                            WHERE GosNumber = @Identifier
                                AND [Date] BETWEEN @StartDate AND @EndDate";
                    }
                    else
                    {
                        query = @"
                            SELECT 
                                COUNT(*) AS 'Количество рейсов',
                                SUM(TotalWithVAT) AS 'Сумма с НДС',
                                SUM(TotalWithoutVAT) AS 'Сумма без НДС',
                                SUM(CASE Zone
                                    WHEN 0 THEN 2700
                                    WHEN 1 THEN 2700
                                    WHEN 2 THEN 3200
                                    WHEN 3 THEN 3600
                                    WHEN 4 THEN 4000
                                    WHEN 5 THEN 5000
                                    WHEN 6 THEN 6000
                                    WHEN 7 THEN 7000
                                    WHEN 8 THEN 8000
                                    WHEN 9 THEN 9000
                                    WHEN 10 THEN 10000
                                    ELSE 0 END) AS 'Зарплата'
                            FROM TransportRegistry
                            WHERE FIO = @Identifier
                                AND [Date] BETWEEN @StartDate AND @EndDate";
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Identifier", identifier);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                trips = reader["Количество рейсов"] != DBNull.Value ? Convert.ToInt32(reader["Количество рейсов"]) : 0;
                                revenueWithVAT = reader["Сумма с НДС"] != DBNull.Value ? Convert.ToDecimal(reader["Сумма с НДС"]) : 0;
                                revenueWithoutVAT = reader["Сумма без НДС"] != DBNull.Value ? Convert.ToDecimal(reader["Сумма без НДС"]) : 0;
                                salary = reader["Зарплата"] != DBNull.Value ? Convert.ToDecimal(reader["Зарплата"]) : 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка получения данных для сравнения: {ex.Message}\n{ex.StackTrace}");
                throw;
            }

            return (trips, revenueWithVAT, revenueWithoutVAT, salary);
        }

        private void SetupChart(Chart chart, string title, decimal[] values, string[] labels)
        {
            chart.Series.Clear();
            Series series = new Series
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                LabelFormat = "#,##0.##",
                ["PointWidth"] = "0.3" // Уменьшаем ширину столбцов, чтобы дать больше места меткам
            };
            chart.Series.Add(series);

            // Форматируем метки с переносом строки для водителей
            string[] formattedLabels = new string[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                string label = labels[i];
                if (label.StartsWith("Водитель:"))
                {
                    // Разделяем "Водитель:" и ФИО на две строки
                    string name = label.Substring("Водитель:".Length).Trim();
                    formattedLabels[i] = $"Водитель:\n{name}";
                }
                else
                {
                    // Для ТС оставляем как есть, но добавляем перенос, если строка слишком длинная
                    if (label.Length > 15)
                    {
                        int splitIndex = label.IndexOf(" ", "ТС:".Length);
                        if (splitIndex > 0)
                        {
                            string part1 = label.Substring(0, splitIndex);
                            string part2 = label.Substring(splitIndex + 1);
                            formattedLabels[i] = $"{part1}\n{part2}";
                        }
                        else
                        {
                            formattedLabels[i] = label;
                        }
                    }
                    else
                    {
                        formattedLabels[i] = label;
                    }
                }

                // Добавляем точку данных
                series.Points.AddXY(formattedLabels[i], values[i]);
                DataPoint point = series.Points[i];
                point.LabelAngle = 0;
                point.LabelForeColor = Color.Black;
                point.Font = new Font("Arial", 10, FontStyle.Bold);
                point.SetCustomProperty("LabelStyle", "Top");
            }

            // Настраиваем оси
            chart.ChartAreas[0].AxisX.Title = "Объекты";
            chart.ChartAreas[0].AxisY.Title = title;
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.LabelStyle.Angle = 0; // Убираем поворот меток
            chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 9, FontStyle.Regular); // Уменьшаем шрифт для лучшей читаемости
            chart.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false;
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;

            // Включаем перенос строк для меток и увеличиваем пространство
            chart.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = true;
            chart.ChartAreas[0].AxisX.LabelStyle.TruncatedLabels = false; // Отключаем обрезку меток
            chart.ChartAreas[0].AxisX.IsLabelAutoFit = false; // Отключаем автоподгонку
            chart.ChartAreas[0].InnerPlotPosition = new ElementPosition(15, 10, 75, 70); // Увеличиваем нижний отступ для меток
            chart.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Auto;

            chart.Legends.Clear();

            chart.Titles.Clear();
            chart.Titles.Add(new Title
            {
                Text = title,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Alignment = ContentAlignment.TopCenter
            });

            // Логируем создание диаграммы
            LogMessage($"Диаграмма '{title}' успешно настроена.");
        }

        private void radioButtonCurrentMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCurrentMonth.Checked)
            {
                dateTimePickerStart.Enabled = false;
                dateTimePickerEnd.Enabled = false;
            }
        }

        private void radioButtonYear_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonYear.Checked)
            {
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
            try
            {
                MainForm mainForm = new MainForm();
                mainForm.FormClosed += (s, args) =>
                {
                    this.Close();
                };
                mainForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка при возврате на главную форму: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Ошибка при возврате на главную форму: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
            }
        }

        private void LogMessage(string message)
        {
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(logFilePath, true))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка записи в лог: {ex.Message}", "Ошибка логирования", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}