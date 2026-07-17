using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;

namespace TransportCompany.Core.Repositories
{
    /// <summary>Полисы ОСАГО и водительские удостоверения (модуль «Дневник автопарка»).</summary>
    public sealed class FleetDocumentsRepository
    {
        private readonly Db _db;

        public FleetDocumentsRepository(Db db)
        {
            _db = db;
        }

        // ----- ОСАГО -----

        public List<OsagoPolicy> GetOsagoPolicies()
        {
            DataTable table = _db.Query(
                @"SELECT OSAGOId, VehicleRegistrationNumber, PolicyNumber, StartDate, EndDate
                  FROM OSAGO ORDER BY EndDate");

            var result = new List<OsagoPolicy>();
            foreach (DataRow row in table.Rows)
            {
                result.Add(new OsagoPolicy
                {
                    Id = Convert.ToInt32(row["OSAGOId"]),
                    VehicleRegistrationNumber = row["VehicleRegistrationNumber"].ToString(),
                    PolicyNumber = row["PolicyNumber"].ToString(),
                    StartDate = Convert.ToDateTime(row["StartDate"]),
                    EndDate = Convert.ToDateTime(row["EndDate"])
                });
            }
            return result;
        }

        public OsagoPolicy GetOsago(int id)
        {
            DataTable table = _db.Query(
                @"SELECT OSAGOId, VehicleRegistrationNumber, PolicyNumber, StartDate, EndDate
                  FROM OSAGO WHERE OSAGOId = @Id",
                new SqlParameter("@Id", SqlDbType.Int) { Value = id });

            if (table.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = table.Rows[0];
            return new OsagoPolicy
            {
                Id = id,
                VehicleRegistrationNumber = row["VehicleRegistrationNumber"].ToString(),
                PolicyNumber = row["PolicyNumber"].ToString(),
                StartDate = Convert.ToDateTime(row["StartDate"]),
                EndDate = Convert.ToDateTime(row["EndDate"])
            };
        }

        public void SaveOsago(OsagoPolicy policy)
        {
            _db.InTransaction((connection, transaction) =>
            {
                EnsureVehicleExists(connection, transaction, policy.VehicleRegistrationNumber);

                string sql = policy.Id > 0
                    ? @"UPDATE OSAGO SET VehicleRegistrationNumber = @Vehicle, PolicyNumber = @Policy,
                            StartDate = @StartDate, EndDate = @EndDate WHERE OSAGOId = @Id"
                    : @"INSERT INTO OSAGO (VehicleRegistrationNumber, PolicyNumber, StartDate, EndDate)
                            VALUES (@Vehicle, @Policy, @StartDate, @EndDate)";

                using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                {
                    command.Parameters.Add("@Vehicle", SqlDbType.NVarChar, 20).Value = policy.VehicleRegistrationNumber;
                    command.Parameters.Add("@Policy", SqlDbType.NVarChar, 50).Value = policy.PolicyNumber;
                    command.Parameters.Add("@StartDate", SqlDbType.Date).Value = policy.StartDate;
                    command.Parameters.Add("@EndDate", SqlDbType.Date).Value = policy.EndDate;
                    if (policy.Id > 0)
                    {
                        command.Parameters.Add("@Id", SqlDbType.Int).Value = policy.Id;
                    }
                    command.ExecuteNonQuery();
                }
            });
        }

        public void DeleteOsago(int id)
        {
            _db.Execute("DELETE FROM OSAGO WHERE OSAGOId = @Id",
                new SqlParameter("@Id", SqlDbType.Int) { Value = id });
        }

        public List<OsagoPolicy> GetExpiringOsago()
        {
            var expiring = new List<OsagoPolicy>();
            foreach (OsagoPolicy policy in GetOsagoPolicies())
            {
                if (policy.IsExpiringSoon)
                {
                    expiring.Add(policy);
                }
            }
            return expiring;
        }

        // ----- Водительские удостоверения -----

        public List<DriverLicenseRecord> GetLicenses()
        {
            DataTable table = _db.Query(
                @"SELECT LicenseId, DriverFullName, LicenseNumber, IssueDate, ExpiryDate
                  FROM DriverLicenses ORDER BY ExpiryDate");

            var result = new List<DriverLicenseRecord>();
            foreach (DataRow row in table.Rows)
            {
                result.Add(new DriverLicenseRecord
                {
                    Id = Convert.ToInt32(row["LicenseId"]),
                    DriverFullName = row["DriverFullName"].ToString(),
                    LicenseNumber = row["LicenseNumber"].ToString(),
                    IssueDate = Convert.ToDateTime(row["IssueDate"]),
                    ExpiryDate = Convert.ToDateTime(row["ExpiryDate"])
                });
            }
            return result;
        }

        public DriverLicenseRecord GetLicense(int id)
        {
            DataTable table = _db.Query(
                @"SELECT LicenseId, DriverFullName, LicenseNumber, IssueDate, ExpiryDate
                  FROM DriverLicenses WHERE LicenseId = @Id",
                new SqlParameter("@Id", SqlDbType.Int) { Value = id });

            if (table.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = table.Rows[0];
            return new DriverLicenseRecord
            {
                Id = id,
                DriverFullName = row["DriverFullName"].ToString(),
                LicenseNumber = row["LicenseNumber"].ToString(),
                IssueDate = Convert.ToDateTime(row["IssueDate"]),
                ExpiryDate = Convert.ToDateTime(row["ExpiryDate"])
            };
        }

        public void SaveLicense(DriverLicenseRecord license)
        {
            _db.InTransaction((connection, transaction) =>
            {
                EnsureDriverExists(connection, transaction, license.DriverFullName);

                string sql = license.Id > 0
                    ? @"UPDATE DriverLicenses SET DriverFullName = @Driver, LicenseNumber = @Number,
                            IssueDate = @IssueDate, ExpiryDate = @ExpiryDate WHERE LicenseId = @Id"
                    : @"INSERT INTO DriverLicenses (DriverFullName, LicenseNumber, IssueDate, ExpiryDate)
                            VALUES (@Driver, @Number, @IssueDate, @ExpiryDate)";

                using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                {
                    command.Parameters.Add("@Driver", SqlDbType.NVarChar, 200).Value = license.DriverFullName;
                    command.Parameters.Add("@Number", SqlDbType.NVarChar, 50).Value = license.LicenseNumber;
                    command.Parameters.Add("@IssueDate", SqlDbType.Date).Value = license.IssueDate;
                    command.Parameters.Add("@ExpiryDate", SqlDbType.Date).Value = license.ExpiryDate;
                    if (license.Id > 0)
                    {
                        command.Parameters.Add("@Id", SqlDbType.Int).Value = license.Id;
                    }
                    command.ExecuteNonQuery();
                }
            });
        }

        public void DeleteLicense(int id)
        {
            _db.Execute("DELETE FROM DriverLicenses WHERE LicenseId = @Id",
                new SqlParameter("@Id", SqlDbType.Int) { Value = id });
        }

        public List<DriverLicenseRecord> GetExpiringLicenses()
        {
            var expiring = new List<DriverLicenseRecord>();
            foreach (DriverLicenseRecord license in GetLicenses())
            {
                if (license.IsExpiringSoon)
                {
                    expiring.Add(license);
                }
            }
            return expiring;
        }

        // ----- Справочники -----

        private static void EnsureVehicleExists(SqlConnection connection, SqlTransaction transaction, string registrationNumber)
        {
            using (SqlCommand command = new SqlCommand(
                @"IF NOT EXISTS (SELECT 1 FROM Vehicles WHERE RegistrationNumber = @Number)
                      INSERT INTO Vehicles (RegistrationNumber) VALUES (@Number)",
                connection, transaction))
            {
                command.Parameters.Add("@Number", SqlDbType.NVarChar, 20).Value = registrationNumber;
                command.ExecuteNonQuery();
            }
        }

        private static void EnsureDriverExists(SqlConnection connection, SqlTransaction transaction, string fullName)
        {
            using (SqlCommand command = new SqlCommand(
                @"IF NOT EXISTS (SELECT 1 FROM Drivers WHERE FullName = @FullName)
                      INSERT INTO Drivers (FullName) VALUES (@FullName)",
                connection, transaction))
            {
                command.Parameters.Add("@FullName", SqlDbType.NVarChar, 200).Value = fullName;
                command.ExecuteNonQuery();
            }
        }
    }
}
