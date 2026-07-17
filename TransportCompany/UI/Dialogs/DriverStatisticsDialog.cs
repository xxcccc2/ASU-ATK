using System.Drawing;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Dialogs
{
    /// <summary>Сводная статистика по водителю за период.</summary>
    public sealed class DriverStatisticsDialog : Form
    {
        public DriverStatisticsDialog(AppServices services, string driverFullName, Period period)
        {
            Text = $"Статистика — {driverFullName}";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(520, 470);
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            BackColor = Palette.PageBack;
            Font = Fonts.Body;
            AutoScaleMode = AutoScaleMode.Font;

            var header = Styler.SectionTitle(driverFullName);
            header.Location = new Point(20, 14);
            var periodLabel = Styler.MutedLabel($"Период: {period}");
            periodLabel.Location = new Point(21, 40);
            Controls.Add(header);
            Controls.Add(periodLabel);

            Button close = Styler.SecondaryButton("Закрыть");
            close.Location = new Point(410, 424);
            close.Click += (s, e) => Close();
            Controls.Add(close);
            CancelButton = close;

            DriverStatistics statistics;
            try
            {
                statistics = services.Payroll.GetDriverStatistics(driverFullName, period);
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось загрузить статистику.", ex);
                return;
            }

            if (statistics == null)
            {
                var empty = Styler.BodyLabel("Нет данных за выбранный период.");
                empty.Location = new Point(21, 76);
                Controls.Add(empty);
                return;
            }

            int y = 76;
            AddStat(ref y, "Общее количество рейсов", statistics.TotalTrips.ToString("N0"));
            AddStat(ref y, "Общий заработок", statistics.TotalSalary.ToString("N0") + " ₽");
            AddStat(ref y, "Максимальный заработок за месяц", statistics.MaxMonthlySalary.ToString("N0") + " ₽");
            AddStat(ref y, "Минимальный заработок за месяц", statistics.MinMonthlySalary.ToString("N0") + " ₽");
            AddStat(ref y, "Средний заработок за месяц", statistics.AvgMonthlySalary.ToString("N0") + " ₽");
            AddStat(ref y, "Максимум рейсов за месяц", statistics.MaxMonthlyTrips.ToString());
            AddStat(ref y, "Минимум рейсов за месяц", statistics.MinMonthlyTrips.ToString());
            AddStat(ref y, "Среднее число рейсов за месяц", statistics.AvgMonthlyTrips.ToString("F1"));
            AddStat(ref y, "Средняя выручка с НДС за рейс", statistics.AvgRevenueWithVat.ToString("N2") + " ₽");
            AddStat(ref y, "Средняя выручка без НДС за рейс", statistics.AvgRevenueWithoutVat.ToString("N2") + " ₽");
            AddStat(ref y, "Самая частая зона доставки", statistics.MostFrequentZones);
            AddStat(ref y, "Самая редкая зона доставки", statistics.LeastFrequentZones);
        }

        private void AddStat(ref int y, string title, string value)
        {
            var titleLabel = new Label
            {
                Text = title,
                Font = Fonts.Body,
                ForeColor = Palette.TextSecondary,
                AutoSize = true,
                Location = new Point(21, y)
            };
            var valueLabel = new Label
            {
                Text = value,
                Font = Fonts.BodyBold,
                ForeColor = Palette.TextPrimary,
                AutoSize = true,
                Location = new Point(320, y)
            };
            Controls.Add(titleLabel);
            Controls.Add(valueLabel);
            y += 28;
        }
    }
}
