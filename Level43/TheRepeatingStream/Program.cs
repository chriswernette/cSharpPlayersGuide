
// See https://aka.ms/new-console-template for more information
Random myRand = new Random();
RecentNumbers myRecent = new RecentNumbers();

Thread thread = new Thread(Generator);
Thread thread2 = new Thread(CheckSame);
thread.Start();
thread2.Start();
while (true)
{
  
}

//generates random numbers, prints to window, and updates the last 2 saved
void Generator()
{
  while (true)
  {
    //generate random and write to window per the spec
    int randomStorage = 0;
    randomStorage = myRand.Next(0, 10);
    Console.WriteLine(randomStorage);
    myRecent.UpdateNumbers(randomStorage);
    Thread.Sleep(1000);
  }
  
}

//checks if the last 2 saved are the same on any key input
void CheckSame()
{
  while (true)
  {
    if (Console.KeyAvailable)
    {
      Console.ReadKey();
      myRecent.CheckSame();
    }
  }
  
}

public class RecentNumbers
{
  private readonly object _numberLock = new object();
  private int _lastNumber = 0;
  private int _lastNumber2 = 0;
  public int LastNumber
  {
    get
    {
      lock (_numberLock)
      {
        return _lastNumber;
      }
    }
  }
  public int LastNumber2
  {
    get
    {
      lock (_numberLock)
      {
        return _lastNumber2;
      }
    }
  }

  public void UpdateNumbers(object number)
  {
    lock (_numberLock)
    {
      _lastNumber2 = LastNumber;
      _lastNumber = (int)number;
    }
  }

  public void CheckSame()
  {
    lock (_numberLock)
    {
      if (_lastNumber == _lastNumber2)
      {
        Console.WriteLine("You got it right! The last 2 numbers are the same, bravo!");
      }
      else
      {
        Console.WriteLine("Sorry, not a winner.");
      }
    }
  }
}
