// See https://aka.ms/new-console-template for more information

Robot myRobot = new Robot();

Console.WriteLine("Enter three commands for the robot to take. " +
  "Valid commands are O = on, F = off, N = north, S = South, E = East, W = West");

string?[] userinput = new string[3];

//TODO create a new generic command each loop and assign it to the user chosen command?
//TODO Run the robot commands

for (int i = 0; i < 3; i++)
{
  RobotCommand myCommand;
  userinput[i] = Console.ReadLine();
  if (userinput[i] != null)
  {
    userinput[i] = userinput[i].ToUpper();
  }
  else
  {
    Console.WriteLine("Enter something next time dummy!");
    myCommand = new OffCommand();
    myRobot.Commands[i] = myCommand;
    continue;
  }


  if (userinput[i] == "O")
  {
    myCommand = new OnCommand();
    myRobot.Commands[i] = myCommand;
  }
  else if (userinput[i] == "F")
  {
    myCommand = new OffCommand();
    myRobot.Commands[i] = myCommand;
  }
  else if (userinput[i] == "N")
  {
    myCommand = new NorthCommand();
    myRobot.Commands[i] = myCommand;
  }
  else if (userinput[i] == "S")
  {
    myCommand = new SouthCommand();
    myRobot.Commands[i] = myCommand;
  }
  else if (userinput[i] == "E")
  {
    myCommand = new EastCommand();
    myRobot.Commands[i] = myCommand;
  }
  else if (userinput[i] == "W")
  {
    myCommand = new WestCommand();
    myRobot.Commands[i] = myCommand;
  }
  else
  {
    Console.WriteLine("Enter something next time dummy!");
    myCommand = new OffCommand();
    myRobot.Commands[i] = myCommand;
    continue;
  }
}

myRobot.Run(); 
  




public class Robot
{
  public int X { get; set; }
  public int Y { get; set; }
  public bool IsPowered { get; set; }
  public RobotCommand?[] Commands { get; } = new RobotCommand?[3];
  public void Run()
  {
    foreach (RobotCommand? command in Commands)
    {
      command?.Run(this);
      Console.WriteLine($"[{X} {Y} {IsPowered}]");
    }
  }
}

public abstract class RobotCommand
{
  public abstract void Run(Robot robot);
}

public class OnCommand : RobotCommand
{
  public override void Run(Robot robot)
  {
    robot.IsPowered = true;
  }
}

public class OffCommand : RobotCommand
{
  public override void Run(Robot robot)
  {
    robot.IsPowered = false;
  }
}

public class NorthCommand : RobotCommand
{
  public override void Run(Robot robot)
  {
    if (robot.IsPowered)
    {
      robot.Y += 1;
    }
  }
}


public class SouthCommand : RobotCommand
{
  public override void Run(Robot robot)
  {
    if (robot.IsPowered)
    {
      robot.Y -= 1;
    }
  }
}

public class WestCommand : RobotCommand
{
  public override void Run(Robot robot)
  {
    if (robot.IsPowered)
    {
      robot.X -= 1;
    }
  }
}

public class EastCommand : RobotCommand
{
  public override void Run(Robot robot)
  {
    if (robot.IsPowered)
    {
      robot.X += 1;
    }
  }
}