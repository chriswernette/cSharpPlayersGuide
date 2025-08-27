// See https://aka.ms/new-console-template for more information

/// <summary>
/// 
/// </summary>
int AskForNumber(string text)
{
  Console.WriteLine(text);
  int result = Convert.ToInt32(Console.ReadLine());
  return result;
}
int AskForNumberInRange(string text, int min, int max)
{
  while (true)
  {
    Console.WriteLine(text);
    int result = Convert.ToInt32(Console.ReadLine());
    if ((result >= min) && (result <= max))
      return result;
  }

}
