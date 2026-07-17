using TransportCompany.Core.Data;
using TransportCompany.Core.Repositories;
using TransportCompany.Core.Services;
using TransportCompany.Core.Services.Import;

namespace TransportCompany.Core
{
    /// <summary>
    /// Композиция зависимостей приложения (ручной DI, собирается один раз в Program).
    /// Формы и страницы получают сервисы отсюда, а не создают SqlConnection сами.
    /// </summary>
    public sealed class AppServices
    {
        public Db Db { get; }

        public MaintenanceRepository Maintenance { get; }
        public FleetDocumentsRepository FleetDocuments { get; }
        public RegistryRepository Registry { get; }
        public ZoneSettingsRepository ZoneSettings { get; }

        public ZoneRateService ZoneRates { get; }
        public PayrollService Payroll { get; }
        public EarningsService Earnings { get; }
        public ZoneAnalysisService ZoneAnalysis { get; }
        public RegistryImportService RegistryImport { get; }
        public ExpiryService Expiry { get; }

        public AppServices(IDbConnectionFactory connectionFactory)
        {
            Db = new Db(connectionFactory);

            Maintenance = new MaintenanceRepository(Db);
            FleetDocuments = new FleetDocumentsRepository(Db);
            Registry = new RegistryRepository(Db);
            ZoneSettings = new ZoneSettingsRepository(Db);

            ZoneRates = new ZoneRateService(ZoneSettings);
            Payroll = new PayrollService(Db, ZoneRates);
            Earnings = new EarningsService(Db, ZoneRates);
            ZoneAnalysis = new ZoneAnalysisService(Db);
            RegistryImport = new RegistryImportService(Registry);
            Expiry = new ExpiryService(Maintenance, FleetDocuments);
        }
    }
}
