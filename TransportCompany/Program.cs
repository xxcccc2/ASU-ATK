using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TransportCompany
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Объединённое уведомление
            string message = "Приложение запускается...\n\n";

            // Проверка истекающих сроков ОСАГО и удостоверений
            try
            {
                DataTable osagoExpiring = DB.GetExpiringOSAGO();
                DataTable licensesExpiring = DB.GetExpiringLicenses();

                if (osagoExpiring.Rows.Count > 0)
                {
                    message += "Истекают сроки ОСАГО:\n";
                    foreach (DataRow row in osagoExpiring.Rows)
                    {
                        message += $"- Автомобиль: {row["VehicleRegistrationNumber"]}, Полис: {row["PolicyNumber"]}, Истекает: {row["EndDate"]:dd.MM.yyyy}\n";
                    }
                }
                if (licensesExpiring.Rows.Count > 0)
                {
                    message += "\nИстекают сроки водительских удостоверений:\n";
                    foreach (DataRow row in licensesExpiring.Rows)
                    {
                        message += $"- Водитель: {row["DriverFullName"]}, Удостоверение: {row["LicenseNumber"]}, Истекает: {row["ExpiryDate"]:dd.MM.yyyy}\n";
                    }
                }
            }
            catch (Exception ex)
            {
                message += $"\nОшибка при проверке ОСАГО и удостоверений: {ex.Message}\n";
            }

            // Проверка просроченного ТО
            try
            {
                using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
                {
                    string query = @"
                        SELECT [Номер машины], [Дата последнего ТО] 
                        FROM Техобслуживание 
                        WHERE DATEADD(MONTH, 3, [Дата последнего ТО]) < GETDATE()";

                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                message += "\nСледующие машины требуют ТО:\n";
                                while (reader.Read())
                                {
                                    string carNumber = reader["Номер машины"].ToString();
                                    DateTime lastTO = Convert.ToDateTime(reader["Дата последнего ТО"]);
                                    message += $"- {carNumber} (последнее ТО: {lastTO:dd.MM.yyyy})\n";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message += $"\nОшибка при проверке ТО: {ex.Message}\n";
            }

            // Показываем уведомление, если есть что показать
            if (message != "Приложение запускается...\n\n")
            {
                MessageBox.Show(message, "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            try
            {
                Application.Run(new MainForm());
            }
            catch (SystemException ex)
            {
                MessageBox.Show($"Исключение: {ex.Message}");
            }
        }
    }
}