using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;
using TransportCompany.UI.Controls;
using TransportCompany.UI.Dialogs;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Pages
{
    /// <summary>
    /// Расчёт заработной платы водителей за период.
    /// Двойной клик по строке — расчётный листок, кнопка — статистика водителя.
    /// </summary>
    public sealed class SalaryPage : AppPage
    {
        private readonly PeriodSelector _period = new PeriodSelector();
        private readonly DataGridView _grid = new DataGridView();
        private readonly Label _resultLabel;
        private readonly Label _totalLabel;
        private readonly Label _countLabel;
        private Period _lastCalculatedPeriod;

        public override string Title => "Расчёт заработной платы водителей";

        public SalaryPage(AppServices services) : base(services)
        {
            Padding = new Padding(24, 16, 24, 16);

            // ----- Параметры расчёта -----
            var filterCard = new CardPanel { Dock = DockStyle.Top, Height = 132, Padding = new Padding(16, 10, 16, 10) };

            var periodTitle = Styler.MutedLabel("Период расчёта");
            periodTitle.Location = new Point(16, 8);

            _period.Location = new Point(16, 30);

            Button calculate = Styler.PrimaryButton("Рассчитать");
            Button reset = Styler.SecondaryButton("Сбросить");
            Button statistics = Styler.SecondaryButton("Статистика водителя");
            calculate.Location = new Point(620, 30);
            reset.Location = new Point(724, 30);
            statistics.Location = new Point(620, 72);
            calculate.Click += (s, e) => Calculate();
            reset.Click += (s, e) => ResetResults();
            statistics.Click += (s, e) => ShowStatistics();

            filterCard.Controls.Add(periodTitle);
            filterCard.Controls.Add(_period);
            filterCard.Controls.Add(calculate);
            filterCard.Controls.Add(reset);
            filterCard.Controls.Add(statistics);

            // ----- Результат -----
            _resultLabel = Styler.BodyLabel("Выберите период и нажмите «Рассчитать»");
            var resultBar = new Panel { Dock = DockStyle.Top, Height = 36, BackColor = Color.Transparent };
            _resultLabel.Location = new Point(0, 12);
            resultBar.Controls.Add(_resultLabel);

            var card = new CardPanel { Dock = DockStyle.Fill, Padding = new Padding(1) };
            Styler.Grid(_grid);
            _grid.Dock = DockStyle.Fill;
            _grid.ReadOnly = true;
            _grid.Columns.Add("Driver", "Водитель");
            _grid.Columns.Add("Trips", "Рейсов");
            _grid.Columns.Add("Salary", "Зарплата (руб.)");
            _grid.Columns["Trips"].FillWeight = 40;
            _grid.Columns["Salary"].FillWeight = 60;
            _grid.Columns["Salary"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _grid.Columns["Salary"].DefaultCellStyle.Padding = new Padding(0, 0, 12, 0);
            _grid.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) ShowPayslip(); };
            card.Controls.Add(_grid);

            // ----- Итоги -----
            var statusBar = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.Transparent };
            _countLabel = Styler.MutedLabel("");
            _countLabel.Location = new Point(0, 14);
            _totalLabel = new Label
            {
                Font = Fonts.TotalValue,
                ForeColor = Palette.Accent,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            statusBar.Controls.Add(_countLabel);
            statusBar.Controls.Add(_totalLabel);
            statusBar.Resize += (s, e) =>
                _totalLabel.Location = new Point(statusBar.Width - _totalLabel.Width, 10);
            _totalLabel.SizeChanged += (s, e) =>
                _totalLabel.Location = new Point(statusBar.Width - _totalLabel.Width, 10);

            Controls.Add(card);
            Controls.Add(resultBar);
            Controls.Add(statusBar);
            Controls.Add(filterCard);
            card.BringToFront();
        }

        private void Calculate()
        {
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
                List<DriverSalaryRow> salaries = Services.Payroll.CalculateSalaries(period);
                _lastCalculatedPeriod = period;

                _grid.Rows.Clear();
                decimal total = 0;
                foreach (DriverSalaryRow salary in salaries)
                {
                    _grid.Rows.Add(salary.DriverFullName, salary.TripCount, salary.Salary.ToString("N0"));
                    total += salary.Salary;
                }
                _grid.ClearSelection();

                _resultLabel.Text =
                    $"Зарплата водителей за период с {period.Start:dd.MM.yyyy} по {period.End:dd.MM.yyyy}";
                _countLabel.Text = $"Всего записей: {salaries.Count}";
                _totalLabel.Text = $"Итого: {total:N0} ₽";

                if (salaries.Count == 0)
                {
                    UiNotify.Info("Данные за выбранный период отсутствуют.");
                }
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось рассчитать зарплату.", ex);
            }
        }

        private void ResetResults()
        {
            _grid.Rows.Clear();
            _resultLabel.Text = "Выберите период и нажмите «Рассчитать»";
            _totalLabel.Text = "";
            _countLabel.Text = "";
            _lastCalculatedPeriod = null;
        }

        private string SelectedDriver =>
            _grid.CurrentRow?.Cells["Driver"].Value?.ToString();

        private void ShowPayslip()
        {
            string driver = SelectedDriver;
            if (driver == null || _lastCalculatedPeriod == null)
            {
                return;
            }

            using (var dialog = new DriverPayrollDialog(Services, driver, _lastCalculatedPeriod))
            {
                dialog.ShowDialog(FindForm());
            }
        }

        private void ShowStatistics()
        {
            string driver = SelectedDriver;
            if (driver == null || _lastCalculatedPeriod == null)
            {
                UiNotify.Warning("Сначала выполните расчёт и выберите водителя в таблице.");
                return;
            }

            using (var dialog = new DriverStatisticsDialog(Services, driver, _lastCalculatedPeriod))
            {
                dialog.ShowDialog(FindForm());
            }
        }
    }
}
