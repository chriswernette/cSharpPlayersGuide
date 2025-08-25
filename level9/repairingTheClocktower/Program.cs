// See https://aka.ms/new-console-template for more information
Console.Write("What is your number? ");
int clockNum = Convert.ToInt32(Console.ReadLine());

if ((clockNum % 2) == 0)
{
  Console.WriteLine("Tick");
} else
{
  Console.WriteLine("Tock");
}
