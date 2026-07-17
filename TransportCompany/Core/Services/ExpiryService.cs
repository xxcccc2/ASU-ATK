using System.Collections.Generic;
using TransportCompany.Core.Models;
using TransportCompany.Core.Repositories;

namespace TransportCompany.Core.Services
{
    /// <summary>Сводка предупреждений для главной страницы.</summary>
    public sealed class ExpirySummary
    {
        public List<MaintenanceRecord> OverdueMaintenance { get; } = new List<MaintenanceRecord>();
        public List<OsagoPolicy> ExpiringOsago { get; } = new List<OsagoPolicy>();
        public List<DriverLicenseRecord> ExpiringLicenses { get; } = new List<DriverLicenseRecord>();

        public bool HasWarnings =>
            OverdueMaintenance.Count > 0 || ExpiringOsago.Count > 0 || ExpiringLicenses.Count > 0;
    }

    /// <summary>
    /// Проверка истекающих сроков (ОСАГО, удостоверения, ТО).
    /// Вместо блокирующих MessageBox при запуске — данные для панели уведомлений.
    /// </summary>
    public sealed class ExpiryService
    {
        private readonly MaintenanceRepository _maintenance;
        private readonly FleetDocumentsRepository _documents;

        public ExpiryService(MaintenanceRepository maintenance, FleetDocumentsRepository documents)
        {
            _maintenance = maintenance;
            _documents = documents;
        }

        public ExpirySummary GetSummary()
        {
            var summary = new ExpirySummary();
            summary.OverdueMaintenance.AddRange(_maintenance.GetOverdue());
            summary.ExpiringOsago.AddRange(_documents.GetExpiringOsago());
            summary.ExpiringLicenses.AddRange(_documents.GetExpiringLicenses());
            return summary;
        }
    }
}
