using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Controls
{
    /// <summary>
    /// Пункт бокового меню: скруглённая «пилюля» с иконкой Segoe MDL2 и подписью.
    /// Состояния hover / выбран подсвечиваются скруглённым фоном с отступом от краёв.
    /// </summary>
    public sealed class SidebarButton : Button
    {
        private const int PillInset = 10;
        private bool _selected;
        private bool _hover;

        public SidebarButton(string text, string iconGlyph)
        {
            IconGlyph = iconGlyph;
            Text = text;

            SetStyle(ControlStyles.UserPaint
                   | ControlStyles.AllPaintingInWmPaint
                   | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.ResizeRedraw, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Font = Fonts.Body;
            ForeColor = Palette.SidebarText;
            BackColor = Palette.SidebarBack;
            Height = 44;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            TabStop = false;
        }

        public string IconGlyph { get; }

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                ForeColor = value ? Palette.SidebarTextActive : Palette.SidebarText;
                Invalidate();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Palette.SidebarBack);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var pill = new Rectangle(PillInset, 4, Width - PillInset * 2, Height - 8);

            if (_selected || _hover)
            {
                Color fill = _selected ? Palette.SidebarSelected : Palette.SidebarHover;
                using (GraphicsPath path = Styler.RoundedRect(pill, 8))
                using (var brush = new SolidBrush(fill))
                {
                    g.FillPath(brush, path);
                }
            }

            if (!string.IsNullOrEmpty(IconGlyph))
            {
                TextRenderer.DrawText(g, IconGlyph, Fonts.Icon,
                    new Rectangle(pill.X + 10, 0, 26, Height),
                    ForeColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            }

            TextRenderer.DrawText(g, Text, Font,
                new Rectangle(pill.X + 42, 0, pill.Width - 46, Height),
                ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        }
    }
}
