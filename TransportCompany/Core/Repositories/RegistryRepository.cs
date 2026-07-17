using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;

namespace TransportCompany.Core.Repositories
{
    /// <summary>Доступ к реестру рейсов TransportRegistry и журналу импорта.</summary>
    public sealed class RegistryRepository
    {
        private readonly Db _db;

        public RegistryRepository(Db db)
        {
            _db = db;
        }

        public List<string> GetVehicleNumbers()
        {
            return ReadFirstColumn(
                "SELECT DISTINCT GosNumber FROM TransportRegistry WHERE GosNumber IS NOT NULL AND GosNumber <> '' ORDER BY GosNumber");
        }

        public List<string> GetDriverNames()
        {
            return ReadFirstColumn(
                "SELECT DISTINCT FIO FROM TransportRegistry WHERE FIO IS NOT NULL AND FIO <> '' ORDER BY FIO");
        }

        public List<string> GetRegistryNumbers()
        {
            return ReadFirstColumn(
                "SELECT DISTINCT Registry FROM TransportRegistry WHERE Registry IS NOT NULL ORDER BY Registry");
        }

        public DataTable GetRegistryRows(string registryNumber)
        {
            return _db.Query(
                @"SELECT Id, NPP, Date, FIO, NSL, GosNumber, Tonnage, VehicleType,
                         TransportNumber, RCLoad, Branch, DeliveryRegion, TripCost,
                         OrderNumber, UnloadPoints, LoadPoints, Zone, ExtraStores,
                         ExtraLoad, Supply, NQNumber, SumTTK, KmCost, Discount,
                         TotalWithoutVAT, TotalWithVAT, TransportNumber2, Registry
                  FROM TransportRegistry
                  WHERE Registry = @Registry
                  ORDER BY NPP",
                new SqlParameter("@Registry", SqlDbType.NVarChar, 200) { Value = registryNumber });
        }

        /// <summary>Обновление строки реестра по стабильному ключу Id (а не по NPP+Registry).</summary>
        public void UpdateRegistryRow(DataRow row)
        {
            _db.Execute(
                @"UPDATE TransportRegistry SET
                      NPP = @NPP, Date = @Date, FIO = @FIO, NSL = @NSL, GosNumber = @GosNumber,
                      Tonnage = @Tonnage, VehicleType = @VehicleType,
                      TransportNumber = @TransportNumber, RCLoad = @RCLoad,
                      Branch = @Branch, DeliveryRegion = @DeliveryRegion,
                      TripCost = @TripCost, OrderNumber = @OrderNumber,
                      UnloadPoints = @UnloadPoints, LoadPoints = @LoadPoints,
                      Zone = @Zone, ExtraStores = @ExtraStores,
                      ExtraLoad = @ExtraLoad, Supply = @Supply,
                      NQNumber = @NQNumber, SumTTK = @SumTTK,
                      KmCost = @KmCost, Discount = @Discount,
                      TotalWithoutVAT = @TotalWithoutVAT, TotalWithVAT = @TotalWithVAT,
                      TransportNumber2 = @TransportNumber2
                  WHERE Id = @Id",
                Param("@NPP", row["NPP"]),
                Param("@Date", row["Date"]),
                Param("@FIO", row["FIO"]),
                Param("@NSL", row["NSL"]),
                Param("@GosNumber", row["GosNumber"]),
                Param("@Tonnage", row["Tonnage"]),
                Param("@VehicleType", row["VehicleType"]),
                Param("@TransportNumber", row["TransportNumber"]),
                Param("@RCLoad", row["RCLoad"]),
                Param("@Branch", row["Branch"]),
                Param("@DeliveryRegion", row["DeliveryRegion"]),
                Param("@TripCost", row["TripCost"]),
                Param("@OrderNumber", row["OrderNumber"]),
                Param("@UnloadPoints", row["UnloadPoints"]),
                Param("@LoadPoints", row["LoadPoints"]),
                Param("@Zone", row["Zone"]),
                Param("@ExtraStores", row["ExtraStores"]),
                Param("@ExtraLoad", row["ExtraLoad"]),
                Param("@Supply", row["Supply"]),
                Param("@NQNumber", row["NQNumber"]),
                Param("@SumTTK", row["SumTTK"]),
                Param("@KmCost", row["KmCost"]),
                Param("@Discount", row["Discount"]),
                Param("@TotalWithoutVAT", row["TotalWithoutVAT"]),
                Param("@TotalWithVAT", row["TotalWithVAT"]),
                Param("@TransportNumber2", row["TransportNumber2"]),
                Param("@Id", row["Id"]));
        }

