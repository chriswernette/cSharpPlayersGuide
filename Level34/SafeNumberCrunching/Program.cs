// See https://aka.ms/new-console-template for more information

while (true)
{
  Console.Write("Enter an integer chump: ");
  string? number = Console.ReadLine();
  if(int.TryParse(number, out _))
  {
    break;
  }
  else
  {
    Console.WriteLine("Enter a valid integer next time pal!");
  }
}

while (true)
{
  Console.Write("Enter a double chump: ");
  string? number = Console.ReadLine();
  if (double.TryParse(number, out _))
  {
    break;
  }
  else
  {
    Console.WriteLine("Enter a valid double next time pal!");
  }
}

while (true)
{
  Console.Write("Enter a boolean value true/false chump: ");
  string? number = Console.ReadLine();
  if (bool.TryParse(number, out _))
  {
    break;
  }
  else
  {
    Console.WriteLine("Enter a valid boolean next time pal!");
  }
}