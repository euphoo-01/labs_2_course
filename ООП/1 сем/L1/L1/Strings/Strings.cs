using System.Text;

Console.WriteLine(String.Compare("Привет", "пока"));
Console.WriteLine(String.Compare("Привет", "Привет"));

String hello = "Привет";
String name = "Станислав";
String surname = "Лавшук";
String sentence = "Съешь эту вкусную булку, да выпей чаю.";

Console.WriteLine(String.Concat(hello, ", ", name, ' ', surname));

String name2 = String.Copy(name);
name2 = name.Substring(0, 3) + "с";
String welcoming = hello + ", ";
Console.WriteLine(welcoming.Insert(8, name2));

String[] words = sentence.Split(' ');

long number = 375123456789;
Console.WriteLine($"{number:+### ## ###-##-##}");

String? str1 = null;
String str2 = "";

if (String.IsNullOrEmpty(str2) && String.IsNullOrEmpty(str1))
{
    Console.WriteLine("Строки str1 и str2 пустые");
} else
{
    Console.WriteLine("Строки str1 и str2 непустые");
}

StringBuilder sb = new StringBuilder("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean nec.");
sb = sb.Remove(0, 6);
sb = sb.Remove(58, 4);
sb = sb.Insert(0, "Агара гуджу ");
Console.WriteLine(sb);