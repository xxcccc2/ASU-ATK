using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.Core.Data;
using TransportCompany.Core.Repositories;
using TransportCompany.UI.Controls;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Pages
{
    /// <summary>Настройки: тарифы зон (с историей изменений) и подключение к БД.</summary>
    public sealed class SettingsPage : AppPage
    {
        private readonly TabControl _tabs = new TabControl();
        private readonly DataGridView _zonesGrid = new DataGridView();
        private readonly DataGridView _historyGrid = new DataGridView();
        private readonly TextBox _connectionString = new TextBox();
        private readonly Dictionary<int, decimal> _originalCosts = new Dictionary<int, decimal>();
        private Button _saveZonesButton;
        private bool _hasUnsavedChanges;

        public override string Title => "Настройки";

        public SettingsPage(AppServices services) : base(services)
        {
            Padding = new Padding(24, 16, 24, 16);

            _tabs.Dock = DockStyle.Fill;
            _tabs.Font = Fonts.Body;

            _tabs.TabPages.Add(BuildZonesTab());
            _tabs.TabPages.Add(BuildHistoryTab());
            _tabs.TabPages.Add(BuildConnectionTab());
            _tabs.SelectedIndexChanged += (s, e) =>
            {
                if (_tabs.SelectedIndex == 1)
                {
                    LoadHistory();
                }
            };

            Controls.Add(_tabs);
        }

        // ----- Вкладка «Тарифы зон» -----

        private TabPage BuildZonesTab()
        {
            var tab = new TabPage("Тарифы зон") { BackColor = Palette.PageBack, Padding = new Padding(12) };

            var hint = Styler.MutedLabel(
                "Тарифы применяются во всех расчётах: зарплата, заработок, сравнение, расчётный листок.");
            hint.Dock = DockStyle.Top;
            hint.Height = 28;
            hint.Padding = new Padding(0, 6, 0, 0);

            var card = new CardPanel { Dock = DockStyle.Fill, Padding = new Padding(1) };
            Styler.Grid(_zonesGrid);
            _zonesGrid.Dock = DockStyle.Fill;
            _zonesGrid.ReadOnly = false;

            _zonesGrid.Columns.Add("colZoneId", "Зона");
            _zonesGrid.Columns.Add("colCost", "Тариф (руб.)");
            _zonesGrid.Columns.Add("colUpdated", "Обновлено");
            _zonesGrid.Columns["colZoneId"].ReadOnly = true;
            _zonesGrid.Columns["colUpdated"].ReadOnly = true;
            _zonesGrid.Columns["colCost"].DefaultCellStyle.Font = Fonts.BodyBold;
            _zonesGrid.CellValueChanged += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 1)
                {
                    _hasUnsavedChanges = true;
                    UpdateSaveState();
                }
            };
            card.Controls.Add(_zonesGrid);

            var bottomBar = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Bottom,
                Height = 48,
                Padding = new Padding(0, 8, 0, 0),
                BackColor = Color.Transparent
            };
            _saveZonesButton = Styler.PrimaryButton("Сохранить тарифы");
            _saveZonesButton.Click += (s, e) => SaveZones();
            Button reload = Styler.SecondaryButton("Обновить");
            reload.Click += (s, e) => LoadZones();
            reload.Margin = new Padding(8, 0, 0, 0);
            bottomBar.Controls.Add(_saveZonesButton);
            bottomBar.Controls.Add(reload);

            tab.Controls.Add(card);
            tab.Controls.Add(bottomBar);
            tab.Controls.Add(hint);
            card.BringToFront();
            return tab;
        }

        private TabPage BuildHistoryTab()
        {
            var tab = new TabPage("История изменений") { BackColor = Palette.PageBack, Padding = new Padding(12) };

            var card = new CardPanel { Dock = DockStyle.Fill, Padding = new Padding(1) };
            Styler.Grid(_historyGrid);
            _historyGrid.Dock = DockStyle.Fill;
            _historyGrid.ReadOnly = true;
            _historyGrid.Columns.Add("Date", "Дата");
            _historyGrid.Columns.Add("Zone", "Зона");
            _historyGrid.Columns.Add("OldCost", "Старый тариф");
            _historyGrid.Columns.Add("NewCost", "Новый тариф");
            _historyGrid.Columns.Add("ChangedBy", "Кто изменил");
            card.Controls.Add(_historyGrid);

            tab.Controls.Add(card);
            return tab;
        }

        private TabPage BuildConnectionTab()
        {
            var tab = new TabPage("Подключение к БД") { BackColor = Palette.PageBack, Padding = new Padding(12) };

            var card = new CardPanel { Dock = DockStyle.Top, Height = 210, Padding = new Padding(16) };

            var title = Styler.SectionTitle("Строка подключения SQL Server");
            title.Location = new Point(16, 14);

            var hint = Styler.MutedLabel(
                "Изменения вступают в силу после перезапуска приложения.");
            hint.Location = new Point(16, 40);

            _connectionString.Location = new Point(16, 66);
            _connectionString.Width = 640;
            _connectionString.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Styler.Input(_connectionString);

            Button test = Styler.SecondaryButton("Проверить подключение");
            test.Location = new Point(16, 104);
            test.Click += (s, e) => TestConnection();

            Button save = Styler.PrimaryButton("Сохранить");
            save.Location = new Point(210, 104);
            save.Click += (s, e) => SaveConnection();

            card.Controls.Add(title);
            card.Controls.Add(hint);
            card.Controls.Add(_connectionString);
            card.Controls.Add(test);
            card.Controls.Add(save);

            tab.Controls.Add(card);
            return tab;
        }

        public override void OnActivated()
        {
            LoadZones();
            try
            {
                _connectionString.Text = Config.ConnectionString;
            }
            catch (Exception)
            {
                _connectionString.Text = "";
            }
        }

        private void LoadZones()
        {
            _zonesGrid.Rows.Clear();
            _originalCosts.Clear();

            try
            {
                DataTable settings = Services.ZoneSettings.GetZoneSettingsWithDates();
                foreach (DataRow row in settings.Rows)
                {
                    int zoneId = Convert.ToInt32(row["ZoneId"]);
                    decimal cost = Convert.ToDecimal(row["Cost"]);
                    _originalCosts[zoneId] = cost;
                    _zonesGrid.Rows.Add(zoneId, cost,
                        Convert.ToDateTime(row["UpdatedDate"]).ToString("dd.MM.yyyy HH:mm"));
                }
            }
            catch (DataAccessException)
            {
                // Таблицы нет — показываем тарифы по умолчанию.
            }

            if (_zonesGrid.Rows.Count == 0)
            {
                foreach (KeyValuePair<int, decimal> pair in Core.Services.ZoneRateService.DefaultRates)
                {
                    _originalCosts[pair.Key] = pair.Value;
                    _zonesGrid.Rows.Add(pair.Key, pair.Value, "По умолчанию");
                }
            }

            _zonesGrid.ClearSelection();
            _hasUnsavedChanges = false;
            UpdateSaveState();
        }

        private void UpdateSaveState()
        {
            _saveZonesButton.Enabled = _hasUnsavedChanges;
        }

        private void SaveZones()
        {
            var changes = new List<ZoneSettingChange>();

            foreach (DataGridViewRow row in _zonesGrid.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                int zoneId = Convert.ToInt32(row.Cells["colZoneId"].Value);
                object costValue = row.Cells["colCost"].Value;

                if (costValue == null || !decimal.TryParse(costValue.ToString(), out decimal newCost))
                {
                    UiNotify.Warning($"Некорректное значение тарифа для зоны {zoneId}. Введите число.");
                    _zonesGrid.CurrentCell = row.Cells["colCost"];
                    return;
                }

                if (newCost < 0)
                {
                    UiNotify.Warning($"Тариф зоны {zoneId} не может быть отрицательным.");
                    _zonesGrid.CurrentCell = row.Cells["colCost"];
                    return;
                }

                decimal oldCost = _originalCosts.TryGetValue(zoneId, out decimal original) ? original : 0m;
                if (newCost != oldCost)
                {
                    changes.Add(new ZoneSettingChange { ZoneId = zoneId, OldCost = oldCost, NewCost = newCost });
                }
            }

            if (changes.Count == 0)
            {
                UiNotify.Info("Изменений нет.");
                return;
            }

            if (!UiNotify.Confirm(
                $"Сохранить изменения тарифов ({changes.Count})?\n\nНовые тарифы будут применяться во всех расчётах."))
            {
                return;
            }

            try
            {
                Services.ZoneSettings.SaveZoneCosts(changes, Config.CurrentOperator);
                Services.ZoneRates.InvalidateCache();
                UiNotify.Info("Тарифы зон сохранены.");
                LoadZones();
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось сохранить тарифы зон. Изменения отменены.", ex);
            }
        }

        private void LoadHistory()
        {
            _historyGrid.Rows.Clear();
            try
            {
                DataTable history = Services.ZoneSettings.GetHistory();
                foreach (DataRow row in history.Rows)
                {
                    _historyGrid.Rows.Add(
                        Convert.ToDateTime(row["ChangeDate"]).ToString("dd.MM.yyyy HH:mm"),
                        "Зона " + row["ZoneId"],
                        row["OldCost"] == DBNull.Value ? "—" : Convert.ToDecimal(row["OldCost"]).ToString("N2"),
                        Convert.ToDecimal(row["NewCost"]).ToString("N2"),
                        row["ChangedBy"] == DBNull.Value ? "Система" : row["ChangedBy"].ToString());
                }
                _historyGrid.ClearSelection();
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось загрузить историю изменений.", ex);
            }
        }

        private void TestConnection()
        {
            string connectionString = _connectionString.Text.Trim();
            if (connectionString.Length == 0)
            {
                UiNotify.Warning("Введите строку подключения.");
                return;
            }

            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                using (var connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                }
                UiNotify.Info("Подключение успешно установлено.");
            }
            catch (Exception ex)
            {
                UiNotify.Error("Не удалось подключиться к базе данных.", ex);
            }
        }

        private void SaveConnection()
        {
            string connectionString = _connectionString.Text.Trim();

            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                if (string.IsNullOrEmpty(builder.DataSource) || string.IsNullOrEmpty(builder.InitialCatalog))
                {
                    UiNotify.Warning("Строка подключения должна содержать Data Source и Initial Catalog.");
                    return;
                }
            }
            catch (Exception ex)
            {
                UiNotify.Error("Некорректная строка подключения.", ex);
                return;
            }

            if (!UiNotify.Confirm(
                "Сохранить новую строку подключения?\n\nИзменения вступят в силу после перезапуска приложения."))
            {
                return;
            }

            try
            {
                Config.SaveConnectionString(connectionString);
                UiNotify.Info("Строка подключения сохранена. Перезапустите приложение.");
            }
            catch (Exception ex)
            {
                UiNotify.Error("Не удалось сохранить строку подключения.", ex);
            }
        }
    }
}
