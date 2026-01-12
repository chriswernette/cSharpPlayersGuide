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
 * Make a command class, activation for square class, etc.
 */

namespace GameSpace
{
  public class Game : GameBase
  {
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
  }
}

