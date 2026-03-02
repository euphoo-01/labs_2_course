using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using ShopApp.Models;
using ShopApp.Services;

namespace ShopApp.Forms
{
    public class MainForm : Form
    {
        private List<Product> _products = new List<Product>();
        private Stack<List<Product>> _backHistory = new Stack<List<Product>>();
        private Stack<List<Product>> _forwardHistory = new Stack<List<Product>>();

        private Manufacturer _curM = new Manufacturer();
        private Seller _curS = new Seller();

        private TextBox txtName;
        private MaskedTextBox mtxtInv;
        private ComboBox cmbCat;
        private TrackBar trbW;
        private NumericUpDown numP, numQ;
        private RadioButton rbS, rbM;
        private DateTimePicker dtp;
        private ListBox lb;
        private StatusStrip ss;
        private ToolStripStatusLabel statusCount, statusLastAction, statusDate;
        private ToolStrip ts;

        public MainForm()
        {
            this.Text = "Учет товаров";
            this.Size = new Size(950, 750);

            this.MainMenuStrip = CreateMenu();
            this.Controls.Add(this.MainMenuStrip);

            ts = CreateToolbar();
            this.Controls.Add(ts);

            ss = CreateStatusBar();
            this.Controls.Add(ss);

            Panel mainContainer = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 50, 0, 30) };
            Panel left = new Panel { Dock = DockStyle.Left, Width = 400, Padding = new Padding(10) };
            Panel right = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

            int y = 20;
            AddRow(left, "Название:", txtName = new TextBox { Width = 200 }, ref y);
            AddRow(left, "Инв. номер:", mtxtInv = new MaskedTextBox("AAA-000000") { Width = 200 }, ref y);

            cmbCat = new ComboBox { Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCat.Items.AddRange(new[] { "Электроника", "Одежда", "Продукты" }); cmbCat.SelectedIndex = 0;
            AddRow(left, "Категория:", cmbCat, ref y);

            trbW = new TrackBar { Minimum = 1, Maximum = 1000, Width = 150, TickStyle = TickStyle.None };
            Label lblW = new Label { Text = "1 кг", AutoSize = true };
            trbW.ValueChanged += (s, e) => lblW.Text = trbW.Value + " кг";
            FlowLayoutPanel flpW = new FlowLayoutPanel { Width = 210, Height = 30 };
            flpW.Controls.Add(trbW); flpW.Controls.Add(lblW);
            AddRow(left, "Вес:", flpW, ref y);

            FlowLayoutPanel flpSize = new FlowLayoutPanel { Width = 210, Height = 30 };
            rbS = new RadioButton { Text = "Small", Checked = true, AutoSize = true };
            rbM = new RadioButton { Text = "Medium", AutoSize = true };
            flpSize.Controls.AddRange(new Control[] { rbS, rbM });
            AddRow(left, "Размер:", flpSize, ref y);

            AddRow(left, "Цена:", numP = new NumericUpDown { Width = 200, Maximum = 1000000, DecimalPlaces = 2 }, ref y);
            AddRow(left, "Количество:", numQ = new NumericUpDown { Width = 200, Maximum = 10000 }, ref y);
            AddRow(left, "Дата:", dtp = new DateTimePicker { Width = 200 }, ref y);

            Button btnM = new Button { Text = "Производитель", Location = new Point(20, y), Width = 160 };
            btnM.Click += (s, e) => { var f = new ManufacturerForm(_curM); if (f.ShowDialog() == DialogResult.OK) _curM = f.Result; };
            Button btnS = new Button { Text = "Продавец", Location = new Point(190, y), Width = 160 };
            btnS.Click += (s, e) => { var f = new SellerForm(_curS); if (f.ShowDialog() == DialogResult.OK) _curS = f.Result; };
            left.Controls.AddRange(new Control[] { btnM, btnS });

            Button btnAdd = new Button { Text = "ДОБАВИТЬ В СПИСОК", Location = new Point(20, y + 50), Width = 330, Height = 45 };
            btnAdd.Click += OnAdd;
            left.Controls.Add(btnAdd);

            lb = new ListBox { Dock = DockStyle.Fill };
            right.Controls.Add(lb);

            mainContainer.Controls.Add(right);
            mainContainer.Controls.Add(left);
            this.Controls.Add(mainContainer);

            Timer timer = new Timer { Interval = 1000 };
            timer.Tick += (s, e) => statusDate.Text = DateTime.Now.ToString("F");
            timer.Start();

            ThemeManager.ApplyLight(this);
        }

        private MenuStrip CreateMenu()
        {
            MenuStrip ms = new MenuStrip();
            ToolStripMenuItem mSearch = new ToolStripMenuItem("Поиск");
            ToolStripMenuItem sConstructor = new ToolStripMenuItem("Конструктор поиска", null, (s, e) => OnSearch());
            ToolStripMenuItem sByName = new ToolStripMenuItem("По названию", null, (s, e) => OnSearch());
            ToolStripMenuItem sByCategory = new ToolStripMenuItem("По категории", null, (s, e) => OnSearch());
            mSearch.DropDownItems.AddRange(new[] { sConstructor, sByName, sByCategory });

            ToolStripMenuItem mSort = new ToolStripMenuItem("Сортировка по");
            ToolStripMenuItem sPrice = new ToolStripMenuItem("Цене", null, (s, e) => SortBy(p => p.Price));
            ToolStripMenuItem sDate = new ToolStripMenuItem("Дате поступления", null, (s, e) => SortBy(p => p.ArrivalDate));
            mSort.DropDownItems.AddRange(new[] { sPrice, sDate });

            ToolStripMenuItem mSave = new ToolStripMenuItem("Сохранить", null, (s, e) => Serialize("data.xml", _products));
            ToolStripMenuItem mView = new ToolStripMenuItem("Вид");
            ToolStripMenuItem mShowToolbar = new ToolStripMenuItem("Панель инструментов", null, (s, e) => ts.Visible = !ts.Visible);
            mView.DropDownItems.Add(mShowToolbar);
            ToolStripMenuItem mAbout = new ToolStripMenuItem("О программе", null, (s, e) => MessageBox.Show("Версия 0.1\nРазработчик: Лавшук С.А.\n Пепе... Шнеле... Фа....", "О программе"));

            ms.Items.AddRange(new ToolStripItem[] { mSearch, mSort, mSave, mView, mAbout });
            return ms;
        }

