using System;
using System.Drawing;
using System.Windows.Forms;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Controls
{
    /// <summary>Карточка модуля на главной странице: иконка, название, описание, клик.</summary>
    public sealed class ModuleCard : CardPanel
    {
        public event EventHandler CardClick;

        public ModuleCard(string iconGlyph, string title, string description)
        {
            Size = new Size(300, 92);
            Padding = new Padding(0);
            Cursor = Cursors.Hand;

            var icon = new Panel
            {
                BackColor = Palette.AccentSoft,
                Size = new Size(44, 44),
                Location = new Point(16, 24),
                Cursor = Cursors.Hand
            };
            icon.Paint += (s, e) => TextRenderer.DrawText(
                e.Graphics, iconGlyph, Fonts.IconLarge,
                new Rectangle(0, 0, icon.Width, icon.Height),
                Palette.Accent,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            Styler.Round(icon, 8);

            var titleLabel = new Label
            {
                Text = title,
                Font = Fonts.CardTitle,
                ForeColor = Palette.TextPrimary,
                Location = new Point(72, 18),
                AutoSize = true,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            var descriptionLabel = new Label
            {
                Text = description,
                Font = Fonts.Small,
                ForeColor = Palette.TextSecondary,
                Location = new Point(72, 42),
                Size = new Size(215, 38),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            Controls.Add(icon);
            Controls.Add(titleLabel);
            Controls.Add(descriptionLabel);

            Click += RaiseClick;
            icon.Click += RaiseClick;
            titleLabel.Click += RaiseClick;
            descriptionLabel.Click += RaiseClick;

            MouseEnter += (s, e) => BackColor = Palette.PageBack;
            MouseLeave += (s, e) => BackColor = Palette.CardBack;
        }

        private void RaiseClick(object sender, EventArgs e) => CardClick?.Invoke(this, EventArgs.Empty);
    }
}
