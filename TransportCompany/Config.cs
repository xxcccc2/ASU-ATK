using System;
using System.Configuration;
using System.Windows.Forms;

namespace TransportCompany
{
    public static class Config
    {
        private static string _connectionString;
        
        public static string connectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    try
                    {
                        _connectionString = ConfigurationManager.AppSettings["conString"];
                        if (string.IsNullOrEmpty(_connectionString))
                        {
                            MessageBox.Show("Ошибка: строка подключения не найдена в конфигурации", "Ошибка конфигурации", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при чтении строки подключения: {ex.Message}", "Ошибка конфигурации", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return _connectionString;
            }
            set
            {
                _connectionString = value;
                try
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["conString"].Value = value;
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении строки подключения: {ex.Message}", "Ошибка конфигурации", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static string CurrentOperator { get; set; }
    }
}

