using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Controls
{
    /// <summary>Белая карточка со скруглёнными углами и тонкой рамкой.</summary>
    public class CardPanel : Panel
    {
        private const int Radius = 8;

        public CardPanel()
        {
            BackColor = Palette.CardBack;
            Padding = new Padding(16);
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            Color backdrop = Styler.ResolveBackColor(Parent);

            using (GraphicsPath path = Styler.RoundedRect(bounds, Radius))
            {
                // Углы за пределами скругления перекрываем цветом поверхности,
                // иначе белый прямоугольный фон карточки торчит острыми углами.
                using (var region = new Region(new Rectangle(0, 0, Width, Height)))
                {
                    region.Exclude(path);
                    using (var brush = new SolidBrush(backdrop))
                    {
                        g.FillRegion(brush, region);
                    }
                }

                using (var pen = new Pen(Palette.Border))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        protected override void OnResize(System.EventArgs eventargs)
        {
            base.OnResize(eventargs);
            Invalidate();
        }
    }
}
