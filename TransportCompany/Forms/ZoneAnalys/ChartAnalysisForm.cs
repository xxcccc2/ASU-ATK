using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TransportCompany
{
    public partial class ChartAnalysisForm : Form
    {
        private Dictionary<(int Year, int Month), Dictionary<int, double>> percentages;
        private Dictionary<(int Year, int Month), int> totalTripsPerMonth;
        private Dictionary<(int Year, int Month), Dictionary<int, int>> zoneCounts;
        private DateTime startDate;
        private DateTime endDate;

        public ChartAnalysisForm(
            Dictionary<(int Year, int Month), Dictionary<int, double>> percentages,
            Dictionary<(int Year, int Month), int> totalTripsPerMonth,
            Dictionary<(int Year, int Month), Dictionary<int, int>> zoneCounts,
            DateTime startDate,
            DateTime endDate)
        {
            InitializeComponent();
            this.percentages = percentages;
            this.totalTripsPerMonth = totalTripsPerMonth;
            this.zoneCounts = zoneCounts;
            this.startDate = startDate;
            this.endDate = endDate;

            SetupCharts();
        }

        private void SetupCharts()
        {
            int yPosition = 12;
            var months = percentages.Keys
                .Where(m => m.Year > startDate.Year || (m.Year == startDate.Year && m.Month >= startDate.Month))
                .Where(m => m.Year < endDate.Year || (m.Year == endDate.Year && m.Month <= endDate.Month))
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month)
                .ToList();

            foreach (var monthKey in months)
            {
                string monthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthKey.Month);

                // Заголовок графика
                Label monthHeader = new Label
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(12, yPosition),
                    Text = $"Процентное соотношение рейсов по зонам за {monthName} {monthKey.Year}",
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold)
                };
                this.Controls.Add(monthHeader);
                yPosition += 30;

                // Создаем Chart
                Chart chart = new Chart
                {
                    Location = new System.Drawing.Point(12, yPosition),
                    Size = new System.Drawing.Size(1200, 300)
                };

                ChartArea chartArea = new ChartArea("MainArea");
                chart.ChartAreas.Add(chartArea);

                Series series = new Series("Процент рейсов")
                {
                    ChartType = SeriesChartType.Column
                };

                for (int zone = 0; zone <= 10; zone++)
                {
                    double percentage = percentages[monthKey][zone];
                    series.Points.AddXY(zone, percentage);
                    series.Points.Last().AxisLabel = $"Зона {zone}";
                }

                chart.Series.Add(series);
                chart.Titles.Add($"Рейсы за {monthName} {monthKey.Year} (Всего: {totalTripsPerMonth[monthKey]} рейсов)");
                chartArea.AxisY.Title = "Процент рейсов (%)";
                chartArea.AxisX.Title = "Зона";
                chartArea.AxisX.Interval = 1;

                this.Controls.Add(chart);
                yPosition += 320;
            }

            // Устанавливаем размер формы с учетом всех графиков
            this.ClientSize = new System.Drawing.Size(1280, yPosition + 20);
        }
    }
}