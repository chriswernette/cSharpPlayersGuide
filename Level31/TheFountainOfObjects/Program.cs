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

ConsoleHelper.StartGameText();
HelpCommand startingHelp = new HelpCommand();
startingHelp.Execute();

string gameSize = ConsoleHelper.PromptForGameSize();

FountainOfObjects myFountain = new FountainOfObjects(gameSize);
myFountain.Run();

/*
try
{
  myFountain.Run();
}
catch
{
  //TODO put in exceptions
  ConsoleHelper.BlockLine(); 
}
finally
{
  //TODO cleanup text, print win or lose message, stats like how many moves, how many arrows fired, how many monsters killed etc.
  ConsoleHelper.BlockLine();
} 
*/


public class FountainOfObjects
{
  //TODO understand fields vs properties and if these should be capitalized or not
  public Player player;
  public Monster[] Monsters { get; }
  public Map map;
  public ICommand playerCommand;
  public ISense playerSense;
  //TODO Add monsters hehe

  public FountainOfObjects (string gameSize)
  {
    playerCommand = new NoCommand(); //These two are only used to avoid warnings about unitialized members... is there a better way around this?
    playerSense = new NoSense();
    gameSize = gameSize.Trim().ToLower();

    switch (gameSize)
    {
      case "small":
        map = new Map(4, 4);
        map.AddRoom(RoomType.Fountain, 0, 2);
        map.AddRoom(RoomType.Entrance, 0, 0);
        map.AddRoom(RoomType.Pit, 0, 1);
        Monsters = [
                    new Maelstrom(new Location(2,2)) 
                    ];
        player = new Player(0, 0);
        break;
      case "medium":
        map = new Map(6, 6);
        map.AddRoom(RoomType.Fountain, 0, 4);
        map.AddRoom(RoomType.Entrance, 3, 4);
        map.AddRoom(RoomType.Pit, 0, 0);
        map.AddRoom(RoomType.Pit, 5, 3);
        Monsters = [
                    new Maelstrom(new Location(3,2)) 
                    ];
        player = new Player(3, 4);
        break;
      case "large":
        map = new Map(8, 8);
        map.AddRoom(RoomType.Fountain, 3, 4);
        map.AddRoom(RoomType.Entrance, 6, 2);
        map.AddRoom(RoomType.Pit, 0, 0);
        map.AddRoom(RoomType.Pit, 5, 3);
        map.AddRoom(RoomType.Pit, 1, 4);
        map.AddRoom(RoomType.Pit, 2, 3);
        Monsters = [
                    new Maelstrom(new Location(2,2)), 
                    new Maelstrom(new Location(3,4))
                    ];
        player = new Player(6, 2);
        break;
      default:
        map = new Map(4, 4);
        map.AddRoom(RoomType.Fountain, 0, 2);
        map.AddRoom(RoomType.Entrance, 0, 0);
        map.AddRoom(RoomType.Pit, 0, 1);
        player = new Player(0, 0);
        break;
    }
    

    //TODO add medium and large games
  }

  public void Run()
  {
    while (true)
    {
      this.CurrentStats();
      this.SenseMap();
      playerCommand = this.GetCommand();
      playerCommand.Execute(this);
      foreach (Monster Monster in Monsters)
        if (Monster.MonsterLocation == player.playerLocation)
          Monster.Activate(this);

      if (PitSense.InPit(this)) // maybe this is an OK way to work around the pit sense issue?
        player.GetKilled("Oops, you fell into a pit of death!");
      if (player.CheckForDeath())
      {
        ConsoleHelper.PrintLine(player.deathCause, ConsoleColor.Red);
        return;
      }
      if (CheckForWin(this.map, this.player.playerLocation, this.map.CheckFountainEnabled()))
      {
        ConsoleHelper.WinMessage(this);
        return;
      }
      ConsoleHelper.BlockLine();
    }
  }

  public void CurrentStats()
  {
    ConsoleHelper.PrintLine($"You are in the room at (Row={player.playerLocation.Y}, Column={player.playerLocation.X})");
    ConsoleHelper.PrintLine($"You have {player.Arrows} arrows remaining.");
  }

