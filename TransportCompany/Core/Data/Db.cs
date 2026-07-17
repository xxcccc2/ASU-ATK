using System;
using System.Data;
using System.Data.SqlClient;

namespace TransportCompany.Core.Data
{
    /// <summary>
    /// Тонкая обёртка над ADO.NET: открывает подключение, выполняет команду,
    /// преобразует SqlException в DataAccessException. Никакого UI.
    /// </summary>
    public sealed class Db
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public Db(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public IDbConnectionFactory ConnectionFactory => _connectionFactory;

        public DataTable Query(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection connection = _connectionFactory.Create())
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    AddParameters(command, parameters);
                    connection.Open();

                    DataTable table = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(table);
                    }
                    return table;
                }
            }
            catch (Exception ex) when (!(ex is DataAccessException))
            {
                throw new DataAccessException("Не удалось выполнить запрос к базе данных.", ex);
            }
        }

        public int Execute(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection connection = _connectionFactory.Create())
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    AddParameters(command, parameters);
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) when (!(ex is DataAccessException))
            {
                throw new DataAccessException("Не удалось выполнить изменение данных.", ex);
            }
        }

        public object Scalar(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection connection = _connectionFactory.Create())
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    AddParameters(command, parameters);
                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
            catch (Exception ex) when (!(ex is DataAccessException))
            {
                throw new DataAccessException("Не удалось выполнить запрос к базе данных.", ex);
            }
        }

        /// <summary>
        /// Выполняет несколько операций в одной транзакции. Откат при любом исключении.
        /// </summary>
        public void InTransaction(Action<SqlConnection, SqlTransaction> work)
        {
            try
            {
                using (SqlConnection connection = _connectionFactory.Create())
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            work(connection, transaction);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex) when (!(ex is DataAccessException))
            {
                throw new DataAccessException("Операция с базой данных не была выполнена, изменения отменены.", ex);
            }
        }

        private static void AddParameters(SqlCommand command, SqlParameter[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }
        }
    }
}
