// See https://aka.ms/new-console-template for more information

Robot myRobot = new Robot();

Console.WriteLine("Enter three commands for the robot to take. " +
  "Valid commands are O = on, F = off, N = north, S = South, E = East, W = West, Q = Quit");

string? userInput;

//TODO create a new generic command each loop and assign it to the user chosen command?
//TODO Run the robot commands

while(true)
{
  IRobotCommand myCommand;
  userInput = Console.ReadLine();
   if (userInput != null)
  {
    userInput = userInput.ToUpper();
  }
  else
  {
    Console.WriteLine("Enter something next time dummy!");
    myCommand = new OffCommand();
    myRobot.Commands.Add(myCommand);
    continue;
  }


  if (userInput == "O")
  {
    myCommand = new OnCommand();
    myRobot.Commands.Add(myCommand);
  }
  else if (userInput == "F")
  {
    myCommand = new OffCommand();
    myRobot.Commands.Add(myCommand);
  }
  else if (userInput == "N")
  {
    myCommand = new NorthCommand();
    myRobot.Commands.Add(myCommand);
  }
  else if (userInput == "S")
  {
    myCommand = new SouthCommand();
    myRobot.Commands.Add(myCommand);
  }
  else if (userInput == "E")
  {
    myCommand = new EastCommand();
    myRobot.Commands.Add(myCommand);
  }
  else if (userInput == "W")
  {
    myCommand = new WestCommand();
    myRobot.Commands.Add(myCommand);
  }
  else if (userInput == "Q")
  {
    break;
  }
  else
  {
    Console.WriteLine("Enter something next time dummy!");
    myCommand = new OffCommand();
    myRobot.Commands.Add(myCommand);
    continue;
  }
}

myRobot.Run();





public class Robot
{
  public int X { get; set; }
  public int Y { get; set; }
  public bool IsPowered { get; set; }
  public List<IRobotCommand> Commands { get; } = new List<IRobotCommand>();
  public void Run()
  {
    foreach (IRobotCommand? command in Commands)
    {
      command?.Run(this);
      Console.WriteLine($"[{X} {Y} {IsPowered}]");
    }
  }
}

public interface IRobotCommand
{
  void Run(Robot robot);
}

public class OnCommand : IRobotCommand
{
  public void Run(Robot robot)
  {
    robot.IsPowered = true;
  }
}

public class OffCommand : IRobotCommand
{
  public void Run(Robot robot)
  {
    robot.IsPowered = false;
  }
}

public class NorthCommand : IRobotCommand
{
  public void Run(Robot robot)
  {
    if (robot.IsPowered)
    {
      robot.Y += 1;
    }
  }
}


public class SouthCommand : IRobotCommand
{
  public void Run(Robot robot)
  {
    if (robot.IsPowered)
    {
      robot.Y -= 1;
    }
  }
}

public class WestCommand : IRobotCommand
{
  public void Run(Robot robot)
  {
    if (robot.IsPowered)
    {
      robot.X -= 1;
    }
  }
}

public class EastCommand : IRobotCommand
{
  public void Run(Robot robot)
  {
    if (robot.IsPowered)
    {
      robot.X += 1;
    }
  }
}