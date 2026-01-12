// See https://aka.ms/new-console-template for more information
Console.Write("What operation shall the Numeromechanical Seive perform for you today? " +
  "Valid options are: E = check if even, P = check if positive, T = check if divisible by 10: ");
string? userInput = null;
bool userInputValid = false;
OperationDelegate userDelegate = isDefault;

while(userInputValid != true)
{
  userInput = Console.ReadLine().ToUpper().Trim();
  if (userInput == null)
  {
    Console.WriteLine("Enter something next time dummy!");
  }
  else if (userInput == "E")
  {
    userInputValid = true;
    userDelegate = isEven;
  }
  else if (userInput == "P")
  {
    userInputValid = true;
    userDelegate = isPositive;
  }
  else if (userInput == "T")
  {
    userInputValid = true;
    userDelegate = isMultTen;
  }
  else
  {
    Console.WriteLine($"You entered {userInput}, which is not a valid command! " +
      $"Valid options are: E = check if even, P = check if positive, T = check if divisible by 10: ");
  }
}

Sieve mySieve = new Sieve(userDelegate);

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
  else if(userInput == "Q")
  {
    break;
  }
  else
  {
    Console.WriteLine("Input a valid number next time!");
  }
}

bool isDefault(int number) => true;
bool isEven(int number) => (number % 2) == 0;
bool isPositive(int number) => number > 0;
bool isMultTen(int number) => (number % 10) == 0;

public delegate bool OperationDelegate(int number);


public class Sieve
{
  OperationDelegate seiveOperation;
  public bool IsGood(int number)
  {
    bool result = false;
    result = seiveOperation(number);
    if(result == true)
    {
      Console.WriteLine($"The value {number} is a good number!");
    }
    else
    {
      Console.WriteLine($"The value {number} is a bad number!");
    }
      return result;
  }

  public Sieve(OperationDelegate userInput){
    seiveOperation = userInput;
  }
}
