// See https://aka.ms/new-console-template for more information
using System.Dynamic;

dynamic robots = new List<dynamic>();
int currentID = 1;
string? userInput;

while (true)
{
  dynamic robot = new ExpandoObject();
  robot.ID = currentID;
  Console.WriteLine($"You are producing robot #{currentID}.");
  
  //name
  Console.Write("Do you want to name this robot? Yes/No ");
  userInput = Console.ReadLine().ToLower().Trim();
  if(userInput == "yes")
  {
    Console.Write("What is its name? ");
    userInput = Console.ReadLine();
    robot.Name = userInput;
  }
  else
  {
    //do nothing
  }
  
  //size
  Console.Write("Does this robot have a specific size? Yes/No ");
  userInput = Console.ReadLine().ToLower().Trim();
  if (userInput == "yes")
  {
    Console.Write("What is its height? ");
    userInput = Console.ReadLine();
    int height = 0;
    _ = Int32.TryParse(userInput, out height);
    robot.Height = height;

    Console.Write("What is its width? ");
    userInput = Console.ReadLine();
    int width = 0;
    _ = Int32.TryParse(userInput, out width);
    robot.Width = width;
  }
  else
  {
    //do nothing
  }

  //color
  Console.Write("Does this robot need to be a specific color? Yes/No ");
  userInput = Console.ReadLine().Trim().ToLower();
  if(userInput == "yes")
  {
    Console.Write("What color? ");
    userInput = Console.ReadLine().Trim();
    robot.Color = userInput;
  }
  else
  {
    //do nothing
  }
  robots.Add( robot );

  foreach(KeyValuePair<string, object> property in (IDictionary<string, object>) robot)
    Console.WriteLine($"{property.Key}: {property.Value}");
  currentID++;

}

