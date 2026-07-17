using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TransportCompany.Core;
using TransportCompany.Core.Data;
using TransportCompany.Core.Models;
using TransportCompany.UI.Controls;
using TransportCompany.UI.Dialogs;
using TransportCompany.UI.Theme;

namespace TransportCompany.UI.Pages
{
    /// <summary>Дневник автопарка: полисы ОСАГО и водительские удостоверения.</summary>
    public sealed class FleetDiaryPage : AppPage
    {
        private readonly TabControl _tabs = new TabControl();
        private readonly DataGridView _osagoGrid = new DataGridView();
        private readonly DataGridView _licensesGrid = new DataGridView();
        private readonly Label _osagoCount;
        private readonly Label _licensesCount;

        public override string Title => "Дневник Автопарка";

        public FleetDiaryPage(AppServices services) : base(services)
        {
            Padding = new Padding(24, 16, 24, 16);

            _tabs.Dock = DockStyle.Fill;
            _tabs.Font = Fonts.Body;

            // ----- Вкладка ОСАГО -----
            var osagoTab = new TabPage("ОСАГО на грузовики") { BackColor = Palette.PageBack, Padding = new Padding(12) };
            _osagoCount = Styler.MutedLabel("Записей: 0");
            osagoTab.Controls.Add(BuildTabContent(
                _osagoGrid, _osagoCount,
                onAdd: AddOsago, onEdit: EditOsago, onDelete: DeleteOsago));

            // ----- Вкладка удостоверений -----
            var licensesTab = new TabPage("Водительские удостоверения") { BackColor = Palette.PageBack, Padding = new Padding(12) };
            _licensesCount = Styler.MutedLabel("Записей: 0");
            licensesTab.Controls.Add(BuildTabContent(
                _licensesGrid, _licensesCount,
                onAdd: AddLicense, onEdit: EditLicense, onDelete: DeleteLicense));

            _tabs.TabPages.Add(osagoTab);
            _tabs.TabPages.Add(licensesTab);
            Controls.Add(_tabs);

            SetupOsagoGrid();
            SetupLicensesGrid();
        }

        private Control BuildTabContent(DataGridView grid, Label countLabel,
            Action onAdd, Action onEdit, Action onDelete)
        {
            var container = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            var toolbar = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(0, 4, 0, 0),
                BackColor = Color.Transparent
            };

            Button addButton = Styler.PrimaryButton("Добавить", "+");
            Button editButton = Styler.SecondaryButton("Редактировать");
            Button deleteButton = Styler.DangerButton("Удалить");
            addButton.Click += (s, e) => onAdd();
            editButton.Click += (s, e) => onEdit();
            deleteButton.Click += (s, e) => onDelete();
            editButton.Margin = new Padding(8, 0, 0, 0);
            addButton.Margin = new Padding(8, 0, 0, 0);

            toolbar.Controls.Add(deleteButton);
            toolbar.Controls.Add(editButton);
            toolbar.Controls.Add(addButton);

