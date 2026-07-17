using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Controls
{
    /// <summary>
    /// Кнопка со скруглёнными углами. Рисуется полностью сама (owner-draw):
    /// углы за пределами скругления заливаются цветом фона-родителя, поэтому
    /// не остаётся торчащих прямоугольных углов на любой поверхности.
    /// </summary>
    public sealed class RoundedButton : Button
    {
        private bool _hover;
        private bool _pressed;

        public RoundedButton()
        {
            SetStyle(ControlStyles.UserPaint
                   | ControlStyles.AllPaintingInWmPaint
                   | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.ResizeRedraw, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Font = Fonts.Body;
            Cursor = Cursors.Hand;
            TabStop = true;
        }

        public int CornerRadius { get; set; } = 8;
        public Color FillNormal { get; set; } = Palette.Accent;
        public Color FillHover { get; set; } = Palette.AccentHover;
        public Color FillPress { get; set; } = Palette.AccentPressed;

        /// <summary>Цвет рамки. Color.Empty — без рамки.</summary>
        public Color OutlineColor { get; set; } = Color.Empty;

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            _pressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _pressed = true;
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _pressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Styler.ResolveBackColor(Parent));
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color fill;
            Color textColor;
            if (!Enabled)
            {
                fill = Palette.Border;
                textColor = Palette.TextSecondary;
            }
            else
            {
                fill = _pressed ? FillPress : _hover ? FillHover : FillNormal;
                textColor = ForeColor;
            }

            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = Styler.RoundedRect(bounds, CornerRadius))
            {
                using (var brush = new SolidBrush(fill))
                {
                    g.FillPath(brush, path);
                }
                if (Enabled && OutlineColor != Color.Empty)
                {
                    using (var pen = new Pen(OutlineColor))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }

            TextRenderer.DrawText(g, Text, Font, ClientRectangle, textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
