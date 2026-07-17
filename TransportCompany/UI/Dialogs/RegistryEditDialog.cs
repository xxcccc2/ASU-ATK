using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.Core.Data;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Dialogs
{
    /// <summary>
    /// Редактирование импортированного реестра.
    /// Обновление строк идёт по стабильному ключу Id.
    /// </summary>
    public sealed class RegistryEditDialog : Form
    {
        private static readonly Dictionary<string, string> ColumnHeaders = new Dictionary<string, string>
        {
            { "NPP", "№ п/п" },
            { "Date", "Дата" },
            { "FIO", "ФИО" },
            { "NSL", "№СЛ" },
            { "GosNumber", "Гос № ТС" },
            { "Tonnage", "Тоннаж" },
            { "VehicleType", "Тип ТС" },
            { "TransportNumber", "№ Трансп-ки" },
            { "RCLoad", "РЦ Загрузки" },
            { "Branch", "Филиал" },
            { "DeliveryRegion", "Регион доставки" },
            { "TripCost", "Стоимость рейса" },
            { "OrderNumber", "Поряд. номер" },
            { "UnloadPoints", "Точек выгрузки" },
            { "LoadPoints", "Точек загрузки" },
            { "Zone", "Зона" },
            { "ExtraStores", "Доп. магазины" },
            { "ExtraLoad", "Доп. загрузка" },
            { "Supply", "Подача" },
            { "NQNumber", "НомерNQ" },
            { "SumTTK", "СумТТК" },
            { "KmCost", "Сумма за км" },
            { "Discount", "Скидка/надбавка" },
            { "TotalWithoutVAT", "Итого без НДС" },
            { "TotalWithVAT", "Сумма с НДС" },
            { "TransportNumber2", "№ транс-ки" }
        };

        private readonly AppServices _services;
        private readonly ComboBox _registryCombo = new ComboBox();
        private readonly DataGridView _grid = new DataGridView();
        private readonly Label _countLabel;
        private DataTable _currentData;

        public RegistryEditDialog(AppServices services)
        {
            _services = services;

            Text = "Редактирование реестра";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(1150, 640);
            MinimumSize = new Size(900, 500);
            BackColor = Palette.PageBack;
            Font = Fonts.Body;
            AutoScaleMode = AutoScaleMode.Font;

            var toolbar = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.Transparent };

            var registryLabel = Styler.MutedLabel("Реестр:");
            registryLabel.Location = new Point(16, 6);
            _registryCombo.Location = new Point(16, 24);
            _registryCombo.Width = 220;
            _registryCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            Styler.Input(_registryCombo);
            _registryCombo.SelectedIndexChanged += (s, e) => LoadRegistryData();

            Button save = Styler.PrimaryButton("Сохранить изменения");
            save.Location = new Point(260, 20);
            save.Click += (s, e) => SaveChanges();

            Button close = Styler.SecondaryButton("Закрыть");
            close.Location = new Point(470, 20);
            close.Click += (s, e) => Close();

            toolbar.Controls.Add(registryLabel);
            toolbar.Controls.Add(_registryCombo);
            toolbar.Controls.Add(save);
            toolbar.Controls.Add(close);

            Styler.Grid(_grid);
            _grid.Dock = DockStyle.Fill;
            _grid.ReadOnly = false;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            var statusBar = new Panel { Dock = DockStyle.Bottom, Height = 30, BackColor = Color.Transparent };
            _countLabel = Styler.MutedLabel("Записей: 0");
            _countLabel.Location = new Point(16, 6);
            statusBar.Controls.Add(_countLabel);

            var gridHost = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16, 0, 16, 8), BackColor = Color.Transparent };
            gridHost.Controls.Add(_grid);

            Controls.Add(gridHost);
            Controls.Add(statusBar);
            Controls.Add(toolbar);
            gridHost.BringToFront();

            LoadRegistries();
        }

        private void LoadRegistries()
        {
            try
            {
                List<string> registries = _services.Registry.GetRegistryNumbers();
                _registryCombo.Items.Clear();
                foreach (string registry in registries)
                {
                    _registryCombo.Items.Add(registry);
                }

                if (registries.Count == 0)
                {
                    UiNotify.Info("В базе данных не найдено ни одного реестра.");
                }
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось загрузить список реестров.", ex);
            }
        }

        private void LoadRegistryData()
        {
            if (_registryCombo.SelectedItem == null)
            {
                return;
            }

            try
            {
                _currentData = _services.Registry.GetRegistryRows(_registryCombo.SelectedItem.ToString());
                _grid.DataSource = _currentData;

                foreach (DataGridViewColumn column in _grid.Columns)
                {
                    if (ColumnHeaders.TryGetValue(column.Name, out string header))
                    {
                        column.HeaderText = header;
                    }
                }

                if (_grid.Columns["Id"] != null)
                {
                    _grid.Columns["Id"].Visible = false;
                    _grid.Columns["Id"].ReadOnly = true;
                }
                if (_grid.Columns["Registry"] != null)
                {
                    _grid.Columns["Registry"].Visible = false;
                }

                _countLabel.Text = $"Записей: {_currentData.Rows.Count}";
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось загрузить данные реестра.", ex);
            }
        }

        private void SaveChanges()
        {
            if (_currentData == null)
            {
                UiNotify.Warning("Нет данных для сохранения.");
                return;
            }

            _grid.EndEdit();

            try
            {
                int updated = 0;
                foreach (DataRow row in _currentData.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        _services.Registry.UpdateRegistryRow(row);
                        updated++;
                    }
                }

                _currentData.AcceptChanges();
                UiNotify.Info(updated > 0
                    ? $"Сохранено изменённых строк: {updated}."
                    : "Изменений нет.");
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось сохранить изменения.", ex);
            }
        }
    }
}
