// See https://aka.ms/new-console-template for more information
using System.ComponentModel.Design;

Console.WriteLine("Hello, World!");


/*
 * TODO
 * DONE  enums for door states
 * DONE Create door class with initial constructor that sets the first password (using an integer)
 * DONE Create method for resetting the password
 * DONE Create a method for changing the state (including if they need to enter a password to unlock
 * Create a main function with a while loop that prompts the user for inputs to the door
 * The main function will have to ask for an initial passcode before the while loop starts
 * The main function will also need to be smart enough to prompt for a password if the door is locked
 * The main function will also need a way to terminate, i.e. a quit key
 */


Console.Write("Set an initial integer passcode for the door. " +
  "The value must be between 0 and 2,147,483,647: ");
int initialPasscode = Convert.ToInt32(Console.ReadLine());
Door myDoor = new Door(initialPasscode);


while (true)
{
  Console.WriteLine();
  LockState oldLockState = myDoor.GetLockState();

  Console.WriteLine($"The door is currently {oldLockState.ToString()}. " +
    $"Time to make a decision.");

  Console.Write("What do you want to do? Valid commands are O = Open, " +
    "C = Close, L = Lock, U = Unlock, S = Set Passcode, Q = Quit. " +
    "An unlock command will also prompt for a passcode: ");
  string? command = Console.ReadLine();



  //null checking
  if (command == null)
  {
    Console.WriteLine("You didn't enter anything! Try again.");
  }

  //Open
  else if(command == "O")
  {
    LockState myLockState = LockState.Open;
    myDoor.ChangeLockState(oldLockState, myLockState);
  }

  //Close
  else if (command == "C")
  {
    LockState myLockState = LockState.Closed;
    myDoor.ChangeLockState(oldLockState, myLockState);
  }

  //Lock
  else if (command == "L")
  {
    LockState myLockState = LockState.Locked;
    myDoor.ChangeLockState(oldLockState, myLockState);
  }

  //Unlock
  else if(command == "U")
  {
    LockState myLockState = LockState.Closed;
    Console.Write("What is the passcode?: ");
    int passcode = Convert.ToInt32(Console.ReadLine());
    myDoor.ChangeLockState(oldLockState, myLockState, passcode);
  }

  //Set passcode
  else if(command == "S")
  {
    Console.Write("What is the current passcode?: ");
    int oldPasscode = Convert.ToInt32(Console.ReadLine());
    Console.Write("And what do you want to make the new passcode?: ");
    int newPasscode = Convert.ToInt32(Console.ReadLine()); 
    myDoor.SetPasscode(oldPasscode, newPasscode);
  }

  //Quit the program
  else if(command == "Q")
  {
    break;
  }

  //else, some invalid string
  else
  {
    Console.WriteLine("You entered an invalid string! Try again!");
  }

}



public class Door
{
  private LockState Lock = LockState.Open;
  private int passcode = 0;

  public Door(int passcode)
  {
    this.passcode = passcode;
  }

  public LockState GetLockState()
  {
    return this.Lock;
  }

  public void SetPasscode(int oldPasscode, int newPasscode)
  {
    if (oldPasscode == this.passcode)
    {
      this.passcode = newPasscode;
      Console.WriteLine("Passcode reset.");
    }
    else
    {
      Console.WriteLine("Error when resetting passcode, you did not enter the correct old passcode. Access Denied!");
    }
  }

  public void ChangeLockState(LockState oldState, LockState newState, int enteredPasscode)
  {
    
    if (oldState == newState)
    {
      Console.WriteLine($"The door is already {this.GetLockState().ToString()}");
      return;
    }
    
    if (enteredPasscode == this.passcode)
    {
      if ((newState == LockState.Closed) && (oldState == LockState.Locked))
      {
        this.Lock = LockState.Closed;
        Console.WriteLine("You unlocked the door!");
      }
    }
    else if (enteredPasscode != this.passcode)
    {
      Console.WriteLine("Error, the passcode was not correct, door remains locked!");
    }
    else
    {
      Console.WriteLine("Error, you entered an invalid state transition!");
    }
  }

  public void ChangeLockState(LockState oldState, LockState newState)
  {
    if (oldState == newState)
    {
      Console.WriteLine($"The door is already {this.GetLockState().ToString()}");
    }
    else if (oldState == LockState.Locked)
    {
      Console.WriteLine("You cannot unlock without supplying a passcode!");
    }
    else if ((oldState == LockState.Closed) && (newState == LockState.Locked))
    {
      this.Lock = LockState.Locked;
    }
    else if ((oldState == LockState.Closed) && (newState == LockState.Open))
    {
      this.Lock = LockState.Open;
    }
    else if ((oldState == LockState.Open) && (newState == LockState.Closed))
    {
      this.Lock = LockState.Closed;
    }
    else
    {
      Console.WriteLine("Error, you entered an invalid state transition!");
    }
  }
}

  public enum LockState
  {
    Open,
    Closed,
    Locked
  }