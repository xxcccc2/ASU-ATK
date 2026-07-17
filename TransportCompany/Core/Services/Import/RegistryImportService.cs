using System;
using System.Collections.Generic;
using System.IO;
using TransportCompany.Core.Logging;
using TransportCompany.Core.Models;
using TransportCompany.Core.Repositories;

namespace TransportCompany.Core.Services.Import
{
    /// <summary>
    /// Импорт реестров: выбор парсера → разбор → импорт в одной транзакции на файл.
    /// Прогресс сообщается через колбэк, ошибки не прячутся.
    /// </summary>
    public sealed class RegistryImportService
    {
        private readonly RegistryRepository _repository;
        private readonly List<IRegistryFileParser> _parsers;

        public RegistryImportService(RegistryRepository repository)
        {
            _repository = repository;
            _parsers = new List<IRegistryFileParser>
            {
                // HTML первым: реестры бывают .xls с HTML-содержимым внутри.
                new HtmlRegistryParser(),
                new ExcelRegistryParser()
            };
        }

        public List<ImportFileResult> ImportFiles(IEnumerable<string> filePaths, Action<string> progress)
        {
            var results = new List<ImportFileResult>();

            foreach (string filePath in filePaths)
            {
                progress?.Invoke($"Импорт файла: {Path.GetFileName(filePath)}");

                if (!File.Exists(filePath))
                {
                    results.Add(new ImportFileResult { FileName = filePath, Error = "Файл не найден." });
                    progress?.Invoke("Файл не найден, пропущен.");
                    continue;
                }

                try
                {
                    results.Add(ImportSingleFile(filePath, progress));
                }
                catch (Exception ex)
                {
                    AppLogger.Error($"Ошибка импорта файла {filePath}", ex);
                    results.Add(new ImportFileResult { FileName = filePath, Error = ex.Message });
                    progress?.Invoke($"Ошибка: {ex.Message}");
                }
            }

            return results;
        }

        private ImportFileResult ImportSingleFile(string filePath, Action<string> progress)
        {
            IRegistryFileParser parser = SelectParser(filePath);
            if (parser == null)
            {
                throw new InvalidDataException("Неподдерживаемый формат файла. Поддерживаются .xls, .xlsx, .html.");
            }

            RegistryParseResult parseResult = parser.Parse(filePath);

            foreach (string warning in parseResult.Warnings)
            {
                progress?.Invoke(warning);
            }

            progress?.Invoke(
                $"Реестр №{parseResult.RegistryNumber}: распознано строк — {parseResult.Rows.Count}.");

            ImportFileResult result = _repository.ImportRegistryRows(parseResult, filePath, progress);
            result.SkippedInvalid = parseResult.Warnings.Count;

            progress?.Invoke(
                $"Готово: импортировано {result.ImportedCount}, дубликатов {result.SkippedDuplicates}.");
            AppLogger.Info(
                $"Импорт {filePath}: реестр №{result.RegistryNumber}, импортировано {result.ImportedCount}, дубликатов {result.SkippedDuplicates}.");

            return result;
        }

        private IRegistryFileParser SelectParser(string filePath)
        {
            foreach (IRegistryFileParser parser in _parsers)
            {
                if (parser.CanParse(filePath))
                {
                    return parser;
                }
            }
            return null;
        }
    }
}
