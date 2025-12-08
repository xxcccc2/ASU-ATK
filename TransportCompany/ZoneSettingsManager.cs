using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TransportCompany
{
    /// <summary>
    /// Менеджер настроек стоимости зон.
    /// Используется для получения актуальных стоимостей зон из БД.
    /// </summary>
    public static class ZoneSettingsManager
    {
        private static Dictionary<int, decimal> _cachedCosts = null;
        private static DateTime _lastCacheUpdate = DateTime.MinValue;
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Получить стоимость конкретной зоны
        /// </summary>
        /// <param name="zoneId">ID зоны (0-10)</param>
        /// <returns>Стоимость зоны</returns>
        public static decimal GetZoneCost(int zoneId)
        {
            var costs = GetAllZoneCosts();
            if (costs.ContainsKey(zoneId))
                return costs[zoneId];
            
            // Значение по умолчанию если зона не найдена
            return zoneId * 100m;
        }

        /// <summary>
        /// Получить все стоимости зон
        /// </summary>
        /// <returns>Словарь: ZoneId -> Cost</returns>
        public static Dictionary<int, decimal> GetAllZoneCosts()
        {
            // Проверяем кэш
            if (_cachedCosts != null && DateTime.Now - _lastCacheUpdate < CacheExpiration)
            {
                return new Dictionary<int, decimal>(_cachedCosts);
            }

            return LoadZoneCostsFromDatabase();
        }

        /// <summary>
        /// Принудительно обновить кэш стоимостей
        /// </summary>
        public static void RefreshCache()
        {
            _cachedCosts = null;
            LoadZoneCostsFromDatabase();
        }

        /// <summary>
        /// Получить стоимость зоны на определенную дату (из истории)
        /// </summary>
        /// <param name="zoneId">ID зоны</param>
        /// <param name="date">Дата</param>
        /// <returns>Стоимость зоны на указанную дату</returns>
        public static decimal GetZoneCostAtDate(int zoneId, DateTime date)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.connectionString))
                {
                    connection.Open();
                    
                    // Ищем последнее изменение до указанной даты
                    string query = @"
                        SELECT TOP 1 NewCost 
                        FROM ZoneCostHistory 
                        WHERE ZoneId = @ZoneId AND ChangeDate <= @Date
                        ORDER BY ChangeDate DESC";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ZoneId", zoneId);
                        cmd.Parameters.AddWithValue("@Date", date);
                        
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch
            {
                // В случае ошибки возвращаем текущую стоимость
            }

            return GetZoneCost(zoneId);
        }

        private static Dictionary<int, decimal> LoadZoneCostsFromDatabase()
        {
            var costs = new Dictionary<int, decimal>();

            // Значения по умолчанию
            for (int i = 0; i <= 10; i++)
            {
                costs[i] = i * 100m;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(Config.connectionString))
                {
                    connection.Open();
                    string query = "SELECT ZoneId, Cost FROM ZoneSettings";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int zoneId = reader.GetInt32(0);
                            decimal cost = reader.GetDecimal(1);
                            costs[zoneId] = cost;
                        }
                    }
                }

                _cachedCosts = new Dictionary<int, decimal>(costs);
                _lastCacheUpdate = DateTime.Now;
            }
            catch
            {
                // Если таблица не существует или ошибка - используем значения по умолчанию
            }

            return costs;
        }
    }
}
