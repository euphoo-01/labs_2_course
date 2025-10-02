namespace Garage
{
    class Car
    {
        private string Id, Model, Year, Color, Reg_Number;

        private static string[] Models =
        [
            "Tesla Cybertruck", "Mercedes-Benz S-class", "Hyundai Solaris I",
            "ВАЗ 2107", "Audi 80", "Nissan Skyline R34",
            "Неизвестная модель"
        ];

        private static (string model, string[] years)[] ModelsYears =
        [
            (Models[0], getAvailableYears(2023, 2025)),
            (Models[1], getAvailableYears(2020, 2025)),
            (Models[2], getAvailableYears(2010, 2017)),
            (Models[3], getAvailableYears(1987, 2012)),
            (Models[4], getAvailableYears(1972, 1996)),
            (Models[5], getAvailableYears(1998, 2002)),
            (Models[6], ["Нет даты"])
        ];

        private static string[] getAvailableYears(int from, int to)
        {
            string[] result = new string[to - from + 1];
            int idx = 0;
            for (int i = from; i <= to; i++, idx++)
            {
                result[idx] = i.ToString();
            }

            return result;
        }

        public Car()
        {
        }

        public Car(string model, string color, string year)
        {
            Id = Guid.NewGuid().ToString();
            if (Models.Contains(model))
                Model = model;
            else
                Console.Error.WriteLine("Такой модели нет!");

            if (ModelsYears.First(el => el.model == model).years.Contains(year) == true)
                Year = year;
            else
            {
                Console.Error.WriteLine("Эта модель не выпускалась в этот год!");
                Year = "Неизвестная дата производства";
            }
            Color = color;
            Reg_Number = "Не зарегистрирован";
        }

        public Car(string model, string year, string color, string reg_number)
        {
            Id = Guid.NewGuid().ToString();
            Model = model;
            Year = year;
            Color = color;
            Reg_Number = reg_number;
        }
    }
}