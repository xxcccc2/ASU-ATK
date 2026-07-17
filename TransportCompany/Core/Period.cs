using System;

namespace TransportCompany.Core
{
    /// <summary>
    /// Период расчёта. Единая логика "текущий месяц / за год / произвольный"
    /// вместо копий в каждой форме.
    /// </summary>
    public sealed class Period
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public Period(DateTime start, DateTime end)
        {
            if (start.Date > end.Date)
            {
                throw new ArgumentException("Начальная дата не может быть позже конечной.");
            }

            Start = start.Date;
            End = end.Date;
        }

        public static Period CurrentMonth()
        {
            DateTime today = DateTime.Today;
            return new Period(new DateTime(today.Year, today.Month, 1), today);
        }

        public static Period CurrentYear()
        {
            DateTime today = DateTime.Today;
            return new Period(new DateTime(today.Year, 1, 1), today);
        }

        public override string ToString() => $"{Start:dd.MM.yyyy} — {End:dd.MM.yyyy}";
    }
}
