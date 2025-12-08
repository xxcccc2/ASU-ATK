using NPOI.HSSF.UserModel; // Для .xls
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; // Для .xlsx
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using HtmlAgilityPack; // Для обработки HTML

namespace TransportCompany
{
    public partial class FormImport : Form
    {
        public FormImport()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel и HTML файлы|*.xlsx;*.xls;*.html;*.htm";
                openFileDialog.Multiselect = true; // Разрешаем выбор нескольких файлов
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = string.Join("; ", openFileDialog.FileNames); // Отображаем выбранные файлы через точку с запятой
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilePath.Text))
            {
                MessageBox.Show("Выберите хотя бы один файл для импорта!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Разделяем пути файлов (разделены через "; ")
            string[] filePaths = txtFilePath.Text.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
            if (filePaths.Length == 0)
            {
                MessageBox.Show("Не выбрано ни одного файла для импорта!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int totalRecordsImported = 0;
            try
            {
                foreach (string filePath in filePaths)
                {
                    if (!File.Exists(filePath))
                    {
                        LogMessage($"Файл не найден: {filePath}. Пропускаем.");
                        continue;
                    }

                    LogMessage($"Импорт файла: {filePath}");
                    int recordsImported = ImportToDatabase(filePath);
                    totalRecordsImported += recordsImported;
                    LogMessage($"Файл {filePath} импортирован. Импортировано записей: {recordsImported}");
                }

                LogMessage($"Общий импорт завершен. Всего импортировано записей: {totalRecordsImported}");
                MessageBox.Show($"Импорт завершён. Всего импортировано записей: {totalRecordsImported}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка импорта: {ex.Message}");
                MessageBox.Show($"Ошибка импорта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int ImportToDatabase(string filePath)
        {
            bool isHtml = CheckIfHtmlFile(filePath);
            if (isHtml)
            {
                LogMessage("Обнаружен HTML-файл. Обрабатываем как HTML.");
                return ImportHtmlToDatabase(filePath);
            }
            else
            {
                LogMessage("Обрабатываем как Excel-файл (.xls/.xlsx).");
                return ImportExcelToDatabase(filePath);
            }
        }

        private bool CheckIfHtmlFile(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string firstLine = reader.ReadLine()?.Trim();
                    return firstLine != null && firstLine.StartsWith("<html", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка проверки формата файла: {ex.Message}");
                return false;
            }
        }

        private int ImportExcelToDatabase(string filePath)
        {
            int recordCount = 0;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook;
                string extension = Path.GetExtension(filePath).ToLower();
                if (extension == ".xls")
                {
                    workbook = new HSSFWorkbook(fs);
                }
                else if (extension == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else
                {
                    throw new Exception("Неподдерживаемый формат файла. Поддерживаются только .xls и .xlsx.");
                }

                ISheet worksheet = workbook.GetSheetAt(0);
                int rowCount = worksheet.PhysicalNumberOfRows;

                int registryNumber = ExtractRegistryNumber(worksheet);
                if (registryNumber == -1)
                {
                    throw new Exception("Не удалось найти номер реестра в файле.");
                }

                recordCount = ProcessRows(worksheet, rowCount, registryNumber, filePath);
            }

            return recordCount;
        }

        private int ImportHtmlToDatabase(string filePath)
        {
            int recordCount = 0;

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(filePath, System.Text.Encoding.UTF8);

            // Ищем все таблицы в документе
            var tables = doc.DocumentNode.SelectNodes("//table");
            if (tables == null || !tables.Any())
            {
                throw new Exception("Таблицы не найдены в HTML-файле.");
            }

            // Ищем таблицу с заголовками (class="headers") для извлечения номера реестра
            HtmlNode headerTable = null;
            int registryNumber = -1;
            foreach (var table in tables)
            {
                if (table.GetAttributeValue("class", "").Equals("headers"))
                {
                    headerTable = table;
                    registryNumber = ExtractRegistryNumberFromHtml(doc, table);
                    if (registryNumber != -1)
                    {
                        break;
                    }
                }
            }

            if (headerTable == null || registryNumber == -1)
            {
                throw new Exception("Не удалось найти таблицу с номером реестра в HTML-файле.");
            }

            // Ищем таблицу с данными (class="registry")
            HtmlNode dataTable = null;
            foreach (var table in tables)
            {
                if (table.GetAttributeValue("class", "").Equals("registry"))
                {
                    dataTable = table;
                    break;
                }
            }

            if (dataTable == null)
            {
                throw new Exception("Таблица с данными (class='registry') не найдена в HTML-файле.");
            }

            var rows = dataTable.SelectNodes(".//tr");
            if (rows == null)
            {
                throw new Exception("Строки таблицы не найдены в HTML-файле.");
            }

            recordCount = ProcessHtmlRows(rows, registryNumber, filePath);

            return recordCount;
        }

        private int ExtractRegistryNumber(ISheet worksheet)
        {
            string prefixCell = worksheet.GetRow(0)?.GetCell(2)?.ToString()?.Trim() ?? "";
            if (!string.IsNullOrWhiteSpace(prefixCell) && prefixCell.IndexOf("Реестр Рейсов №", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                string cellValue = worksheet.GetRow(0)?.GetCell(3)?.ToString()?.Trim() ?? "";
                cellValue = new string(cellValue.Where(c => !char.IsControl(c) && !char.IsWhiteSpace(c) || char.IsLetterOrDigit(c) || c == ' ').ToArray()).Trim();

                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    string registryText = Regex.Match(cellValue, @"^\d+").Value;
                    if (!string.IsNullOrEmpty(registryText) && int.TryParse(registryText, out int registryNumber))
                    {
                        return registryNumber;
                    }
                }
            }

            LogMessage("Не удалось найти номер реестра в ячейках C1 и D1.");
            return -1;
        }

        private int ExtractRegistryNumberFromHtml(HtmlAgilityPack.HtmlDocument doc, HtmlNode table)
        {
            var firstRow = table.SelectSingleNode(".//tr[1]");
            if (firstRow == null)
            {
                LogMessage("Первая строка таблицы не найдена в HTML.");
                return -1;
            }

            var cells = firstRow.SelectNodes("td");
            if (cells == null || cells.Count < 4)
            {
                LogMessage("Недостаточно ячеек в первой строке HTML-таблицы.");
                return -1;
            }

            string prefixCell = cells[2].InnerText.Trim();
            if (!string.IsNullOrWhiteSpace(prefixCell) && prefixCell.IndexOf("Реестр Рейсов №", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                string cellValue = cells[3].InnerText.Trim();
                cellValue = new string(cellValue.Where(c => !char.IsControl(c) && !char.IsWhiteSpace(c) || char.IsLetterOrDigit(c) || c == ' ').ToArray()).Trim();

                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    string registryText = Regex.Match(cellValue, @"^\d+").Value;
                    if (!string.IsNullOrEmpty(registryText) && int.TryParse(registryText, out int registryNumber))
                    {
                        return registryNumber;
                    }
                }
            }

            LogMessage("Не удалось найти номер реестра в HTML.");
            return -1;
        }

        private int ProcessRows(ISheet worksheet, int rowCount, int registryNumber, string filePath)
        {
            int recordCount = 0;

            using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
            {
                connection.Open();

                for (int row = 14; row < rowCount; row++)
                {
                    IRow currentRow = worksheet.GetRow(row);
                    if (currentRow == null || IsRowEmpty(currentRow))
                    {
                        continue;
                    }

                    string cell2Value = currentRow.GetCell(1)?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(cell2Value) && cell2Value.StartsWith("ИТОГО", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    if (row < 14 || string.IsNullOrWhiteSpace(currentRow.GetCell(2)?.ToString())) continue;

                    DateTime? date = null;
                    var dateCell = currentRow.GetCell(1);
                    if (dateCell != null)
                    {
                        if (dateCell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(dateCell))
                        {
                            date = dateCell.DateCellValue;
                        }
                        else if (DateTime.TryParse(dateCell.ToString(), out DateTime parsedDate))
                        {
                            date = parsedDate;
                        }
                        else
                        {
                            LogMessage($"Строка {row + 1}: Неверный формат даты ({dateCell?.ToString()}). Пропускаем.");
                            continue;
                        }
                    }

                    int npp = SafeConvertToInt32(currentRow.GetCell(0), row + 1, "NPP");
                    if (npp == int.MinValue) continue;

                    string fio = currentRow.GetCell(2)?.ToString()?.Trim();
                    if (string.IsNullOrWhiteSpace(fio) || fio.Length < 2) continue;

                    string nsl = currentRow.GetCell(3)?.ToString()?.Trim();
                    string gosNumber = currentRow.GetCell(4)?.ToString()?.Trim();
                    double tonnage = SafeConvertToDouble(currentRow.GetCell(5), row + 1, "Tonnage");
                    if (tonnage == double.MinValue) continue;

                    string vehicleType = currentRow.GetCell(6)?.ToString()?.Trim();
                    string transportNumber = currentRow.GetCell(7)?.ToString()?.Trim();
                    string rcLoad = currentRow.GetCell(8)?.ToString()?.Trim();
                    string branch = currentRow.GetCell(9)?.ToString()?.Trim();
                    string deliveryRegion = currentRow.GetCell(10)?.ToString()?.Trim();
                    decimal tripCost = SafeConvertToDecimal(currentRow.GetCell(11), row + 1, "TripCost");
                    if (tripCost == decimal.MinValue) continue;

                    string orderNumber = currentRow.GetCell(12)?.ToString()?.Trim();
                    int unloadPoints = SafeConvertToInt32(currentRow.GetCell(13), row + 1, "UnloadPoints");
                    if (unloadPoints == int.MinValue) continue;

                    int loadPoints = SafeConvertToInt32(currentRow.GetCell(14), row + 1, "LoadPoints");
                    if (loadPoints == int.MinValue) continue;

                    int zone = SafeConvertToInt32(currentRow.GetCell(15), row + 1, "Zone");
                    if (zone == int.MinValue) continue;

                    decimal extraStores = SafeConvertToDecimal(currentRow.GetCell(16), row + 1, "ExtraStores");
                    if (extraStores == decimal.MinValue) continue;

                    decimal extraLoad = SafeConvertToDecimal(currentRow.GetCell(17), row + 1, "ExtraLoad");
                    if (extraLoad == decimal.MinValue) continue;

                    decimal supply = SafeConvertToDecimal(currentRow.GetCell(18), row + 1, "Supply");
                    if (supply == decimal.MinValue) continue;

                    string nqNumber = currentRow.GetCell(19)?.ToString()?.Trim();
                    decimal sumTTK = SafeConvertToDecimal(currentRow.GetCell(20), row + 1, "SumTTK");
                    if (sumTTK == decimal.MinValue) continue;

                    decimal kmCost = SafeConvertToDecimal(currentRow.GetCell(21), row + 1, "KmCost");
                    if (kmCost == decimal.MinValue) continue;

                    decimal discount = SafeConvertToDecimal(currentRow.GetCell(22), row + 1, "Discount");
                    if (discount == decimal.MinValue) continue;

                    decimal totalWithoutVAT = SafeConvertToDecimal(currentRow.GetCell(23), row + 1, "TotalWithoutVAT");
                    if (totalWithoutVAT == decimal.MinValue) continue;

                    decimal totalWithVAT = SafeConvertToDecimal(currentRow.GetCell(24), row + 1, "TotalWithVAT");
                    if (totalWithVAT == decimal.MinValue) continue;

                    string transportNumber2 = currentRow.GetCell(25)?.ToString()?.Trim();

                    recordCount += InsertRowToDatabase(npp, date, fio, nsl, gosNumber, tonnage, vehicleType, transportNumber, rcLoad, branch, deliveryRegion, tripCost, orderNumber, unloadPoints, loadPoints, zone, extraStores, extraLoad, supply, nqNumber, sumTTK, kmCost, discount, totalWithoutVAT, totalWithVAT, transportNumber2, registryNumber, row + 1, filePath);
                }
            }

            return recordCount;
        }

        private int ProcessHtmlRows(HtmlNodeCollection rows, int registryNumber, string filePath)
        {
            int recordCount = 0;

            using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
            {
                connection.Open();

                // Начинаем обработку с 3-й строки (индекс 2), так как первые две строки — заголовки
                int startRow = 2;
                for (int rowIndex = startRow; rowIndex < rows.Count; rowIndex++)
                {
                    var row = rows[rowIndex];
                    var cells = row.SelectNodes("td");
                    if (cells == null || cells.Count < 26)
                    {
                        LogMessage($"Строка {rowIndex + 1}: Недостаточно ячеек ({cells?.Count ?? 0}). Пропускаем.");
                        continue;
                    }

                    // Проверяем, является ли строка итоговой
                    string cell2Value = cells[1].InnerText.Trim();
                    if (!string.IsNullOrEmpty(cell2Value) && cell2Value.StartsWith("ИТОГО", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    // Пропускаем строки, где ФИО пустое или слишком короткое (вероятно, заголовки)
                    string fio = cells[2].InnerText.Trim();
                    if (string.IsNullOrWhiteSpace(fio) || fio.Length < 2)
                    {
                        continue;
                    }

                    DateTime? date = null;
                    string dateText = cells[1].InnerText.Trim();
                    if (!string.IsNullOrEmpty(dateText))
                    {
                        if (DateTime.TryParse(dateText, out DateTime parsedDate))
                        {
                            date = parsedDate;
                        }
                        else
                        {
                            LogMessage($"Строка {rowIndex + 1}: Неверный формат даты ('{dateText}'). Пропускаем.");
                            continue;
                        }
                    }

                    int npp = SafeConvertToInt32(cells[0].InnerText, rowIndex + 1, "NPP");
                    if (npp == int.MinValue) continue;

                    string nsl = cells[3].InnerText.Trim();
                    string gosNumber = cells[4].InnerText.Trim();
                    double tonnage = SafeConvertToDouble(cells[5].InnerText, rowIndex + 1, "Tonnage");
                    if (tonnage == double.MinValue) continue;

                    string vehicleType = cells[6].InnerText.Trim();
                    string transportNumber = cells[7].InnerText.Trim();
                    string rcLoad = cells[8].InnerText.Trim();
                    string branch = cells[9].InnerText.Trim();
                    string deliveryRegion = cells[10].InnerText.Trim();
                    decimal tripCost = SafeConvertToDecimal(cells[11].InnerText, rowIndex + 1, "TripCost");
                    if (tripCost == decimal.MinValue) continue;

                    string orderNumber = cells[12].InnerText.Trim();
                    int unloadPoints = SafeConvertToInt32(cells[13].InnerText, rowIndex + 1, "UnloadPoints");
                    if (unloadPoints == int.MinValue) continue;

                    int loadPoints = SafeConvertToInt32(cells[14].InnerText, rowIndex + 1, "LoadPoints");
                    if (loadPoints == int.MinValue) continue;

                    int zone = SafeConvertToInt32(cells[15].InnerText, rowIndex + 1, "Zone");
                    if (zone == int.MinValue) continue;

                    decimal extraStores = SafeConvertToDecimal(cells[16].InnerText, rowIndex + 1, "ExtraStores");
                    if (extraStores == decimal.MinValue) continue;

                    decimal extraLoad = SafeConvertToDecimal(cells[17].InnerText, rowIndex + 1, "ExtraLoad");
                    if (extraLoad == decimal.MinValue) continue;

                    decimal supply = SafeConvertToDecimal(cells[18].InnerText, rowIndex + 1, "Supply");
                    if (supply == decimal.MinValue) continue;

                    string nqNumber = cells[19].InnerText.Trim();
                    decimal sumTTK = SafeConvertToDecimal(cells[20].InnerText, rowIndex + 1, "SumTTK");
                    if (sumTTK == decimal.MinValue) continue;

                    decimal kmCost = SafeConvertToDecimal(cells[21].InnerText, rowIndex + 1, "KmCost");
                    if (kmCost == decimal.MinValue) continue;

                    decimal discount = SafeConvertToDecimal(cells[22].InnerText, rowIndex + 1, "Discount");
                    if (discount == decimal.MinValue) continue;

                    decimal totalWithoutVAT = SafeConvertToDecimal(cells[23].InnerText, rowIndex + 1, "TotalWithoutVAT");
                    if (totalWithoutVAT == decimal.MinValue) continue;

                    decimal totalWithVAT = SafeConvertToDecimal(cells[24].InnerText, rowIndex + 1, "TotalWithVAT");
                    if (totalWithVAT == decimal.MinValue) continue;

                    string transportNumber2 = cells[25].InnerText.Trim();

                    recordCount += InsertRowToDatabase(npp, date, fio, nsl, gosNumber, tonnage, vehicleType, transportNumber, rcLoad, branch, deliveryRegion, tripCost, orderNumber, unloadPoints, loadPoints, zone, extraStores, extraLoad, supply, nqNumber, sumTTK, kmCost, discount, totalWithoutVAT, totalWithVAT, transportNumber2, registryNumber, rowIndex + 1, filePath);
                }
            }

            return recordCount;
        }

        private int InsertRowToDatabase(int npp, DateTime? date, string fio, string nsl, string gosNumber, double tonnage, string vehicleType, string transportNumber, string rcLoad, string branch, string deliveryRegion, decimal tripCost, string orderNumber, int unloadPoints, int loadPoints, int zone, decimal extraStores, decimal extraLoad, decimal supply, string nqNumber, decimal sumTTK, decimal kmCost, decimal discount, decimal totalWithoutVAT, decimal totalWithVAT, string transportNumber2, int registryNumber, int rowIndex, string filePath)
        {
            int recordCount = 0;

            using (SqlConnection connection = new SqlConnection(DB.ConnectionString))
            {
                connection.Open();

                // Проверка на существование записи
                string checkQuery = @"
        SELECT COUNT(*) 
        FROM TransportRegistry 
        WHERE Date = @Date 
        AND FIO = @FIO 
        AND TransportNumber = @TransportNumber 
        AND Registry = @Registry";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Date", (object)(date.HasValue ? date.Value : DBNull.Value));
                    checkCommand.Parameters.AddWithValue("@FIO", (object)(string.IsNullOrEmpty(fio) ? DBNull.Value : fio));
                    checkCommand.Parameters.AddWithValue("@TransportNumber", (object)(string.IsNullOrEmpty(transportNumber) ? DBNull.Value : transportNumber));
                    checkCommand.Parameters.AddWithValue("@Registry", registryNumber);

                    int existingCount = (int)checkCommand.ExecuteScalar();
                    if (existingCount > 0)
                    {
                        LogMessage($"Строка {rowIndex}: Запись уже существует (Дата={date}, ФИО={fio}, Транспорт={transportNumber}). Пропускаем.");
                        return 0;
                    }
                }

                string query = @"
        INSERT INTO TransportRegistry (NPP, Date, FIO, NSL, GosNumber, Tonnage, VehicleType, TransportNumber, RCLoad, Branch, DeliveryRegion, 
        TripCost, OrderNumber, UnloadPoints, LoadPoints, Zone, ExtraStores, ExtraLoad, Supply, NQNumber, SumTTK, KmCost, Discount, 
        TotalWithoutVAT, TotalWithVAT, TransportNumber2, Registry)
        VALUES (@NPP, @Date, @FIO, @NSL, @GosNumber, @Tonnage, @VehicleType, @TransportNumber, @RCLoad, @Branch, @DeliveryRegion, 
        @TripCost, @OrderNumber, @UnloadPoints, @LoadPoints, @Zone, @ExtraStores, @ExtraLoad, @Supply, @NQNumber, @SumTTK, @KmCost, 
        @Discount, @TotalWithoutVAT, @TotalWithVAT, @TransportNumber2, @Registry)";

                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NPP", npp);
                        command.Parameters.AddWithValue("@Date", (object)(date.HasValue ? date.Value : DBNull.Value));
                        command.Parameters.AddWithValue("@FIO", (object)(string.IsNullOrEmpty(fio) ? DBNull.Value : fio));
                        command.Parameters.AddWithValue("@NSL", (object)(string.IsNullOrEmpty(nsl) ? DBNull.Value : nsl));
                        command.Parameters.AddWithValue("@GosNumber", (object)(string.IsNullOrEmpty(gosNumber) ? DBNull.Value : gosNumber));
                        command.Parameters.AddWithValue("@Tonnage", tonnage);
                        command.Parameters.AddWithValue("@VehicleType", (object)(string.IsNullOrEmpty(vehicleType) ? DBNull.Value : vehicleType));
                        command.Parameters.AddWithValue("@TransportNumber", (object)(string.IsNullOrEmpty(transportNumber) ? DBNull.Value : transportNumber));
                        command.Parameters.AddWithValue("@RCLoad", (object)(string.IsNullOrEmpty(rcLoad) ? DBNull.Value : rcLoad));
                        command.Parameters.AddWithValue("@Branch", (object)(string.IsNullOrEmpty(branch) ? DBNull.Value : branch));
                        command.Parameters.AddWithValue("@DeliveryRegion", (object)(string.IsNullOrEmpty(deliveryRegion) ? DBNull.Value : deliveryRegion));
                        command.Parameters.AddWithValue("@TripCost", tripCost);
                        command.Parameters.AddWithValue("@OrderNumber", (object)(string.IsNullOrEmpty(orderNumber) ? DBNull.Value : orderNumber));
                        command.Parameters.AddWithValue("@UnloadPoints", unloadPoints);
                        command.Parameters.AddWithValue("@LoadPoints", loadPoints);
                        command.Parameters.AddWithValue("@Zone", zone);
                        command.Parameters.AddWithValue("@ExtraStores", extraStores);
                        command.Parameters.AddWithValue("@ExtraLoad", extraLoad);
                        command.Parameters.AddWithValue("@Supply", supply);
                        command.Parameters.AddWithValue("@NQNumber", (object)(string.IsNullOrEmpty(nqNumber) ? DBNull.Value : nqNumber));
                        command.Parameters.AddWithValue("@SumTTK", sumTTK);
                        command.Parameters.AddWithValue("@KmCost", kmCost);
                        command.Parameters.AddWithValue("@Discount", discount);
                        command.Parameters.AddWithValue("@TotalWithoutVAT", totalWithoutVAT);
                        command.Parameters.AddWithValue("@TotalWithVAT", totalWithVAT);
                        command.Parameters.AddWithValue("@TransportNumber2", (object)(string.IsNullOrEmpty(transportNumber2) ? DBNull.Value : transportNumber2));
                        command.Parameters.AddWithValue("@Registry", registryNumber);

                        command.ExecuteNonQuery();
                        recordCount++;
                    }
                }
                catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
                {
                    LogMessage($"Строка {rowIndex}: Дубликат рейса (Дата={date}, ФИО={fio}, Транспорт={transportNumber}). Пропускаем.");
                    return 0;
                }
                catch (Exception ex)
                {
                    LogMessage($"Ошибка вставки строки {rowIndex}: {ex.Message}");
                    return 0;
                }

                string logQuery = @"
        INSERT INTO ImportLog (RegistryNumber, FilePath, ImportDate, RecordCount)
        VALUES (@RegistryNumber, @FilePath, @ImportDate, @RecordCount)";
                using (SqlCommand logCommand = new SqlCommand(logQuery, connection))
                {
                    logCommand.Parameters.AddWithValue("@RegistryNumber", registryNumber);
                    logCommand.Parameters.AddWithValue("@FilePath", filePath);
                    logCommand.Parameters.AddWithValue("@ImportDate", DateTime.Now);
                    logCommand.Parameters.AddWithValue("@RecordCount", recordCount);
                    logCommand.ExecuteNonQuery();
                }

                LogRowToFile(filePath, registryNumber, recordCount);
            }

            return recordCount;
        }

        private void LogRowToFile(string filePath, int registryNumber, int recordCount)
        {
            using (StreamWriter sw = new StreamWriter("ImportLog.txt", true))
            {
                sw.WriteLine($"Дата импорта: {DateTime.Now}, Файл: {filePath}, Номер реестра: {registryNumber}, Импортировано записей: {recordCount}");
            }
        }

        private bool IsRowEmpty(IRow row)
        {
            if (row == null) return true;
            for (int col = 0; col < row.LastCellNum; col++)
            {
                if (!string.IsNullOrWhiteSpace(row.GetCell(col)?.ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        private int SafeConvertToInt32(string value, int row, string columnName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                LogMessage($"Строка {row}: Значение столбца {columnName} пустое. Пропускаем.");
                return int.MinValue;
            }

            if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }
            else
            {
                LogMessage($"Строка {row}: Неверный формат числа в столбце {columnName} ({value}). Пропускаем.");
                return int.MinValue;
            }
        }

        private int SafeConvertToInt32(ICell cell, int row, string columnName)
        {
            if (cell == null || cell.CellType == CellType.Blank)
            {
                LogMessage($"Строка {row}: Значение столбца {columnName} пустое. Пропускаем.");
                return int.MinValue;
            }

            string stringValue;
            if (cell.CellType == CellType.Numeric)
            {
                stringValue = cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                stringValue = cell.ToString();
            }

            if (int.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }
            else
            {
                LogMessage($"Строка {row}: Неверный формат числа в столбце {columnName} ({stringValue}). Пропускаем.");
                return int.MinValue;
            }
        }

        private double SafeConvertToDouble(string value, int row, string columnName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                LogMessage($"Строка {row}: Значение столбца {columnName} пустое. Пропускаем.");
                return double.MinValue;
            }

            if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            else
            {
                LogMessage($"Строка {row}: Неверный формат числа в столбце {columnName} ({value}). Пропускаем.");
                return double.MinValue;
            }
        }

        private double SafeConvertToDouble(ICell cell, int row, string columnName)
        {
            if (cell == null || cell.CellType == CellType.Blank)
            {
                LogMessage($"Строка {row}: Значение столбца {columnName} пустое. Пропускаем.");
                return double.MinValue;
            }

            string stringValue;
            if (cell.CellType == CellType.Numeric)
            {
                stringValue = cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                stringValue = cell.ToString();
            }

            if (double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            else
            {
                LogMessage($"Строка {row}: Неверный формат числа в столбце {columnName} ({stringValue}). Пропускаем.");
                return double.MinValue;
            }
        }

        private decimal SafeConvertToDecimal(string value, int row, string columnName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                LogMessage($"Строка {row}: Значение столбца {columnName} пустое. Пропускаем.");
                return decimal.MinValue;
            }

            // Удаляем пробелы как разделители тысяч и заменяем запятую на точку
            value = value.Trim().Replace(" ", "").Replace(",", ".");

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            else
            {
                LogMessage($"Строка {row}: Неверный формат числа в столбце {columnName} ({value}). Пропускаем.");
                return decimal.MinValue;
            }
        }

        private decimal SafeConvertToDecimal(ICell cell, int row, string columnName)
        {
            if (cell == null || cell.CellType == CellType.Blank)
            {
                LogMessage($"Строка {row}: Значение столбца {columnName} пустое. Пропускаем.");
                return decimal.MinValue;
            }

            string stringValue = cell.ToString(); // Берем отображаемое значение

            if (decimal.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            else
            {
                LogMessage($"Строка {row}: Неверный формат числа в столбце {columnName} ({stringValue}). Пропускаем.");
                return decimal.MinValue;
            }
        }

        private void LogMessage(string message)
        {
            txtLog.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}");
            txtLog.ScrollToCaret();
        }

        private void btnOpenDisplayForm_Click(object sender, EventArgs e)
        {
            DriverSalaryForm salaryForm = new DriverSalaryForm();
            salaryForm.Show();
            this.Hide();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Close();
        }
    }
}