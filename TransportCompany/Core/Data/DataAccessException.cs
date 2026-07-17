using System;

namespace TransportCompany.Core.Data
{
    /// <summary>
    /// Ошибка слоя доступа к данным. Слой данных не показывает MessageBox —
    /// он пробрасывает это исключение, а UI решает, как сообщить пользователю.
    /// </summary>
    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DataAccessException(string message)
            : base(message)
        {
        }
    }
}
