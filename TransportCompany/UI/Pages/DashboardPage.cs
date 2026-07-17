using System;
using System.Drawing;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.Core.Logging;
using TransportCompany.Core.Models;
using TransportCompany.Core.Services;
using TransportCompany.UI.Controls;
using TransportCompany.UI.Shell;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Pages
{
    /// <summary>Главная: приветствие, карточки модулей и панель предупреждений о сроках.</summary>
    public sealed class DashboardPage : AppPage
    {
        private readonly MainShellForm _shell;
        private readonly FlowLayoutPanel _cardsPanel;
        private readonly CardPanel _alertsCard;
        private readonly FlowLayoutPanel _alertsContent;
        private bool _alertsLoaded;

        public override string Title => "Главная";

        public DashboardPage(AppServices services, MainShellForm shell) : base(services)
        {
            _shell = shell;

            var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(24, 16, 24, 24) };

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            var welcome = Styler.PageTitle("Добро пожаловать в АТК-Форум");
            welcome.Margin = new Padding(3, 8, 3, 0);
            var subtitle = Styler.MutedLabel("Выберите модуль для работы");
            subtitle.Margin = new Padding(4, 4, 3, 16);

            _cardsPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MaximumSize = new Size(920, 0),
                Margin = new Padding(0)
            };

            AddCard("", "Техосмотр", "Учёт технического обслуживания транспорта", MainShellForm.PageMaintenance);
            AddCard("", "Расчёт ЗП", "Расчёт заработной платы водителей", MainShellForm.PageSalary);
            AddCard("", "Дневник Автопарка", "Учёт ОСАГО и водительских удостоверений", MainShellForm.PageFleetDiary);
            AddCard("", "Заработок ТС и Водителей", "Анализ заработка ТС и водителей", MainShellForm.PageEarnings);
            AddCard("", "Импорт Реестров", "Импорт данных из реестров", MainShellForm.PageImport);
            AddCard("", "Сравнение ТС/Водителей", "Сравнительный анализ показателей", MainShellForm.PageComparison);
            AddCard("", "Анализ зон", "Анализ зон по транспортным средствам", MainShellForm.PageZoneAnalysis);
            AddCard("", "Настройки", "Тарифы зон и подключение к БД", MainShellForm.PageSettings);

            // Панель предупреждений
            _alertsCard = new CardPanel
            {
                Width = 900,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(3, 16, 3, 3),
                Visible = false
            };

            var alertsTitle = new Label
            {
                Text = "⚠ Требуют внимания",
                Font = Fonts.CardTitle,
                ForeColor = Palette.Warning,
                AutoSize = true,
                Location = new Point(8, 8)
            };

            _alertsContent = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Location = new Point(12, 36),
                MaximumSize = new Size(860, 0)
            };

            _alertsCard.Controls.Add(alertsTitle);
            _alertsCard.Controls.Add(_alertsContent);

            var version = Styler.MutedLabel($"© {DateTime.Today.Year} АТК-Форум  ·  Версия 2.0.0");
            version.Margin = new Padding(4, 20, 3, 3);

            layout.Controls.Add(welcome);
            layout.Controls.Add(subtitle);
            layout.Controls.Add(_cardsPanel);
            layout.Controls.Add(_alertsCard);
            layout.Controls.Add(version);

            scroll.Controls.Add(layout);
            Controls.Add(scroll);
        }

        public override void OnActivated()
        {
            if (_alertsLoaded)
            {
                return;
            }
            _alertsLoaded = true;
            LoadAlerts();
        }

        private void AddCard(string icon, string title, string description, string pageKey)
        {
            var card = new ModuleCard(icon, title, description) { Margin = new Padding(0, 0, 16, 16) };
            card.CardClick += (s, e) => _shell.Navigate(pageKey);
            _cardsPanel.Controls.Add(card);
        }

        private void LoadAlerts()
        {
            try
            {
                ExpirySummary summary = Services.Expiry.GetSummary();
                if (!summary.HasWarnings)
                {
                    return;
                }

                _alertsContent.Controls.Clear();

                foreach (MaintenanceRecord record in summary.OverdueMaintenance)
                {
                    AddAlertLine(
                        $"Требуется ТО: {record.CarNumber} (последнее ТО {record.LastServiceDate:dd.MM.yyyy})");
                }
                foreach (OsagoPolicy policy in summary.ExpiringOsago)
                {
                    AddAlertLine(
                        $"Истекает ОСАГО: {policy.VehicleRegistrationNumber}, полис {policy.PolicyNumber} — до {policy.EndDate:dd.MM.yyyy}");
                }
                foreach (DriverLicenseRecord license in summary.ExpiringLicenses)
                {
                    AddAlertLine(
                        $"Истекает ВУ: {license.DriverFullName}, №{license.LicenseNumber} — до {license.ExpiryDate:dd.MM.yyyy}");
                }

                _alertsCard.Visible = true;
            }
            catch (Exception ex)
            {
                // Недоступная БД не должна ломать главную страницу.
                AppLogger.Error("Не удалось загрузить предупреждения на главной странице", ex);
            }
        }

        private void AddAlertLine(string text)
        {
            var line = new Label
            {
                Text = "•  " + text,
                Font = Fonts.Body,
                ForeColor = Palette.TextPrimary,
                AutoSize = true,
                Margin = new Padding(0, 2, 0, 2)
            };
            _alertsContent.Controls.Add(line);
        }
    }
}
