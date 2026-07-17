using System.Drawing;
using System.Windows.Forms;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Controls
{
    /// <summary>Пункт бокового меню: иконка Segoe MDL2 + подпись, состояния hover/выбран.</summary>
    public sealed class SidebarButton : Button
    {
        private bool _selected;

        public SidebarButton(string text, string iconGlyph)
        {
            Text = "  " + text;
            IconGlyph = iconGlyph;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Font = Fonts.Body;
            ForeColor = Palette.SidebarText;
            BackColor = Palette.SidebarBack;
            TextAlign = ContentAlignment.MiddleLeft;
            TextImageRelation = TextImageRelation.ImageBeforeText;
            ImageAlign = ContentAlignment.MiddleLeft;
            Height = 42;
            Dock = DockStyle.Top;
            Cursor = Cursors.Hand;
            Padding = new Padding(40, 0, 8, 0);
            UseVisualStyleBackColor = false;
            FlatAppearance.MouseOverBackColor = Palette.SidebarHover;
            FlatAppearance.MouseDownBackColor = Palette.SidebarSelected;
            TabStop = false;
        }

        public string IconGlyph { get; }

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                BackColor = value ? Palette.SidebarSelected : Palette.SidebarBack;
                ForeColor = value ? Palette.SidebarTextActive : Palette.SidebarText;
                FlatAppearance.MouseOverBackColor = value ? Palette.SidebarSelected : Palette.SidebarHover;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (!string.IsNullOrEmpty(IconGlyph))
            {
                TextRenderer.DrawText(pevent.Graphics, IconGlyph, Fonts.Icon,
                    new Rectangle(12, 0, 28, Height),
                    ForeColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            }

            if (_selected)
            {
                using (var brush = new SolidBrush(Color.White))
                {
                    pevent.Graphics.FillRectangle(brush, 0, 8, 3, Height - 16);
                }
            }
        }
    }
}
