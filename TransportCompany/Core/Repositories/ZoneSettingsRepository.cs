using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TransportCompany.Core.Data;

namespace TransportCompany.Core.Repositories
{
    public sealed class ZoneSettingChange
    {
        public int ZoneId { get; set; }
        public decimal OldCost { get; set; }
        public decimal NewCost { get; set; }
    }

    public sealed class ZoneSettingsRepository
    {
        private readonly Db _db;

        public ZoneSettingsRepository(Db db)
        {
            _db = db;
        }

        /// <summary>Текущие тарифы зон из БД. Пустой словарь, если таблица не заполнена.</summary>
        public Dictionary<int, decimal> GetZoneCosts()
        {
            var costs = new Dictionary<int, decimal>();
            DataTable table = _db.Query("SELECT ZoneId, Cost FROM ZoneSettings");
            foreach (DataRow row in table.Rows)
            {
                costs[Convert.ToInt32(row["ZoneId"])] = Convert.ToDecimal(row["Cost"]);
            }
            return costs;
        }

        public DataTable GetZoneSettingsWithDates()
        {
            return _db.Query("SELECT ZoneId, Cost, UpdatedDate FROM ZoneSettings ORDER BY ZoneId");
        }

        /// <summary>
        /// Сохраняет изменённые тарифы одной транзакцией: история + обновление для каждой зоны.
        /// Либо применяются все изменения, либо ни одного.
        /// </summary>
        public void SaveZoneCosts(IEnumerable<ZoneSettingChange> changes, string changedBy)
        {
            _db.InTransaction((connection, transaction) =>
            {
                foreach (ZoneSettingChange change in changes)
                {
                    using (SqlCommand history = new SqlCommand(
                        @"INSERT INTO ZoneCostHistory (ZoneId, OldCost, NewCost, ChangedBy)
                          VALUES (@ZoneId, @OldCost, @NewCost, @ChangedBy)",
                        connection, transaction))
                    {
                        history.Parameters.Add("@ZoneId", SqlDbType.Int).Value = change.ZoneId;
                        history.Parameters.Add("@OldCost", SqlDbType.Decimal).Value = change.OldCost;
                        history.Parameters.Add("@NewCost", SqlDbType.Decimal).Value = change.NewCost;
                        history.Parameters.Add("@ChangedBy", SqlDbType.NVarChar, 100).Value = changedBy;
                        history.ExecuteNonQuery();
                    }

                    using (SqlCommand upsert = new SqlCommand(
                        @"IF EXISTS (SELECT 1 FROM ZoneSettings WHERE ZoneId = @ZoneId)
                              UPDATE ZoneSettings SET Cost = @Cost, UpdatedDate = GETDATE() WHERE ZoneId = @ZoneId
                          ELSE
                              INSERT INTO ZoneSettings (ZoneId, Cost) VALUES (@ZoneId, @Cost)",
                        connection, transaction))
                    {
                        upsert.Parameters.Add("@ZoneId", SqlDbType.Int).Value = change.ZoneId;
                        upsert.Parameters.Add("@Cost", SqlDbType.Decimal).Value = change.NewCost;
                        upsert.ExecuteNonQuery();
                    }
                }
            });
        }

        public DataTable GetHistory()
        {
            return _db.Query(
                @"SELECT ChangeDate, ZoneId, OldCost, NewCost, ChangedBy
                  FROM ZoneCostHistory ORDER BY ChangeDate DESC");
        }
    }
}
