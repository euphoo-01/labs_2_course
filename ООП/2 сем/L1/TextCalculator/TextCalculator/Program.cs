using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextCalculatorApp
{
    public class TextCalculator
    {
        public delegate void CalculationResultHandler(string resultType, string resultValue);

        public event CalculationResultHandler OnResultCalculated;

        protected virtual void NotifyResult(string type, string value)
        {
            OnResultCalculated?.Invoke(type, value);
        }

        public void GetLength(string text)
        {
            NotifyResult("Длина строки", text.Length.ToString());
        }

        public void GetWordCount(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                NotifyResult("Количество слов", "0");
                return;
            }
            var words = text.Split(new char[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            NotifyResult("Количество слов", words.Length.ToString());
        }

        public void GetSentenceCount(string text)
        {
            int count;
            if (String.IsNullOrWhiteSpace(text))
            {
                count = 0;
            }
            else
            {
                count = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;    
            }
            NotifyResult("Количество предложений", count.ToString());
        }

        public void GetVowelsCount(string text)
        {
            string vowels = "aeiouyаеёиоуыэюя"; 
            int count = text.Count(c => vowels.Contains(char.ToLower(c)));
            NotifyResult("Гласные", count.ToString());
        }

        public void GetConsonantsCount(string text)
        {
            string vowels = "aeiouyаеёиоуыэюя";
            int count = text.Count(c => char.IsLetter(c) && !vowels.Contains(char.ToLower(c)));
            NotifyResult("Согласные", count.ToString());
        }

        public void GetCharAtIndex(string text, string indexStr)
        {
            if (int.TryParse(indexStr, out int index))
            {
                if (index >= 0 && index < text.Length)
                    NotifyResult($"Символ по индексу {index}", text[index].ToString());
                else
                    NotifyResult("Ошибка", "Индекс выходит за границы строки");
            }
            else
            {
                NotifyResult("Ошибка", "Некорректный формат числа");
            }
        }

        public void ReplaceSubstring(TextBox textBox, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                NotifyResult("Ошибка", "Строка для поиска пуста");
                return;
            }
            string res = textBox.Text.Replace(oldValue, newValue);
            textBox.Text = res;
            NotifyResult("Результат замены", res);
        }

        public void DeleteSubstring(TextBox textBox, string toDelete)
        {
            ReplaceSubstring(textBox, toDelete, string.Empty);
        }
    }

    public class MainForm : Form
    {
        private readonly TextCalculator _calculator;

        private TextBox _txtInput;
        private Label _lblResultTitle;
        private Label _lblResultValue;
        
        private TextBox _txtParam1;
        private TextBox _txtParam2;
        private Label _lblParam1;
        private Label _lblParam2;

        public MainForm()
        {
            _calculator = new TextCalculator();
            _calculator.OnResultCalculated += ShowResult;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Текстовый калькулятор";
            this.Size = new Size(650, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font(FontFamily.GenericSansSerif, 10F, FontStyle.Regular);

            Label lblInput = new Label() 
            { 
                Text = "Введите текст:", 
                Location = new Point(20, 20), 
                AutoSize = true,
                ForeColor = Color.DimGray
            };

            _txtInput = new TextBox() 
            { 
                Location = new Point(20, 45), 
                Size = new Size(590, 80), 
                Multiline = true, 
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            _txtInput.Text = "Введи текст";

            _lblParam1 = new Label() { Text = "Параметр 1 (что менять/индекс):", Location = new Point(20, 140), AutoSize = true, ForeColor = Color.Black};
            _txtParam1 = new TextBox() { Location = new Point(20, 165), Size = new Size(280, 25), BorderStyle = BorderStyle.FixedSingle };

            _lblParam2 = new Label() { Text = "Параметр 2 (на что менять):", Location = new Point(320, 140), AutoSize = true, ForeColor = Color.Black };
            _txtParam2 = new TextBox() { Location = new Point(320, 165), Size = new Size(290, 25), BorderStyle = BorderStyle.FixedSingle };

            int btnY = 210;
            int btnX = 20;
            int btnWidth = 180;
            int btnHeight = 40;
            int margin = 10;

            CreateButton("Длина строки", (s, e) => _calculator.GetLength(_txtInput.Text), btnX, btnY, btnWidth, btnHeight);
            CreateButton("Кол-во слов", (s, e) => _calculator.GetWordCount(_txtInput.Text), btnX + btnWidth + margin, btnY, btnWidth, btnHeight);
            CreateButton("Предложения", (s, e) => _calculator.GetSentenceCount(_txtInput.Text), btnX + (btnWidth + margin) * 2, btnY, btnWidth, btnHeight);
            
            btnY += btnHeight + margin;
            CreateButton("Гласные", (s, e) => _calculator.GetVowelsCount(_txtInput.Text), btnX, btnY, btnWidth, btnHeight);
            CreateButton("Согласные", (s, e) => _calculator.GetConsonantsCount(_txtInput.Text), btnX + btnWidth + margin, btnY, btnWidth, btnHeight);
            CreateButton("Символ [index]", (s, e) => _calculator.GetCharAtIndex(_txtInput.Text, _txtParam1.Text), btnX + (btnWidth + margin) * 2, btnY, btnWidth, btnHeight);

            btnY += btnHeight + margin;
            CreateButton("Заменить", (s, e) => _calculator.ReplaceSubstring(_txtInput, _txtParam1.Text, _txtParam2.Text), btnX, btnY, btnWidth, btnHeight, true);
            CreateButton("Удалить подстроку", (s, e) => _calculator.DeleteSubstring(_txtInput, _txtParam1.Text), btnX + btnWidth + margin, btnY, btnWidth, btnHeight, true);

            _lblResultTitle = new Label() 
            { 
                Text = "Результат:", 
                Location = new Point(20, 380), 
                AutoSize = true, 
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64)
            };
            
            _lblResultValue = new Label() 
            { 
                Text = "...", 
                Location = new Point(20, 410), 
                AutoSize = true, 
                ForeColor = Color.Teal,
                Font = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Regular),
                MaximumSize = new Size(580, 0) 
            };

            this.Controls.Add(lblInput);
            this.Controls.Add(_txtInput);
            this.Controls.Add(_lblParam1);
            this.Controls.Add(_txtParam1);
            this.Controls.Add(_lblParam2);
            this.Controls.Add(_txtParam2);
            this.Controls.Add(_lblResultTitle);
            this.Controls.Add(_lblResultValue);
        }

        private void CreateButton(string text, EventHandler onClick, int x, int y, int w, int h, bool isAction = false)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(w, h);
            btn.Click += onClick;
            
            btn.FlatStyle = FlatStyle.Flat;
            
            btn.Cursor = Cursors.Hand;
            
            if (isAction)
            {
                btn.BackColor = Color.FromArgb(231, 76, 60); 
                btn.ForeColor = Color.White;
            }
            else
            {
                btn.BackColor = Color.FromArgb(52, 152, 219); 
                btn.ForeColor = Color.White;
            }

            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(btn.BackColor);
            
            this.Controls.Add(btn);
        }

        private void ShowResult(string type, string value)
        {
            _lblResultTitle.Text = $"Результат операции '{type}':";
            _lblResultValue.Text = value;
            
            if(value.Length > 100) 
            {
                 MessageBox.Show(value, "Результат");
            }
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}