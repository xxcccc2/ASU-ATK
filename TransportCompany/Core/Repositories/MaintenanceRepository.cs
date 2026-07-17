using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;

namespace TransportCompany.Core.Repositories
{
    public sealed class MaintenanceRepository
    {
        private readonly Db _db;

        public MaintenanceRepository(Db db)
        {
            _db = db;
        }

        public List<MaintenanceRecord> GetAll()
        {
            DataTable table = _db.Query(
                @"SELECT id, [Номер машины], [Дата последнего ТО], [Пробег (км)], [Комментарий]
                  FROM Техобслуживание
                  ORDER BY [Дата последнего ТО] DESC");

            var records = new List<MaintenanceRecord>();
            foreach (DataRow row in table.Rows)
            {
                records.Add(new MaintenanceRecord
                {
                    Id = Convert.ToInt32(row["id"]),
                    CarNumber = row["Номер машины"].ToString(),
                    LastServiceDate = row["Дата последнего ТО"] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(row["Дата последнего ТО"]),
                    MileageKm = row["Пробег (км)"] == DBNull.Value ? 0 : Convert.ToInt32(row["Пробег (км)"]),
                    Comment = row["Комментарий"] == DBNull.Value ? "" : row["Комментарий"].ToString()
                });
            }
            return records;
        }

        public void Add(MaintenanceRecord record)
        {
            _db.Execute(
                @"INSERT INTO Техобслуживание ([Номер машины], [Дата последнего ТО], [Пробег (км)], [Комментарий])
                  VALUES (@CarNumber, @Date, @Mileage, @Comment)",
                new SqlParameter("@CarNumber", SqlDbType.NVarChar, 20) { Value = record.CarNumber },
                new SqlParameter("@Date", SqlDbType.Date) { Value = record.LastServiceDate },
                new SqlParameter("@Mileage", SqlDbType.Int) { Value = record.MileageKm },
                new SqlParameter("@Comment", SqlDbType.NVarChar, 500) { Value = (object)record.Comment ?? DBNull.Value });
        }

        public void Update(MaintenanceRecord record)
        {
            _db.Execute(
                @"UPDATE Техобслуживание
                  SET [Номер машины] = @CarNumber,
                      [Дата последнего ТО] = @Date,
                      [Пробег (км)] = @Mileage,
                      [Комментарий] = @Comment
                  WHERE id = @Id",
                new SqlParameter("@CarNumber", SqlDbType.NVarChar, 20) { Value = record.CarNumber },
                new SqlParameter("@Date", SqlDbType.Date) { Value = record.LastServiceDate },
                new SqlParameter("@Mileage", SqlDbType.Int) { Value = record.MileageKm },
                new SqlParameter("@Comment", SqlDbType.NVarChar, 500) { Value = (object)record.Comment ?? DBNull.Value },
                new SqlParameter("@Id", SqlDbType.Int) { Value = record.Id });
        }

        public void Delete(int id)
        {
            _db.Execute("DELETE FROM Техобслуживание WHERE id = @Id",
                new SqlParameter("@Id", SqlDbType.Int) { Value = id });
        }

        public List<MaintenanceRecord> GetOverdue()
        {
            var overdue = new List<MaintenanceRecord>();
            foreach (MaintenanceRecord record in GetAll())
            {
                if (record.IsOverdue)
                {
                    overdue.Add(record);
                }
            }
            return overdue;
        }
    }
}
