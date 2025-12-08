using MaterialSkin;
using MaterialSkin.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace TransportCompany
{
    public class BaseForm : MaterialForm
    {
        protected MaterialSkinManager materialSkinManager;

        public BaseForm()
        {
            // Инициализация MaterialSkin
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            
            // Настройка цветовой схемы
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            
            // Основные цвета (можно настроить под вашу палитру)
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue800,    // Основной цвет
                Primary.Blue900,    // Темный основной цвет
                Primary.Blue500,    // Светлый основной цвет
                Accent.LightBlue200,// Акцентный цвет
                TextShade.WHITE     // Цвет текста
            );

            // Настройка шрифтов
            materialSkinManager.ROBOTO_MEDIUM_12 = new Font("Segoe UI", 12F, FontStyle.Regular);
            materialSkinManager.ROBOTO_MEDIUM_11 = new Font("Segoe UI", 11F, FontStyle.Regular);
            materialSkinManager.ROBOTO_REGULAR_11 = new Font("Segoe UI", 11F, FontStyle.Regular);
            materialSkinManager.ROBOTO_MEDIUM_10 = new Font("Segoe UI", 10F, FontStyle.Regular);

            // Настройка формы
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);
        }

        // Метод для создания MaterialButton
        protected MaterialButton CreateMaterialButton(string text, Point location, Size size)
        {
            return new MaterialButton
            {
                Text = text,
                Location = location,
                Size = size,
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccent = true
            };
        }

        // Метод для создания MaterialTextBox
        protected MaterialTextBox2 CreateMaterialTextBox(string hint, Point location, Size size)
        {
            return new MaterialTextBox2
            {
                Hint = hint,
                Location = location,
                Size = size,
                UseAccent = true
            };
        }

        // Метод для создания MaterialComboBox
        protected MaterialComboBox CreateMaterialComboBox(Point location, Size size)
        {
            return new MaterialComboBox
            {
                Location = location,
                Size = size,
                UseAccent = true
            };
        }

        // Метод для создания MaterialDataTable
        protected MaterialDataTable CreateMaterialDataTable(Point location, Size size)
        {
            return new MaterialDataTable
            {
                Location = location,
                Size = size,
                UseAccent = true
            };
        }
    }
} 