  public ICommand GetCommand()
  {
    ICommand returnCommand = new NoCommand();
    ConsoleHelper.PrintLine("What do you want to do? ", newLine: false);

    while (true)
    {
      string? stringCommand = Console.ReadLine();
      if (stringCommand == null || stringCommand == "") //try again, not letting null thru
        continue;
      stringCommand = stringCommand.ToLower().Trim();
      returnCommand = stringCommand switch
      {
        "move north" => returnCommand = new MoveCommand(Direction.North),
        "move east" => returnCommand = new MoveCommand(Direction.East),
        "move south" => returnCommand = new MoveCommand(Direction.South),
        "move west" => returnCommand = new MoveCommand(Direction.West),
        "shoot north" => returnCommand = new MoveCommand(Direction.North),
        "shoot east" => returnCommand = new MoveCommand(Direction.East),
        "shoot south" => returnCommand = new MoveCommand(Direction.South),
        "shoot west" => returnCommand = new MoveCommand(Direction.West),
        "enable fountain" => returnCommand = new EnableFountainCommand(),
        _ => returnCommand = new InvalidCommand(),
      }; 
     return returnCommand;
    }
  }

  //this should check all the nearby rooms for pits and monsters, and the current room for entrance, pit (death), or fountain
  public void SenseMap()
  {
    //check each sense one by one, for example
    this.playerSense = new EntranceSense();
    if (this.playerSense.Sense(this))
      playerSense.DisplaySense(this);

    this.playerSense = new FountainSense();
    if (playerSense.Sense(this))
      playerSense.DisplaySense(this);

    this.playerSense = new PitSense();
    if (playerSense.Sense(this))
      playerSense.DisplaySense(this);

    this.playerSense = new MaelstromSense();
    if(playerSense.Sense(this))
      playerSense.DisplaySense(this);
  }
 
  //TODO Add CheckForWin
  public bool CheckForWin(Map map, Location playerLocation, bool fountainEnabled)
  {
    if (map.GetRoomType(playerLocation) == RoomType.Entrance && fountainEnabled)
        return true;
    else
      return false;
  }

}

//TODO clean up fields/parameters, should they be all caps? Provide getters and setters? Re-read that portion of book
public class Player
{
  public Location playerLocation = new Location(0, 0);
  public bool isAlive;
  public string deathCause;
  public int Arrows;

  //initializer
  public Player(int row, int col)
  {
    playerLocation = new Location(row, col);
    isAlive = true;
    deathCause = string.Empty;
    Arrows = 5;
  }

  public bool CheckForDeath() => this.isAlive == false;

  public void GetKilled(string reasonYouDied)
  {
    this.isAlive = false;
    this.deathCause = reasonYouDied;
  }

  public void FireArrow(Direction dir)
  {
    this.Arrows--;
    //TODO add direction and monsters
  }

}

public abstract class Monster
{
  public bool IsAlive;
  public Location MonsterLocation;

  public Monster(Location startingLocation)
  {
    MonsterLocation = startingLocation;
    IsAlive = true;
  }

  public void GetKilled()
  {
    this.IsAlive = false;
  }

  public abstract void Activate(FountainOfObjects game);

}

/* if player and Maelstrom location are in the same room, player moves 1N 2E. MS moves 1S 2W, clamp to map
       place 1 in medium, 2 in large game  
     */
public class Maelstrom : Monster
{

  public Maelstrom(Location startingLocation) : base(startingLocation) { }

  public override void Activate(FountainOfObjects game)
  {
    Location pLoc = game.player.playerLocation;
    
    int newPX = Math.Clamp(pLoc.X - 1, 0, game.map.Row - 1);
    int newPY = Math.Clamp(pLoc.Y + 2, 0, game.map.Column - 1);
    int newMX = Math.Clamp(MonsterLocation.X + 1, 0, game.map.Row - 1);
    int newMY = Math.Clamp(MonsterLocation.Y - 2, 0, game.map.Column - 1);

    game.player.playerLocation.X = newPX;
    game.player.playerLocation.Y = newPY;

    MonsterLocation.X = newMX; 
    MonsterLocation.Y = newMY;

    ConsoleHelper.PrintLine("You are swept away by a maelstrom!", ConsoleColor.Magenta);

  }
}

/* if player and Amarok are in the same room, player dies
       place 1 in small, 2 in medium, 3 in large  
     */
public class Amarok : Monster
{

