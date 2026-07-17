using System;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.Core.Data;
using TransportCompany.Core.Logging;
using TransportCompany.UI;
using TransportCompany.UI.Shell;

namespace TransportCompany
{
    /// <summary>
    /// Точка входа и composition root: здесь собираются зависимости приложения.
    /// Проверки истекающих сроков больше не блокируют запуск —
    /// они показываются на главной странице.
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppLogger.Info("Запуск приложения АТК-Форум");

            AppServices services;
            try
            {
                var connectionFactory = new SqlConnectionFactory(() => Config.ConnectionString);
                services = new AppServices(connectionFactory);
            }
            catch (Exception ex)
            {
                AppLogger.Error("Не удалось инициализировать приложение", ex);
                UiNotify.Error(
                    "Не удалось инициализировать приложение. Проверьте конфигурацию подключения к базе данных.",
                    ex);
                return;
            }

            Application.ThreadException += (sender, args) =>
            {
                AppLogger.Error("Необработанное исключение UI", args.Exception);
                UiNotify.Error("Произошла непредвиденная ошибка.", args.Exception);
            };

            try
            {
                Application.Run(new MainShellForm(services));
            }
            catch (Exception ex)
            {
                AppLogger.Error("Критическая ошибка приложения", ex);
                UiNotify.Error("Критическая ошибка приложения.", ex);
            }

            AppLogger.Info("Завершение работы приложения");
        }
    }
}
