namespace Helpers
{
  public static class ColoredConsole
  {
    public static string? Prompt(string question)
    {
      string output;
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine(question);
      return Console.ReadLine();
    }
    public static void WriteLine(string? text, ConsoleColor color)
    {
      Console.ForegroundColor = color;
      Console.WriteLine(text);
    }
    public static void Write(string? text, ConsoleColor color)
    {
      Console.ForegroundColor = color;
      Console.Write(text);
    }
  }
}
