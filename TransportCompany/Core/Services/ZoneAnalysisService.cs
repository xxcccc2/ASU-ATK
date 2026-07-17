using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;

namespace TransportCompany.Core.Services
{
    /// <summary>
    /// Анализ распределения рейсов по зонам. Возвращает типизированную модель
    /// вместо вложенных словарей в Form.Tag с dynamic.
    /// </summary>
    public sealed class ZoneAnalysisService
    {
        public const int MinZone = 0;
        public const int MaxZone = 10;

        private readonly Db _db;

        public ZoneAnalysisService(Db db)
        {
            _db = db;
        }

        public ZoneAnalysisResult Analyze(Period period)
        {
            DataTable trips = _db.Query(
                @"SELECT GosNumber, Zone, YEAR([Date]) AS TripYear, MONTH([Date]) AS TripMonth
                  FROM TransportRegistry
                  WHERE [Date] BETWEEN @StartDate AND @EndDate
                    AND GosNumber IS NOT NULL AND GosNumber <> ''
                    AND Zone IS NOT NULL",
                new SqlParameter("@StartDate", SqlDbType.Date) { Value = period.Start },
                new SqlParameter("@EndDate", SqlDbType.Date) { Value = period.End });

            var byMonth = new Dictionary<(int Year, int Month), ZoneMonthStatistics>();

            foreach (DataRow row in trips.Rows)
            {
                string gosNumber = row["GosNumber"].ToString().Trim();
                int zone = Convert.ToInt32(row["Zone"]);
                var key = (Year: Convert.ToInt32(row["TripYear"]), Month: Convert.ToInt32(row["TripMonth"]));

                if (!byMonth.TryGetValue(key, out ZoneMonthStatistics month))
                {
                    month = new ZoneMonthStatistics { Year = key.Year, Month = key.Month };
                    byMonth[key] = month;
                }

                if (!month.TripsByVehicle.TryGetValue(gosNumber, out Dictionary<int, int> zonesOfVehicle))
                {
                    zonesOfVehicle = new Dictionary<int, int>();
                    for (int z = MinZone; z <= MaxZone; z++)
                    {
                        zonesOfVehicle[z] = 0;
                    }
                    month.TripsByVehicle[gosNumber] = zonesOfVehicle;
                }

                if (!zonesOfVehicle.ContainsKey(zone))
                {
                    zonesOfVehicle[zone] = 0;
                }

                zonesOfVehicle[zone]++;
                month.TotalTrips++;
            }

            var result = new ZoneAnalysisResult();
            foreach (ZoneMonthStatistics month in byMonth.Values
                .OrderBy(m => m.Year).ThenBy(m => m.Month))
            {
                for (int zone = MinZone; zone <= MaxZone; zone++)
                {
                    int total = month.TripsByVehicle.Values.Sum(v => v.TryGetValue(zone, out int c) ? c : 0);
                    month.ZoneTotals[zone] = total;
                    month.ZonePercentages[zone] = month.TotalTrips > 0
                        ? total * 100.0 / month.TotalTrips
                        : 0;
                }
                result.Months.Add(month);
            }

            return result;
        }
    }
}
