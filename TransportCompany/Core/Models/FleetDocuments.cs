using System;

namespace TransportCompany.Core.Models
{
    public sealed class OsagoPolicy
    {
        public int Id { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsExpiringSoon => EndDate >= DateTime.Today && EndDate <= DateTime.Today.AddDays(30);
        public bool IsExpired => EndDate < DateTime.Today;
    }

    public sealed class DriverLicenseRecord
    {
        public int Id { get; set; }
        public string DriverFullName { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public bool IsExpiringSoon => ExpiryDate >= DateTime.Today && ExpiryDate <= DateTime.Today.AddDays(30);
        public bool IsExpired => ExpiryDate < DateTime.Today;
    }
}
