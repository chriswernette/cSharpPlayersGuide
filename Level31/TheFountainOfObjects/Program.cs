// See https://aka.ms/new-console-template for more information

/* The Fountain of Objects
 * 
 * 2D 4x4 grid based game, the objective is to walk into the fountain of objects room, activate the
 * fountain, and then walk back to the entrance.
 * 
 * Flow: 
 * 1. The player is told what they can sense in the dark (see, hear, smell), then they get the chance to perform
 * some action by typing it into the command line.
 * 2. Their action is resolved (the player moves, state of things in the game changes, checking for a win/loss etc.)
 * then the loop repeats
 * Most rooms are empty, nothing to sense.
 * 
 * The player is in one of the rooms and can move between them by typing commands like "move north" "move south", "move east", "move west"
 * The player cannot move past the edge of the map (clip to (0,0) and (3,3))
 * 
 * The room at (0,0) is the cave entrance and exit. The player should start here. The player can sense light coming from outside the 
 * cavern when in this room. "You see light in this room coming from outside the cavern. This is the entrance."
 * 
 * The room at (0,2) is the fountain room, containing the fountain of objects. The fountain can either be enabled or disabled. The player can hear the fountain
 * but hears different things if it's on or not. "You hear water dripping in this room. The Fountain of Objects is here!" or
 * "You hear the rushing waters from the Fountain of Objects. It has been reactivated!"
 * The fountain is initially off. In the fountain room, the player can type "enable fountain" to enable it. If the player is not in the fountain room
 * and types this, there should be no effect, and the player should be told so.
 * You win by moving to the fountain room, enabling the Fountain of Objects and moving back to the cavern entrance.
 * Use different text color to display different types of text in the console window. Narrative = magenta, descriptive text = white, 
 * input from user = cyan, text describing the entrance light = yellow, messages about the fountain = blue/
 */


/*TODO
 * Try to make the game more object oriented... make classes for monsters, maybe make a map class or a location type for each square
 * Make a command class, activation for square class, monsters class and derive different monsters from them etc.
 * 
 * Re-read object oriented chapters and take some notes in a notepad/text file
 * Watch some youtubes about each of the Object Oriented concepts
 * Re-read the solution code
 * Redo the game as is, but more object oriented - change the start location for different game sizes too!
 * Once tested the redesigned game, add 2 more expansion packs! Maelstrom and arrow?
 */



/*CLASS LIST
 * Game - contains map (small, med, large), monsters, player, fountain enabled
 * Map - nxn set of rooms
 * Room - contains roomtype, monster or no monster, fountain or no fountain? 
 * Console helper - can write message to console or print those break line things?
 * 
 * 
 */

/*METHOD LIST
 * Console helper - prints things to the console in pretty formats/colors, also takes user input and formats it or re-prompts?
 * 
 * 
 * 
 * 
 * 
 */
public class Player
{
  public Location playerLocation = new Location(0,0);
  public bool isAlive;
  public string deathCause;
  public int arrowsRemaining;

  //initializer
  public Player(int row, int col)
  {
    playerLocation = new Location(row, col);
    isAlive = true;
    deathCause = string.Empty;
    arrowsRemaining = 5;
  }
  //returns list of senses and prints out messages for each
  public List<Sense> SenseNearbyRooms(Map map)
  {
    ConsoleHelper helper = new ConsoleHelper();
    List<Sense> senses = new List<Sense>();

    for (int i = playerLocation.X - 1; i > playerLocation.X; i++)
    {
      for (int j = playerLocation.Y - 1; j > playerLocation.Y; j++)
      {
        if (map.Rooms[i, j] == RoomType.Pit)
        {
          senses.Add(Sense.Pit);
          helper.printLine("You feel a draft. There is a pit in a nearby room.", ConsoleColor.White); // what if multiple pits? This will print multiple times? OK?
        }
      }
    }
    if (map.Rooms[playerLocation.X, playerLocation.Y] == RoomType.Entrance)
    {
      senses.Add(Sense.Light);
      helper.printLine("You see light coming from the cavern entrance.", ConsoleColor.Yellow);
    }
    if (map.Rooms[playerLocation.X, playerLocation.Y] == RoomType.Fountain)
    {
      senses.Add(Sense.Fountain);
      if(map.CheckFountainEnabled() == false)
        helper.printLine("You hear water dripping in this room. The fountain of objects is here!", ConsoleColor.Blue);
      if (map.CheckFountainEnabled() == true)
        helper.printLine("You hear the rushing waters from the fountain of objects. It has been reactivated!", ConsoleColor.Blue); //Should we also print something like this when they reactivate?
    }
    return senses;
  }


