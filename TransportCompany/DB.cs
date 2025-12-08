using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TransportCompany
{
    public static class DB
    {
        public static string ConnectionString { get; set; } = Config.connectionString;

        // Перегруженный метод Query для выполнения команды с параметрами
        public static bool Query(SqlCommand command)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                try
                {
                    cn.Open();
                    command.Connection = cn;
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                    return false;
                }
            }
        }

        // Метод Query для выполнения строки запроса с параметрами
        public static bool Query(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                    return false;
                }
            }
        }

        // Метод для загрузки данных в DataSet
        public static bool LoadData(string query, ref DataSet ds, string tableName, params SqlParameter[] parameters)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                try
                {
                    cn.Open();
                    ds.Clear();
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, tableName);
                        }
                    }
                    return true;
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                    return false;
                }
            }
        }

        // Перегруженный метод ExecuteScalar для работы с параметрами
        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteScalar();
                    }
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                    return null;
                }
            }
        }

        // Перегруженный метод ExecuteScalar для работы с командой
        public static object ExecuteScalar(SqlCommand command)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                try
                {
                    cn.Open();
                    command.Connection = cn;
                    return command.ExecuteScalar();
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                    return null;
                }
            }
        }

        // Метод для получения данных об истекающих ОСАГО
        public static DataTable GetExpiringOSAGO()
        {
            DataTable dt = new DataTable();
            string query = "SELECT VehicleRegistrationNumber, PolicyNumber, EndDate FROM OSAGO WHERE EndDate <= DATEADD(day, 30, GETDATE()) AND EndDate >= GETDATE()";
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                try
                {
                    cn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, cn))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Ошибка при получении данных ОСАГО: " + ex.Message);
                }
            }
            return dt;
        }

        // Метод для получения данных об истекающих водительских удостоверениях
        public static DataTable GetExpiringLicenses()
        {
            DataTable dt = new DataTable();
            string query = "SELECT DriverFullName, LicenseNumber, ExpiryDate FROM DriverLicenses WHERE ExpiryDate <= DATEADD(day, 30, GETDATE()) AND ExpiryDate >= GETDATE()";
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                try
                {
                    cn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, cn))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("Ошибка при получении данных водительских удостоверений: " + ex.Message);
                }
            }
            return dt;
        }
    }
}