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

//TODO ask user input of small, medium or large game. Call small medium large constructor. 

DateTime gameStart = DateTime.Now;
Console.WriteLine("-----------------------------------------------");
Console.Write("What size game would you like to play? Small, medium, or large: ");
string? gameSize = null;

while(gameSize == null)
{
  gameSize = Console.ReadLine();
  if(gameSize == null)
  {
    Console.WriteLine("Enter something dummy!");
  }
}

Game myGame = new Game(gameSize);

Console.WriteLine("-----------------------------------------------");

bool winner;
bool dead;
while(true)
{
  myGame.PlayGame();
  winner = myGame.CheckForWin();
  dead = myGame.CheckInPit(false);
  if (winner)
  {
    break;
  }
  if (dead)
  {
    break;
  }
}
//final announcement, whick we wouldn't normally reach
myGame.AnnounceRoomLocationEffects();
DateTime gameEnd = DateTime.Now;
TimeSpan timePlayed = gameEnd - gameStart;
Console.WriteLine($"Played for {timePlayed.Minutes} minutes and {timePlayed.Seconds} seconds.");

public class Game
{
  public bool FountainEnabled = false;
  public int[] PlayerLocation = { 0, 0 };
  private static int[] FountainLocation = { 0, 2 };
  public static readonly int[] EntranceLocation = { 0, 0 };
  public static int[][] PitLocations = new int[3][]; 
  private int GameSize = 3;
  public static readonly int SmallGameSize = 3;
  public static readonly int MediumGameSize = 5;
  public static readonly int LargeGameSize = 7;

  public Game(string StringGameSize)
  {
    StringGameSize = StringGameSize.Trim().ToLower();
    switch (StringGameSize)
    {
      case "small":
        GameSize = 3;
        FountainLocation = new int[] { 0, 2 };
        PitLocations[0] = new int[] { 0, 3 };
        PitLocations[1] = new int[] { 10, 10 };
        PitLocations[2] = new int[] { 10, 10 };
        break;
      case "medium":
        GameSize = 5;
        FountainLocation = new int[] { 2, 2 };
        PitLocations[0] = new int[] { 0, 3 };
        PitLocations[1] = new int[] { 3, 4 };
        PitLocations[2] = new int[] { 10, 10 };
        break;
      case "large":
        GameSize = 7;
        FountainLocation = new int[] { 2, 4 };
        PitLocations[0] = new int[] { 0, 3 };
        PitLocations[1] = new int[] { 3, 4 };
        PitLocations[2] = new int[] { 4, 6 };
        break;
      default:
        GameSize = 3;
        FountainLocation = new int[] { 0, 2 };
        break;
    }
  }

  public Game()
  {
    GameSize = 3;
    FountainLocation = new int[] { 0, 2 };
    PitLocations[0] = new int[] { 0, 3 };
    PitLocations[1] = new int[] { 10, 10 };
    PitLocations[2] = new int[] { 10, 10 };
  }

  public void PlayGame()
  {
    /*Announce the room location effects which could be fountain, entrance, or win, or more in the future
    * This needs to be called first, and then take the user input..
    */
    AnnounceRoomLocationEffects();

    string? turnCommand = TakeUserInput();
    if(turnCommand != null)
    {
      //make standard input
      string formattedInput = turnCommand.Trim().ToLower();

      //determine if they wanted to move or enable the fountain, if yes, then enable if in the correct room
      bool enable = EnableOrMove(formattedInput);
      if (enable)
      {
        bool inRoom = CheckInFountainRoom(false);
        EnableFountain(inRoom);
      }
      else
      {
        //convert string to coordinate notation, then try to move (might hit a wall)
        int[] moveCommand = StringToCoord(formattedInput);
        UpdatePlayerLocation(moveCommand);
      } 
    }
    else //null check
    {
      Console.ForegroundColor = ConsoleColor.Magenta;
      Console.WriteLine("Enter something next time dummy!");
      Console.ForegroundColor = ConsoleColor.White;
    }
}

  /* AnnounceRoomLocationEffects
   * This function will print the player's current location as well as any effects of the room they are in or nearby rooms
   */
  public void AnnounceRoomLocationEffects()
  {
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine($"You are in the room at (Row={PlayerLocation[0]}, Column={PlayerLocation[1]})");
    Console.ForegroundColor = ConsoleColor.White;

    CheckInFountainRoom(true); //we will add more checks later in the game and call them one after another
    CheckForPitNearby();
    CheckInPit(true);
    CheckForEntrance();
  }

  /* TakeUserInput
   * This function will announce where the user is, print any statuses of that room or adjacent rooms, 
   * and take user input from the command line and trim and lowercase it, for consumption in other functions like 
   * StringToCoord. However, it will not do error checking if it's a valid input, which will be done in those functions.
   */
  public string? TakeUserInput()
  {
    string? command;
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write("What do you want to do? ");
    Console.ForegroundColor = ConsoleColor.Cyan;
    command = Console.ReadLine();
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("-------------------------------------------------------------");
    return command;
  }
  
  /* StringToCoord
   * Converts text player has entered on the command line to a move command
   */
  public int[] StringToCoord(string input)
  {
    int[] intCoord = { 0, 0 };
    if (input == null)
    {
      Console.ForegroundColor = ConsoleColor.Magenta;
      Console.WriteLine("Input something next time!");
      Console.ForegroundColor = ConsoleColor.White;
    }
    else if (input == "move north")
    {
      intCoord[0] = -1;
    }
    else if (input == "move south")
    {
      intCoord[0] = 1;
    }
    else if (input == "move east")
    {
      intCoord[1] = 1;
    }
    else if (input == "move west")
    {
      intCoord[1] = -1;
    }
    else if (input == "enable fountain")
    {
    }
    else
    {
      Console.ForegroundColor = ConsoleColor.Magenta;
      Console.WriteLine("Make a valid command next time");
      Console.ForegroundColor = ConsoleColor.White;
    }
    return intCoord;
  }

