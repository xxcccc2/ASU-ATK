using System;
using System.Drawing;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Controls
{
    /// <summary>
    /// Выбор периода: текущий месяц / за год / произвольный + даты.
    /// Один контрол вместо одинакового кода в четырёх формах.
    /// </summary>
    public sealed class PeriodSelector : UserControl
    {
        private readonly RadioButton _currentMonth = new RadioButton();
        private readonly RadioButton _currentYear = new RadioButton();
        private readonly RadioButton _custom = new RadioButton();
        private readonly DateTimePicker _start = new DateTimePicker();
        private readonly DateTimePicker _end = new DateTimePicker();

        public event EventHandler PeriodChanged;

        public PeriodSelector()
        {
            Size = new Size(560, 84);
            BackColor = Color.Transparent;
            Font = Fonts.Body;

            _currentMonth.Text = "Текущий месяц";
            _currentYear.Text = "За год";
            _custom.Text = "Произвольный период";

            RadioButton[] radios = { _currentMonth, _currentYear, _custom };
            int y = 2;
            foreach (RadioButton radio in radios)
            {
                radio.AutoSize = true;
                radio.Location = new Point(0, y);
                radio.Font = Fonts.Body;
                radio.ForeColor = Palette.TextPrimary;
                radio.CheckedChanged += OnModeChanged;
                Controls.Add(radio);
                y += 26;
            }

            var startLabel = new Label
            {
                Text = "Начальная дата:",
                Font = Fonts.Small,
                ForeColor = Palette.TextSecondary,
                AutoSize = true,
                Location = new Point(200, 4)
            };
            var endLabel = new Label
            {
                Text = "Конечная дата:",
                Font = Fonts.Small,
                ForeColor = Palette.TextSecondary,
                AutoSize = true,
                Location = new Point(380, 4)
            };

            _start.Format = DateTimePickerFormat.Short;
            _end.Format = DateTimePickerFormat.Short;
            _start.Width = 160;
            _end.Width = 160;
            _start.Location = new Point(202, 24);
            _end.Location = new Point(382, 24);
            _start.Font = Fonts.Body;
            _end.Font = Fonts.Body;
            _start.ValueChanged += (s, e) => { if (_custom.Checked) PeriodChanged?.Invoke(this, EventArgs.Empty); };
            _end.ValueChanged += (s, e) => { if (_custom.Checked) PeriodChanged?.Invoke(this, EventArgs.Empty); };

            Controls.Add(startLabel);
            Controls.Add(endLabel);
            Controls.Add(_start);
            Controls.Add(_end);

            _currentMonth.Checked = true;
        }

        /// <summary>Текущий выбранный период. Бросает ArgumentException при неверных датах.</summary>
        public Period GetPeriod()
        {
            if (_currentMonth.Checked)
            {
                return Period.CurrentMonth();
            }
            if (_currentYear.Checked)
            {
                return Period.CurrentYear();
            }
            return new Period(_start.Value, _end.Value);
        }

        private void OnModeChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
            {
                return;
            }

            bool custom = _custom.Checked;
            _start.Enabled = custom;
            _end.Enabled = custom;

            if (_currentMonth.Checked)
            {
                Period period = Period.CurrentMonth();
                _start.Value = period.Start;
                _end.Value = period.End;
            }
            else if (_currentYear.Checked)
            {
                Period period = Period.CurrentYear();
                _start.Value = period.Start;
                _end.Value = period.End;
            }

            PeriodChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
