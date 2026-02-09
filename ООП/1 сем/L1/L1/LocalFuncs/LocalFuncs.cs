using System.ComponentModel.DataAnnotations;

class Program
{
    static void Main()
    {
        (int max, int min, int sum, char s) localFunc(int[] arr, string str)
        {
            char s = str[0];
            int max = arr.Max();
            int min = arr.Min();
            int sum = arr.Sum();
            return (max, min, sum, s);
        }

        Console.WriteLine(localFunc([1, 2, 3, 4, 5, 6], "Hi"));

        int checkedMax()
        {
            checked {
                int a = int.MaxValue;
                return a;
            }
        }
        int uncheckedMax()
        {
            unchecked
            {
                int a = int.MaxValue + 1;
                return a;
            }
        }

        Console.WriteLine(checkedMax());
        Console.WriteLine(uncheckedMax());
    }
}