  public Amarok(Location startingLocation) : base(startingLocation) { }

  public override void Activate(FountainOfObjects game)
  {
    game.player.GetKilled("You met your untimely end with an Amarok. He ate you alive.");
  }
}

public class Map 
{
  public int Row {  get; }
  public int Column { get; }
  bool FountainEnabled { get; set; }
  public RoomType [,] Rooms;
  public Map(int row, int column)
  {
    Rooms = new RoomType[row, column];
    Row = row;
    Column = column;
    FountainEnabled = false;
  }
  
  public void AddRoom(RoomType type, int row, int col) => this.Rooms[row,col] = type;

  public RoomType GetRoomType(Location location) => this.Rooms[location.X, location.Y];

  public bool IsRoomType(Location location, RoomType room) => this.Rooms[location.X, location.Y] == room;
  
  public bool CheckNeighboringRooms(Location location, RoomType room)
  {
    bool result = false;
    int westX = Math.Clamp(location.X - 1, 0, this.Row - 1);
    int eastX = Math.Clamp(location.X + 1, 0, this.Row - 1);
    int northY = Math.Clamp(location.Y - 1, 0, this.Column - 1);
    int southY = Math.Clamp(location.Y + 1, 0, this.Column - 1);
    for (int i = westX; i <= eastX; i++)
    {
      for (int j = northY; j <= southY; j++)
      {
        if (i == location.X && j == location.Y) //do nothing, this is the same as current location
        { }
        if (this.Rooms[i, j] == room)
        {
          result = true;
        }
      }
    }
    return result;
  }

  public bool CheckFountainEnabled() => this.FountainEnabled;
  public void EnableFountain() => FountainEnabled = true;

  public bool CheckOnMap(Location location)
  {
    if(location.X > (Row-1) || location.Y > (Column-1) || location.X < 0 || location.Y < 0)
      return false;
    else
      return true;
  }
}

public static class ConsoleHelper
{
  public static void PrintLine(string line, ConsoleColor color = ConsoleColor.White, bool newLine = true)
  {
    Console.ForegroundColor = color;
    if(newLine)
      Console.WriteLine(line);
    else
      Console.Write(line);
  }
  
  //I think the above definition is *better*, allows you to put in those params or not, has defaults
  /*public static void printLine(string line) //commonly printing white lines
  {
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(line);
  }*/
  public static void BlockLine()
  {
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("--------------------------------------------------------------");
  }

  public static void StartGameText()
  {
    Console.WriteLine("You enter the Cavern of Objects, a maze of rooms filled with dangerous pits in search of the Fountain of Objects.");
    Console.WriteLine("Light is visible only in the entrance, and no other light is seen anywhere in the caverns.");
    Console.WriteLine("You must navigate the Caverns with your other senses.");
    Console.WriteLine("Find the Fountain of Objects, activate it, and return to the entrance.");
    Console.WriteLine("Look out for pits. You will feel a breeze if a pit is in an adjacent room.");
    Console.WriteLine("Maelstroms are violent forces of sentient wind. Entering a room with one could transport you to any other location in the caverns." +
      " You will be able to hear their growling and groaning in nearby rooms.");
    Console.WriteLine("Amoraks roam the caverns. Encountering one is certain death, but you can smell their rotten stench in nearby rooms.");
    Console.WriteLine("You carry with you a bow and a quiver of arrows. You can use them to shoot monsters in the caverns but be warned you have a limited supply.");
    BlockLine();
  }

  public static string PromptForGameSize()
  {
    PrintLine("What size game would you like to play? Valid options are small, medium, or large: ");
    string response = "";
    while (true)
    {
      response = Console.ReadLine();
      if (response == null)
      {
        PrintLine("Enter something next time! ");
        continue;
      }
      switch (response)
      {
        case "small":
          return response;
        case "medium":
          return response;
        case "large":
          return response;
        default:
          PrintLine("Input a valid game size next time!");
          break;
      }
    }
  }

  public static void WinMessage(FountainOfObjects game)
  {
    game.CurrentStats();
    PrintLine("The Fountain of Objects has been reactivated, and you have escaped with your life!", ConsoleColor.Green);
    PrintLine("You win!", ConsoleColor.Green);
    Console.ForegroundColor = ConsoleColor.White; // for the next round
  }
}

