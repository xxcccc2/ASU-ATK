using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TransportCompany.UI.Theme
{
    /// <summary>Единая стилизация стандартных WinForms-контролов под дизайн-систему.</summary>
    public static class Styler
    {
        // ----- Кнопки -----

        public static Button PrimaryButton(string text, string iconGlyph = null)
        {
            Button button = BaseButton(text, iconGlyph);
            button.BackColor = Palette.Accent;
            button.ForeColor = Palette.TextOnAccent;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Palette.AccentHover;
            button.FlatAppearance.MouseDownBackColor = Palette.AccentPressed;
            return button;
        }

        public static Button SecondaryButton(string text, string iconGlyph = null)
        {
            Button button = BaseButton(text, iconGlyph);
            button.BackColor = Color.White;
            button.ForeColor = Palette.TextPrimary;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Palette.Border;
            button.FlatAppearance.MouseOverBackColor = Palette.PageBack;
            return button;
        }

        public static Button DangerButton(string text, string iconGlyph = null)
        {
            Button button = BaseButton(text, iconGlyph);
            button.BackColor = Color.White;
            button.ForeColor = Palette.Danger;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Palette.Border;
            button.FlatAppearance.MouseOverBackColor = Palette.DangerSoft;
            return button;
        }

        private static Button BaseButton(string text, string iconGlyph)
        {
            var button = new Button
            {
                Text = iconGlyph == null ? text : iconGlyph + "  " + text,
                FlatStyle = FlatStyle.Flat,
                Font = Fonts.Body,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(12, 6, 12, 6),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false,
                TabStop = true
            };
            return button;
        }

        // ----- Таблицы -----

        public static void Grid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = Palette.CardBack;
            grid.GridColor = Palette.GridLine;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 38;
            grid.RowTemplate.Height = 34;

            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Palette.GridHeaderBack,
                ForeColor = Palette.TextSecondary,
                SelectionBackColor = Palette.GridHeaderBack,
                SelectionForeColor = Palette.TextSecondary,
                Font = Fonts.BodyBold,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0)
            };

            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Palette.CardBack,
                ForeColor = Palette.TextPrimary,
                SelectionBackColor = Palette.GridSelection,
                SelectionForeColor = Palette.TextPrimary,
                Font = Fonts.Body,
                Padding = new Padding(8, 0, 0, 0)
            };

            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Palette.GridRowAlt,
                ForeColor = Palette.TextPrimary,
                SelectionBackColor = Palette.GridSelection,
                SelectionForeColor = Palette.TextPrimary
            };
        }

        // ----- Поля ввода -----

        public static void Input(Control control)
        {
            control.Font = Fonts.Body;
            control.BackColor = Color.White;
            control.ForeColor = Palette.TextPrimary;

            if (control is TextBox textBox)
            {
                textBox.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.FlatStyle = FlatStyle.Flat;
            }
            else if (control is NumericUpDown numeric)
            {
                numeric.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        // ----- Метки -----

        public static Label PageTitle(string text) => MakeLabel(text, Fonts.PageTitle, Palette.TextPrimary);

        public static Label SectionTitle(string text) => MakeLabel(text, Fonts.SectionTitle, Palette.TextPrimary);

        public static Label BodyLabel(string text) => MakeLabel(text, Fonts.Body, Palette.TextPrimary);

        public static Label MutedLabel(string text) => MakeLabel(text, Fonts.Small, Palette.TextSecondary);

        private static Label MakeLabel(string text, Font font, Color color)
        {
            return new Label
            {
                Text = text,
                Font = font,
                ForeColor = color,
                AutoSize = true,
                BackColor = Color.Transparent
            };
        }

        // ----- Скругление -----

        public static void Round(Control control, int radius)
        {
            void Apply()
            {
                if (control.Width <= 0 || control.Height <= 0)
                {
                    return;
                }
                using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, control.Width, control.Height), radius))
                {
                    control.Region = new Region(path);
                }
            }

            Apply();
            control.SizeChanged += (s, e) => Apply();
        }

        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d - 1, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d - 1, bounds.Bottom - d - 1, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d - 1, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
