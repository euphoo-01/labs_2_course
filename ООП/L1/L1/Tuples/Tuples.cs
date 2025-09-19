(int, string, char, string, ulong) tuple1 = (1, "Hi", 'A', "Bye", 73);

Console.WriteLine(tuple1);
Console.WriteLine();
Console.WriteLine(tuple1.Item1 + " " + tuple1.Item4 + " " + tuple1.Item2);

Console.WriteLine();
var tuple2 = ("lsijef", "fijslefj");
var (str1, str2) = tuple2;
Console.WriteLine(str1);
Console.WriteLine(str2);

Console.WriteLine();
(string name, int age, string city) person = ("Vasiliy", 21, "New-York");
(string name, _, _) = person;
(_, int age, _) = person;
Console.WriteLine(name + "'s age: " + age);


var tuple3 = (A: 5, B: 6, C: 1, D: 3);
var tuple4 = (A: 5, B: 6, C: 1, D: 3);
var tuple5 = (A: 5, B: 3, C: 1, D: 3);

Console.WriteLine();
Console.WriteLine(tuple5 + " == " + tuple4 + " ?: " + (tuple5 == tuple4));
Console.WriteLine(tuple3 + " == " + tuple4 + " ?: " + (tuple3 == tuple4));
Console.WriteLine(tuple5 + " != " + tuple4 + " ?: " + (tuple5 != tuple4));