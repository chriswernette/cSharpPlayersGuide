// See https://aka.ms/new-console-template for more information
Console.Write("What operation shall the Numeromechanical Seive perform for you today? " +
  "Valid options are: E = check if even, P = check if positive, T = check if divisible by 10: ");
string? userInput = null;
bool userInputValid = false;
Sieve mySieve = new Sieve(x => true);

while (userInputValid != true)
{
  userInput = Console.ReadLine().ToUpper().Trim();
  if (userInput == null)
  {
    Console.WriteLine("Enter something next time dummy!");
  }
  else if (userInput == "E")
  {
    userInputValid = true;
    mySieve = new Sieve(x => x % 2 == 0);
  }
  else if (userInput == "P")
  {
    userInputValid = true;
    mySieve = new Sieve(x => x > 0);
  }
  else if (userInput == "T")
  {
    userInputValid = true;
    mySieve = new Sieve(x => x % 10 == 0);
  }

  else
  {
    Console.WriteLine($"You entered {userInput}, which is not a valid command! " +
      $"Valid options are: E = check if even, P = check if positive, T = check if divisible by 10: ");
  }
}

int userIntegerInput = 0;
while (true)
{


  Console.Write("Enter a number and the Numeromechanical Sieve will tell you if it's good or bad! Q to quit: ");
  userInput = Console.ReadLine().Trim().ToUpper();
  userInputValid = int.TryParse(userInput, out userIntegerInput);
  if (userInputValid)
  {
    mySieve.IsGood(userIntegerInput);
  }
  else if (userInput == "Q")
  {
    break;
  }
  else
  {
    Console.WriteLine("Input a valid number next time!");
  }
}

public delegate bool OperationDelegate(int number);

public class Sieve
{
  OperationDelegate seiveOperation;
  public bool IsGood(int number)
  {
    bool result = false;
    result = seiveOperation(number);
    if (result == true)
    {
      Console.WriteLine($"The value {number} is a good number!");
    }
    else
    {
      Console.WriteLine($"The value {number} is a bad number!");
    }
    return result;
  }

  public Sieve(OperationDelegate userInput)
  {
    seiveOperation = userInput;
  }
}
