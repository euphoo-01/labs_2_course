using System;

namespace L4
{
    public class Printer
    {
        public void IAmPrinting(IPurchasable someobj)
        {
            if (someobj == null)
            {
                throw new Exception("Объект для печати не задан!");
                return;
            }

            if (someobj is Flowers flowers)
            {
                Console.WriteLine("Информация о цветах:");
            }
            else if (someobj is Cake cake)
            {
                Console.WriteLine("Информация о торте:");
            }
            else if (someobj is Watch watch)
            {
                Console.WriteLine("Информация о часах:");
            }
            else if (someobj is Candies candies)
            {
                Console.WriteLine("Информация о конфетах:");
            }
            else
            {
                Console.WriteLine("Информацию о товаре:");
            }

            Console.WriteLine(someobj.ToString());
            Console.WriteLine();
        }
    }
}
