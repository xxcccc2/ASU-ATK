using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;
using TransportCompany.UI.Controls;
using TransportCompany.UI.Dialogs;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Pages
{
    /// <summary>Техосмотр: поиск, таблица, добавление/редактирование/удаление.</summary>
    public sealed class MaintenancePage : AppPage
    {
        private readonly TextBox _search = new TextBox();
        private readonly ComboBox _dateFilter = new ComboBox();
        private readonly DataGridView _grid = new DataGridView();
        private readonly Label _countLabel;
        private List<MaintenanceRecord> _records = new List<MaintenanceRecord>();

        public override string Title => "Техосмотр";

        public MaintenancePage(AppServices services) : base(services)
        {
            Padding = new Padding(24, 16, 24, 16);

            // ----- Панель инструментов -----
            var toolbar = new Panel { Dock = DockStyle.Top, Height = 48, BackColor = Color.Transparent };

            _search.Width = 220;
            _search.Location = new Point(0, 10);
            Styler.Input(_search);
            SetPlaceholder(_search, "Поиск по номеру...");
            _search.TextChanged += (s, e) => ApplyFilter();

            _dateFilter.Width = 150;
            _dateFilter.Location = new Point(232, 10);
            _dateFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            _dateFilter.Items.AddRange(new object[] { "Все даты", "Последний год", "Последние 3 месяца", "Просроченные ТО" });
            _dateFilter.SelectedIndex = 0;
            Styler.Input(_dateFilter);
            _dateFilter.SelectedIndexChanged += (s, e) => ApplyFilter();

            Button addButton = Styler.PrimaryButton("Добавить", "+");
            Button editButton = Styler.SecondaryButton("Редактировать");
            Button deleteButton = Styler.DangerButton("Удалить");
            addButton.Click += (s, e) => AddRecord();
            editButton.Click += (s, e) => EditRecord();
            deleteButton.Click += (s, e) => DeleteRecord();

            var buttonsFlow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Right,
                Width = 400,
                Padding = new Padding(0, 6, 0, 0),
                BackColor = Color.Transparent
            };
            deleteButton.Margin = new Padding(8, 0, 0, 0);
            editButton.Margin = new Padding(8, 0, 0, 0);
            buttonsFlow.Controls.Add(deleteButton);
            buttonsFlow.Controls.Add(editButton);
            buttonsFlow.Controls.Add(addButton);

            toolbar.Controls.Add(_search);
            toolbar.Controls.Add(_dateFilter);
            toolbar.Controls.Add(buttonsFlow);

            // ----- Таблица -----
            var card = new CardPanel { Dock = DockStyle.Fill, Padding = new Padding(1) };
            Styler.Grid(_grid);
            _grid.Dock = DockStyle.Fill;
            _grid.ReadOnly = true;
            _grid.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditRecord(); };
            card.Controls.Add(_grid);

            // ----- Строка состояния -----
            var statusBar = new Panel { Dock = DockStyle.Bottom, Height = 32, BackColor = Color.Transparent };
            _countLabel = Styler.MutedLabel("Записей: 0");
            _countLabel.Location = new Point(0, 10);
            statusBar.Controls.Add(_countLabel);

            Controls.Add(card);
            Controls.Add(statusBar);
            Controls.Add(toolbar);
            card.BringToFront();
        }

        public override void OnActivated() => LoadData();

        private void LoadData()
        {
            try
            {
                _records = Services.Maintenance.GetAll();
                ApplyFilter();
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось загрузить данные техобслуживания.", ex);
            }
        }

        private void ApplyFilter()
        {
            IEnumerable<MaintenanceRecord> filtered = _records;

            string search = _search.ForeColor == Palette.TextSecondary ? "" : _search.Text.Trim();
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(r =>
                    r.CarNumber.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            switch (_dateFilter.SelectedIndex)
            {
                case 1:
                    filtered = filtered.Where(r => r.LastServiceDate >= DateTime.Today.AddYears(-1));
                    break;
                case 2:
                    filtered = filtered.Where(r => r.LastServiceDate >= DateTime.Today.AddMonths(-3));
                    break;
                case 3:
                    filtered = filtered.Where(r => r.IsOverdue);
                    break;
            }

            List<MaintenanceRecord> list = filtered.ToList();

            _grid.SuspendLayout();
            _grid.DataSource = null;
            _grid.Columns.Clear();
            _grid.Columns.Add("CarNumber", "Номер машины");
            _grid.Columns.Add("Date", "Дата ТО");
            _grid.Columns.Add("Mileage", "Пробег (км)");
            _grid.Columns.Add("Status", "Статус");
            _grid.Columns.Add("Comment", "Комментарий");
            _grid.Columns["Comment"].FillWeight = 160;

            foreach (MaintenanceRecord record in list)
            {
                int rowIndex = _grid.Rows.Add(
                    record.CarNumber,
                    record.LastServiceDate.ToString("dd.MM.yyyy"),
                    record.MileageKm.ToString("N0"),
                    record.IsOverdue ? "Просрочено" : "В порядке",
                    record.Comment);

                DataGridViewRow row = _grid.Rows[rowIndex];
                row.Tag = record;
                DataGridViewCell statusCell = row.Cells["Status"];
                statusCell.Style.ForeColor = record.IsOverdue ? Palette.Danger : Palette.Success;
                statusCell.Style.SelectionForeColor = statusCell.Style.ForeColor;
                statusCell.Style.Font = Fonts.BodyBold;
            }
            _grid.ResumeLayout();
            _grid.ClearSelection();

            _countLabel.Text = $"Записей: {list.Count}";
        }

        private MaintenanceRecord SelectedRecord =>
            _grid.CurrentRow?.Tag as MaintenanceRecord;

        private void AddRecord()
        {
            using (var dialog = new MaintenanceEditDialog(null))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    Services.Maintenance.Add(dialog.Record);
                    LoadData();
                }
                catch (DataAccessException ex)
                {
                    UiNotify.Error("Не удалось добавить запись ТО.", ex);
                }
            }
        }

        private void EditRecord()
        {
            MaintenanceRecord record = SelectedRecord;
            if (record == null)
            {
                UiNotify.Warning("Выберите запись для редактирования.");
                return;
            }

            using (var dialog = new MaintenanceEditDialog(record))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    Services.Maintenance.Update(dialog.Record);
                    LoadData();
                }
                catch (DataAccessException ex)
                {
                    UiNotify.Error("Не удалось сохранить запись ТО.", ex);
                }
            }
        }

        private void DeleteRecord()
        {
            MaintenanceRecord record = SelectedRecord;
            if (record == null)
            {
                UiNotify.Warning("Выберите запись для удаления.");
                return;
            }

            if (!UiNotify.Confirm(
                $"Удалить запись ТО для машины {record.CarNumber} от {record.LastServiceDate:dd.MM.yyyy}?"))
            {
                return;
            }

            try
            {
                Services.Maintenance.Delete(record.Id);
                LoadData();
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось удалить запись ТО.", ex);
            }
        }

        private static void SetPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Palette.TextSecondary;

            textBox.GotFocus += (s, e) =>
            {
                if (textBox.ForeColor == Palette.TextSecondary)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Palette.TextPrimary;
                }
            };
            textBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Palette.TextSecondary;
                }
            };
        }
    }
}
