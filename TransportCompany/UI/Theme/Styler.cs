using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TransportCompany.UI.Controls;

namespace TransportCompany.UI.Theme
{
    /// <summary>Единая стилизация стандартных WinForms-контролов под дизайн-систему.</summary>
    public static class Styler
    {
        // ----- Кнопки -----

        public static Button PrimaryButton(string text, string iconGlyph = null)
        {
            RoundedButton button = BaseButton(text, iconGlyph);
            button.FillNormal = Palette.Accent;
            button.FillHover = Palette.AccentHover;
            button.FillPress = Palette.AccentPressed;
            button.ForeColor = Palette.TextOnAccent;
            return button;
        }

        public static Button SecondaryButton(string text, string iconGlyph = null)
        {
            RoundedButton button = BaseButton(text, iconGlyph);
            button.FillNormal = Color.White;
            button.FillHover = Palette.PageBack;
            button.FillPress = Palette.Border;
            button.OutlineColor = Palette.Border;
            button.ForeColor = Palette.TextPrimary;
            return button;
        }

        public static Button DangerButton(string text, string iconGlyph = null)
        {
            RoundedButton button = BaseButton(text, iconGlyph);
            button.FillNormal = Color.White;
            button.FillHover = Palette.DangerSoft;
            button.FillPress = Palette.DangerSoft;
            button.OutlineColor = Palette.Border;
            button.ForeColor = Palette.Danger;
            return button;
        }

        private static RoundedButton BaseButton(string text, string iconGlyph)
        {
            var button = new RoundedButton
            {
                Text = string.IsNullOrEmpty(iconGlyph) ? text : iconGlyph + "  " + text,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(14, 7, 14, 7)
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

        /// <summary>
        /// Ближайший непрозрачный цвет фона вверх по дереву контролов.
        /// Нужен, чтобы скруглённые кнопки/карточки заливали углы цветом поверхности,
        /// на которой лежат (белая карточка или серый фон страницы).
        /// </summary>
        public static Color ResolveBackColor(Control start)
        {
            for (Control control = start; control != null; control = control.Parent)
            {
                if (control.BackColor.A == 255 && control.BackColor != Color.Transparent)
                {
                    return control.BackColor;
                }
            }
            return Palette.PageBack;
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
