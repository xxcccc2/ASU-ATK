using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TransportCompany.Core;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;
using TransportCompany.UI.Controls;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Pages
{
    /// <summary>Сравнение двух объектов (ТС или водителей) по 4 показателям.</summary>
    public sealed class ComparisonPage : AppPage
    {
        private readonly ComboBox _object1 = new ComboBox();
        private readonly ComboBox _object2 = new ComboBox();
        private readonly PeriodSelector _period = new PeriodSelector();
        private readonly Chart _chartTrips = new Chart();
        private readonly Chart _chartSalary = new Chart();
        private readonly Chart _chartWithoutVat = new Chart();
        private readonly Chart _chartWithVat = new Chart();
        private bool _objectsLoaded;

        public override string Title => "Сравнение ТС и водителей";

        public ComparisonPage(AppServices services) : base(services)
        {
            Padding = new Padding(24, 16, 24, 16);

            // ----- Фильтры -----
            var filterCard = new CardPanel { Dock = DockStyle.Top, Height = 150, Padding = new Padding(16, 10, 16, 10) };

            var label1 = Styler.MutedLabel("Первый объект");
            label1.Location = new Point(16, 10);
            _object1.Location = new Point(16, 30);
            _object1.Width = 240;
            _object1.DropDownStyle = ComboBoxStyle.DropDownList;
            Styler.Input(_object1);

            var label2 = Styler.MutedLabel("Второй объект");
            label2.Location = new Point(16, 64);
            _object2.Location = new Point(16, 84);
            _object2.Width = 240;
            _object2.DropDownStyle = ComboBoxStyle.DropDownList;
            Styler.Input(_object2);

            var periodLabel = Styler.MutedLabel("Период");
            periodLabel.Location = new Point(300, 10);
            _period.Location = new Point(300, 30);

            Button compare = Styler.PrimaryButton("Сравнить");
            compare.Location = new Point(880, 60);
            compare.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            compare.Click += (s, e) => Compare();

            filterCard.Controls.Add(label1);
            filterCard.Controls.Add(_object1);
            filterCard.Controls.Add(label2);
            filterCard.Controls.Add(_object2);
            filterCard.Controls.Add(periodLabel);
            filterCard.Controls.Add(_period);
            filterCard.Controls.Add(compare);

            // ----- Диаграммы 2x2 -----
            var chartsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 8, 0, 0)
            };
            chartsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            chartsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            chartsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            chartsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            chartsLayout.Controls.Add(WrapChart(_chartTrips, "Количество рейсов"), 0, 0);
            chartsLayout.Controls.Add(WrapChart(_chartSalary, "Зарплата (руб.)"), 1, 0);
            chartsLayout.Controls.Add(WrapChart(_chartWithoutVat, "Выручка без НДС (руб.)"), 0, 1);
            chartsLayout.Controls.Add(WrapChart(_chartWithVat, "Выручка с НДС (руб.)"), 1, 1);

            Controls.Add(chartsLayout);
            Controls.Add(filterCard);
            chartsLayout.BringToFront();
        }

        private static Control WrapChart(Chart chart, string title)
        {
            var card = new CardPanel { Dock = DockStyle.Fill, Margin = new Padding(6), Padding = new Padding(8) };

            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.White;
            var area = new ChartArea("Main")
            {
                BackColor = Color.White
            };
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Palette.GridLine;
            area.AxisX.LabelStyle.Font = Fonts.Small;
            area.AxisY.LabelStyle.Font = Fonts.Small;
            area.AxisX.LineColor = Palette.Border;
            area.AxisY.LineColor = Palette.Border;
            chart.ChartAreas.Add(area);

            chart.Titles.Add(new Title(title, Docking.Top, Fonts.CardTitle, Palette.TextPrimary));

            card.Controls.Add(chart);
            return card;
        }

        public override void OnActivated()
        {
            if (_objectsLoaded)
            {
                return;
            }

            try
            {
                _object1.Items.Clear();
                _object2.Items.Clear();

                foreach (string number in Services.Registry.GetVehicleNumbers())
                {
                    _object1.Items.Add("ТС: " + number);
                    _object2.Items.Add("ТС: " + number);
                }
                foreach (string driver in Services.Registry.GetDriverNames())
                {
                    _object1.Items.Add("Водитель: " + driver);
                    _object2.Items.Add("Водитель: " + driver);
                }

                _objectsLoaded = true;
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось загрузить объекты для сравнения.", ex);
            }
        }

        private void Compare()
        {
            if (_object1.SelectedItem == null || _object2.SelectedItem == null)
            {
                UiNotify.Warning("Выберите два объекта для сравнения.");
                return;
            }

            string first = _object1.SelectedItem.ToString();
            string second = _object2.SelectedItem.ToString();
            if (first == second)
            {
                UiNotify.Warning("Нельзя сравнивать одинаковые объекты.");
                return;
            }

            Period period;
            try
            {
                period = _period.GetPeriod();
            }
            catch (ArgumentException ex)
            {
                UiNotify.Warning(ex.Message);
                return;
            }

            try
            {
                ComparisonData data1 = GetData(first, period);
                ComparisonData data2 = GetData(second, period);

                FillChart(_chartTrips, data1, data2, d => d.TripCount);
                FillChart(_chartSalary, data1, data2, d => (double)d.Salary);
                FillChart(_chartWithoutVat, data1, data2, d => (double)d.RevenueWithoutVat);
                FillChart(_chartWithVat, data1, data2, d => (double)d.RevenueWithVat);
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось выполнить сравнение.", ex);
            }
        }

        private ComparisonData GetData(string selection, Period period)
        {
            bool isVehicle = selection.StartsWith("ТС:");
            string identifier = selection.Substring(selection.IndexOf(':') + 1).Trim();
            return Services.Earnings.GetComparisonData(isVehicle, identifier, period);
        }

        private static void FillChart(Chart chart, ComparisonData data1, ComparisonData data2,
            Func<ComparisonData, double> valueSelector)
        {
            chart.Series.Clear();
            chart.Legends.Clear();

            var legend = new Legend
            {
                Docking = Docking.Bottom,
                Font = Fonts.Small,
                ForeColor = Palette.TextSecondary
            };
            chart.Legends.Add(legend);

            var series1 = new Series(Shorten(data1.DisplayName))
            {
                ChartType = SeriesChartType.Column,
                Color = Palette.ChartBlue,
                IsValueShownAsLabel = true,
                LabelFormat = "#,##0.##",
                Font = Fonts.Small,
                ["PointWidth"] = "0.5"
            };
            series1.Points.AddXY("", valueSelector(data1));

            var series2 = new Series(Shorten(data2.DisplayName))
            {
                ChartType = SeriesChartType.Column,
                Color = Palette.ChartGreen,
                IsValueShownAsLabel = true,
                LabelFormat = "#,##0.##",
                Font = Fonts.Small,
                ["PointWidth"] = "0.5"
            };
            series2.Points.AddXY("", valueSelector(data2));

            chart.Series.Add(series1);
            chart.Series.Add(series2);
        }

        private static string Shorten(string name) =>
            name.Length > 30 ? name.Substring(0, 27) + "..." : name;
    }
}