  public void GetKilled(string reasonYouDied)
  {
    this.isAlive = false;
    this.deathCause = reasonYouDied;
  }

  public void MoveCommand(Direction dir, int mapSize)
  {
    if (dir == Direction.North)
      playerLocation.Y = ClampAndReport(playerLocation.Y - 1, 0, mapSize);
    if (dir == Direction.East)
      playerLocation.X = ClampAndReport(playerLocation.X + 1, 0, mapSize);
    if (dir == Direction.South)
      playerLocation.Y = ClampAndReport(playerLocation.Y + 1, 0, mapSize);
    if(dir == Direction.West)
      playerLocation.X = ClampAndReport(playerLocation.X - 1, 0, mapSize);
  }
  private int ClampAndReport(int value, int min, int max)
  {
    if(value < min || value > max)
    {
      ConsoleHelper helper = new ConsoleHelper();
      helper.printLine("You tried to walk off the edge of the map! Resetting your position.", ConsoleColor.Red);
      helper.blockLine();
    }
    return Math.Clamp(value, min, max);
  }

  public bool checkForDeath()
  {
    return this.isAlive;
  }

  public void FireArrow(Direction dir)
  {
    this.arrowsRemaining--;
    //TODO add direction and monsters
  }



}

public class Map 
{
  int Row {  get; }
  int Column { get; }
  bool FountainEnabled { get; set; }
  public RoomType [,] Rooms;
  public Map(int row, int column)
  {
    Rooms = new RoomType[row, column];
    Row = row;
    Column = column;
    FountainEnabled = false;
  }
  public bool CheckFountainEnabled()
  {
    return FountainEnabled;
  }
  public void EnableFountain()
  {
    FountainEnabled = true;
  }
}

public class ConsoleHelper
{
  public void printLine(string line, ConsoleColor color)
  {
    Console.ForegroundColor = color;
    Console.WriteLine(line);
  }
  public void blockLine()
  {
    Console.WriteLine("--------------------------------------------------------------");
  }

  public void startGameText()
  {
    Console.WriteLine("You enter the Cavern of Objects, a maze of rooms filled with dangerous pits in search of the Fountain of Objects.");
    Console.WriteLine("Light is visible only in the entrance, and no other light is seen anywhere in the caverns.");
    Console.WriteLine("You must navigate the Caverns with your other senses.");
    Console.WriteLine("Find the Fountain of Objects, activate it, and return to the entrance.");
    Console.WriteLine("Look out for pits. You will feel a breeze if a pit is in an adjacent room.");
    Console.WriteLine("Maelstroms are violent forces of sentient wind. Entering a room with one could transport you to any other location in the caverns." +
      "You will be able to hear their growling and groaning in nearby rooms.");
    Console.WriteLine("Amoraks roam the caverns. Encountering one is certain death, but you can smell their rotten stench in nearby rooms.");
    Console.WriteLine("You carry with you a bow and a quiver of arrows. You can use them to shoot monsters in the caverns but be warned you have a limited supply.");
  }
  public void helpText()
  {
    Console.WriteLine("There are four main forms of commands: typing 'help' displays this message");
    Console.WriteLine("Typing 'enable fountain' attempts to enable the fountain of objects, but will only work if you are in the fountain room");
    Console.WriteLine("Typing 'move' + a direction will attempt to move you in that direction, for example 'move east'. But you cannot walk off the map.");
    Console.WriteLine("Typing 'shoot' + a direction will attempt to fire an arrow in that direction. Be warned that you have a limited number of arrows!");
    Console.WriteLine("Typing anything else will have no affect.");
  }
}

public struct Location
{
  public int X { get; set; }
  public int Y { get; set; }
  public Location(int x, int y)
  {
    X = x;
    Y = y;
  }
};
public enum Direction { North, East, South, West}
public enum Sense { NoSense, Light, Fountain, Pit}
public enum RoomType { Empty, Entrance, Fountain, Pit}