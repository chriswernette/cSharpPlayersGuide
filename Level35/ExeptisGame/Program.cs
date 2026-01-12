// See https://aka.ms/new-console-template for more information
int oatmealRaisin = -1;
Random r = new Random();
oatmealRaisin = r.Next(0, 10);
List<int> chosenNumbers = new List<int>();
int currentPlayer = 0;

//todo 2 players, probably just mod 2 to toggle between player 1 and player 2
//todo add a message saying whose turn it is
//todo add try/catch block, where to put it? Maybe just right around the same # check?
//todo create custom exception type
try
{
  while (true)
  {
    Console.WriteLine($"Player {currentPlayer + 1}'s turn.");
    Console.WriteLine("Enter a number between 0 and 9: ");
    string? userInput;
    userInput = Console.ReadLine();
    int userInt;
    if (int.TryParse(userInput, out userInt))
    {
      Console.WriteLine($"You entered {userInt}.");
    }
    else
    {
      Console.WriteLine("Not a number dummy!");
    }

    if (chosenNumbers.Contains(userInt))
    {
      Console.WriteLine("That cookie has already been eaten stupid! Pick a new number.");
      continue;
    }
    else
    {
      chosenNumbers.Add(userInt);
    }

    if (userInt == oatmealRaisin)
    {
      throw new OatmealCookieException($"Player {currentPlayer + 1} ate the oatmeal raisin cookie!");
    }
    else
    {
      currentPlayer = (currentPlayer + 1)%2;
    }
  }
}
catch (OatmealCookieException e)
{
  Console.WriteLine(e);
  Console.WriteLine($"Player {currentPlayer + 1} ate the oatmeal raisin cookie and lost the game!");
}


public class OatmealCookieException : Exception
{
  public OatmealCookieException() { }
  public OatmealCookieException(string message) : base(message) {
  }
}