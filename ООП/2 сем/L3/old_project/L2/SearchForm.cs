using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ShopApp.Models;
using ShopApp.Services;

namespace ShopApp.Forms
{
    public class SearchForm : Form
    {
        private List<Product> _source;
        public List<Product> Results { get; private set; }

        private TextBox txtNameReg, txtInvReg;
        private ComboBox cmbCat;
        private NumericUpDown numMinP, numMaxP;
        private CheckBox chkName, chkInv, chkCat, chkPrice;

        public SearchForm(List<Product> source)
        {
            _source = source;
            this.Text = "Конструктор поиска";
            this.Size = new Size(400, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;

            int y = 20;
            chkName = new CheckBox { Text = "По названию (Regex):", Location = new Point(20, y), AutoSize = true };
            txtNameReg = new TextBox { Location = new Point(200, y), Width = 150 };
            this.Controls.AddRange(new Control[] { chkName, txtNameReg });
            y += 40;

            chkInv = new CheckBox { Text = "По инв. номеру (Regex):", Location = new Point(20, y), AutoSize = true };
            txtInvReg = new TextBox { Location = new Point(200, y), Width = 150 };
            this.Controls.AddRange(new Control[] { chkInv, txtInvReg });
            y += 40;

            chkCat = new CheckBox { Text = "По категории:", Location = new Point(20, y), AutoSize = true };
            cmbCat = new ComboBox { Location = new Point(200, y), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCat.Items.AddRange(new[] { "Электроника", "Одежда", "Продукты" });
            cmbCat.SelectedIndex = 0;
            this.Controls.AddRange(new Control[] { chkCat, cmbCat });
            y += 40;

            chkPrice = new CheckBox { Text = "По цене (диапазон):", Location = new Point(20, y), AutoSize = true };
            numMinP = new NumericUpDown { Location = new Point(200, y), Width = 70, Maximum = 1000000 };
            numMaxP = new NumericUpDown { Location = new Point(280, y), Width = 70, Maximum = 1000000, Value = 1000 };
            this.Controls.AddRange(new Control[] { chkPrice, numMinP, numMaxP });
            y += 60;

            Button btnSearch = new Button { Text = "Найти", Location = new Point(100, y), Width = 200, Height = 40, BackColor = Color.LightBlue };
            btnSearch.Click += OnSearch;
            this.Controls.Add(btnSearch);

            ThemeManager.ApplyLight(this);
        }

        private void OnSearch(object sender, EventArgs e)
        {
            var query = _source.AsEnumerable();

            if (chkName.Checked)
            {
                Regex rgx = new Regex(txtNameReg.Text);
                query = query.Where(p => rgx.IsMatch(p.Name));
            }

            if (chkInv.Checked)
            {
                Regex rgx = new Regex(txtInvReg.Text);
                query = query.Where(p => rgx.IsMatch(p.InventoryNumber));
            }

            if (chkCat.Checked)
            {
                query = query.Where(p => p.Category == cmbCat.Text);
            }

            if (chkPrice.Checked)
            {
                query = query.Where(p => p.Price >= numMinP.Value && p.Price <= numMaxP.Value);
            }

            Results = query.ToList();
            DialogResult = DialogResult.OK;
        }
    }

    public class ResultsForm : Form
    {
        private List<Product> _results;
        private ListBox lb;

        public ResultsForm(List<Product> results)
        {
            _results = results;
            this.Text = "Результаты поиска";
            this.Size = new Size(500, 400);

            lb = new ListBox { Dock = DockStyle.Fill };
            foreach (var p in _results)
                lb.Items.Add($"{p.Name} | {p.Category} | {p.Price}р | {p.InventoryNumber}");

            ToolStrip ts = new ToolStrip();
            ToolStripButton btnSave = new ToolStripButton("Сохранить результаты");
            btnSave.Click += (s, e) =>
            {
                SaveFileDialog sfd = new SaveFileDialog { Filter = "XML files|*.xml" };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var sw = new System.IO.StreamWriter(sfd.FileName))
                        new System.Xml.Serialization.XmlSerializer(typeof(List<Product>)).Serialize(sw, _results);
                    MessageBox.Show("Сохранено!");
                }
            };
            ts.Items.Add(btnSave);

            this.Controls.Add(lb);
            this.Controls.Add(ts);
            ThemeManager.ApplyLight(this);
        }
    }
}
