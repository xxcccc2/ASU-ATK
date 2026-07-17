using System;
using System.Configuration;

namespace TransportCompany
{
    /// <summary>
    /// Конфигурация приложения. Слой конфигурации не показывает UI —
    /// ошибки пробрасываются исключениями, вызывающий код решает, как их отобразить.
    /// </summary>
    public static class Config
    {
        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConfigurationManager.AppSettings["conString"];
                    if (string.IsNullOrEmpty(_connectionString))
                    {
                        throw new ConfigurationErrorsException(
                            "Строка подключения 'conString' не найдена в конфигурации приложения.");
                    }
                }
                return _connectionString;
            }
        }

        public static void SaveConnectionString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Строка подключения не может быть пустой.");
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["conString"].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            _connectionString = value;
        }

        public static string CurrentOperator { get; set; } = Environment.UserName;
    }
}
