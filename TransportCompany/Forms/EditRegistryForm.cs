using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;

namespace TransportCompany.Forms
{
    public partial class EditRegistryForm : Form
    {
        private DataSet registriesDataSet;
        private DataSet currentRegistryData;
        private string selectedRegistry;

        public EditRegistryForm()
        {
            InitializeComponent();
            LoadRegistries();
        }

        private void LoadRegistries()
        {
            try
            {
                registriesDataSet = new DataSet();
                string query = "SELECT DISTINCT Registry FROM TransportRegistry WHERE Registry IS NOT NULL ORDER BY Registry";
                
                if (DB.LoadData(query, ref registriesDataSet, "Registries"))
                {
                    if (registriesDataSet.Tables["Registries"].Rows.Count > 0)
                    {
                        cmbRegistries.DataSource = registriesDataSet.Tables["Registries"];
                        cmbRegistries.DisplayMember = "Registry";
                        cmbRegistries.ValueMember = "Registry";
                        cmbRegistries.SelectedIndex = -1;
                    }
                    else
                    {
                        MessageBox.Show("В базе данных не найдено ни одного реестра.", "Информация", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке реестров: {ex.Message}\n\nДетали: {ex.StackTrace}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbRegistries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRegistries.SelectedValue != null && cmbRegistries.SelectedIndex >= 0)
            {
                try
                {
                    if (cmbRegistries.SelectedValue is DataRowView rowView)
                    {
                        selectedRegistry = rowView["Registry"].ToString();
                    }
                    else
                    {
                        selectedRegistry = cmbRegistries.SelectedValue.ToString();
                    }
                    
                    if (!string.IsNullOrEmpty(selectedRegistry))
                    {
                        LoadRegistryData(selectedRegistry);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при выборе реестра: {ex.Message}", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadRegistryData(string registryNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(registryNumber))
                {
                    MessageBox.Show("Номер реестра не может быть пустым.", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                currentRegistryData = new DataSet();
                string query = @"SELECT NPP, Date, FIO, NSL, GosNumber, Tonnage, VehicleType, 
                                TransportNumber, RCLoad, Branch, DeliveryRegion, TripCost, 
                                OrderNumber, UnloadPoints, LoadPoints, Zone, ExtraStores, 
                                ExtraLoad, Supply, NQNumber, SumTTK, KmCost, Discount, 
                                TotalWithoutVAT, TotalWithVAT, TransportNumber2, Registry
                                FROM TransportRegistry 
                                WHERE Registry = @Registry
                                ORDER BY NPP";
                
                var parameters = new System.Data.SqlClient.SqlParameter[]
                {
                    new System.Data.SqlClient.SqlParameter("@Registry", registryNumber)
                };

                if (DB.LoadData(query, ref currentRegistryData, "RegistryData", parameters))
                {
                    if (currentRegistryData.Tables["RegistryData"].Rows.Count > 0)
                    {
                        dgvRegistryData.DataSource = currentRegistryData.Tables["RegistryData"];
                        
                        // Настройка отображения колонок
                        SetupDataGridView();
                        
                        lblRecordCount.Text = $"Записей: {currentRegistryData.Tables["RegistryData"].Rows.Count}";
                    }
                    else
                    {
                        dgvRegistryData.DataSource = null;
                        lblRecordCount.Text = "Записей: 0";
                        MessageBox.Show($"Для реестра '{registryNumber}' не найдено записей.", "Информация", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    dgvRegistryData.DataSource = null;
                    lblRecordCount.Text = "Записей: 0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных реестра: {ex.Message}\n\nДетали: {ex.StackTrace}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvRegistryData.DataSource = null;
                lblRecordCount.Text = "Записей: 0";
            }
        }

        private void SetupDataGridView()
        {
            dgvRegistryData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRegistryData.AllowUserToAddRows = false;
            dgvRegistryData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRegistryData.MultiSelect = false;
            
            // Настройка цветов для лучшей видимости
            dgvRegistryData.BackgroundColor = System.Drawing.Color.White;
            dgvRegistryData.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvRegistryData.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvRegistryData.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            dgvRegistryData.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            dgvRegistryData.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            dgvRegistryData.AlternatingRowsDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvRegistryData.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.DarkBlue;
            dgvRegistryData.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvRegistryData.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dgvRegistryData.EnableHeadersVisualStyles = false;
            
            // Настройка русских названий столбцов
            SetupColumnHeaders();
            
            // Скрыть колонку Registry, так как она одинакова для всех записей
            if (dgvRegistryData.Columns["Registry"] != null)
            {
                dgvRegistryData.Columns["Registry"].Visible = false;
            }
        }
        
        private void SetupColumnHeaders()
        {
            // Словарь соответствия английских названий столбцов русским
            var columnHeaders = new Dictionary<string, string>
            {
                { "NPP", "№ п/п" },
                { "Date", "Дата" },
                { "FIO", "ФИО" },
                { "NSL", "№СЛ" },
                { "GosNumber", "Гос № ТС" },
                { "Tonnage", "Тоннаж" },
                { "VehicleType", "Тип ТС" },
                { "TransportNumber", "№ Трансп-ки" },
                { "RCLoad", "РЦ Загрузки" },
                { "Branch", "Филиал" },
                { "DeliveryRegion", "Регион доставки" },
                { "TripCost", "Стоимость рейса" },
                { "OrderNumber", "Поряд. номер" },
                { "UnloadPoints", "Кол-во точек выгр" },
                { "LoadPoints", "Кол-во точек загр" },
                { "Zone", "Зона" },
                { "ExtraStores", "Сумма за доп магазины" },
                { "ExtraLoad", "Сумма за доп загрузку" },
                { "Supply", "Сумма за подачу" },
                { "NQNumber", "НомерNQ" },
                { "SumTTK", "СумТТК" },
                { "KmCost", "Сумма за км" },
                { "Discount", "Сумма скидки(-) / надбавки(+)" },
                { "TotalWithoutVAT", "Итого без НДС" },
                { "TotalWithVAT", "Сумма с НДС" },
                { "TransportNumber2", "№ транс-ки" }
            };
            
            // Применяем русские названия к столбцам
            foreach (DataGridViewColumn column in dgvRegistryData.Columns)
            {
                if (columnHeaders.ContainsKey(column.Name))
                {
                    column.HeaderText = columnHeaders[column.Name];
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentRegistryData?.Tables["RegistryData"] == null)
                {
                    MessageBox.Show("Нет данных для сохранения.", "Предупреждение", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Сохранение изменений в базу данных
                SaveChangesToDatabase();
                
                MessageBox.Show("Изменения успешно сохранены!", "Успех", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveChangesToDatabase()
        {
            foreach (DataRow row in currentRegistryData.Tables["RegistryData"].Rows)
            {
                if (row.RowState == DataRowState.Modified)
                {
                    UpdateRowInDatabase(row);
                }
            }
        }

        private void UpdateRowInDatabase(DataRow row)
        {
            string updateQuery = @"UPDATE TransportRegistry SET 
                                  Date = @Date, FIO = @FIO, NSL = @NSL, GosNumber = @GosNumber, 
                                  Tonnage = @Tonnage, VehicleType = @VehicleType, 
                                  TransportNumber = @TransportNumber, RCLoad = @RCLoad, 
                                  Branch = @Branch, DeliveryRegion = @DeliveryRegion, 
                                  TripCost = @TripCost, OrderNumber = @OrderNumber, 
                                  UnloadPoints = @UnloadPoints, LoadPoints = @LoadPoints, 
                                  Zone = @Zone, ExtraStores = @ExtraStores, 
                                  ExtraLoad = @ExtraLoad, Supply = @Supply, 
                                  NQNumber = @NQNumber, SumTTK = @SumTTK, 
                                  KmCost = @KmCost, Discount = @Discount, 
                                  TotalWithoutVAT = @TotalWithoutVAT, TotalWithVAT = @TotalWithVAT, 
                                  TransportNumber2 = @TransportNumber2
                                  WHERE NPP = @NPP AND Registry = @Registry";

            var parameters = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@NPP", row["NPP"]),
                new System.Data.SqlClient.SqlParameter("@Date", row["Date"]),
                new System.Data.SqlClient.SqlParameter("@FIO", row["FIO"]),
                new System.Data.SqlClient.SqlParameter("@NSL", row["NSL"]),
                new System.Data.SqlClient.SqlParameter("@GosNumber", row["GosNumber"]),
                new System.Data.SqlClient.SqlParameter("@Tonnage", row["Tonnage"]),
                new System.Data.SqlClient.SqlParameter("@VehicleType", row["VehicleType"]),
                new System.Data.SqlClient.SqlParameter("@TransportNumber", row["TransportNumber"]),
                new System.Data.SqlClient.SqlParameter("@RCLoad", row["RCLoad"]),
                new System.Data.SqlClient.SqlParameter("@Branch", row["Branch"]),
                new System.Data.SqlClient.SqlParameter("@DeliveryRegion", row["DeliveryRegion"]),
                new System.Data.SqlClient.SqlParameter("@TripCost", row["TripCost"]),
                new System.Data.SqlClient.SqlParameter("@OrderNumber", row["OrderNumber"]),
                new System.Data.SqlClient.SqlParameter("@UnloadPoints", row["UnloadPoints"]),
                new System.Data.SqlClient.SqlParameter("@LoadPoints", row["LoadPoints"]),
                new System.Data.SqlClient.SqlParameter("@Zone", row["Zone"]),
                new System.Data.SqlClient.SqlParameter("@ExtraStores", row["ExtraStores"]),
                new System.Data.SqlClient.SqlParameter("@ExtraLoad", row["ExtraLoad"]),
                new System.Data.SqlClient.SqlParameter("@Supply", row["Supply"]),
                new System.Data.SqlClient.SqlParameter("@NQNumber", row["NQNumber"]),
                new System.Data.SqlClient.SqlParameter("@SumTTK", row["SumTTK"]),
                new System.Data.SqlClient.SqlParameter("@KmCost", row["KmCost"]),
                new System.Data.SqlClient.SqlParameter("@Discount", row["Discount"]),
                new System.Data.SqlClient.SqlParameter("@TotalWithoutVAT", row["TotalWithoutVAT"]),
                new System.Data.SqlClient.SqlParameter("@TotalWithVAT", row["TotalWithVAT"]),
                new System.Data.SqlClient.SqlParameter("@TransportNumber2", row["TransportNumber2"]),
                new System.Data.SqlClient.SqlParameter("@Registry", row["Registry"])
            };

            DB.Query(updateQuery, parameters);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}