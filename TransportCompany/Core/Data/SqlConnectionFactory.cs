using System;
using System.Data.SqlClient;

namespace TransportCompany.Core.Data
{
    public sealed class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly Func<string> _connectionStringProvider;

        public SqlConnectionFactory(Func<string> connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider
                ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        public SqlConnection Create()
        {
            string connectionString = _connectionStringProvider();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new DataAccessException("Строка подключения к базе данных не настроена.");
            }

            return new SqlConnection(connectionString);
        }
    }
}
