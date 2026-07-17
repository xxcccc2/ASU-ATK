using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;

namespace TransportCompany.Core.Services
{
    /// <summary>Расчёт зарплаты водителей. Вся логика вынесена из форм.</summary>
    public sealed class PayrollService
    {
        private readonly Db _db;
        private readonly ZoneRateService _zoneRates;

        public PayrollService(Db db, ZoneRateService zoneRates)
        {
            _db = db;
            _zoneRates = zoneRates;
        }

        /// <summary>Зарплата каждого водителя за период: один рейс = тариф зоны рейса.</summary>
        public List<DriverSalaryRow> CalculateSalaries(Period period)
        {
            DataTable trips = _db.Query(
                @"SELECT FIO, Zone
                  FROM TransportRegistry
                  WHERE Date BETWEEN @StartDate AND @EndDate
                    AND Zone IS NOT NULL
                    AND FIO IS NOT NULL AND LEN(FIO) > 2 AND FIO NOT LIKE '%[0-9]%'
                    AND FIO NOT LIKE '%[/\\]%'",
                new SqlParameter("@StartDate", SqlDbType.Date) { Value = period.Start },
                new SqlParameter("@EndDate", SqlDbType.Date) { Value = period.End });

            Dictionary<int, decimal> rates = _zoneRates.GetAllRates();
            var salaries = new Dictionary<string, DriverSalaryRow>();

            foreach (DataRow row in trips.Rows)
            {
                string fio = row["FIO"].ToString().Trim();
                int zone = Convert.ToInt32(row["Zone"]);

                if (!salaries.TryGetValue(fio, out DriverSalaryRow salary))
                {
                    salary = new DriverSalaryRow { DriverFullName = fio };
                    salaries[fio] = salary;
                }

                salary.TripCount++;
                salary.Salary += rates.TryGetValue(zone, out decimal rate) ? rate : 0m;
            }

            return salaries.Values.OrderBy(s => s.DriverFullName).ToList();
        }

        /// <summary>Расчётный листок водителя: смены и суммы в разрезе зон.</summary>
        public List<PayrollLine> GetPayrollLines(string driverFullName, Period period)
        {
            DataTable table = _db.Query(
                @"SELECT Zone, COUNT(*) AS ShiftCount
                  FROM TransportRegistry
                  WHERE FIO = @FIO AND Date BETWEEN @StartDate AND @EndDate AND Zone IS NOT NULL
                  GROUP BY Zone ORDER BY Zone",
                new SqlParameter("@FIO", SqlDbType.NVarChar, 200) { Value = driverFullName },
                new SqlParameter("@StartDate", SqlDbType.Date) { Value = period.Start },
                new SqlParameter("@EndDate", SqlDbType.Date) { Value = period.End });

            Dictionary<int, decimal> rates = _zoneRates.GetAllRates();
            var lines = new List<PayrollLine>();
            foreach (DataRow row in table.Rows)
            {
                int zone = Convert.ToInt32(row["Zone"]);
                lines.Add(new PayrollLine
                {
                    Zone = zone,
                    ShiftCount = Convert.ToInt32(row["ShiftCount"]),
                    Rate = rates.TryGetValue(zone, out decimal rate) ? rate : 0m
                });
            }
            return lines;
        }

        /// <summary>Сводная статистика по водителю за период (в разрезе месяцев).</summary>
        public DriverStatistics GetDriverStatistics(string driverFullName, Period period)
        {
            DataTable table = _db.Query(
                @"SELECT Date, Zone, TotalWithVAT, TotalWithoutVAT
                  FROM TransportRegistry
                  WHERE FIO = @FIO AND Date BETWEEN @StartDate AND @EndDate AND Zone IS NOT NULL",
                new SqlParameter("@FIO", SqlDbType.NVarChar, 200) { Value = driverFullName },
                new SqlParameter("@StartDate", SqlDbType.Date) { Value = period.Start },
                new SqlParameter("@EndDate", SqlDbType.Date) { Value = period.End });

            if (table.Rows.Count == 0)
            {
                return null;
            }

            Dictionary<int, decimal> rates = _zoneRates.GetAllRates();
            var monthlySalary = new Dictionary<(int Year, int Month), decimal>();
            var monthlyTrips = new Dictionary<(int Year, int Month), int>();
            var zones = new List<int>();
            decimal totalWithVat = 0, totalWithoutVat = 0, totalSalary = 0;

            foreach (DataRow row in table.Rows)
            {
                DateTime date = Convert.ToDateTime(row["Date"]);
                int zone = Convert.ToInt32(row["Zone"]);
                var monthKey = (date.Year, date.Month);

                decimal rate = rates.TryGetValue(zone, out decimal r) ? r : 0m;
                monthlySalary[monthKey] = monthlySalary.TryGetValue(monthKey, out decimal s) ? s + rate : rate;
                monthlyTrips[monthKey] = monthlyTrips.TryGetValue(monthKey, out int t) ? t + 1 : 1;
                totalSalary += rate;

                zones.Add(zone);
                totalWithVat += row["TotalWithVAT"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TotalWithVAT"]);
                totalWithoutVat += row["TotalWithoutVAT"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TotalWithoutVAT"]);
            }

            int totalTrips = zones.Count;

            var zoneGroups = zones.GroupBy(z => z)
                .OrderByDescending(g => g.Count()).ThenBy(g => g.Key)
                .ToList();
            int maxZoneCount = zoneGroups.First().Count();
            int minZoneCount = zoneGroups.Last().Count();

            return new DriverStatistics
            {
                DriverFullName = driverFullName,
                TotalTrips = totalTrips,
                TotalSalary = totalSalary,
                MaxMonthlySalary = monthlySalary.Values.Max(),
                MinMonthlySalary = monthlySalary.Values.Min(),
                AvgMonthlySalary = monthlySalary.Values.Average(),
                MaxMonthlyTrips = monthlyTrips.Values.Max(),
                MinMonthlyTrips = monthlyTrips.Values.Min(),
                AvgMonthlyTrips = monthlyTrips.Values.Average(),
                AvgRevenueWithVat = totalTrips > 0 ? totalWithVat / totalTrips : 0,
                AvgRevenueWithoutVat = totalTrips > 0 ? totalWithoutVat / totalTrips : 0,
                MostFrequentZones = string.Join(", ",
                    zoneGroups.Where(g => g.Count() == maxZoneCount).Select(g => g.Key.ToString())),
                LeastFrequentZones = string.Join(", ",
                    zoneGroups.Where(g => g.Count() == minZoneCount).Select(g => g.Key.ToString()))
            };
        }
    }
}