        /// <summary>
        /// Импорт всех строк одного файла в единой транзакции с одним подключением:
        /// проверка дубликата, вставка, одна итоговая запись в ImportLog.
        /// </summary>
        public ImportFileResult ImportRegistryRows(RegistryParseResult parseResult, string filePath,
            Action<string> progress = null)
        {
            var result = new ImportFileResult
            {
                FileName = filePath,
                RegistryNumber = parseResult.RegistryNumber
            };

            _db.InTransaction((connection, transaction) =>
            {
                foreach (RegistryRow row in parseResult.Rows)
                {
                    if (RowExists(connection, transaction, row, parseResult.RegistryNumber))
                    {
                        result.SkippedDuplicates++;
                        progress?.Invoke(
                            $"Строка {row.SourceRowNumber}: запись уже существует (Дата={row.Date:dd.MM.yyyy}, ФИО={row.Fio}). Пропущена.");
                        continue;
                    }

                    InsertRow(connection, transaction, row, parseResult.RegistryNumber);
                    result.ImportedCount++;
                }

                WriteImportLog(connection, transaction, parseResult.RegistryNumber, filePath, result.ImportedCount);
            });

            return result;
        }

        public List<ImportLogEntry> GetImportLog(int top = 100)
        {
            DataTable table = _db.Query(
                @"SELECT TOP (@Top) RegistryNumber, FilePath, ImportDate, RecordCount
                  FROM ImportLog ORDER BY ImportDate DESC",
                new SqlParameter("@Top", SqlDbType.Int) { Value = top });

            var entries = new List<ImportLogEntry>();
            foreach (DataRow row in table.Rows)
            {
                entries.Add(new ImportLogEntry
                {
                    RegistryNumber = row["RegistryNumber"] == DBNull.Value ? 0 : Convert.ToInt32(row["RegistryNumber"]),
                    FilePath = row["FilePath"].ToString(),
                    ImportDate = Convert.ToDateTime(row["ImportDate"]),
                    RecordCount = row["RecordCount"] == DBNull.Value ? 0 : Convert.ToInt32(row["RecordCount"])
                });
            }
            return entries;
        }

        private static bool RowExists(SqlConnection connection, SqlTransaction transaction,
            RegistryRow row, int registryNumber)
        {
            using (SqlCommand command = new SqlCommand(
                @"SELECT COUNT(*) FROM TransportRegistry
                  WHERE Date = @Date AND FIO = @FIO AND TransportNumber = @TransportNumber AND Registry = @Registry",
                connection, transaction))
            {
                command.Parameters.AddWithValue("@Date", (object)row.Date ?? DBNull.Value);
                command.Parameters.AddWithValue("@FIO", (object)NullIfEmpty(row.Fio) ?? DBNull.Value);
                command.Parameters.AddWithValue("@TransportNumber", (object)NullIfEmpty(row.TransportNumber) ?? DBNull.Value);
                command.Parameters.AddWithValue("@Registry", registryNumber);
                return (int)command.ExecuteScalar() > 0;
            }
        }

