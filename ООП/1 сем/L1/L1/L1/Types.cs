using System.Diagnostics.Metrics;
using System.Runtime.Remoting;

bool p1 = false;
byte p2 = 255;
sbyte p3 = 127;
char p4 = 'a';
decimal p5 = 1.000003m;
double p6 = 0.6;
float p7 = 0.003f;
int p8 = 1;
uint p9 = 4294967295;
nint p10 = 294967295;
long p11 = 14294967295;
ulong p12 = 14294967295;
short p13 = 32767;
ushort p14 = 65535;

Console.Write(System.Convert.ToString(p1) + '\t' + p2 + '\t' + p3 + '\t' + p4 + '\t' + p5 + '\t' + p6 + '\t' + p7 + '\t' 
    + p8 + '\t' + p9 + '\t' + p10 + '\t' + p11 + '\t' + p12 + '\t' + p13 + '\t' + p14 + '\n');

Console.WriteLine("Введите true или false: ");
p1 = System.Convert.ToBoolean(Console.ReadLine());

Console.WriteLine("Введите число от 0 до 255: ");
p2 = System.Convert.ToByte(Console.ReadLine());

Console.WriteLine("Введите число от -128 до 127: ");
p3 = System.Convert.ToSByte(Console.ReadLine());

Console.WriteLine("Введите букву: ");
p4 = System.Convert.ToChar(Console.ReadLine());

Console.WriteLine("Введите дробное число от ±1.0 x 10^(-28) до ±7.9228 x 10^(28): ");
p5 = System.Convert.ToDecimal(Console.ReadLine());

Console.WriteLine("Введите дробное число от ±5.0 x 10^(–324) до ±1.7 × 10^(308): ");
p6 = System.Convert.ToDouble(Console.ReadLine());

Console.WriteLine("Введите дробное число от ±1.5 x 10^(–45) до ±3.4 x 10^(38): ");
p7 = System.Convert.ToSingle(Console.ReadLine());

Console.WriteLine("Введите целое число от -2 147 483 648 до 2 147 483 647: ");
p8 = System.Convert.ToInt32(Console.ReadLine());

Console.WriteLine("Введите целое число от 0 до 4 294 967 295: ");
p9 = System.Convert.ToUInt32(Console.ReadLine());

Console.WriteLine("Введите целое число от " + System.Int64.MinValue + " до " + System.Int64.MaxValue + ": ");
p10 = (nint)System.Convert.ToInt64(Console.ReadLine());

Console.WriteLine("Введите целое число от " + System.Int64.MinValue + " до " + System.Int64.MaxValue + ": ");
p11 = System.Convert.ToInt64(Console.ReadLine());

Console.WriteLine("Введите целое число от " + System.UInt64.MinValue + " до " + System.UInt64.MaxValue + ": ");
p12 = System.Convert.ToUInt64(Console.ReadLine());

Console.WriteLine("Введите целое число от -32 768 до 32 767: ");
p13 = System.Convert.ToInt16(Console.ReadLine());

Console.WriteLine("Введите целое число от 0 до 65535: ");
p14 = System.Convert.ToUInt16(Console.ReadLine());

short b = 1;
int a = b;

float c = 1.5f;
double d = c;

nint e = 135;
c = e;

uint f = 2394;
ulong g = f;

byte h = 255;
short i = h;


int j = 1;
Object obj = j;
Console.WriteLine("Результат распаковки: " + (int)obj);

var k = "Hello World!";
var l = 1252352351325235;
var m = -0.5f;
var n = 0.5;

Console.WriteLine("Тип переменной k: " + k.GetType());
Console.WriteLine("Тип переменной l: " +  l.GetType());
Console.WriteLine("Тип переменной m: " +  m.GetType());
Console.WriteLine("Тип переменной n: " + n.GetType());

string? s = null;
Console.WriteLine("Введите какую-нибудь строку: ");
s = Console.ReadLine();
if (String.IsNullOrEmpty(s))
    s = null;
Console.WriteLine("Вы ввели: " + s);

var o = 10;
//o = "string";