namespace L3
{
    public class LabList : List<int>
    {
        private class Production
        {
            private Guid id;
            private string org;

            public Guid Id
            {
                get { return id; }
            }

            public string Org
            {
                get { return org; }
            }

            public Production()
            {
                this.id = Guid.NewGuid();
                this.org = "unknown";
            }

            public Production(string org)
            {
                this.id = Guid.NewGuid();
                this.org = org;
            }
        }

        private class Developer
        {
            private Guid id;
            private string fullName;
            private string division;

            public Guid Id
            {
                get { return id; }
            }

            public string FullName
            {
                get { return fullName; }
            }

            public string Division
            {
                get { return division; }
            }

            public Developer()
            {
                this.id = Guid.NewGuid();
                this.fullName = "unknown";
                this.division = "unknown";
            }

            public Developer(string fullName, string division)
            {
                this.id = Guid.NewGuid();
                this.fullName = fullName;
                this.division = division;
            }
        }

        public LabList(string devName, string devDivision, string org, params int[] list) : base(list)
        {
            this.developer = new Developer(devName, devDivision);
            this.production = new Production(org);
        }

        public LabList() : base()
        {
            this.developer = new Developer();
            this.production = new Production();
        }

        private Production production = new Production();
        private Developer developer = new Developer();

        public static LabList operator +(LabList a, LabList b)
        {
            LabList result = new LabList();
            result.AddRange(a.Concat(b));
            return result;
        }

        public static LabList operator --(LabList a)
        {
            a.RemoveAt(0);
            return a;
        }

        public static bool operator ==(LabList a, LabList b)
        {
            int idx = 0;
            bool result = true;
            a.ForEach(el =>
            {
                if (!el.Equals(b[idx++]))
                    result = false;
            });
            return result;
        }

        public static bool operator !=(LabList a, LabList b)
        {
            int idx = 0;
            bool result = true;
            a.ForEach(el =>
            {
                if (el.Equals(b[idx++]))
                    result = false;
            });
            return result;
        }

        public static bool operator ==(LabList a, bool b = true)
        {
            if (a.Count > 0)
                return true;
            else
                return false;
        }

        public static bool operator !=(LabList a, bool b = true)
        {
            if (a.Count > 0)
                return false;
            else
                return true;
        }

        public override string ToString()
        {
            if (this.Count == 0)
                return "empty";
            string result =
                $"{this.production.Id}, {this.production.Org}\n{this.developer.Id}" +
                $", {this.developer.FullName}, {this.developer.Division}\n";
            this.ForEach(el => result += $"{el}, ");
            return result;
        }
    }

    public static class LabListExtension
    {
        public static int lastNumber(this string s)
        {
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (char.IsDigit(s[i]))
                {
                    return int.Parse(s[i].ToString());
                }
            }

            throw new Exception("В строке нет цифр");
        }

        public static int lastNumber(this LabList l)
        {
            if (l.Count > 0)
                return l[l.Count - 1];
            else
                return -1;
        }

        public static int deleteByValue(this LabList l, int el)
        {
            if (l.Remove(el))
                return 0;
            else
                return -1;
        }

        public static int sum(LabList l)
        {
            int sum = 0;
            l.ForEach(el => sum += el);
            return sum;
        }

        public static int maxMinDiff(LabList l)
        {
            return l.Max() - l.Min();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            LabList a = new LabList();
            LabList b = new LabList();
            LabList c = new LabList("Иван Пупкин", "Дизайн", "Корпорация Зла", 1, 2, 3, 4, 5, 6, 7, 8);
            Console.WriteLine(c);
            a.Add(1);
            a.Add(500);
            a.Add(12515);
            a.Add(2362361);
            Console.WriteLine(a);
            Console.WriteLine(b);
            
            a--;
            Console.WriteLine(a.lastNumber());
            Console.WriteLine(b == true);
            Console.WriteLine(a == true);

            if (a.deleteByValue(a.lastNumber()) == 0)
            {
                Console.WriteLine(a.lastNumber());
            }
            else
            {
                Console.WriteLine("Не удалось удалить элеметн из списка!");
            }

            b.Add(123);
            b.Add(1234567);

            Console.WriteLine(a + b);
            Console.WriteLine("1209847u9ababa".lastNumber());
            
            Console.WriteLine(LabListExtension.sum(b));
            
            
        }
    }
}