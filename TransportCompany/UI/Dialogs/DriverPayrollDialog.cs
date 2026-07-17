using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Dialogs
{
    /// <summary>Расчётный листок водителя: смены и суммы в разрезе зон.</summary>
    public sealed class DriverPayrollDialog : Form
    {
        public DriverPayrollDialog(AppServices services, string driverFullName, Period period)
        {
            Text = $"Расчётный листок — {driverFullName}";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(560, 440);
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

            var grid = new DataGridView { Location = new Point(20, 66), Size = new Size(520, 300) };
            Styler.Grid(grid);
            grid.ReadOnly = true;
            grid.Columns.Add("Zone", "Зона");
            grid.Columns.Add("Shifts", "Количество смен");
            grid.Columns.Add("Rate", "Тариф (руб.)");
            grid.Columns.Add("Total", "Стоимость (руб.)");

            var totalLabel = new Label
            {
                Font = Fonts.TotalValue,
                ForeColor = Palette.Accent,
                AutoSize = true,
                Location = new Point(20, 378)
            };

            Button close = Styler.SecondaryButton("Закрыть");
            close.Location = new Point(450, 396);
            close.Click += (s, e) => Close();

            Controls.Add(header);
            Controls.Add(periodLabel);
            Controls.Add(grid);
            Controls.Add(totalLabel);
            Controls.Add(close);
            CancelButton = close;

            try
            {
                List<PayrollLine> lines = services.Payroll.GetPayrollLines(driverFullName, period);
                decimal total = 0;
                foreach (PayrollLine line in lines)
                {
                    grid.Rows.Add(line.Zone, line.ShiftCount, line.Rate.ToString("N0"), line.Total.ToString("N0"));
                    total += line.Total;
                }
                grid.ClearSelection();
                totalLabel.Text = $"Итого заработок: {total:N0} ₽";
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось загрузить расчётный листок.", ex);
            }
        }
    }
}