  /* UpdatePlayerLocation
   * Takes the move in coordinate pair as integer and updates the players location on the map.
   * Takes into account the boundaries of the map and clips the player's location.
   */
  public void UpdatePlayerLocation(int[] playerMove)
  {
    for (int i = 0; i < playerMove.Length; i++)
    {
      PlayerLocation[i] += playerMove[i];
      //check for out of range and display a message if the player broke the rules
      if (PlayerLocation[i] < 0)
      {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Error, you walked off the edge of the map! Resetting you to your last location");
        Console.ForegroundColor = ConsoleColor.White;
        PlayerLocation[i] = 0;
      }
      if (PlayerLocation[i] > GameSize)
      {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Error, you walked off the edge of the map! Resetting you to your last location");
        Console.ForegroundColor = ConsoleColor.White;
        PlayerLocation[i] = GameSize;
      }
    }
  }

  /*EnableOrMove
   * Determines if the player wants to try to turn on the fountain, or to move. Happens no matter what
   */
  public bool EnableOrMove(string input)
  {
    bool enable = false; 
    if (input == "enable fountain")
    {
      enable = true;
    }
    return enable;
  }

  /*EnableFountain
   * Turns on the fountain of objects!
   */
  public void EnableFountain(bool inRoom)
  {
    if (inRoom)
    {
      FountainEnabled = true;
    }
    else
    {
      Console.ForegroundColor = ConsoleColor.Magenta;
      Console.WriteLine("You are not in the fountain room, you dufus.");
      Console.ForegroundColor = ConsoleColor.White;
    }
  }

 /*CheckInFountainRoom
 * Returns true if you are in the fountain room. Announces the fountain text if called on
 */
  public bool CheckInFountainRoom(bool Announce)
  {
    bool inFountainRoom = false;
    if ((PlayerLocation[0] == FountainLocation[0]) && (PlayerLocation[1] == FountainLocation[1]))
    {
      inFountainRoom = true;
      if (Announce)
      {
        if (FountainEnabled)
        {
          Console.ForegroundColor = ConsoleColor.Blue;
          Console.WriteLine("You hear the rushing waters from the Fountain of Objects. It has been reactivated!");
          Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.Blue;
          Console.WriteLine("You hear water dripping in this room. The Fountain of Objects is here!");
          Console.ForegroundColor = ConsoleColor.White;
        }
      }
    }
    return inFountainRoom;
  }

  /*CheckForPitNearby
   * At the end of every turn, check if the player is 1 room away from a pit and make the announcement if so
   * I.e. if they are due west of pit room, or if they are north, or northwest, etc. 1 away left or right and 1 away up or down
   * Will have to use abs function I think... Check all 3 pit locations every time, since the 2 other pits will be placed
   * off the map if it's a small room, it shouldn't detect them..
   */


  //TODO - debug, I think the coords are messed up maybe? It's saying you're near a pit when you're not actually
  public void CheckForPitNearby()
  {
    for (int i = 0; i < PitLocations.Length; i++)
    {
      int xDistance = Math.Abs(PitLocations[i][0] - PlayerLocation[0]);
      int yDistance = Math.Abs(PitLocations[i][1] - PlayerLocation[1]);
      //want to announce if close to pit room, but not if in the pit room, because then we will announce death instead.
      if((xDistance <= 1) && (yDistance <= 1) && ((xDistance + yDistance) != 0))
      {
        Console.WriteLine("You feel a draft. There is a pit nearby.");
      }
    }
  }

  /*CheckInPit
   * At the end of every turn, if the player stepped into the pit, they die, and the game ends.
   */
  public bool CheckInPit(bool announce)
  {
    bool died = false;
    for (int i = 0; i < PitLocations.Length; i++)
    {
      bool xTrue = PitLocations[i][0] == PlayerLocation[0];
      bool yTrue = PitLocations[i][1] == PlayerLocation[1];
      if (xTrue && yTrue)
      {
        died = true;
        if (announce)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("You fell into a pit! You have died. Game over.");
          Console.ForegroundColor = ConsoleColor.White;
        }
      }
    }
    return died;
  }

  /*CheckForEntrance
   * At the end of every step, checks to see if the player is at the entrance and 
   * if they have won the game and display a congrats message if so
   */
  public void CheckForEntrance()
  {
    if ((PlayerLocation[0] == EntranceLocation[0]) && (PlayerLocation[1] == EntranceLocation[1]))
    {
      if (FountainEnabled == true)
      {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("The Fountain of Objects has been reactivated, and you have escaped with your life! You win!");
        Console.ForegroundColor = ConsoleColor.White;
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("You see light in this room coming from outside the cavern. This is the entrance.");
        Console.ForegroundColor = ConsoleColor.White;
      }
    }
  }

  /*CheckForWin
   * used to escape the forever while loop in the main program
   */
  public bool CheckForWin()
  {
    if ((PlayerLocation[0] == EntranceLocation[0]) && (PlayerLocation[1] == EntranceLocation[1]))
    {
      if (FountainEnabled == true)
      {
        return true;
      }
    }
    return false;
  }
}
