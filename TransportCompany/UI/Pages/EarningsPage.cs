using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TransportCompany.Core;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;
using TransportCompany.UI.Controls;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Pages
{
    /// <summary>Заработок ТС и водителей: фильтры, таблица, экспорт в Excel.</summary>
    public sealed class EarningsPage : AppPage
    {
        private readonly ComboBox _vehicleCombo = new ComboBox();
        private readonly ComboBox _driverCombo = new ComboBox();
        private readonly PeriodSelector _period = new PeriodSelector();
        private readonly DataGridView _grid = new DataGridView();
        private bool _referencesLoaded;
        private bool _suppressEvents;

        public override string Title => "Заработок ТС и водителей";

        public EarningsPage(AppServices services) : base(services)
        {
            Padding = new Padding(24, 16, 24, 16);

            // ----- Фильтры -----
            var filterCard = new CardPanel { Dock = DockStyle.Top, Height = 150, Padding = new Padding(16, 10, 16, 10) };

            var vehicleLabel = Styler.MutedLabel("Госномер ТС");
            vehicleLabel.Location = new Point(16, 10);
            _vehicleCombo.Location = new Point(16, 30);
            _vehicleCombo.Width = 220;
            _vehicleCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            Styler.Input(_vehicleCombo);
            _vehicleCombo.SelectedIndexChanged += OnVehicleSelected;

            var driverLabel = Styler.MutedLabel("ФИО водителя");
            driverLabel.Location = new Point(16, 64);
            _driverCombo.Location = new Point(16, 84);
            _driverCombo.Width = 220;
            _driverCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            Styler.Input(_driverCombo);
            _driverCombo.SelectedIndexChanged += OnDriverSelected;

            var periodLabel = Styler.MutedLabel("Период");
            periodLabel.Location = new Point(280, 10);
            _period.Location = new Point(280, 30);
            _period.PeriodChanged += (s, e) => UpdateEarnings();

            filterCard.Controls.Add(vehicleLabel);
            filterCard.Controls.Add(_vehicleCombo);
            filterCard.Controls.Add(driverLabel);
            filterCard.Controls.Add(_driverCombo);
            filterCard.Controls.Add(periodLabel);
            filterCard.Controls.Add(_period);

            // ----- Таблица -----
            var card = new CardPanel { Dock = DockStyle.Fill, Padding = new Padding(1) };
            Styler.Grid(_grid);
            _grid.Dock = DockStyle.Fill;
            _grid.ReadOnly = true;
            _grid.Columns.Add("Vehicle", "Госномер ТС");
            _grid.Columns.Add("Driver", "ФИО водителя");
            _grid.Columns.Add("Trips", "Количество рейсов");
            _grid.Columns.Add("WithoutVat", "Сумма без НДС (руб.)");
            _grid.Columns.Add("WithVat", "Сумма с НДС (руб.)");
            _grid.Columns.Add("Salary", "Зарплата (руб.)");
            card.Controls.Add(_grid);

            // ----- Низ: экспорт -----
            var bottomBar = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Bottom,
                Height = 48,
                Padding = new Padding(0, 8, 0, 0),
                BackColor = Color.Transparent
            };
            Button export = Styler.SecondaryButton("Экспорт в Excel");
            export.Click += (s, e) => ExportToExcel();
            bottomBar.Controls.Add(export);

            Controls.Add(card);
            Controls.Add(bottomBar);
            Controls.Add(filterCard);
            card.BringToFront();
        }

        public override void OnActivated()
        {
            if (_referencesLoaded)
            {
                return;
            }

            try
            {
                _suppressEvents = true;
                _vehicleCombo.Items.Clear();
                _vehicleCombo.Items.Add("");
                foreach (string number in Services.Registry.GetVehicleNumbers())
                {
                    _vehicleCombo.Items.Add(number);
                }

                _driverCombo.Items.Clear();
                _driverCombo.Items.Add("");
                foreach (string driver in Services.Registry.GetDriverNames())
                {
                    _driverCombo.Items.Add(driver);
                }
                _suppressEvents = false;
                _referencesLoaded = true;
            }
            catch (DataAccessException ex)
            {
                _suppressEvents = false;
                UiNotify.Error("Не удалось загрузить списки ТС и водителей.", ex);
            }
        }

        private void OnVehicleSelected(object sender, EventArgs e)
        {
            if (_suppressEvents)
            {
                return;
            }

            if (_vehicleCombo.SelectedIndex > 0)
            {
                _suppressEvents = true;
                _driverCombo.SelectedIndex = 0;
                _suppressEvents = false;
            }
            UpdateEarnings();
        }

        private void OnDriverSelected(object sender, EventArgs e)
        {
            if (_suppressEvents)
            {
                return;
            }

            if (_driverCombo.SelectedIndex > 0)
            {
                _suppressEvents = true;
                _vehicleCombo.SelectedIndex = 0;
                _suppressEvents = false;
            }
            UpdateEarnings();
        }

        private void UpdateEarnings()
        {
            string vehicle = _vehicleCombo.SelectedIndex > 0 ? _vehicleCombo.SelectedItem.ToString() : null;
            string driver = _driverCombo.SelectedIndex > 0 ? _driverCombo.SelectedItem.ToString() : null;

            _grid.Rows.Clear();
            if (vehicle == null && driver == null)
            {
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
                EarningsRow row = vehicle != null
                    ? Services.Earnings.GetVehicleEarnings(vehicle, period)
                    : Services.Earnings.GetDriverEarnings(driver, period);

                if (row == null)
                {
                    return;
                }

                int rowIndex = _grid.Rows.Add(
                    row.VehicleNumber, row.DriverFullName, row.TripCount,
                    row.TotalWithoutVat.ToString("N2"), row.TotalWithVat.ToString("N2"),
                    row.Salary.ToString("N2"));

                DataGridViewRow gridRow = _grid.Rows[rowIndex];
                gridRow.Cells[0].Style.ForeColor = Palette.Accent;
                gridRow.Cells[1].Style.ForeColor = Palette.Accent;
                gridRow.Cells[5].Style.Font = Fonts.BodyBold;
                _grid.ClearSelection();
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось обновить данные о заработке.", ex);
            }
        }

        private void ExportToExcel()
        {
            if (_grid.Rows.Count == 0)
            {
                UiNotify.Warning("Нет данных для экспорта. Выберите ТС или водителя.");
                return;
            }

            using (var dialog = new SaveFileDialog
            {
                Filter = "Excel файлы|*.xlsx",
                FileName = $"Заработок_{DateTime.Today:yyyy-MM-dd}.xlsx"
            })
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    var workbook = new XSSFWorkbook();
                    ISheet sheet = workbook.CreateSheet("Заработок");

                    IRow header = sheet.CreateRow(0);
                    for (int c = 0; c < _grid.Columns.Count; c++)
                    {
                        header.CreateCell(c).SetCellValue(_grid.Columns[c].HeaderText);
                    }

                    for (int r = 0; r < _grid.Rows.Count; r++)
                    {
                        IRow row = sheet.CreateRow(r + 1);
                        for (int c = 0; c < _grid.Columns.Count; c++)
                        {
                            row.CreateCell(c).SetCellValue(_grid.Rows[r].Cells[c].Value?.ToString() ?? "");
                        }
                    }

                    for (int c = 0; c < _grid.Columns.Count; c++)
                    {
                        sheet.AutoSizeColumn(c);
                    }

                    using (FileStream stream = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(stream);
                    }

                    UiNotify.Info("Экспорт завершён: " + dialog.FileName);
                }
                catch (Exception ex)
                {
                    UiNotify.Error("Не удалось выполнить экспорт в Excel.", ex);
                }
            }
        }
    }
}