            var card = new CardPanel { Dock = DockStyle.Fill, Padding = new Padding(1) };
            Styler.Grid(grid);
            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) onEdit(); };
            card.Controls.Add(grid);

            var statusBar = new Panel { Dock = DockStyle.Bottom, Height = 30, BackColor = Color.Transparent };
            countLabel.Location = new Point(0, 8);
            statusBar.Controls.Add(countLabel);

            container.Controls.Add(card);
            container.Controls.Add(statusBar);
            container.Controls.Add(toolbar);
            card.BringToFront();
            return container;
        }

        private void SetupOsagoGrid()
        {
            _osagoGrid.Columns.Add("Vehicle", "Номер машины");
            _osagoGrid.Columns.Add("Policy", "Номер полиса");
            _osagoGrid.Columns.Add("Start", "Начало действия");
            _osagoGrid.Columns.Add("End", "Окончание действия");
            _osagoGrid.Columns.Add("Status", "Статус");
        }

        private void SetupLicensesGrid()
        {
            _licensesGrid.Columns.Add("Driver", "Водитель");
            _licensesGrid.Columns.Add("Number", "Номер ВУ");
            _licensesGrid.Columns.Add("Issue", "Дата выдачи");
            _licensesGrid.Columns.Add("Expiry", "Действует до");
            _licensesGrid.Columns.Add("Status", "Статус");
            _licensesGrid.Columns["Driver"].FillWeight = 150;
        }

        public override void OnActivated() => LoadData();

        private void LoadData()
        {
            try
            {
                LoadOsago();
                LoadLicenses();
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error("Не удалось загрузить данные дневника автопарка.", ex);
            }
        }

        private void LoadOsago()
        {
            List<OsagoPolicy> policies = Services.FleetDocuments.GetOsagoPolicies();

            _osagoGrid.Rows.Clear();
            foreach (OsagoPolicy policy in policies)
            {
                string status = policy.IsExpired ? "Истёк" : policy.IsExpiringSoon ? "Истекает" : "Действует";
                int rowIndex = _osagoGrid.Rows.Add(
                    policy.VehicleRegistrationNumber, policy.PolicyNumber,
                    policy.StartDate.ToString("dd.MM.yyyy"), policy.EndDate.ToString("dd.MM.yyyy"),
                    status);

                DataGridViewRow row = _osagoGrid.Rows[rowIndex];
                row.Tag = policy;
                StyleStatusCell(row.Cells["Status"], policy.IsExpired, policy.IsExpiringSoon);
            }
            _osagoGrid.ClearSelection();
            _osagoCount.Text = $"Записей: {policies.Count}";
        }

        private void LoadLicenses()
        {
            List<DriverLicenseRecord> licenses = Services.FleetDocuments.GetLicenses();

            _licensesGrid.Rows.Clear();
            foreach (DriverLicenseRecord license in licenses)
            {
                string status = license.IsExpired ? "Истёк" : license.IsExpiringSoon ? "Истекает" : "Действует";
                int rowIndex = _licensesGrid.Rows.Add(
                    license.DriverFullName, license.LicenseNumber,
                    license.IssueDate.ToString("dd.MM.yyyy"), license.ExpiryDate.ToString("dd.MM.yyyy"),
                    status);

                DataGridViewRow row = _licensesGrid.Rows[rowIndex];
                row.Tag = license;
                StyleStatusCell(row.Cells["Status"], license.IsExpired, license.IsExpiringSoon);
            }
            _licensesGrid.ClearSelection();
            _licensesCount.Text = $"Записей: {licenses.Count}";
        }

        private static void StyleStatusCell(DataGridViewCell cell, bool expired, bool expiringSoon)
        {
            Color color = expired ? Palette.Danger : expiringSoon ? Palette.Warning : Palette.Success;
            cell.Style.ForeColor = color;
            cell.Style.SelectionForeColor = color;
            cell.Style.Font = Fonts.BodyBold;
        }

        // ----- ОСАГО -----

        private void AddOsago()
        {
            using (var dialog = new OsagoEditDialog(null))
            {
                if (dialog.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    TrySave(() => Services.FleetDocuments.SaveOsago(dialog.Policy), "Не удалось сохранить полис ОСАГО.");
                }
            }
        }

        private void EditOsago()
        {
            if (!(_osagoGrid.CurrentRow?.Tag is OsagoPolicy policy))
            {
                UiNotify.Warning("Выберите запись для редактирования.");
                return;
            }

            using (var dialog = new OsagoEditDialog(policy))
            {
                if (dialog.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    TrySave(() => Services.FleetDocuments.SaveOsago(dialog.Policy), "Не удалось сохранить полис ОСАГО.");
                }
            }
        }

        private void DeleteOsago()
        {
            if (!(_osagoGrid.CurrentRow?.Tag is OsagoPolicy policy))
            {
                UiNotify.Warning("Выберите запись для удаления.");
                return;
            }

            if (UiNotify.Confirm($"Удалить полис {policy.PolicyNumber} для {policy.VehicleRegistrationNumber}?"))
            {
                TrySave(() => Services.FleetDocuments.DeleteOsago(policy.Id), "Не удалось удалить полис ОСАГО.");
            }
        }

        // ----- Удостоверения -----

        private void AddLicense()
        {
            using (var dialog = new LicenseEditDialog(null))
            {
                if (dialog.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    TrySave(() => Services.FleetDocuments.SaveLicense(dialog.License), "Не удалось сохранить удостоверение.");
                }
            }
        }

        private void EditLicense()
        {
            if (!(_licensesGrid.CurrentRow?.Tag is DriverLicenseRecord license))
            {
                UiNotify.Warning("Выберите запись для редактирования.");
                return;
            }

            using (var dialog = new LicenseEditDialog(license))
            {
                if (dialog.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    TrySave(() => Services.FleetDocuments.SaveLicense(dialog.License), "Не удалось сохранить удостоверение.");
                }
            }
        }

        private void DeleteLicense()
        {
            if (!(_licensesGrid.CurrentRow?.Tag is DriverLicenseRecord license))
            {
                UiNotify.Warning("Выберите запись для удаления.");
                return;
            }

            if (UiNotify.Confirm($"Удалить удостоверение {license.LicenseNumber} ({license.DriverFullName})?"))
            {
                TrySave(() => Services.FleetDocuments.DeleteLicense(license.Id), "Не удалось удалить удостоверение.");
            }
        }

        private void TrySave(Action action, string errorMessage)
        {
            try
            {
                action();
                LoadData();
            }
            catch (DataAccessException ex)
            {
                UiNotify.Error(errorMessage, ex);
            }
        }
    }
}
