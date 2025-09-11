// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

int input = 10;

countdown(input);
void countdown(int x)
{
  Console.WriteLine(x);
  if (x == 1)
  {
    return;
  }
  else
  {
    countdown(x - 1);
  }
}