        private static void InsertRow(SqlConnection connection, SqlTransaction transaction,
            RegistryRow row, int registryNumber)
        {
            using (SqlCommand command = new SqlCommand(
                @"INSERT INTO TransportRegistry (NPP, Date, FIO, NSL, GosNumber, Tonnage, VehicleType,
                      TransportNumber, RCLoad, Branch, DeliveryRegion, TripCost, OrderNumber,
                      UnloadPoints, LoadPoints, Zone, ExtraStores, ExtraLoad, Supply, NQNumber,
                      SumTTK, KmCost, Discount, TotalWithoutVAT, TotalWithVAT, TransportNumber2, Registry)
                  VALUES (@NPP, @Date, @FIO, @NSL, @GosNumber, @Tonnage, @VehicleType,
                      @TransportNumber, @RCLoad, @Branch, @DeliveryRegion, @TripCost, @OrderNumber,
                      @UnloadPoints, @LoadPoints, @Zone, @ExtraStores, @ExtraLoad, @Supply, @NQNumber,
                      @SumTTK, @KmCost, @Discount, @TotalWithoutVAT, @TotalWithVAT, @TransportNumber2, @Registry)",
                connection, transaction))
            {
                command.Parameters.AddWithValue("@NPP", row.Npp);
                command.Parameters.AddWithValue("@Date", (object)row.Date ?? DBNull.Value);
                command.Parameters.AddWithValue("@FIO", (object)NullIfEmpty(row.Fio) ?? DBNull.Value);
                command.Parameters.AddWithValue("@NSL", (object)NullIfEmpty(row.Nsl) ?? DBNull.Value);
                command.Parameters.AddWithValue("@GosNumber", (object)NullIfEmpty(row.GosNumber) ?? DBNull.Value);
                command.Parameters.AddWithValue("@Tonnage", row.Tonnage);
                command.Parameters.AddWithValue("@VehicleType", (object)NullIfEmpty(row.VehicleType) ?? DBNull.Value);
                command.Parameters.AddWithValue("@TransportNumber", (object)NullIfEmpty(row.TransportNumber) ?? DBNull.Value);
                command.Parameters.AddWithValue("@RCLoad", (object)NullIfEmpty(row.RcLoad) ?? DBNull.Value);
                command.Parameters.AddWithValue("@Branch", (object)NullIfEmpty(row.Branch) ?? DBNull.Value);
                command.Parameters.AddWithValue("@DeliveryRegion", (object)NullIfEmpty(row.DeliveryRegion) ?? DBNull.Value);
                command.Parameters.AddWithValue("@TripCost", row.TripCost);
                command.Parameters.AddWithValue("@OrderNumber", (object)NullIfEmpty(row.OrderNumber) ?? DBNull.Value);
                command.Parameters.AddWithValue("@UnloadPoints", row.UnloadPoints);
                command.Parameters.AddWithValue("@LoadPoints", row.LoadPoints);
                command.Parameters.AddWithValue("@Zone", row.Zone);
                command.Parameters.AddWithValue("@ExtraStores", row.ExtraStores);
                command.Parameters.AddWithValue("@ExtraLoad", row.ExtraLoad);
                command.Parameters.AddWithValue("@Supply", row.Supply);
                command.Parameters.AddWithValue("@NQNumber", (object)NullIfEmpty(row.NqNumber) ?? DBNull.Value);
                command.Parameters.AddWithValue("@SumTTK", row.SumTtk);
                command.Parameters.AddWithValue("@KmCost", row.KmCost);
                command.Parameters.AddWithValue("@Discount", row.Discount);
                command.Parameters.AddWithValue("@TotalWithoutVAT", row.TotalWithoutVat);
                command.Parameters.AddWithValue("@TotalWithVAT", row.TotalWithVat);
                command.Parameters.AddWithValue("@TransportNumber2", (object)NullIfEmpty(row.TransportNumber2) ?? DBNull.Value);
                command.Parameters.AddWithValue("@Registry", registryNumber);
                command.ExecuteNonQuery();
            }
        }

        private static void WriteImportLog(SqlConnection connection, SqlTransaction transaction,
            int registryNumber, string filePath, int recordCount)
        {
            using (SqlCommand command = new SqlCommand(
                @"INSERT INTO ImportLog (RegistryNumber, FilePath, ImportDate, RecordCount)
                  VALUES (@RegistryNumber, @FilePath, @ImportDate, @RecordCount)",
                connection, transaction))
            {
                command.Parameters.AddWithValue("@RegistryNumber", registryNumber);
                command.Parameters.AddWithValue("@FilePath", filePath);
                command.Parameters.AddWithValue("@ImportDate", DateTime.Now);
                command.Parameters.AddWithValue("@RecordCount", recordCount);
                command.ExecuteNonQuery();
            }
        }

        private List<string> ReadFirstColumn(string sql)
        {
            DataTable table = _db.Query(sql);
            var values = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                values.Add(row[0].ToString().Trim());
            }
            return values;
        }

        private static string NullIfEmpty(string value) => string.IsNullOrEmpty(value) ? null : value;

        private static SqlParameter Param(string name, object value) =>
            new SqlParameter(name, value ?? DBNull.Value);
    }
}
