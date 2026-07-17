using System;
using System.Collections.Generic;
using TransportCompany.Core.Logging;
using TransportCompany.Core.Repositories;

namespace TransportCompany.Core.Services
{
    /// <summary>
    /// ЕДИНСТВЕННЫЙ источник тарифов зон для всех расчётов
    /// (зарплата, заработок, сравнение, расчётный листок).
    /// Раньше тарифы дублировались в 4 местах: switch в двух формах,
    /// CASE в SQL сравнения и ZoneSettingsManager со своими значениями.
    /// </summary>
    public sealed class ZoneRateService
    {
        /// <summary>
        /// Исторические тарифы, зашитые в прежних расчётах зарплаты.
        /// Используются, когда таблица ZoneSettings недоступна или пуста.
        /// </summary>
        public static readonly IReadOnlyDictionary<int, decimal> DefaultRates =
            new Dictionary<int, decimal>
            {
                { 0, 2700m }, { 1, 2700m }, { 2, 3200m }, { 3, 3600m }, { 4, 4000m },
                { 5, 5000m }, { 6, 6000m }, { 7, 7000m }, { 8, 8000m },
                { 9, 9000m }, { 10, 10000m }
            };

        private static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(5);

        private readonly ZoneSettingsRepository _repository;
        private Dictionary<int, decimal> _cache;
        private DateTime _cacheLoadedAt = DateTime.MinValue;

        public ZoneRateService(ZoneSettingsRepository repository)
        {
            _repository = repository;
        }

        public decimal GetRate(int zone)
        {
            Dictionary<int, decimal> rates = GetAllRates();
            return rates.TryGetValue(zone, out decimal rate) ? rate : 0m;
        }

        public Dictionary<int, decimal> GetAllRates()
        {
            if (_cache != null && DateTime.Now - _cacheLoadedAt < CacheLifetime)
            {
                return new Dictionary<int, decimal>(_cache);
            }

            var rates = new Dictionary<int, decimal>(
                (IDictionary<int, decimal>)DefaultRates);

            try
            {
                Dictionary<int, decimal> fromDb = _repository.GetZoneCosts();
                if (fromDb.Count > 0)
                {
                    foreach (KeyValuePair<int, decimal> pair in fromDb)
                    {
                        rates[pair.Key] = pair.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Warn("Не удалось загрузить тарифы зон из БД, используются тарифы по умолчанию: " + ex.Message);
            }

            _cache = new Dictionary<int, decimal>(rates);
            _cacheLoadedAt = DateTime.Now;
            return rates;
        }

        public void InvalidateCache()
        {
            _cache = null;
        }
    }
}
