using System;

namespace TransportCompany.Core.Models
{
    public sealed class MaintenanceRecord
    {
        public int Id { get; set; }
        public string CarNumber { get; set; }
        public DateTime LastServiceDate { get; set; }
        public int MileageKm { get; set; }
        public string Comment { get; set; }

        /// <summary>ТО считается просроченным, если прошло больше 3 месяцев.</summary>
        public bool IsOverdue => LastServiceDate.AddMonths(3) < DateTime.Today;
    }
}
