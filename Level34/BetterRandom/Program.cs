// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;

Random myRandom = new Random();
Console.WriteLine(myRandom.NextDouble(10.0));

Console.WriteLine(myRandom.NextString("farts", "farty", "bad gas", "gay", "I'm gay", "Gayness in the anus"));

Console.WriteLine(myRandom.CoinFlip(.99));

Console.WriteLine(myRandom.CoinFlip());

public static class RandomExtensions
{
  public static double NextDouble(this Random random, double maxValue)
  {
    return random.NextDouble() * maxValue;
  }

  public static string NextString(this Random random, params string[] strings)
  {
    string returnString = "";
    int numStrings = strings.Length;
    if (numStrings > 0)
    {
      int selectionID = random.Next(numStrings);
      returnString = strings[selectionID];
    }
    return returnString;
  }

  //heads = 0, tails = 1?
  public static bool CoinFlip(this Random random, double frequency = 0.5)
  {
    bool returnBool;
    double randomFlip = random.NextDouble();
    if (randomFlip < frequency)
    {
      returnBool = true;
    }
    else
    {
      returnBool = false;
    }
    return returnBool;
  }
 
}