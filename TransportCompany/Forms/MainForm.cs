using System;
using System.Data;
using System.Windows.Forms;
using TransportCompany.Forms;

namespace TransportCompany
{
    public partial class MainForm : Form
    {
        public string login = string.Empty;
        private bool toNotificationShown = false;

        public MainForm()
        {
            InitializeComponent();
            this.Text = $"АТК-Форум";
        }

        private void CheckOverdueTO()
        {
            if (toNotificationShown) return;

            try
            {
                DataSet toDS = new DataSet();
                string query = @"SELECT [Номер машины], [Дата последнего ТО] 
                               FROM Техобслуживание 
                               WHERE DATEADD(MONTH, 3, [Дата последнего ТО]) < GETDATE()";

                if (DB.LoadData(query, ref toDS, "OverdueTO"))
                {
                    if (toDS.Tables["OverdueTO"].Rows.Count > 0)
                    {
                        string message = "ВНИМАНИЕ! Следующие машины требуют ТО:\n\n";
                        foreach (DataRow row in toDS.Tables["OverdueTO"].Rows)
                        {
                            string carNumber = row["Номер машины"].ToString();
                            DateTime lastTO = Convert.ToDateTime(row["Дата последнего ТО"]);
                            message += $"- {carNumber} (последнее ТО: {lastTO:dd.MM.yyyy})\n";
                        }

                        MessageBox.Show(message, "Требуется техническое обслуживание",
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        toNotificationShown = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при проверке ТО: {ex.Message}");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Дополнительная инициализация при необходимости
        }

        private void btnTO_Click(object sender, EventArgs e)
        {
            ТОForm1 toForm = new ТОForm1();
            if (toForm.ShowDialog() == DialogResult.OK)
            {
                toNotificationShown = false;
                CheckOverdueTO();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FleetDiaryForm form = new FleetDiaryForm())
            {
                form.ShowDialog();
            }
        }

        private void btnImportData_Click(object sender, EventArgs e)
        {
            try
            {
                using (FormImport FormImport = new FormImport())
                {
                    FormImport.ShowDialog();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы импорта: {ex.Message}\n{ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenReestrs_Click(object sender, EventArgs e)
        {
            using (DriverSalaryForm DriverSalaryForm = new DriverSalaryForm())
            {
                DriverSalaryForm salaryForm = new DriverSalaryForm();
                salaryForm.Show();
                this.Hide();
            }
        }

        private void btnOpenEarningsForm_Click(object sender, EventArgs e)
        {
            try
            {
                VehicleDriverEarningsForm earningsForm = new VehicleDriverEarningsForm();
                earningsForm.FormClosed += (s, args) =>
                {
                    this.Show();
                };
                earningsForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы заработка: {ex.Message}\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
            }
        }

        private void btnOpenComparisonForm_Click(object sender, EventArgs e)
        {
            try
            {
                ComparisonForm comparisonForm = new ComparisonForm();
                comparisonForm.FormClosed += (s, args) =>
                {
                    this.Show();
                };
                comparisonForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы сравнения: {ex.Message}\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
            }
        }

        private void btnOpenZoneAnalysis_Click_1(object sender, EventArgs e)
        {
            try
            {
                ZoneAnalysisForm form = new ZoneAnalysisForm();
                form.FormClosed += (s, args) =>
                {
                    this.Show();
                };
                form.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы анализа зон: {ex.Message}\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
            }
        }

        private void btnOpenDriverStatistics_Click(object sender, EventArgs e)
        {
            try
            {
                DriverStatisticsForm statisticsForm = new DriverStatisticsForm();
                statisticsForm.FormClosed += (s, args) =>
                {
                    this.Show();
                };
                statisticsForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы статистики водителей: {ex.Message}\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
            }
        }

        private void btnEditRegistry_Click(object sender, EventArgs e)
        {
            try
            {
                TransportCompany.Forms.EditRegistryForm editForm = new TransportCompany.Forms.EditRegistryForm();
                editForm.FormClosed += (s, args) =>
                {
                    this.Show();
                };
                editForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы редактирования реестра: {ex.Message}\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                using (Forms.SettingsForm settingsForm = new Forms.SettingsForm())
                {
                    settingsForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии настроек: {ex.Message}\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}