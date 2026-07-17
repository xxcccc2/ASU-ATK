using System;
using System.IO;

namespace TransportCompany.Core.Logging
{
    /// <summary>
    /// Единый файловый журнал приложения: %LocalAppData%\ATK-Forum\Logs\atk-ГГГГММДД.log.
    /// Заменяет разрозненные ComparisonLog.txt / EarningsLog.txt / ImportLog.txt в рабочем каталоге.
    /// </summary>
    public static class AppLogger
    {
        private static readonly object Sync = new object();

        public static string LogDirectory =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ATK-Forum", "Logs");

        public static void Info(string message) => WriteLine("INFO ", message);

        public static void Warn(string message) => WriteLine("WARN ", message);

        public static void Error(string message, Exception exception = null)
        {
            WriteLine("ERROR", exception == null ? message : message + Environment.NewLine + exception);
        }

        private static void WriteLine(string level, string message)
        {
            try
            {
                lock (Sync)
                {
                    Directory.CreateDirectory(LogDirectory);
                    string file = Path.Combine(LogDirectory, $"atk-{DateTime.Now:yyyyMMdd}.log");
                    File.AppendAllText(file, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}{Environment.NewLine}");
                }
            }
            catch
            {
                // Журналирование не должно ронять приложение.
            }
        }
    }
}
