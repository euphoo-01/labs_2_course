namespace L4
{

    public class GiftException : Exception
    {
        public GiftException(string message) : base(message) { }
    }

    public class InvalidFillingException : GiftException
    {
        public InvalidFillingException(string filling)
            : base($"Кондитерская не предоставляет начинку: {filling}") { }
    }

    public class ComponentNotFoundException : GiftException
    {
        public ComponentNotFoundException(string name)
            : base($"Компонент '{name}' не найден в контейнере подарка") { }
    }

    public class InvalidDimensionsException : ArgumentException
    {
        public InvalidDimensionsException(double l, double w, double h)
            : base($"Некорректные размеры: {l}×{w}×{h}") { }
    }

    public interface ILogger
    {
        void Info(string msg);
        void Warning(string msg);
        void Error(string msg);
    }

    public class ConsoleLogger : ILogger
    {
        public void Info(string msg) =>
            Console.WriteLine($"{DateTime.Now}, INFO: {msg}");

        public void Warning(string msg) =>
            Console.WriteLine($"{DateTime.Now}, WARNING: {msg}");

        public void Error(string msg) =>
            Console.WriteLine($"{DateTime.Now}, ERROR: {msg}");
    }

    public class FileLogger : ILogger
    {
        private readonly string _path;

        public FileLogger(string path)
        {
            _path = path;
        }

        private void Write(string level, string msg)
        {
            File.AppendAllText(_path,
                $"{DateTime.Now}, {level}: {msg}\n");
        }

        public void Info(string msg) => Write("INFO", msg);
        public void Warning(string msg) => Write("WARNING", msg);
        public void Error(string msg) => Write("ERROR", msg);
    }
}
