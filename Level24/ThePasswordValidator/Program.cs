// See https://aka.ms/new-console-template for more information

PasswordValidator myPasswordValidator = new();

while (true)
{
  Console.Write("Enter a password to be validated (q to quit): ");
  string? password;
  password = Console.ReadLine();
  if (password == null)
  {
    Console.WriteLine("You didn't enter anything, try again!");
    continue;
  }
  else if (password == "q")
  {
    break;
  }
  else
  {
    bool result;
    result = myPasswordValidator.IsValidPassword(password);
    if (result)
    {
      Console.WriteLine("Valid password!");
    }
    else
    {
      Console.WriteLine("Invalid password!");
    }
  }
}

//could store the password data here but it doesn't seem like we need to? So just
//a class with 2 methods? Should the methods be static?
public class PasswordValidator
{
  public bool IsValidPassword(string password)
  {
    bool lengthRule; 
    lengthRule = ((password.Length >= 6) && (password.Length <= 13));
    bool upperRule = false;
    bool lowerRule = false;
    bool numberRule = false;
    
    //initialize to true, if any Ts or &s set to false
    bool uppercaseTOrAmpersandRule = true;

    foreach (char letter in password)
    {
      if (char.IsUpper(letter))
      {
        upperRule = true;
      }
      else if (char.IsLower(letter))
      {
        lowerRule = true;
      }
      else if (char.IsDigit(letter))
      {
        numberRule = true;
      }

      //set to false if current letter fails the check
      uppercaseTOrAmpersandRule = uppercaseTOrAmpersandRule && SillyRuleChecker(letter);
    }

    return (upperRule && lowerRule && numberRule && uppercaseTOrAmpersandRule);

  }

  public bool SillyRuleChecker(char letter)
  {
    char upperCaseT = 'T';
    char ampersand = '&';

    if ((letter == upperCaseT) || (letter == ampersand))
    {
      return false;
    }
    else
    {
      return true;
    }
  }
}