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
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = Styler.RoundedRect(bounds, Radius))
            using (var pen = new Pen(Palette.Border))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        protected override void OnResize(System.EventArgs eventargs)
        {
            base.OnResize(eventargs);
            Invalidate();
        }
    }
}
