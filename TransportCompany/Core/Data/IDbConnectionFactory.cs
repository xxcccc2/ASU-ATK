using System.Data.SqlClient;

namespace TransportCompany.Core.Data
{
    /// <summary>
    /// Фабрика подключений к БД. Единственная точка, знающая строку подключения.
    /// </summary>
    public interface IDbConnectionFactory
    {
        SqlConnection Create();
    }
}
