using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShopApp.Models;
using ShopApp.Services;

namespace ShopApp.Forms
{
    public class ManufacturerForm : Form
    {
        public Manufacturer Result { get; private set; }
        private TextBox t1, t2;

        public ManufacturerForm(Manufacturer cur)
        {
            this.Text = "Производитель";
            this.Size = new Size(300, 220);
            this.StartPosition = FormStartPosition.CenterParent;

            var lbl1 = new Label { Text = "Организация:", Location = new Point(20, 20), AutoSize = true };
            t1 = new TextBox { Location = new Point(20, 40), Width = 240, Text = cur.Organization };
            var lbl2 = new Label { Text = "Страна:", Location = new Point(20, 70), AutoSize = true };
            t2 = new TextBox { Location = new Point(20, 90), Width = 240, Text = cur.Country };

            Button btn = new Button { Text = "Готово", Location = new Point(100, 130), Width = 80 };
            btn.Click += (s, e) =>
            {
                var m = new Manufacturer { Organization = t1.Text, Country = t2.Text };
                var context = new ValidationContext(m);
                var results = new List<ValidationResult>();
                if (Validator.TryValidateObject(m, context, results, true))
                {
                    Result = m;
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show(string.Join("\n", results.Select(r => r.ErrorMessage)), "Ошибка валидации");
                }
            };

            this.Controls.AddRange(new Control[] { lbl1, t1, lbl2, t2, btn });
            ThemeManager.ApplyLight(this);
        }
    }

    public class SellerForm : Form
    {
        public Seller Result { get; private set; }
        private TextBox t1; private NumericUpDown n1;

        public SellerForm(Seller cur)
        {
            this.Text = "Продавец";
            this.Size = new Size(300, 220);
            this.StartPosition = FormStartPosition.CenterParent;

            var lbl1 = new Label { Text = "ФИО:", Location = new Point(20, 20), AutoSize = true };
            t1 = new TextBox { Location = new Point(20, 40), Width = 240, Text = cur.FullName };
            var lbl2 = new Label { Text = "Стаж:", Location = new Point(20, 70), AutoSize = true };
            n1 = new NumericUpDown { Location = new Point(20, 90), Width = 240, Value = cur.Experience };

            Button btn = new Button { Text = "Готово", Location = new Point(100, 130), Width = 80 };
            btn.Click += (s, e) =>
            {
                var sel = new Seller { FullName = t1.Text, Experience = (int)n1.Value };
                var context = new ValidationContext(sel);
                var results = new List<ValidationResult>();
                if (Validator.TryValidateObject(sel, context, results, true))
                {
                    Result = sel;
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show(string.Join("\n", results.Select(r => r.ErrorMessage)), "Ошибка валидации");
                }
            };

            this.Controls.AddRange(new Control[] { lbl1, t1, lbl2, n1, btn });
            ThemeManager.ApplyLight(this);
        }
    }
}
