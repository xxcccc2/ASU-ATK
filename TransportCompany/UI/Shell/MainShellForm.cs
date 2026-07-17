using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.UI.Controls;
using TransportCompany.UI.Pages;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Shell
{
    /// <summary>
    /// Единственное главное окно приложения: сайдбар + верхняя панель + область контента.
    /// Страницы — UserControl, создаются лениво и переиспользуются.
    /// Заменяет прежнюю навигацию Show/Hide/новый MainForm.
    /// </summary>
    public sealed class MainShellForm : Form
    {
        private readonly AppServices _services;
        private readonly Panel _sidebar = new Panel();
        private readonly Panel _contentPanel = new Panel();
        private readonly Label _topBarTitle;
        private readonly Dictionary<string, AppPage> _pages = new Dictionary<string, AppPage>();
        private readonly Dictionary<string, SidebarButton> _menuButtons = new Dictionary<string, SidebarButton>();
        private readonly Dictionary<string, Func<AppPage>> _pageFactories;

        public const string PageDashboard = "dashboard";
        public const string PageMaintenance = "maintenance";
        public const string PageSalary = "salary";
        public const string PageFleetDiary = "fleet";
        public const string PageEarnings = "earnings";
        public const string PageImport = "import";
        public const string PageComparison = "comparison";
        public const string PageZoneAnalysis = "zones";
        public const string PageSettings = "settings";

        public MainShellForm(AppServices services)
        {
            _services = services;

            _pageFactories = new Dictionary<string, Func<AppPage>>
            {
                { PageDashboard, () => new DashboardPage(_services, this) },
                { PageMaintenance, () => new MaintenancePage(_services) },
                { PageSalary, () => new SalaryPage(_services) },
                { PageFleetDiary, () => new FleetDiaryPage(_services) },
                { PageEarnings, () => new EarningsPage(_services) },
                { PageImport, () => new ImportPage(_services) },
                { PageComparison, () => new ComparisonPage(_services) },
                { PageZoneAnalysis, () => new ZoneAnalysisPage(_services) },
                { PageSettings, () => new SettingsPage(_services) }
            };

            Text = "АТК-Форум";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1100, 700);
            ClientSize = new Size(1280, 760);
            BackColor = Palette.PageBack;
            Font = Fonts.Body;
            AutoScaleMode = AutoScaleMode.Font;

            try
            {
                Icon = new Icon("icon.ico");
            }
            catch
            {
                // Иконка не критична.
            }

            // ----- Верхняя панель -----
            var topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Palette.TopBarBack
            };
            topBar.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(Palette.Border), 0, topBar.Height - 1, topBar.Width, topBar.Height - 1);

            _topBarTitle = new Label
            {
                Text = "Главная",
                Font = Fonts.SectionTitle,
                ForeColor = Palette.TextPrimary,
                AutoSize = true,
                Location = new Point(24, 15)
            };
            topBar.Controls.Add(_topBarTitle);

            var userLabel = new Label
            {
                Text = "  " + Config.CurrentOperator,
                Font = Fonts.Body,
                ForeColor = Palette.TextSecondary,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            topBar.Controls.Add(userLabel);
            topBar.Resize += (s, e) => userLabel.Location = new Point(topBar.Width - userLabel.Width - 24, 17);
            userLabel.Location = new Point(topBar.Width - userLabel.Width - 24, 17);

            // ----- Сайдбар -----
            _sidebar.Dock = DockStyle.Left;
            _sidebar.Width = 248;
            _sidebar.BackColor = Palette.SidebarBack;

            // Нижние пункты (добавляются первыми, DockStyle.Bottom)
            var exitButton = new SidebarButton("Выход", "");
            exitButton.Dock = DockStyle.Bottom;
            exitButton.Click += (s, e) => Close();

            var settingsButton = new SidebarButton("Настройки", "");
            settingsButton.Dock = DockStyle.Bottom;
            settingsButton.Click += (s, e) => Navigate(PageSettings);
            _menuButtons[PageSettings] = settingsButton;

            var bottomSpacer = new Panel { Dock = DockStyle.Bottom, Height = 10, BackColor = Palette.SidebarBack };

            _sidebar.Controls.Add(bottomSpacer);
            _sidebar.Controls.Add(exitButton);
            _sidebar.Controls.Add(settingsButton);

            // Основные пункты (в обратном порядке — DockStyle.Top складывает сверху)
            AddMenuButton(PageZoneAnalysis, "Анализ зон", "");
            AddMenuButton(PageComparison, "Сравнение ТС/Водителей", "");
            AddMenuButton(PageImport, "Импорт Реестров", "");
            AddMenuButton(PageEarnings, "Заработок ТС и Водителей", "");
            AddMenuButton(PageFleetDiary, "Дневник Автопарка", "");
            AddMenuButton(PageSalary, "Расчёт ЗП", "");
            AddMenuButton(PageMaintenance, "Техосмотр", "");
            AddMenuButton(PageDashboard, "Главная", "");

            // Шапка сайдбара
            var brand = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Palette.SidebarBack };
            brand.Paint += (s, e) =>
            {
                using (var brush = new SolidBrush(Palette.Accent))
                {
                    e.Graphics.FillRectangle(brush, 16, 14, 28, 28);
                }
                TextRenderer.DrawText(e.Graphics, "", Fonts.Icon,
                    new Rectangle(16, 14, 28, 28), Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(e.Graphics, "АТК-Форум", Fonts.CardTitle,
                    new Rectangle(54, 0, 160, 56), Color.White,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            };
            _sidebar.Controls.Add(brand);

            // ----- Контент -----
            _contentPanel.Dock = DockStyle.Fill;
            _contentPanel.BackColor = Palette.PageBack;

            Controls.Add(_contentPanel);
            Controls.Add(topBar);
            Controls.Add(_sidebar);

            Navigate(PageDashboard);
        }

        private void AddMenuButton(string pageKey, string text, string iconGlyph)
        {
            var button = new SidebarButton(text, iconGlyph);
            button.Click += (s, e) => Navigate(pageKey);
            _menuButtons[pageKey] = button;
            _sidebar.Controls.Add(button);
        }

        public void Navigate(string pageKey)
        {
            if (!_pageFactories.ContainsKey(pageKey))
            {
                return;
            }

            if (!_pages.TryGetValue(pageKey, out AppPage page))
            {
                page = _pageFactories[pageKey]();
                _pages[pageKey] = page;
            }

            _contentPanel.SuspendLayout();
            _contentPanel.Controls.Clear();
            _contentPanel.Controls.Add(page);
            _contentPanel.ResumeLayout();

            foreach (KeyValuePair<string, SidebarButton> pair in _menuButtons)
            {
                pair.Value.Selected = pair.Key == pageKey;
            }

            _topBarTitle.Text = page.Title;
            page.OnActivated();
        }
    }
}
