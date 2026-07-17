using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;
using TransportCompany.Core.Services;
using TransportCompany.UI.Controls;
using TransportCompany.UI.Dialogs;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Pages
{
    /// <summary>Анализ зон: помесячные таблицы рейсов по зонам и доли в процентах.</summary>
    public sealed class ZoneAnalysisPage : AppPage
    {
        private readonly DateTimePicker _startDate = new DateTimePicker();
        private readonly DateTimePicker _endDate = new DateTimePicker();
        private readonly FlowLayoutPanel _resultsPanel;
        private ZoneAnalysisResult _lastResult;

        public override string Title => "Анализ зон";

        public ZoneAnalysisPage(AppServices services) : base(services)
        {
            Padding = new Padding(24, 16, 24, 16);

            // ----- Фильтры -----
            var filterCard = new CardPanel { Dock = DockStyle.Top, Height = 76, Padding = new Padding(16, 10, 16, 10) };

            var startLabel = Styler.MutedLabel("Начальная дата");
            startLabel.Location = new Point(16, 8);
            _startDate.Location = new Point(16, 28);
            _startDate.Width = 160;
            _startDate.Format = DateTimePickerFormat.Short;
            _startDate.Font = Fonts.Body;
            _startDate.Value = new DateTime(DateTime.Today.Year, 1, 1);

            var endLabel = Styler.MutedLabel("Конечная дата");
            endLabel.Location = new Point(196, 8);
            _endDate.Location = new Point(196, 28);
            _endDate.Width = 160;
            _endDate.Format = DateTimePickerFormat.Short;
            _endDate.Font = Fonts.Body;

            Button analyze = Styler.PrimaryButton("Анализировать");
            analyze.Location = new Point(380, 26);
            analyze.Click += (s, e) => Analyze();

            Button charts = Styler.SecondaryButton("Графики");
            charts.Location = new Point(516, 26);
            charts.Click += (s, e) => ShowCharts();

            filterCard.Controls.Add(startLabel);
            filterCard.Controls.Add(_startDate);
            filterCard.Controls.Add(endLabel);
            filterCard.Controls.Add(_endDate);
            filterCard.Controls.Add(analyze);
            filterCard.Controls.Add(charts);

            // ----- Результаты -----
            _resultsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 8, 0, 0)
            };

            Controls.Add(_resultsPanel);
            Controls.Add(filterCard);
            _resultsPanel.BringToFront();
        }

        private void Analyze()
        {
            if (_startDate.Value > _endDate.Value)
            {
                UiNotify.Warning("Начальная дата не может быть позже конечной.");
                return;
            }

            if ((_endDate.Value - _startDate.Value).TotalDays > 366)
            {
                UiNotify.Warning("Период не должен превышать 1 года.");
                return;
            }

            try
            {
                var period = new Period(_startDate.Value, _endDate.Value);
                _lastResult = Services.ZoneAnalysis.Analyze(period);

                _resultsPanel.SuspendLayout();
                foreach (Control control in _resultsPanel.Controls.Cast<Control>().ToList())
                {
                    control.Dispose();
                }
                _resultsPanel.Controls.Clear();

                if (_lastResult.Months.Count == 0)
                {
                    _resultsPanel.ResumeLayout();
                    UiNotify.Info("Данные за выбранный период отсутствуют.");
                    return;
                }

                foreach (ZoneMonthStatistics month in _lastResult.Months)
                {
                    _resultsPanel.Controls.Add(BuildMonthCard(month));
                }
                _resultsPanel.ResumeLayout();
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось выполнить анализ зон.", ex);
            }
        }

        private Control BuildMonthCard(ZoneMonthStatistics month)
        {
            string monthName = System.Globalization.CultureInfo.CurrentCulture
                .DateTimeFormat.GetMonthName(month.Month);

            var card = new CardPanel
            {
                Width = Math.Max(600, _resultsPanel.ClientSize.Width - 30),
                Height = 470,
                Margin = new Padding(0, 0, 0, 16),
                Padding = new Padding(16)
            };

            var title = new Label
            {
                Text = $"Статистика по зонам за {monthName} {month.Year}",
                Font = Fonts.CardTitle,
                ForeColor = Palette.Accent,
                AutoSize = true,
                Location = new Point(16, 12)
            };

            var totalLabel = Styler.MutedLabel($"Общее количество рейсов: {month.TotalTrips}");
            totalLabel.Location = new Point(16, 38);

            var grid = new DataGridView
            {
                Location = new Point(16, 62),
                Size = new Size(card.Width - 32, 330),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            Styler.Grid(grid);
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.RowTemplate.Height = 26;
            grid.ColumnHeadersHeight = 32;

            grid.Columns.Add("Zone", "Зона");
            List<string> vehicles = month.TripsByVehicle.Keys.ToList();
            foreach (string vehicle in vehicles)
            {
                grid.Columns.Add(vehicle, vehicle);
            }

            for (int zone = ZoneAnalysisService.MinZone; zone <= ZoneAnalysisService.MaxZone; zone++)
            {
                var values = new List<object> { zone.ToString() };
                foreach (string vehicle in vehicles)
                {
                    month.TripsByVehicle[vehicle].TryGetValue(zone, out int count);
                    values.Add(count.ToString());
                }
                grid.Rows.Add(values.ToArray());
            }
            grid.ClearSelection();

            string percentagesText = string.Join("  |  ",
                Enumerable.Range(ZoneAnalysisService.MinZone, ZoneAnalysisService.MaxZone + 1)
                    .Select(zone =>
                        $"{zone} зона: {month.ZonePercentages[zone]:F1}% ({month.ZoneTotals[zone]} шт)"));

            var percentages = new Label
            {
                Text = percentagesText,
                Font = Fonts.Small,
                ForeColor = Palette.TextSecondary,
                Location = new Point(16, 402),
                Size = new Size(card.Width - 32, 48),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            card.Controls.Add(title);
            card.Controls.Add(totalLabel);
            card.Controls.Add(grid);
            card.Controls.Add(percentages);
            return card;
        }

        private void ShowCharts()
        {
            if (_lastResult == null || _lastResult.Months.Count == 0)
            {
                UiNotify.Warning("Сначала выполните анализ.");
                return;
            }

            using (var dialog = new ZoneChartsDialog(_lastResult))
            {
                dialog.ShowDialog(FindForm());
            }
        }
    }
}
