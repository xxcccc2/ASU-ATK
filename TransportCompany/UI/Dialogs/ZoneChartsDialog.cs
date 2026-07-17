using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TransportCompany.Core.Models;
using TransportCompany.Core.Services;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Dialogs
{
    /// <summary>Графики процентного распределения рейсов по зонам за каждый месяц.</summary>
    public sealed class ZoneChartsDialog : Form
    {
        public ZoneChartsDialog(ZoneAnalysisResult result)
        {
            Text = "Графики по зонам";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(1100, 680);
            MinimumSize = new Size(800, 500);
            BackColor = Palette.PageBack;
            Font = Fonts.Body;
            AutoScaleMode = AutoScaleMode.Font;

            var scroll = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(16)
            };

            foreach (ZoneMonthStatistics month in result.Months)
            {
                string monthName = System.Globalization.CultureInfo.CurrentCulture
                    .DateTimeFormat.GetMonthName(month.Month);

                var chart = new Chart
                {
                    Width = 1020,
                    Height = 300,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, 16)
                };

                var area = new ChartArea("Main") { BackColor = Color.White };
                area.AxisX.Title = "Зона";
                area.AxisY.Title = "Процент рейсов (%)";
                area.AxisX.Interval = 1;
                area.AxisX.MajorGrid.Enabled = false;
                area.AxisY.MajorGrid.LineColor = Palette.GridLine;
                area.AxisX.LabelStyle.Font = Fonts.Small;
                area.AxisY.LabelStyle.Font = Fonts.Small;
                chart.ChartAreas.Add(area);

                var series = new Series
                {
                    ChartType = SeriesChartType.Column,
                    Color = Palette.ChartBlue,
                    IsValueShownAsLabel = true,
                    LabelFormat = "F1",
                    Font = Fonts.Small
                };

                for (int zone = ZoneAnalysisService.MinZone; zone <= ZoneAnalysisService.MaxZone; zone++)
                {
                    series.Points.AddXY(zone, month.ZonePercentages[zone]);
                }

                chart.Series.Add(series);
                chart.Titles.Add(new Title(
                    $"{monthName} {month.Year} — всего {month.TotalTrips} рейсов",
                    Docking.Top, Fonts.CardTitle, Palette.TextPrimary));

                scroll.Controls.Add(chart);
            }

            Controls.Add(scroll);
        }
    }
}
