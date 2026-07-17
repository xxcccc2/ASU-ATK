using System.Drawing;

namespace TransportCompany.UI.Theme
{
    /// <summary>
    /// Дизайн-система приложения: палитра и шрифты.
    /// Светлая тема с синим акцентом и тёмно-синим сайдбаром (по макету).
    /// </summary>
    public static class Palette
    {
        // Акцент
        public static readonly Color Accent = Color.FromArgb(25, 118, 210);        // #1976D2
        public static readonly Color AccentHover = Color.FromArgb(21, 101, 192);   // #1565C0
        public static readonly Color AccentPressed = Color.FromArgb(13, 71, 161);  // #0D47A1
        public static readonly Color AccentSoft = Color.FromArgb(227, 240, 252);   // светло-голубая подложка

        // Сайдбар
        public static readonly Color SidebarBack = Color.FromArgb(18, 42, 77);     // тёмно-синий
        public static readonly Color SidebarHover = Color.FromArgb(28, 58, 100);
        public static readonly Color SidebarSelected = Color.FromArgb(31, 111, 214);
        public static readonly Color SidebarText = Color.FromArgb(214, 226, 240);
        public static readonly Color SidebarTextActive = Color.White;

        // Поверхности
        public static readonly Color PageBack = Color.FromArgb(244, 246, 250);     // фон контента
        public static readonly Color CardBack = Color.White;
        public static readonly Color Border = Color.FromArgb(224, 228, 235);
        public static readonly Color TopBarBack = Color.White;

        // Текст
        public static readonly Color TextPrimary = Color.FromArgb(31, 41, 55);
        public static readonly Color TextSecondary = Color.FromArgb(107, 114, 128);
        public static readonly Color TextOnAccent = Color.White;

        // Состояния
        public static readonly Color Success = Color.FromArgb(46, 158, 91);
        public static readonly Color SuccessSoft = Color.FromArgb(232, 246, 238);
        public static readonly Color Warning = Color.FromArgb(217, 119, 6);
        public static readonly Color WarningSoft = Color.FromArgb(255, 247, 224);
        public static readonly Color Danger = Color.FromArgb(211, 47, 47);
        public static readonly Color DangerSoft = Color.FromArgb(253, 235, 235);

        // Таблицы
        public static readonly Color GridHeaderBack = Color.FromArgb(243, 245, 247);
        public static readonly Color GridLine = Color.FromArgb(229, 232, 236);
        public static readonly Color GridRowAlt = Color.FromArgb(250, 251, 253);
        public static readonly Color GridSelection = Color.FromArgb(227, 240, 252);

        // Диаграммы
        public static readonly Color ChartBlue = Color.FromArgb(33, 118, 210);
        public static readonly Color ChartGreen = Color.FromArgb(76, 175, 80);
    }

    public static class Fonts
    {
        public const string Family = "Segoe UI";
        public const string IconFamily = "Segoe MDL2 Assets";

        public static readonly Font Body = new Font(Family, 9.75f);
        public static readonly Font BodyBold = new Font(Family, 9.75f, FontStyle.Bold);
        public static readonly Font Small = new Font(Family, 8.75f);
        public static readonly Font Caption = new Font(Family, 8.75f, FontStyle.Regular);
        public static readonly Font PageTitle = new Font(Family, 15f, FontStyle.Bold);
        public static readonly Font SectionTitle = new Font(Family, 11.5f, FontStyle.Bold);
        public static readonly Font CardTitle = new Font(Family, 10.5f, FontStyle.Bold);
        public static readonly Font Icon = new Font(IconFamily, 12f);
        public static readonly Font IconLarge = new Font(IconFamily, 16f);
        public static readonly Font TotalValue = new Font(Family, 12f, FontStyle.Bold);
    }
}
