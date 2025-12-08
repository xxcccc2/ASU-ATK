using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO; // Добавляем для логирования

namespace TransportCompany
{
    public partial class VehicleDriverEarningsForm : Form
    {
        private string logFilePath = "EarningsLog.txt";

        public VehicleDriverEarningsForm()
        {
            try
            {
                LogMessage("Конструктор VehicleDriverEarningsForm запущен.");
                InitializeComponent();
                LogMessage("Компоненты формы инициализированы.");
                LoadVehicleNumbers();
                LogMessage("Госномера загружены.");
                LoadDriverNames();
                LogMessage("ФИО водителей загружены.");
                SetupDateFilters();
                LogMessage("Фильтры дат установлены.");
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка в конструкторе: {ex.Message}\n{ex.StackTrace}");
                throw; // Пробрасываем исключение дальше, чтобы увидеть его в MainForm
            }
        }

        private void VehicleDriverEarningsForm_Load(object sender, EventArgs e)
        {
            try
            {
                LogMessage("Событие Load формы запущено.");
                radioButtonCurrentMonth.Checked = true;
                UpdateEarnings();
                LogMessage("Данные формы обновлены.");
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка в Load: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void LoadVehicleNumbers()
        {
            try
            {
                LogMessage("Начало загрузки госномеров.");
                using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    LogMessage("Подключение к базе данных открыто.");
                    string query = "SELECT DISTINCT GosNumber FROM TransportRegistry WHERE GosNumber IS NOT NULL ORDER BY GosNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            comboBoxVehicleNumber.Items.Add(""); // Пустой выбор
                            while (reader.Read())
                            {
                                comboBoxVehicleNumber.Items.Add(reader["GosNumber"].ToString());
                            }
                        }
                    }
                }
                LogMessage("Госномера успешно загружены.");
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка загрузки номеров ТС: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void LoadDriverNames()
        {
            try
            {
                LogMessage("Начало загрузки ФИО водителей.");
                using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    LogMessage("Подключение к базе данных открыто.");
                    string query = "SELECT DISTINCT FIO FROM TransportRegistry WHERE FIO IS NOT NULL ORDER BY FIO";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            comboBoxDriverFIO.Items.Add(""); // Пустой выбор
                            while (reader.Read())
                            {
                                comboBoxDriverFIO.Items.Add(reader["FIO"].ToString());
                            }
                        }
                    }
                }
                LogMessage("ФИО водителей успешно загружены.");
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка загрузки ФИО водителей: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void SetupDateFilters()
        {
            try
            {
                LogMessage("Установка фильтров дат.");
                dateTimePickerStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dateTimePickerEnd.Value = DateTime.Now;
                LogMessage("Фильтры дат установлены.");
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка установки фильтров дат: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void UpdateEarnings()
        {
            string vehicleNumber = comboBoxVehicleNumber.SelectedItem?.ToString();
            string fio = comboBoxDriverFIO.SelectedItem?.ToString();
            DateTime startDate, endDate;

            try
            {
                LogMessage("Начало обновления данных о заработке.");
                if (radioButtonCurrentMonth.Checked)
                {
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    endDate = DateTime.Now;
                    LogMessage($"Выбран период: Текущий месяц (с {startDate:yyyy-MM-dd} по {endDate:yyyy-MM-dd}).");
                }
                else if (radioButtonYear.Checked)
                {
                    startDate = new DateTime(DateTime.Now.Year, 1, 1);
                    endDate = DateTime.Now;
                    LogMessage($"Выбран период: За год (с {startDate:yyyy-MM-dd} по {endDate:yyyy-MM-dd}).");
                }
                else
                {
                    startDate = dateTimePickerStart.Value;
                    endDate = dateTimePickerEnd.Value;
                    LogMessage($"Выбран период: Пользовательский (с {startDate:yyyy-MM-dd} по {endDate:yyyy-MM-dd}).");
                }

                using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
                {
                    connection.Open();
                    LogMessage("Подключение к базе данных открыто для обновления данных.");
                    string query;
                    if (!string.IsNullOrEmpty(vehicleNumber))
                    {
                        LogMessage($"Обновление данных для ТС: {vehicleNumber}.");
                        query = @"
                    SELECT 
                        GosNumber AS 'Госномер',
                        '' AS 'ФИО',
                        COUNT(*) AS 'Количество рейсов',
                        SUM(TotalWithoutVAT) AS 'Сумма без НДС',
                        SUM(TotalWithVAT) AS 'Сумма с НДС',
                        SUM(CASE Zone
                            WHEN 0 THEN 2700
                            WHEN 1 THEN 2700
                            WHEN 2 THEN 3200
                            WHEN 3 THEN 3600
                            WHEN 4 THEN 4000
                            WHEN 5 THEN 5000
                            WHEN 6 THEN 6000
                            WHEN 7 THEN 7000
                            WHEN 8 THEN 8000
                            WHEN 9 THEN 9000
                            WHEN 10 THEN 10000
                            ELSE 0 END) AS 'Зарплата'
                    FROM TransportRegistry
                    WHERE GosNumber = @GosNumber
                        AND [Date] BETWEEN @StartDate AND @EndDate
                    GROUP BY GosNumber";
                    }
                    else if (!string.IsNullOrEmpty(fio))
                    {
                        LogMessage($"Обновление данных для водителя: {fio}.");
                        query = @"
                    SELECT 
                        '' AS 'Госномер',
                        FIO AS 'ФИО',
                        COUNT(*) AS 'Количество рейсов',
                        SUM(TotalWithoutVAT) AS 'Сумма без НДС',
                        SUM(TotalWithVAT) AS 'Сумма с НДС',
                        SUM(CASE Zone
                            WHEN 0 THEN 2700
                            WHEN 1 THEN 2700
                            WHEN 2 THEN 3200
                            WHEN 3 THEN 3600
                            WHEN 4 THEN 4000
                            WHEN 5 THEN 5000
                            WHEN 6 THEN 6000
                            WHEN 7 THEN 7000
                            WHEN 8 THEN 8000
                            WHEN 9 THEN 9000
                            WHEN 10 THEN 10000
                            ELSE 0 END) AS 'Зарплата'
                    FROM TransportRegistry
                    WHERE FIO = @FIO
                        AND [Date] BETWEEN @StartDate AND @EndDate
                    GROUP BY FIO";
                    }
                    else
                    {
                        LogMessage("Ни ТС, ни водитель не выбраны. Очистка таблицы.");
                        dataGridViewEarnings.DataSource = null;
                        return;
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(vehicleNumber))
                            command.Parameters.AddWithValue("@GosNumber", vehicleNumber);
                        else
                            command.Parameters.AddWithValue("@FIO", fio);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            LogMessage("Выполнение запроса к базе данных.");
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            LogMessage($"Получено {dt.Rows.Count} строк из базы данных.");

                            dataGridViewEarnings.DataSource = dt;
                            LogMessage("Данные успешно привязаны к DataGridView.");

                            // Форматирование колонок
                            if (dt.Columns.Contains("Сумма без НДС"))
                                dataGridViewEarnings.Columns["Сумма без НДС"].DefaultCellStyle.Format = "N2";
                            if (dt.Columns.Contains("Сумма с НДС"))
                                dataGridViewEarnings.Columns["Сумма с НДС"].DefaultCellStyle.Format = "N2";
                            if (dt.Columns.Contains("Зарплата"))
                                dataGridViewEarnings.Columns["Зарплата"].DefaultCellStyle.Format = "N2";
                            LogMessage("Колонки отформатированы.");
                        }
                    }
                }
                LogMessage("Данные о заработке успешно обновлены.");
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка обновления данных: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Ошибка обновления данных: {ex.Message}\n{ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void comboBoxVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxVehicleNumber.SelectedIndex > 0)
                {
                    comboBoxDriverFIO.SelectedIndex = 0;
                }
                UpdateEarnings();
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка при выборе госномера: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void comboBoxDriverFIO_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxDriverFIO.SelectedIndex > 0)
                {
                    comboBoxVehicleNumber.SelectedIndex = 0;
                }
                UpdateEarnings();
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка при выборе ФИО: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void radioButtonCurrentMonth_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButtonCurrentMonth.Checked)
                {
                    dateTimePickerStart.Enabled = false;
                    dateTimePickerEnd.Enabled = false;
                    UpdateEarnings();
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка при выборе текущего месяца: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void radioButtonYear_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButtonYear.Checked)
                {
                    dateTimePickerStart.Enabled = false;
                    dateTimePickerEnd.Enabled = false;
                    UpdateEarnings();
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка при выборе года: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void radioButtonCustomPeriod_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButtonCustomPeriod.Checked)
                {
                    dateTimePickerStart.Enabled = true;
                    dateTimePickerEnd.Enabled = true;
                    UpdateEarnings();
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка при выборе периода: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButtonCustomPeriod.Checked)
                    UpdateEarnings();
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка при изменении начальной даты: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void dateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButtonCustomPeriod.Checked)
                    UpdateEarnings();
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка при изменении конечной даты: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                using (MainForm mainForm = new MainForm())
                {
                    mainForm.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка при возврате на главную форму: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private void LogMessage(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch (Exception ex)
            {
                // Если не удалось записать лог, показываем сообщение, но не прерываем выполнение
                MessageBox.Show($"Ошибка записи в лог: {ex.Message}", "Ошибка логирования",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}