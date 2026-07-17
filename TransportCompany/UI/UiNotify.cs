using System;
using System.Windows.Forms;
using TransportCompany.Core.Logging;

namespace TransportCompany.UI
{
    /// <summary>
    /// Единая точка сообщений пользователю. Технические детали (stack trace)
    /// уходят в журнал, пользователь видит понятный текст.
    /// </summary>
    public static class UiNotify
    {
        public static void Info(string message, string title = "Информация")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warning(string message, string title = "Внимание")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(string message, Exception exception = null, string title = "Ошибка")
        {
            if (exception != null)
            {
                AppLogger.Error(message, exception);
                message = message + Environment.NewLine + Environment.NewLine + exception.Message;
            }
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool Confirm(string message, string title = "Подтверждение")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                   == DialogResult.Yes;
        }
    }
}