public interface ICommand
{
  void Execute(FountainOfObjects game);
}

public class NoCommand : ICommand
{
  public void Execute(FountainOfObjects game) { }
}

public class InvalidCommand: ICommand
{
  public void Execute(FountainOfObjects game) => ConsoleHelper.PrintLine("You did not enter a valid command. Try again.");
}

public class HelpCommand : ICommand
{
  public void Execute(FountainOfObjects game)
  {
    Console.WriteLine("There are four main forms of commands: typing 'help' displays this message");
    Console.WriteLine("Typing 'enable fountain' attempts to enable the fountain of objects, but will only work if you are in the fountain room");
    Console.WriteLine("Typing 'move' + a direction will attempt to move you in that direction, for example 'move east'. But you cannot walk off the map.");
    Console.WriteLine("Typing 'shoot' + a direction will attempt to fire an arrow in that direction. Be warned that you have a limited number of arrows!");
    Console.WriteLine("Typing anything else will have no affect.");
    ConsoleHelper.BlockLine();
  }
  public void Execute() //research if there's a better way to do this, the FoO game is really an optional arg/not needed for help, but we need it to fully implement the interface...
  {
    Console.WriteLine("There are four main forms of commands: typing 'help' displays this message");
    Console.WriteLine("Typing 'enable fountain' attempts to enable the fountain of objects, but will only work if you are in the fountain room");
    Console.WriteLine("Typing 'move' + a direction will attempt to move you in that direction, for example 'move east'. But you cannot walk off the map.");
    Console.WriteLine("Typing 'shoot' + a direction will attempt to fire an arrow in that direction. Be warned that you have a limited number of arrows!");
    Console.WriteLine("Typing anything else will have no affect.");
    ConsoleHelper.BlockLine();
  }
}
public class EnableFountainCommand : ICommand
{
  public void Execute(FountainOfObjects game)
  {
    Location pLoc = game.player.playerLocation;
    Map gameMap = game.map;
    if (gameMap.GetRoomType(pLoc) == RoomType.Fountain)
    {
      gameMap.EnableFountain();
      ConsoleHelper.PrintLine("You hear the rushing waters from the Fountain of Objects. It has been reactivated!", ConsoleColor.Blue);
    }
    else
      ConsoleHelper.PrintLine("You are not in the fountain room you dummy!", ConsoleColor.White);
  }
}

public class MoveCommand : ICommand
{
  public Direction direction {  get; }

  public MoveCommand(Direction dir)
  {
    direction = dir;
  }
  public void Execute(FountainOfObjects game)
  {    
    Location pLoc = game.player.playerLocation;
    Map gameMap = game.map;
    Location newLoc = pLoc;
    if(this.direction == Direction.North) 
      newLoc = new Location(pLoc.X, pLoc.Y - 1);
    if (this.direction == Direction.East)
      newLoc = new Location(pLoc.X + 1, pLoc.Y);
    if (this.direction == Direction.South)
      newLoc = new Location(pLoc.X, pLoc.Y + 1);
    if(this.direction == Direction.West)
      newLoc = new Location(pLoc.X - 1, pLoc.Y);

    //if it's a valid move, make it, otherwise send message they tried to walk off the map
    if (gameMap.CheckOnMap(newLoc))
      game.player.playerLocation = newLoc;
    else
      ConsoleHelper.PrintLine("You hit a wall! You can't move there!");

  }
}

public class ShootCommand : ICommand
{
  public Direction direction { get; }

  public ShootCommand(Direction dir)
  {
    direction = dir;
  }
  public void Execute(FountainOfObjects game)
  {
    //TODO check for monster in the shooting direction, and update it's life to 0 if shot
    //TODO decrement player arrow
  }
}

public interface ISense
{
  bool Sense(FountainOfObjects game);
  void DisplaySense(FountainOfObjects game);
}

public class NoSense : ISense
{
  public bool Sense(FountainOfObjects game) => false;
  public void DisplaySense(FountainOfObjects game) { }
}
public class FountainSense : ISense
{
  public bool Sense(FountainOfObjects game)
  {
    Location pLoc = game.player.playerLocation;
    bool result = game.map.IsRoomType(pLoc, RoomType.Fountain);
    return result;
  }