        private ToolStrip CreateToolbar()
        {
            ts = new ToolStrip { Dock = DockStyle.Top };
            var bSearch = new ToolStripButton("Поиск", null, (s, e) => OnSearch());
            var bSort = new ToolStripButton("Сорт. Цена", null, (s, e) => SortBy(p => p.Price));
            var bClear = new ToolStripButton("Очистить", null, (s, e) => { SaveState(); _products.Clear(); UpdateList(); UpdateStatus("Очистка"); });
            var bDel = new ToolStripButton("Удалить", null, (s, e) => { if (lb.SelectedIndex != -1) { SaveState(); _products.RemoveAt(lb.SelectedIndex); UpdateList(); UpdateStatus("Удаление"); } });

            var bBack = new ToolStripButton("Назад", null, (s, e) => GoBack());
            var bForward = new ToolStripButton("Вперед", null, (s, e) => GoForward());

            var bHide = new ToolStripButton("Скрыть", null, (s, e) => ts.Visible = false);

            ts.Items.AddRange(new ToolStripItem[] { bSearch, bSort, bClear, bDel, new ToolStripSeparator(), bBack, bForward, new ToolStripSeparator(), bHide });
            return ts;
        }

        private StatusStrip CreateStatusBar()
        {
            StatusStrip strip = new StatusStrip();
            statusCount = new ToolStripStatusLabel("Объектов: 0");
            statusLastAction = new ToolStripStatusLabel("Действие: -");
            statusDate = new ToolStripStatusLabel(DateTime.Now.ToString("F"));
            strip.Items.AddRange(new ToolStripItem[] { statusCount, new ToolStripSeparator(), statusLastAction, new ToolStripSeparator(), statusDate });
            return strip;
        }

        private void AddRow(Panel p, string txt, Control c, ref int y)
        {
            p.Controls.Add(new Label { Text = txt, Location = new Point(20, y + 3), AutoSize = true });
            c.Location = new Point(140, y);
            p.Controls.Add(c);
            y += 40;
        }

        private void OnAdd(object s, EventArgs e)
        {
            var p = new Product
            {
                Name = txtName.Text,
                InventoryNumber = mtxtInv.Text,
                Category = cmbCat.Text,
                Weight = trbW.Value,
                Size = rbS.Checked ? "Small" : "Medium",
                Price = numP.Value,
                Quantity = (int)numQ.Value,
                ArrivalDate = dtp.Value,
                ManufacturerInfo = _curM,
                SellerInfo = _curS
            };

            var context = new ValidationContext(p);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(p, context, results, true))
            {
                SaveState();
                _products.Add(p);
                UpdateList();
                UpdateStatus("Добавление товара");
            }
            else
            {
                MessageBox.Show(string.Join("\n", results.Select(r => r.ErrorMessage)), "Ошибка валидации");
            }
        }

        private void OnSearch()
        {
            var sf = new SearchForm(_products);
            if (sf.ShowDialog() == DialogResult.OK)
            {
                var rf = new ResultsForm(sf.Results);
                rf.Show();
                UpdateStatus("Поиск");
            }
        }

        private void SortBy<TKey>(Func<Product, TKey> keySelector)
        {
            SaveState();
            _products = _products.OrderBy(keySelector).ToList();
            UpdateList();
            UpdateStatus("Сортировка");
        }

        private void SaveState()
        {
            _backHistory.Push(new List<Product>(_products));
            _forwardHistory.Clear();
        }

        private void GoBack()
        {
            if (_backHistory.Count > 0)
            {
                _forwardHistory.Push(new List<Product>(_products));
                _products = _backHistory.Pop();
                UpdateList();
                UpdateStatus("Назад");
            }
        }

        private void GoForward()
        {
            if (_forwardHistory.Count > 0)
            {
                _backHistory.Push(new List<Product>(_products));
                _products = _forwardHistory.Pop();
                UpdateList();
                UpdateStatus("Вперед");
            }
        }

        private void UpdateList()
        {
            lb.Items.Clear();
            foreach (var p in _products)
                lb.Items.Add($"{p.Name} | {p.Price}р | {p.Category}");
            statusCount.Text = $"Объектов: {_products.Count}";
        }

        private void UpdateStatus(string action)
        {
            statusLastAction.Text = $"Действие: {action}";
        }

        private void Serialize(string path, List<Product> data)
        {
            try
            {
                using (var sw = new StreamWriter(path))
                    new XmlSerializer(typeof(List<Product>)).Serialize(sw, data);
                MessageBox.Show("Данные сохранены в " + path);
                UpdateStatus("Сохранение");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
