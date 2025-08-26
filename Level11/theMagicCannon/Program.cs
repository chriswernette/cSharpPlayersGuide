// See https://aka.ms/new-console-template for more information
for (int i = 1; i < 101; i++)
{
  int fire = i % 3;
  int electric = i % 5;
  if ((fire == 0) && (electric == 0))
  {
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine($"{i}: Power Burst!");
  }
  else if (fire == 0)
  {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"{i}: Fire!");
  }
  else if (electric == 0)
  {
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"{i}: Electric!");
  }
  else
  {
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"{i}: Normal");
  }
}
