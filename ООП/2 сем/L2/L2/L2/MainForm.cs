using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using ShopApp.Models;
using ShopApp.Services;

namespace ShopApp.Forms
{
    public class MainForm : Form
    {
        private List<Product> _products = new List<Product>();
        private Manufacturer _curM = new Manufacturer();
        private Seller _curS = new Seller();

        private TextBox txtName; private MaskedTextBox mtxtInv;
        private ComboBox cmbCat; private TrackBar trbW;
        private NumericUpDown numP, numQ; private RadioButton rbS, rbM;
        private DateTimePicker dtp; private ListBox lb;

        public MainForm()
        {
            this.Text = "Учет товаров (Light Flat)";
            this.Size = new Size(850, 600);
            
            Panel left = new Panel { Dock = DockStyle.Left, Width = 380, Padding = new Padding(10) };
            Panel right = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

            int y = 20;
            AddRow(left, "Название:", txtName = new TextBox { Width = 200 }, ref y);
            AddRow(left, "Инв. номер:", mtxtInv = new MaskedTextBox("AAA-000000") { Width = 200 }, ref y);
            
            cmbCat = new ComboBox { Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCat.Items.AddRange(new[] { "Электроника", "Одежда", "Продукты" }); cmbCat.SelectedIndex = 0;
            AddRow(left, "Категория:", cmbCat, ref y);

            trbW = new TrackBar { Minimum = 0, Maximum = 100, Width = 150, TickStyle = TickStyle.None };
            Label lblW = new Label { Text = "0 кг", AutoSize = true };
            trbW.ValueChanged += (s, e) => lblW.Text = trbW.Value + " кг";
            FlowLayoutPanel flpW = new FlowLayoutPanel { Width = 210, Height = 30 };
            flpW.Controls.Add(trbW); flpW.Controls.Add(lblW);
            AddRow(left, "Вес:", flpW, ref y);

            FlowLayoutPanel flpSize = new FlowLayoutPanel { Width = 210, Height = 30 };
            rbS = new RadioButton { Text = "Small", Checked = true, AutoSize = true };
            rbM = new RadioButton { Text = "Medium", AutoSize = true };
            flpSize.Controls.AddRange(new Control[] { rbS, rbM });
            AddRow(left, "Размер:", flpSize, ref y);

            AddRow(left, "Цена:", numP = new NumericUpDown { Width = 200, Maximum = 100000 }, ref y);
            AddRow(left, "Количество:", numQ = new NumericUpDown { Width = 200 }, ref y);
            AddRow(left, "Дата:", dtp = new DateTimePicker { Width = 200 }, ref y);

            Button btnM = new Button { Text = "Производитель", Location = new Point(20, y), Width = 150 };
            btnM.Click += (s, e) => { var f = new ManufacturerForm(_curM); if(f.ShowDialog()==DialogResult.OK) _curM = f.Result; };
            Button btnS = new Button { Text = "Продавец", Location = new Point(180, y), Width = 150 };
            btnS.Click += (s, e) => { var f = new SellerForm(_curS); if(f.ShowDialog()==DialogResult.OK) _curS = f.Result; };
            left.Controls.AddRange(new Control[] { btnM, btnS });

            Button btnAdd = new Button { Text = "ДОБАВИТЬ В СПИСОК", Location = new Point(20, y + 40), Width = 310, Height = 40, BackColor = ThemeManager.AccentColor, ForeColor = Color.White };
            btnAdd.Click += OnAdd;
            left.Controls.Add(btnAdd);

            lb = new ListBox { Dock = DockStyle.Fill };
            right.Controls.Add(lb);

            Panel bot = new Panel { Dock = DockStyle.Bottom, Height = 50 };
            Button bSave = new Button { Text = "Сохранить", Location = new Point(10, 10), Width = 100 };
            bSave.Click += (s, e) => Serialize("data.xml");
            Button bLoad = new Button { Text = "Загрузить", Location = new Point(120, 10), Width = 100 };
            bLoad.Click += (s, e) => Deserialize("data.xml");
            Button bCalc = new Button { Text = "Бюджет", Location = new Point(230, 10), Width = 100 };
            bCalc.Click += (s, e) => MessageBox.Show($"Общая сумма: {_products.Sum(p => p.Price * p.Quantity):N2}");
            
            bot.Controls.AddRange(new Control[] { bSave, bLoad, bCalc });
            right.Controls.Add(bot);

            this.Controls.AddRange(new Control[] { right, left });
            ThemeManager.ApplyLight(this);
        }

        private void AddRow(Panel p, string txt, Control c, ref int y) {
            p.Controls.Add(new Label { Text = txt, Location = new Point(20, y + 3), AutoSize = true });
            c.Location = new Point(130, y);
            p.Controls.Add(c);
            y += 35;
        }

        private void OnAdd(object s, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtName.Text)) return;
            var p = new Product {
                Name = txtName.Text, InventoryNumber = mtxtInv.Text, Category = cmbCat.Text,
                Price = numP.Value, Quantity = (int)numQ.Value, ManufacturerInfo = _curM, SellerInfo = _curS
            };
            _products.Add(p);
            lb.Items.Add($"{p.Name} | {p.Price}р | {p.ManufacturerInfo.Organization}");
        }

        private void Serialize(string path) {
            using (var sw = new StreamWriter(path))
                new XmlSerializer(typeof(List<Product>)).Serialize(sw, _products);
        }

        private void Deserialize(string path) {
            if (!File.Exists(path)) return;
            using (var sr = new StreamReader(path))
                _products = (List<Product>)new XmlSerializer(typeof(List<Product>)).Deserialize(sr);
            lb.Items.Clear();
            foreach (var p in _products) lb.Items.Add($"{p.Name} | {p.Price}р");
        }
    }
}