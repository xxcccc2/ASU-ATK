using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Pages
{
    /// <summary>Базовый класс страницы приложения внутри оболочки MainShellForm.</summary>
    public class AppPage : UserControl
    {
        protected AppServices Services { get; }

        public AppPage()
        {
            // Конструктор без параметров нужен только дизайнеру WinForms.
        }

        public AppPage(AppServices services)
        {
            Services = services;
            Dock = DockStyle.Fill;
            BackColor = Palette.PageBack;
            AutoScaleMode = AutoScaleMode.Font;
            Font = Fonts.Body;
        }

        public virtual string Title => "";

        /// <summary>Вызывается оболочкой при каждом показе страницы.</summary>
        public virtual void OnActivated()
        {
        }
    }
}
