using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace TransportCompany.Forms
{
    public partial class SettingsForm : Form
    {
        private bool hasUnsavedChanges = false;
        private Dictionary<int, decimal> originalZoneCosts = new Dictionary<int, decimal>();

        public SettingsForm()
        {
            InitializeComponent();
            LoadZoneSettings();
            LoadConnectionString();
        }

        private void LoadZoneSettings()
        {
            try
            {
                dgvZones.Rows.Clear();
                originalZoneCosts.Clear();

                using (SqlConnection connection = new SqlConnection(Config.connectionString))
                {
                    connection.Open();
                    string query = "SELECT ZoneId, Cost, UpdatedDate FROM ZoneSettings ORDER BY ZoneId";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int zoneId = reader.GetInt32(0);
                            decimal cost = reader.GetDecimal(1);
                            DateTime updatedDate = reader.GetDateTime(2);

                            originalZoneCosts[zoneId] = cost;
                            dgvZones.Rows.Add(zoneId, cost, updatedDate.ToString("dd.MM.yyyy HH:mm"));
                        }
                    }
                }

                if (dgvZones.Rows.Count == 0)
                {
                    for (int i = 0; i <= 10; i++)
                    {
                        originalZoneCosts[i] = i * 100m;
                        dgvZones.Rows.Add(i, i * 100m, "Не установлено");
                    }
                }

                hasUnsavedChanges = false;
                UpdateSaveButtonState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки настроек зон: {ex.Message}\n\nВозможно, таблица ZoneSettings не существует в БД.", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                for (int i = 0; i <= 10; i++)
                {
                    originalZoneCosts[i] = i * 100m;
                    dgvZones.Rows.Add(i, i * 100m, "Не установлено");
                }
            }
        }

        private void LoadConnectionString()
        {
            txtConnectionString.Text = Config.connectionString ?? "";
            txtOriginalConnectionString.Text = Config.connectionString ?? "";
        }

        private void LoadZoneHistory()
        {
            try
            {
                dgvHistory.Rows.Clear();

                using (SqlConnection connection = new SqlConnection(Config.connectionString))
                {
                    connection.Open();
                    string query = @"SELECT ZoneId, OldCost, NewCost, ChangeDate, ChangedBy 
                                    FROM ZoneCostHistory 
                                    ORDER BY ChangeDate DESC";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int zoneId = reader.GetInt32(0);
                            string oldCost = reader.IsDBNull(1) ? "-" : reader.GetDecimal(1).ToString("N2");
                            decimal newCost = reader.GetDecimal(2);
                            DateTime changeDate = reader.GetDateTime(3);
                            string changedBy = reader.IsDBNull(4) ? "Система" : reader.GetString(4);

                            dgvHistory.Rows.Add(
                                changeDate.ToString("dd.MM.yyyy HH:mm"),
                                $"Зона {zoneId}",
                                oldCost,
                                newCost.ToString("N2"),
                                changedBy
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSaveZones_Click(object sender, EventArgs e)
        {
            if (!ValidateZoneCosts())
                return;

            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите сохранить изменения стоимости зон?\n\nИзменения будут применены ко всему проекту.",
                "Подтверждение сохранения",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection connection = new SqlConnection(Config.connectionString))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dgvZones.Rows)
                    {
                        if (row.IsNewRow) continue;

                        int zoneId = Convert.ToInt32(row.Cells["colZoneId"].Value);
                        decimal newCost = Convert.ToDecimal(row.Cells["colCost"].Value);
                        decimal oldCost = originalZoneCosts.ContainsKey(zoneId) ? originalZoneCosts[zoneId] : 0;

                        if (newCost != oldCost)
                        {
                            string historyQuery = @"INSERT INTO ZoneCostHistory (ZoneId, OldCost, NewCost, ChangedBy) 
                                                   VALUES (@ZoneId, @OldCost, @NewCost, @ChangedBy)";
                            using (SqlCommand historyCmd = new SqlCommand(historyQuery, connection))
                            {
                                historyCmd.Parameters.AddWithValue("@ZoneId", zoneId);
                                historyCmd.Parameters.AddWithValue("@OldCost", oldCost);
                                historyCmd.Parameters.AddWithValue("@NewCost", newCost);
                                historyCmd.Parameters.AddWithValue("@ChangedBy", Environment.UserName);
                                historyCmd.ExecuteNonQuery();
                            }

                            string updateQuery = @"IF EXISTS (SELECT 1 FROM ZoneSettings WHERE ZoneId = @ZoneId)
                                                   UPDATE ZoneSettings SET Cost = @Cost, UpdatedDate = GETDATE() WHERE ZoneId = @ZoneId
                                                   ELSE
                                                   INSERT INTO ZoneSettings (ZoneId, Cost) VALUES (@ZoneId, @Cost)";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                            {
                                updateCmd.Parameters.AddWithValue("@ZoneId", zoneId);
                                updateCmd.Parameters.AddWithValue("@Cost", newCost);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                MessageBox.Show("Настройки зон успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                hasUnsavedChanges = false;
                LoadZoneSettings();
                LoadZoneHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения настроек зон: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateZoneCosts()
        {
            foreach (DataGridViewRow row in dgvZones.Rows)
            {
                if (row.IsNewRow) continue;

                object costValue = row.Cells["colCost"].Value;
                if (costValue == null || !decimal.TryParse(costValue.ToString(), out decimal cost))
                {
                    MessageBox.Show($"Некорректное значение стоимости в строке {row.Index + 1}. Введите числовое значение.",
                        "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvZones.CurrentCell = row.Cells["colCost"];
                    return false;
                }

                if (cost < 0)
                {
                    MessageBox.Show($"Стоимость зоны {row.Cells["colZoneId"].Value} не может быть отрицательной.",
                        "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvZones.CurrentCell = row.Cells["colCost"];
                    return false;
                }
            }
            return true;
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            string testConnectionString = txtConnectionString.Text.Trim();

            if (string.IsNullOrEmpty(testConnectionString))
            {
                MessageBox.Show("Введите строку подключения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(testConnectionString))
                {
                    connection.Open();
                    MessageBox.Show("Подключение успешно установлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения:\n\n{ex.Message}", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveConnection_Click(object sender, EventArgs e)
        {
            string newConnectionString = txtConnectionString.Text.Trim();

            if (string.IsNullOrEmpty(newConnectionString))
            {
                MessageBox.Show("Строка подключения не может быть пустой!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!newConnectionString.ToLower().Contains("data source") || !newConnectionString.ToLower().Contains("initial catalog"))
            {
                MessageBox.Show("Строка подключения должна содержать 'Data Source' и 'Initial Catalog'.",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult testResult = MessageBox.Show(
                "Рекомендуется сначала проверить подключение.\n\nПроверить подключение перед сохранением?",
                "Проверка подключения",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (testResult == DialogResult.Cancel)
                return;

            if (testResult == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(newConnectionString))
                    {
                        connection.Open();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Подключение не удалось:\n\n{ex.Message}\n\nСохранение отменено.",
                        "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            DialogResult saveResult = MessageBox.Show(
                "Вы уверены, что хотите сохранить новую строку подключения?\n\nИзменения вступят в силу после перезапуска приложения.",
                "Подтверждение сохранения",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (saveResult != DialogResult.Yes)
                return;

            try
            {
                Config.connectionString = newConnectionString;
                txtOriginalConnectionString.Text = newConnectionString;
                MessageBox.Show("Строка подключения успешно сохранена!\n\nДля применения изменений перезапустите приложение.",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения строки подключения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvZones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvZones.Columns["colCost"].Index)
            {
                hasUnsavedChanges = true;
                UpdateSaveButtonState();
            }
        }

        private void UpdateSaveButtonState()
        {
            btnSaveZones.Enabled = hasUnsavedChanges;
            if (hasUnsavedChanges)
            {
                btnSaveZones.BackColor = Color.FromArgb(200, 80, 80);
            }
            else
            {
                btnSaveZones.BackColor = Color.FromArgb(46, 89, 132);
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabHistory)
            {
                LoadZoneHistory();
            }
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hasUnsavedChanges)
            {
                DialogResult result = MessageBox.Show(
                    "Есть несохраненные изменения в настройках зон.\n\nСохранить изменения перед закрытием?",
                    "Несохраненные изменения",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    btnSaveZones_Click(sender, e);
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
