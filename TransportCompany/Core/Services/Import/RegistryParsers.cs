using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TransportCompany.Core.Models;

namespace TransportCompany.Core.Services.Import
{
    public interface IRegistryFileParser
    {
        bool CanParse(string filePath);
        RegistryParseResult Parse(string filePath);
    }

    /// <summary>Общие преобразования значений ячеек реестра.</summary>
    internal static class ParseUtil
    {
        public static int? ToInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return int.TryParse(value.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out int result)
                ? result
                : (int?)null;
        }

        public static double? ToDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return double.TryParse(value.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double result)
                ? result
                : (double?)null;
        }

        public static decimal? ToDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            string normalized = value.Trim().Replace(" ", "").Replace(" ", "").Replace(",", ".");
            return decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result)
                ? result
                : (decimal?)null;
        }

        public static int ExtractRegistryNumber(string cellValue)
        {
            if (string.IsNullOrWhiteSpace(cellValue)) return -1;

            string cleaned = new string(cellValue
                .Where(c => !char.IsControl(c) && !char.IsWhiteSpace(c) || char.IsLetterOrDigit(c) || c == ' ')
                .ToArray()).Trim();

            string digits = Regex.Match(cleaned, @"^\d+").Value;
            return int.TryParse(digits, out int number) ? number : -1;
        }
    }

    /// <summary>Разбор реестра из Excel (.xls / .xlsx) через NPOI.</summary>
    public sealed class ExcelRegistryParser : IRegistryFileParser
    {
        private const int FirstDataRow = 14;

        public bool CanParse(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension == ".xls" || extension == ".xlsx";
        }

        public RegistryParseResult Parse(string filePath)
        {
            var result = new RegistryParseResult();

            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = Path.GetExtension(filePath).ToLowerInvariant() == ".xls"
                    ? (IWorkbook)new HSSFWorkbook(stream)
                    : new XSSFWorkbook(stream);

                ISheet sheet = workbook.GetSheetAt(0);

                result.RegistryNumber = ReadRegistryNumber(sheet);
                if (result.RegistryNumber == -1)
                {
                    throw new InvalidDataException("Не удалось найти номер реестра в файле (ячейки C1/D1).");
                }

                int rowCount = sheet.PhysicalNumberOfRows;
                for (int rowIndex = FirstDataRow; rowIndex < rowCount; rowIndex++)
                {
                    IRow row = sheet.GetRow(rowIndex);
                    if (row == null || IsRowEmpty(row))
                    {
                        continue;
                    }

                    string dateCellText = row.GetCell(1)?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(dateCellText) &&
                        dateCellText.StartsWith("ИТОГО", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    RegistryRow parsed = ParseRow(row, rowIndex + 1, result);
                    if (parsed != null)
                    {
                        result.Rows.Add(parsed);
                    }
                }
            }

            return result;
        }

        private static int ReadRegistryNumber(ISheet sheet)
        {
            string prefix = sheet.GetRow(0)?.GetCell(2)?.ToString()?.Trim() ?? "";
            if (prefix.IndexOf("Реестр Рейсов №", StringComparison.CurrentCultureIgnoreCase) < 0)
            {
                return -1;
            }

            return ParseUtil.ExtractRegistryNumber(sheet.GetRow(0)?.GetCell(3)?.ToString());
        }

        private static RegistryRow ParseRow(IRow row, int rowNumber, RegistryParseResult result)
        {
            string fio = row.GetCell(2)?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(fio) || fio.Length < 2)
            {
                return null;
            }

            DateTime? date = ReadDate(row.GetCell(1));
            if (date == null)
            {
                result.Warnings.Add($"Строка {rowNumber}: неверный формат даты. Пропущена.");
                return null;
            }

            int? npp = ReadInt(row.GetCell(0));
            double? tonnage = ReadDouble(row.GetCell(5));
            decimal? tripCost = ReadDecimal(row.GetCell(11));
            int? unloadPoints = ReadInt(row.GetCell(13));
            int? loadPoints = ReadInt(row.GetCell(14));
            int? zone = ReadInt(row.GetCell(15));
            decimal? extraStores = ReadDecimal(row.GetCell(16));
            decimal? extraLoad = ReadDecimal(row.GetCell(17));
            decimal? supply = ReadDecimal(row.GetCell(18));
            decimal? sumTtk = ReadDecimal(row.GetCell(20));
            decimal? kmCost = ReadDecimal(row.GetCell(21));
            decimal? discount = ReadDecimal(row.GetCell(22));
            decimal? totalWithoutVat = ReadDecimal(row.GetCell(23));
            decimal? totalWithVat = ReadDecimal(row.GetCell(24));

            if (npp == null || tonnage == null || tripCost == null || unloadPoints == null ||
                loadPoints == null || zone == null || extraStores == null || extraLoad == null ||
                supply == null || sumTtk == null || kmCost == null || discount == null ||
                totalWithoutVat == null || totalWithVat == null)
            {
                result.Warnings.Add($"Строка {rowNumber}: не удалось разобрать числовые значения. Пропущена.");
                return null;
            }

            return new RegistryRow
            {
                SourceRowNumber = rowNumber,
                Npp = npp.Value,
                Date = date,
                Fio = fio,
                Nsl = row.GetCell(3)?.ToString()?.Trim(),
                GosNumber = row.GetCell(4)?.ToString()?.Trim(),
                Tonnage = tonnage.Value,
                VehicleType = row.GetCell(6)?.ToString()?.Trim(),
                TransportNumber = row.GetCell(7)?.ToString()?.Trim(),
                RcLoad = row.GetCell(8)?.ToString()?.Trim(),
                Branch = row.GetCell(9)?.ToString()?.Trim(),
                DeliveryRegion = row.GetCell(10)?.ToString()?.Trim(),
                TripCost = tripCost.Value,
                OrderNumber = row.GetCell(12)?.ToString()?.Trim(),
                UnloadPoints = unloadPoints.Value,
                LoadPoints = loadPoints.Value,
                Zone = zone.Value,
                ExtraStores = extraStores.Value,
                ExtraLoad = extraLoad.Value,
                Supply = supply.Value,
                NqNumber = row.GetCell(19)?.ToString()?.Trim(),
                SumTtk = sumTtk.Value,
                KmCost = kmCost.Value,
                Discount = discount.Value,
                TotalWithoutVat = totalWithoutVat.Value,
                TotalWithVat = totalWithVat.Value,
                TransportNumber2 = row.GetCell(25)?.ToString()?.Trim()
            };
        }

        private static DateTime? ReadDate(ICell cell)
        {
            if (cell == null) return null;

            if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
            {
                return cell.DateCellValue;
            }

            return DateTime.TryParse(cell.ToString(), out DateTime parsed) ? parsed : (DateTime?)null;
        }

        private static int? ReadInt(ICell cell)
        {
            if (cell == null || cell.CellType == CellType.Blank) return null;
            string text = cell.CellType == CellType.Numeric
                ? cell.NumericCellValue.ToString(CultureInfo.InvariantCulture)
                : cell.ToString();
            return ParseUtil.ToInt(text);
        }

        private static double? ReadDouble(ICell cell)
        {
            if (cell == null || cell.CellType == CellType.Blank) return null;
            string text = cell.CellType == CellType.Numeric
                ? cell.NumericCellValue.ToString(CultureInfo.InvariantCulture)
                : cell.ToString();
            return ParseUtil.ToDouble(text);
        }

        private static decimal? ReadDecimal(ICell cell)
        {
            if (cell == null || cell.CellType == CellType.Blank) return null;
            return ParseUtil.ToDecimal(cell.ToString());
        }

        private static bool IsRowEmpty(IRow row)
        {
            for (int col = 0; col < row.LastCellNum; col++)
            {
                if (!string.IsNullOrWhiteSpace(row.GetCell(col)?.ToString()))
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>Разбор реестра из HTML-выгрузки (таблицы class="headers" и class="registry").</summary>
    public sealed class HtmlRegistryParser : IRegistryFileParser
    {
        private const int FirstDataRow = 2;
        private const int ExpectedCellCount = 26;

        public bool CanParse(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            if (extension == ".html" || extension == ".htm")
            {
                return true;
            }

            // Реестры иногда выгружаются как .xls с HTML внутри.
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string firstLine = reader.ReadLine()?.Trim();
                    return firstLine != null && firstLine.StartsWith("<html", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch
            {
                return false;
            }
        }

        public RegistryParseResult Parse(string filePath)
        {
            var result = new RegistryParseResult();

            var document = new HtmlDocument();
            document.Load(filePath, System.Text.Encoding.UTF8);

            var tables = document.DocumentNode.SelectNodes("//table");
            if (tables == null || tables.Count == 0)
            {
                throw new InvalidDataException("Таблицы не найдены в HTML-файле.");
            }

            result.RegistryNumber = -1;
            foreach (HtmlNode table in tables)
            {
                if (table.GetAttributeValue("class", "").Equals("headers"))
                {
                    result.RegistryNumber = ReadRegistryNumber(table);
                    if (result.RegistryNumber != -1)
                    {
                        break;
                    }
                }
            }

            if (result.RegistryNumber == -1)
            {
                throw new InvalidDataException("Не удалось найти номер реестра в HTML-файле.");
            }

            HtmlNode dataTable = tables.FirstOrDefault(t => t.GetAttributeValue("class", "").Equals("registry"));
            if (dataTable == null)
            {
                throw new InvalidDataException("Таблица с данными (class='registry') не найдена в HTML-файле.");
            }

            var rows = dataTable.SelectNodes(".//tr");
            if (rows == null)
            {
                throw new InvalidDataException("Строки таблицы не найдены в HTML-файле.");
            }

            for (int rowIndex = FirstDataRow; rowIndex < rows.Count; rowIndex++)
            {
                var cells = rows[rowIndex].SelectNodes("td");
                if (cells == null || cells.Count < ExpectedCellCount)
                {
                    result.Warnings.Add($"Строка {rowIndex + 1}: недостаточно ячеек ({cells?.Count ?? 0}). Пропущена.");
                    continue;
                }

                string dateText = cells[1].InnerText.Trim();
                if (dateText.StartsWith("ИТОГО", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                RegistryRow parsed = ParseRow(cells, rowIndex + 1, result);
                if (parsed != null)
                {
                    result.Rows.Add(parsed);
                }
            }

            return result;
        }

        private static int ReadRegistryNumber(HtmlNode headerTable)
        {
            var cells = headerTable.SelectSingleNode(".//tr[1]")?.SelectNodes("td");
            if (cells == null || cells.Count < 4)
            {
                return -1;
            }

            string prefix = cells[2].InnerText.Trim();
            if (prefix.IndexOf("Реестр Рейсов №", StringComparison.CurrentCultureIgnoreCase) < 0)
            {
                return -1;
            }

            return ParseUtil.ExtractRegistryNumber(cells[3].InnerText);
        }

        private static RegistryRow ParseRow(HtmlNodeCollection cells, int rowNumber, RegistryParseResult result)
        {
            string fio = cells[2].InnerText.Trim();
            if (string.IsNullOrWhiteSpace(fio) || fio.Length < 2)
            {
                return null;
            }

            string dateText = cells[1].InnerText.Trim();
            if (!DateTime.TryParse(dateText, out DateTime date))
            {
                result.Warnings.Add($"Строка {rowNumber}: неверный формат даты ('{dateText}'). Пропущена.");
                return null;
            }

            int? npp = ParseUtil.ToInt(cells[0].InnerText);
            double? tonnage = ParseUtil.ToDouble(cells[5].InnerText);
            decimal? tripCost = ParseUtil.ToDecimal(cells[11].InnerText);
            int? unloadPoints = ParseUtil.ToInt(cells[13].InnerText);
            int? loadPoints = ParseUtil.ToInt(cells[14].InnerText);
            int? zone = ParseUtil.ToInt(cells[15].InnerText);
            decimal? extraStores = ParseUtil.ToDecimal(cells[16].InnerText);
            decimal? extraLoad = ParseUtil.ToDecimal(cells[17].InnerText);
            decimal? supply = ParseUtil.ToDecimal(cells[18].InnerText);
            decimal? sumTtk = ParseUtil.ToDecimal(cells[20].InnerText);
            decimal? kmCost = ParseUtil.ToDecimal(cells[21].InnerText);
            decimal? discount = ParseUtil.ToDecimal(cells[22].InnerText);
            decimal? totalWithoutVat = ParseUtil.ToDecimal(cells[23].InnerText);
            decimal? totalWithVat = ParseUtil.ToDecimal(cells[24].InnerText);

            if (npp == null || tonnage == null || tripCost == null || unloadPoints == null ||
                loadPoints == null || zone == null || extraStores == null || extraLoad == null ||
                supply == null || sumTtk == null || kmCost == null || discount == null ||
                totalWithoutVat == null || totalWithVat == null)
            {
                result.Warnings.Add($"Строка {rowNumber}: не удалось разобрать числовые значения. Пропущена.");
                return null;
            }

            return new RegistryRow
            {
                SourceRowNumber = rowNumber,
                Npp = npp.Value,
                Date = date,
                Fio = fio,
                Nsl = cells[3].InnerText.Trim(),
                GosNumber = cells[4].InnerText.Trim(),
                Tonnage = tonnage.Value,
                VehicleType = cells[6].InnerText.Trim(),
                TransportNumber = cells[7].InnerText.Trim(),
                RcLoad = cells[8].InnerText.Trim(),
                Branch = cells[9].InnerText.Trim(),
                DeliveryRegion = cells[10].InnerText.Trim(),
                TripCost = tripCost.Value,
                OrderNumber = cells[12].InnerText.Trim(),
                UnloadPoints = unloadPoints.Value,
                LoadPoints = loadPoints.Value,
                Zone = zone.Value,
                ExtraStores = extraStores.Value,
                ExtraLoad = extraLoad.Value,
                Supply = supply.Value,
                NqNumber = cells[19].InnerText.Trim(),
                SumTtk = sumTtk.Value,
                KmCost = kmCost.Value,
                Discount = discount.Value,
                TotalWithoutVat = totalWithoutVat.Value,
                TotalWithVat = totalWithVat.Value,
                TransportNumber2 = cells[25].InnerText.Trim()
            };
        }
    }
}