  public void DisplaySense(FountainOfObjects game)
  {
    if (game.map.CheckFountainEnabled())
      ConsoleHelper.PrintLine("You hear the rushing waters from the Fountain of Objects. It has been reactivated!", ConsoleColor.Blue);
    else
      ConsoleHelper.PrintLine("You hear water dripping in this room. The Fountain of Objects is here!", ConsoleColor.Blue);
  }
}

  public class EntranceSense : ISense
  {
    public bool Sense(FountainOfObjects game) => game.map.IsRoomType(game.player.playerLocation, RoomType.Entrance);
    public void DisplaySense(FountainOfObjects game) => ConsoleHelper.PrintLine("You see light coming from the cavern entrance.", ConsoleColor.Yellow);
  }

public class PitSense : ISense
{
  public bool Sense(FountainOfObjects game)
  {
    Location pLoc = game.player.playerLocation;
    bool pitSensed = game.map.CheckNeighboringRooms(pLoc, RoomType.Pit);
    return pitSensed;
  }
  public static bool InPit(FountainOfObjects game) => game.map.IsRoomType(game.player.playerLocation, RoomType.Pit);

  public void DisplaySense(FountainOfObjects game) => ConsoleHelper.PrintLine("You feel a draft. There is a pit in a nearby room.");

}

public class MonsterSense: ISense
{
  public bool Sense(FountainOfObjects game)
  {
    Location pLoc = game.player.playerLocation;
    bool monsterSensed = false;

    foreach (Monster monster in game.Monsters)
    {
      if (monster.GetType() == typeof(Maelstrom))
      {
        Location mLoc = monster.MonsterLocation;
        monsterSensed = monsterSensed || CheckNeighboringRooms(pLoc, mLoc);
      }
    }
    return monsterSensed;
  }

  public void DisplaySense(FountainOfObjects game) => ConsoleHelper.PrintLine("There's a monster nearby!");

  public bool CheckNeighboringRooms(Location playerLocation, Location monsterLocation)
  {
    bool result = false;

    int xDist = Math.Abs(playerLocation.X - monsterLocation.X);
    int yDist = Math.Abs(playerLocation.Y - monsterLocation.Y);

    if (xDist == 0 && yDist == 1)
    {
      result = true;
    }
    else if (xDist == 1 && yDist == 0)
    {
      result = true;
    }
    else if (xDist == 1 && yDist == 1)
    {
      result = true;
    }
    return result;
  }
}

public class MaelstromSense : MonsterSense, ISense
{
  public new bool Sense(FountainOfObjects game)
  {
    Location pLoc = game.player.playerLocation;
    bool maelstromSensed = false;

    foreach (Monster monster in game.Monsters)
    {
      if(monster.GetType() == typeof(Maelstrom))
      {
        Location mLoc = monster.MonsterLocation;
        maelstromSensed = maelstromSensed || CheckNeighboringRooms(pLoc, mLoc);
      }
    }
    return maelstromSensed;
  }
  public new void DisplaySense(FountainOfObjects game) => ConsoleHelper.PrintLine("You hear the growling and groaning of a maelstrom nearby.", ConsoleColor.Cyan);
}

public class AmarokSense : MonsterSense
{
  public new bool Sense(FountainOfObjects game)
  {
    Location pLoc = game.player.playerLocation;
    bool amarokSensed = false;

    foreach (Monster monster in game.Monsters)
    {
      if (monster.GetType() == typeof(Amarok))
      {
        Location mLoc = monster.MonsterLocation;
        amarokSensed = amarokSensed || CheckNeighboringRooms(pLoc, mLoc);
      }
    }
    return amarokSensed;
  }

  public new void DisplaySense(FountainOfObjects game) => ConsoleHelper.PrintLine("You hear the growling and groaning of a maelstrom nearby.", ConsoleColor.Cyan);
}


public class Location
{
  public int X { get; set; }
  public int Y { get; set; }
  public Location(int x, int y)
  {
    X = x;
    Y = y;
  }
  public static bool operator ==(Location a, Location b) => (a.X == b.X) && (a.Y == b.Y); 
  public static bool operator !=(Location a, Location b) => (a.X != b.X) || (a.Y != b.Y);
};
public enum Direction { North, East, South, West}
public enum RoomType { Empty, Entrance, Fountain, Pit}