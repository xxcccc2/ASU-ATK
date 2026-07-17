using System;
using System.Collections.Generic;

namespace TransportCompany.Core.Models
{
    /// <summary>Одна строка реестра рейсов, распознанная из файла (Excel или HTML).</summary>
    public sealed class RegistryRow
    {
        public int Npp { get; set; }
        public DateTime? Date { get; set; }
        public string Fio { get; set; }
        public string Nsl { get; set; }
        public string GosNumber { get; set; }
        public double Tonnage { get; set; }
        public string VehicleType { get; set; }
        public string TransportNumber { get; set; }
        public string RcLoad { get; set; }
        public string Branch { get; set; }
        public string DeliveryRegion { get; set; }
        public decimal TripCost { get; set; }
        public string OrderNumber { get; set; }
        public int UnloadPoints { get; set; }
        public int LoadPoints { get; set; }
        public int Zone { get; set; }
        public decimal ExtraStores { get; set; }
        public decimal ExtraLoad { get; set; }
        public decimal Supply { get; set; }
        public string NqNumber { get; set; }
        public decimal SumTtk { get; set; }
        public decimal KmCost { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalWithoutVat { get; set; }
        public decimal TotalWithVat { get; set; }
        public string TransportNumber2 { get; set; }

        /// <summary>Номер строки в исходном файле — для сообщений об ошибках.</summary>
        public int SourceRowNumber { get; set; }
    }

    /// <summary>Результат разбора одного файла реестра.</summary>
    public sealed class RegistryParseResult
    {
        public int RegistryNumber { get; set; }
        public List<RegistryRow> Rows { get; } = new List<RegistryRow>();
        public List<string> Warnings { get; } = new List<string>();
    }

    /// <summary>Итог импорта одного файла.</summary>
    public sealed class ImportFileResult
    {
        public string FileName { get; set; }
        public int RegistryNumber { get; set; }
        public int ImportedCount { get; set; }
        public int SkippedDuplicates { get; set; }
        public int SkippedInvalid { get; set; }
        public string Error { get; set; }

        public bool Success => Error == null;
    }

    /// <summary>Запись журнала импорта из БД.</summary>
    public sealed class ImportLogEntry
    {
        public int RegistryNumber { get; set; }
        public string FilePath { get; set; }
        public DateTime ImportDate { get; set; }
        public int RecordCount { get; set; }
    }
}
