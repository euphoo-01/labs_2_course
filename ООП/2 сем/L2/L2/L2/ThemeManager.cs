using System.Drawing;
using System.Windows.Forms;

namespace ShopApp.Services
{
    public static class ThemeManager
    {
        // Палитра светлой темы
        public static readonly Color BgColor = Color.FromArgb(240, 240, 243);      // Светло-серый фон окна
        public static readonly Color ControlBg = Color.FromArgb(255, 255, 255);   // Чисто белый фон полей
        public static readonly Color TextColor = Color.FromArgb(30, 30, 30);      // Почти черный текст
        public static readonly Color AccentColor = Color.FromArgb(0, 120, 215);   // Синий акцент (кнопки)
        public static readonly Color BorderColor = Color.FromArgb(200, 200, 200); // Серые границы

        public static void ApplyLight(Control parent)
        {
            // Устанавливаем базовые цвета для контейнера
            parent.BackColor = BgColor;
            parent.ForeColor = TextColor;

            foreach (Control c in parent.Controls)
            {
                // Поля ввода, списки и выпадающие списки
                if (c is TextBox || c is NumericUpDown || c is ListBox || c is ComboBox || c is MaskedTextBox)
                {
                    c.BackColor = ControlBg;
                    c.ForeColor = TextColor;
                    
                    if (c is TextBox t) t.BorderStyle = BorderStyle.FixedSingle;
                    if (c is ListBox lb) lb.BorderStyle = BorderStyle.FixedSingle;
                    
                    // Для ComboBox в Mono лучше использовать FlatStyle
                    if (c is ComboBox cb) cb.FlatStyle = FlatStyle.Flat;
                }
                // Календарь (DateTimePicker) - самая проблемная часть в Mono
                else if (c is DateTimePicker dtp)
                {
                    dtp.BackColor = ControlBg;
                    dtp.ForeColor = TextColor;
                    
                    // Принудительно красим внутренний календарь, чтобы не было белого на белом
                    dtp.CalendarMonthBackground = ControlBg;
                    dtp.CalendarForeColor = TextColor;
                    dtp.CalendarTitleBackColor = BgColor;
                    dtp.CalendarTitleForeColor = TextColor;
                    dtp.CalendarTrailingForeColor = Color.Gray;
                }
                // Кнопки в стиле Flat
                else if (c is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = BorderColor;
                    btn.FlatAppearance.BorderSize = 1;
                    
                    // Если кнопка "главная" (например, Добавить), красим её в акцентный цвет
                    if (btn.Text.ToUpper().Contains("ДОБАВИТЬ") || btn.Text.ToUpper().Contains("OK"))
                    {
                        btn.BackColor = AccentColor;
                        btn.ForeColor = Color.White;
                        btn.FlatAppearance.BorderColor = AccentColor;
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(225, 225, 230);
                        btn.ForeColor = TextColor;
                    }
                }
                // Радиокнопки и чекбоксы
                else if (c is RadioButton || c is CheckBox)
                {
                    c.ForeColor = TextColor;
                    c.BackColor = Color.Transparent; // Чтобы не перекрывали фон контейнера
                }
                // Метки (Labels)
                else if (c is Label lbl)
                {
                    lbl.ForeColor = TextColor;
                }
                
                if (c.HasChildren)
                {
                    if (!(c is Button))
                        ApplyLight(c);
                }
            }
        }
    }
}