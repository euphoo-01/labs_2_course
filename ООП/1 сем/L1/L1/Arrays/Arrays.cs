using System.Numerics;

const int matrix_n = 5;
int[,] matrix = new int[matrix_n, matrix_n];

for (int i = 0; i < matrix_n; i++)
{
    for (int j = 0; j < matrix_n; j++)
    {
        matrix[i, j] = i+j;
        Console.Write(matrix[i, j].ToString() + '\t');
    }
    Console.Write('\n');
}

const int strs_n = 5;
string[] strs = new string[strs_n];

void print_string_array(string[] arr)
{
    foreach(string str in arr)
    {
        Console.WriteLine(Array.IndexOf(arr, str) + ") " + str);
    }
}
strs[0] = "В четверг четвёртого числа в четыре с четвертью часа лигурийский регулировщик регулировал в Лигурии";
strs[1] = "но тридцать три корабля лави́ровали, лави́ровали, да так и не вы́лавировали";
strs[2] = "А потом протокол про протокол протоколом запротоколи́ровал, как интервьюе́ром интервьюи́руемый лигурийский регулировщик речи́сто,";
strs[3] = "да не чисто, рапортовал, да не дорапортова́л, дорапорто́вывал, да так зарапортова́лся про размокропого́дившуюся погоду,";
strs[4] = "что дабы инцидент не стал претендентом на судебный прецедент, лигурийский регулировщик акклиматизировался в неконституционном Константинополе.";

print_string_array(strs);

Console.WriteLine("Какую строку заменить на вашу? ");
int input_n = System.Convert.ToInt16(Console.ReadLine());
Console.WriteLine("Введите содержимое новой строки: ");
strs[input_n] = Console.ReadLine();


print_string_array(strs);

float[][] jagged = new float[3][];
jagged[0] = new float[2];
jagged[1] = new float[3];
jagged[2] = new float[4];

Console.WriteLine("Введите значения зубчатого массива (float): ");
foreach (float[] col in jagged)
{
    Console.WriteLine(Array.IndexOf(jagged, col) + "): ");
    foreach(float f in col)
    {
        Console.WriteLine(Array.IndexOf(col, f) + "): ");
        col[Array.IndexOf(col, f)] = System.Convert.ToSingle(Console.ReadLine());
    }
}

Console.WriteLine("Содержимое зубчатого массива: ");
foreach (float[] col in jagged)
{
    foreach(float f in col)
    {
        Console.Write(f + "\t");
    }
    Console.WriteLine();
}

Console.WriteLine();

var arr1 = new int[10];
for (int i = 0; i < arr1.Length; i++)
{
    arr1[i] = i * i;
    Console.Write(arr1[i].ToString() + '\t');
}
Console.WriteLine();
Console.WriteLine();

char a = 'A';
var arr2 = new string[5];
for (int i = 0; i < arr2.Length; i++)
{
    arr2[i] = System.Convert.ToChar((a + i)).ToString();
    Console.WriteLine(arr2[i]);
}
Console.WriteLine();