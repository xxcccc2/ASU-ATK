using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;
using TransportCompany.UI.Controls;
using TransportCompany.UI.Dialogs;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Pages
{
    /// <summary>
    /// Импорт реестров: выбор файлов (кнопка или перетаскивание), асинхронный импорт
    /// с живым журналом, история импорта из БД, редактирование реестров.
    /// </summary>
    public sealed class ImportPage : AppPage
    {
        private readonly TextBox _filePath = new TextBox();
        private readonly TextBox _liveLog = new TextBox();
        private readonly ListBox _historyList = new ListBox();
        private readonly Label _statusLabel;
        private readonly Panel _dropZone = new Panel();
        private readonly List<string> _selectedFiles = new List<string>();
        private Button _importButton;

        public override string Title => "Импорт реестров";

        public ImportPage(AppServices services) : base(services)
        {
            Padding = new Padding(24, 16, 24, 16);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));

            // ===== Левая колонка: выбор файла и импорт =====
            var leftCard = new CardPanel { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 8, 0), Padding = new Padding(16) };

            var selectTitle = Styler.SectionTitle("Выбор файла");
            selectTitle.Location = new Point(16, 12);

            _filePath.Location = new Point(16, 44);
            _filePath.Width = 320;
            _filePath.ReadOnly = true;
            Styler.Input(_filePath);

            Button browse = Styler.PrimaryButton("Выбрать");
            browse.Location = new Point(344, 42);
            browse.Click += (s, e) => BrowseFiles();

            _importButton = Styler.PrimaryButton("Импортировать");
            _importButton.Location = new Point(16, 82);
            _importButton.Click += async (s, e) => await ImportAsync();

            Button clear = Styler.SecondaryButton("Очистить");
            clear.Location = new Point(148, 82);
            clear.Click += (s, e) => ClearSelection();

            Button editRegistry = Styler.SecondaryButton("Редактировать реестр");
            editRegistry.Location = new Point(240, 82);
            editRegistry.Click += (s, e) => OpenRegistryEditor();

            // Зона перетаскивания
            _dropZone.Location = new Point(16, 126);
            _dropZone.Size = new Size(420, 150);
            _dropZone.BackColor = Palette.PageBack;
            _dropZone.AllowDrop = true;
            _dropZone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _dropZone.Paint += PaintDropZone;
            _dropZone.DragEnter += (s, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                    _dropZone.BackColor = Palette.AccentSoft;
                }
            };
            _dropZone.DragLeave += (s, e) => _dropZone.BackColor = Palette.PageBack;
            _dropZone.DragDrop += (s, e) =>
            {
                _dropZone.BackColor = Palette.PageBack;
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                AddFiles(files);
            };
            _dropZone.Click += (s, e) => BrowseFiles();
            _dropZone.Cursor = Cursors.Hand;

            _statusLabel = Styler.MutedLabel("Готов к импорту данных");
            _statusLabel.Location = new Point(16, 288);

            // Живой журнал текущего импорта
            _liveLog.Location = new Point(16, 314);
            _liveLog.Multiline = true;
            _liveLog.ReadOnly = true;
            _liveLog.ScrollBars = ScrollBars.Vertical;
            _liveLog.BackColor = Color.White;
            _liveLog.Font = new Font("Consolas", 8.75f);
            _liveLog.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            _liveLog.Size = new Size(420, 160);

            leftCard.Controls.Add(selectTitle);
            leftCard.Controls.Add(_filePath);
            leftCard.Controls.Add(browse);
            leftCard.Controls.Add(_importButton);
            leftCard.Controls.Add(clear);
            leftCard.Controls.Add(editRegistry);
            leftCard.Controls.Add(_dropZone);
            leftCard.Controls.Add(_statusLabel);
            leftCard.Controls.Add(_liveLog);
            leftCard.Resize += (s, e) =>
            {
                int width = leftCard.ClientSize.Width - 32;
                _dropZone.Width = width;
                _liveLog.Width = width;
                _liveLog.Height = leftCard.ClientSize.Height - _liveLog.Top - 16;
                _filePath.Width = Math.Max(120, width - browse.Width - 12);
                browse.Left = _filePath.Right + 8;
            };

            // ===== Правая колонка: журнал импорта =====
            var rightCard = new CardPanel { Dock = DockStyle.Fill, Margin = new Padding(8, 0, 0, 0), Padding = new Padding(16) };

            var historyTitle = Styler.SectionTitle("Журнал импорта");
            historyTitle.Location = new Point(16, 12);

            _historyList.Location = new Point(16, 44);
            _historyList.BorderStyle = BorderStyle.None;
            _historyList.Font = Fonts.Small;
            _historyList.ForeColor = Palette.TextPrimary;
            _historyList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            _historyList.ItemHeight = 18;

            rightCard.Controls.Add(historyTitle);
            rightCard.Controls.Add(_historyList);
            rightCard.Resize += (s, e) =>
            {
                _historyList.Width = rightCard.ClientSize.Width - 32;
                _historyList.Height = rightCard.ClientSize.Height - _historyList.Top - 16;
            };

            layout.Controls.Add(leftCard, 0, 0);
            layout.Controls.Add(rightCard, 1, 0);
            Controls.Add(layout);
        }

        private void PaintDropZone(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Palette.Border) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash, Width = 2 })
            {
                e.Graphics.DrawRectangle(pen, 1, 1, _dropZone.Width - 3, _dropZone.Height - 3);
            }

            TextRenderer.DrawText(e.Graphics, "", Fonts.IconLarge,
                new Rectangle(0, _dropZone.Height / 2 - 40, _dropZone.Width, 32),
                Palette.Accent, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            TextRenderer.DrawText(e.Graphics,
                "Перетащите файлы сюда\nили нажмите, чтобы выбрать (.xls, .xlsx, .html)",
                Fonts.Body,
                new Rectangle(0, _dropZone.Height / 2 - 4, _dropZone.Width, 44),
                Palette.TextSecondary, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        public override void OnActivated() => LoadHistory();

        private void LoadHistory()
        {
            try
            {
                List<ImportLogEntry> entries = Services.Registry.GetImportLog();
                _historyList.Items.Clear();
                if (entries.Count == 0)
                {
                    _historyList.Items.Add("Нет данных для отображения");
                    return;
                }

                foreach (ImportLogEntry entry in entries)
                {
                    _historyList.Items.Add(
                        $"{entry.ImportDate:dd.MM.yyyy HH:mm}  Реестр №{entry.RegistryNumber}  ({entry.RecordCount} зап.)  {Path.GetFileName(entry.FilePath)}");
                }
            }
            catch (DataAccessException)
            {
                _historyList.Items.Clear();
                _historyList.Items.Add("Журнал импорта недоступен");
            }
        }

        private void BrowseFiles()
        {
            using (var dialog = new OpenFileDialog
            {
                Filter = "Excel и HTML файлы|*.xlsx;*.xls;*.html;*.htm",
                Multiselect = true
            })
            {
                if (dialog.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    AddFiles(dialog.FileNames);
                }
            }
        }

        private void AddFiles(IEnumerable<string> files)
        {
            string[] allowed = { ".xls", ".xlsx", ".html", ".htm" };
            foreach (string file in files)
            {
                if (allowed.Contains(Path.GetExtension(file).ToLowerInvariant()) &&
                    !_selectedFiles.Contains(file))
                {
                    _selectedFiles.Add(file);
                }
            }

            _filePath.Text = string.Join("; ", _selectedFiles.Select(Path.GetFileName));
            _statusLabel.Text = $"Выбрано файлов: {_selectedFiles.Count}";
        }

        private void ClearSelection()
        {
            _selectedFiles.Clear();
            _filePath.Text = "";
            _liveLog.Clear();
            _statusLabel.Text = "Готов к импорту данных";
        }

        private async Task ImportAsync()
        {
            if (_selectedFiles.Count == 0)
            {
                UiNotify.Warning("Выберите хотя бы один файл для импорта.");
                return;
            }

            _importButton.Enabled = false;
            _statusLabel.Text = "Идёт импорт...";
            _liveLog.Clear();

            var progress = new Progress<string>(message =>
            {
                _liveLog.AppendText($"{DateTime.Now:HH:mm:ss}  {message}{Environment.NewLine}");
            });

            try
            {
                List<string> files = _selectedFiles.ToList();
                List<ImportFileResult> results = await Task.Run(() =>
                    Services.RegistryImport.ImportFiles(files,
                        message => ((IProgress<string>)progress).Report(message)));

                int imported = results.Where(r => r.Success).Sum(r => r.ImportedCount);
                int duplicates = results.Where(r => r.Success).Sum(r => r.SkippedDuplicates);
                int failed = results.Count(r => !r.Success);

                _statusLabel.Text =
                    $"Импорт завершён: {imported} записей, дубликатов {duplicates}" +
                    (failed > 0 ? $", файлов с ошибками {failed}" : "");

                LoadHistory();

                if (failed > 0)
                {
                    string errors = string.Join(Environment.NewLine,
                        results.Where(r => !r.Success)
                               .Select(r => $"{Path.GetFileName(r.FileName)}: {r.Error}"));
                    UiNotify.Warning("Часть файлов не импортирована:" + Environment.NewLine + errors);
                }
                else
                {
                    UiNotify.Info($"Импорт завершён. Импортировано записей: {imported}.");
                }
            }
            catch (Exception ex)
            {
                UiNotify.Error("Ошибка импорта.", ex);
                _statusLabel.Text = "Импорт завершился с ошибкой";
            }
            finally
            {
                _importButton.Enabled = true;
            }
        }

        private void OpenRegistryEditor()
        {
            using (var dialog = new RegistryEditDialog(Services))
            {
                dialog.ShowDialog(FindForm());
            }
        }
    }
}
