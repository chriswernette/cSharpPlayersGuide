// See https://aka.ms/new-console-template for more information
using System.Dynamic;

int a = 5;
int b = -7;
Console.WriteLine(Adds.Add(a, b)); // Output: -2

double c = 5.76;
double d = -7.32;
Console.WriteLine(Adds.Add(c, d)); // Output: -1.56

string e = "Hello, ";
string f = "World!";
Console.WriteLine(Adds.Add(e, f)); // Output: Hello, World!

DateTime g = DateTime.Now;
TimeSpan h = new TimeSpan(1, 5, 5);
Console.WriteLine(Adds.Add(g, h));

public static class Adds
{
  /* Reference code
  public static int Add(int a, int b) => a + b;
  public static double Add(double a, double b) => a + b;
  public static string Add(string a, string b) => a + b;
  public static DateTime Add(DateTime a, TimeSpan b) => a + b;
  End reference code */

  //new method using dynamic
  public static dynamic Add(dynamic a, dynamic b) => a + b;

}

