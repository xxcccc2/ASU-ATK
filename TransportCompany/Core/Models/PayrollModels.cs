using System.Collections.Generic;

namespace TransportCompany.Core.Models
{
    /// <summary>Строка результата расчёта зарплаты: водитель и сумма за период.</summary>
    public sealed class DriverSalaryRow
    {
        public string DriverFullName { get; set; }
        public int TripCount { get; set; }
        public decimal Salary { get; set; }
    }

    /// <summary>Строка расчётного листка: зона, число смен, тариф, сумма.</summary>
    public sealed class PayrollLine
    {
        public int Zone { get; set; }
        public int ShiftCount { get; set; }
        public decimal Rate { get; set; }
        public decimal Total => Rate * ShiftCount;
    }

    /// <summary>Сводная статистика по водителю за период (по месяцам).</summary>
    public sealed class DriverStatistics
    {
        public string DriverFullName { get; set; }
        public int TotalTrips { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal MaxMonthlySalary { get; set; }
        public decimal MinMonthlySalary { get; set; }
        public decimal AvgMonthlySalary { get; set; }
        public int MaxMonthlyTrips { get; set; }
        public int MinMonthlyTrips { get; set; }
        public double AvgMonthlyTrips { get; set; }
        public decimal AvgRevenueWithVat { get; set; }
        public decimal AvgRevenueWithoutVat { get; set; }
        public string MostFrequentZones { get; set; }
        public string LeastFrequentZones { get; set; }
    }

    /// <summary>Заработок ТС или водителя за период.</summary>
    public sealed class EarningsRow
    {
        public string VehicleNumber { get; set; }
        public string DriverFullName { get; set; }
        public int TripCount { get; set; }
        public decimal TotalWithoutVat { get; set; }
        public decimal TotalWithVat { get; set; }
        public decimal Salary { get; set; }
    }

    /// <summary>Показатели одного объекта (ТС или водителя) для сравнения.</summary>
    public sealed class ComparisonData
    {
        public string DisplayName { get; set; }
        public int TripCount { get; set; }
        public decimal RevenueWithVat { get; set; }
        public decimal RevenueWithoutVat { get; set; }
        public decimal Salary { get; set; }
    }

    /// <summary>Статистика рейсов по зонам за один месяц (типизированная замена dynamic/Tag).</summary>
    public sealed class ZoneMonthStatistics
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalTrips { get; set; }

        /// <summary>Госномер → (зона → количество рейсов). Все зоны 0..10 присутствуют.</summary>
        public SortedDictionary<string, Dictionary<int, int>> TripsByVehicle { get; } =
            new SortedDictionary<string, Dictionary<int, int>>();

        /// <summary>Зона → суммарное количество рейсов за месяц.</summary>
        public Dictionary<int, int> ZoneTotals { get; } = new Dictionary<int, int>();

        /// <summary>Зона → доля рейсов в процентах.</summary>
        public Dictionary<int, double> ZonePercentages { get; } = new Dictionary<int, double>();
    }

    public sealed class ZoneAnalysisResult
    {
        public List<ZoneMonthStatistics> Months { get; } = new List<ZoneMonthStatistics>();
    }
}
