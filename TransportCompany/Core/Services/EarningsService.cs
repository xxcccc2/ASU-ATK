using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;

namespace TransportCompany.Core.Services
{
    /// <summary>
    /// Заработок ТС и водителей + данные для сравнения.
    /// Зарплата считается в C# через ZoneRateService — SQL с зашитыми тарифами удалён.
    /// </summary>
    public sealed class EarningsService
    {
        private readonly Db _db;
        private readonly ZoneRateService _zoneRates;

        public EarningsService(Db db, ZoneRateService zoneRates)
        {
            _db = db;
            _zoneRates = zoneRates;
        }

        public EarningsRow GetVehicleEarnings(string gosNumber, Period period)
        {
            EarningsRow row = Aggregate("GosNumber", gosNumber, period);
            if (row != null)
            {
                row.VehicleNumber = gosNumber;
                row.DriverFullName = "";
            }
            return row;
        }

        public EarningsRow GetDriverEarnings(string driverFullName, Period period)
        {
            EarningsRow row = Aggregate("FIO", driverFullName, period);
            if (row != null)
            {
                row.VehicleNumber = "";
                row.DriverFullName = driverFullName;
            }
            return row;
        }

        public ComparisonData GetComparisonData(bool isVehicle, string identifier, Period period)
        {
            EarningsRow row = Aggregate(isVehicle ? "GosNumber" : "FIO", identifier, period)
                ?? new EarningsRow();

            return new ComparisonData
            {
                DisplayName = (isVehicle ? "ТС: " : "Водитель: ") + identifier,
                TripCount = row.TripCount,
                RevenueWithVat = row.TotalWithVat,
                RevenueWithoutVat = row.TotalWithoutVat,
                Salary = row.Salary
            };
        }

        private EarningsRow Aggregate(string filterColumn, string identifier, Period period)
        {
            // filterColumn ограничен фиксированным набором — не пользовательский ввод.
            if (filterColumn != "GosNumber" && filterColumn != "FIO")
            {
                throw new ArgumentException("Недопустимая колонка фильтра.", nameof(filterColumn));
            }

            DataTable trips = _db.Query(
                $@"SELECT Zone, TotalWithoutVAT, TotalWithVAT
                   FROM TransportRegistry
                   WHERE {filterColumn} = @Identifier AND [Date] BETWEEN @StartDate AND @EndDate",
                new SqlParameter("@Identifier", SqlDbType.NVarChar, 200) { Value = identifier },
                new SqlParameter("@StartDate", SqlDbType.Date) { Value = period.Start },
                new SqlParameter("@EndDate", SqlDbType.Date) { Value = period.End });

            if (trips.Rows.Count == 0)
            {
                return null;
            }

            Dictionary<int, decimal> rates = _zoneRates.GetAllRates();
            var result = new EarningsRow();

            foreach (DataRow row in trips.Rows)
            {
                result.TripCount++;
                result.TotalWithoutVat += row["TotalWithoutVAT"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TotalWithoutVAT"]);
                result.TotalWithVat += row["TotalWithVAT"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TotalWithVAT"]);

                if (row["Zone"] != DBNull.Value)
                {
                    int zone = Convert.ToInt32(row["Zone"]);
                    result.Salary += rates.TryGetValue(zone, out decimal rate) ? rate : 0m;
                }
            }

            return result;
        }
    }
}
