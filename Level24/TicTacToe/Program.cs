// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography.X509Certificates;



/*
 * Tic-Tac-Toe: take input from 2 different players for a move
 * Player 1 designates what square to place and X
 * Update the board with an X
 * Player 2 designates what square to place an O
 * Update the board with an O
 * Prevent players from playing an occuppied space
 * 
 * Detect who wins first
 * Detect if the board is full, i.e. draw/cat
 * 
 * Display the results/outcome when the game is over
 * 
 * Display the state of the board after each turn. E.g.
 * 
 * It is X's turn
 *    | X |   
 * ___________
 *    | O | X 
 * -----------
 *  O |   |   
 * What square do you want to play in?
 * 
 * Board is represented by the number pad for input. EZ
 * 7 8 9
 * 4 5 6
 * 1 2 3
 * 
 */
Board myBoard = new Board();
while (true)
{
  /*flow of play is to print the board, make a move, then check if
   * there's a winner, check if there's a cat's game. Announce who the winner is, 
   * or that it's a cats game, reset the board if there's a winner or cats game.
  */
  Console.WriteLine($"It is {myBoard.WhoseTurn}'s turn");
  myBoard.PrintBoard();
  Console.WriteLine("Enter a number from 1-9 on the numpad corresponding to " +
    "the move you wish to make. Numbers or strings outside this range will be " +
    "rejected. q to quit the game.");
  string? moveChoice = Console.ReadLine();
  if (moveChoice == "q")
  {
    Console.WriteLine("You chose to quit the game, game over.");
    break;
  }

  int intMoveChoice;
  bool isNumeric = int.TryParse(moveChoice, out intMoveChoice);
  if (!isNumeric)
  {
    Console.WriteLine("You entered a string or null character that doesn't correspond" +
      " to a valid command or move choice, try again!");
    continue;
  }




  /*next check if they made a valid move, check if the square is not occuppied
   * if either is false then break and redo the turn.
   * */
  bool checkIndex = myBoard.MoveToArrayIndex(intMoveChoice);
  if (checkIndex == true)
  {
    bool checkSquareNotOccupied = myBoard.CheckSquareNotOccupied();
    if (checkSquareNotOccupied == true)
    {
      myBoard.MakeMove();
      bool winner = myBoard.CheckWinner();
      bool cat = myBoard.CheckCatsGame();
      if (winner == true)
      {
        Console.WriteLine($"Player {myBoard.WhoseTurn} won! The board will now be reset.");
        myBoard.ResetBoard();
        continue;
      }
      else if (cat == true)
      {
        Console.WriteLine("It's a cat's game! Better luck next time.");
        myBoard.ResetBoard();
        continue;
      }
      //want to update turn last so the winner prints correctly
      myBoard.UpdateTurn();
    }
    //if index is not valid the move indexer will take care of printing the message 
    else
    {
      continue;
    }
  }
  else
  {
    continue;
  }


}




public class Board
{
  //properties are the board itself and the move indexer
  public Square[,] BoardSquares = new Square[3, 3] {{Square.Null, Square.Null, Square.Null },
    { Square.Null, Square.Null, Square.Null }, {Square.Null, Square.Null, Square.Null } };

  public int[] MoveIndexer = new int[2] { -1, -1 };
  public string WhoseTurn = "X";

  //needed to reinitialize the board each game
  public void ResetBoard()
  {
    for (int i = 0; i < this.BoardSquares.GetLength(0); i++)
    {
      for (int j = 0; j < this.BoardSquares.GetLength(1); j++)
      {
        BoardSquares[i, j] = Square.Null;
      }
    }

    //we check for -1 indices as an invalid move
    MoveIndexer[0] = -1;
    MoveIndexer[1] = -1;
    WhoseTurn = "X";
    Console.WriteLine();
  }

  //convert between user input and values to index the Board matrix
  public bool MoveToArrayIndex(int move)
  {
    bool indexedCorrectly = true;
    if (move == 7)
    {
      MoveIndexer[0] = 0;
      MoveIndexer[1] = 0;
    }
    else if (move == 8)
    {
      MoveIndexer[0] = 0;
      MoveIndexer[1] = 1;
    }
    else if (move == 9)
    {
      MoveIndexer[0] = 0;
      MoveIndexer[1] = 2;
    }
    else if (move == 4)
    {
      MoveIndexer[0] = 1;
      MoveIndexer[1] = 0;
    }
    else if (move == 5)
    {
      MoveIndexer[0] = 1;
      MoveIndexer[1] = 1;
    }
    else if (move == 6)
    {
      MoveIndexer[0] = 1;
      MoveIndexer[1] = 2;
    }
    else if (move == 1)
    {
      MoveIndexer[0] = 2;
      MoveIndexer[1] = 0;
    }
    else if (move == 2)
    {
      MoveIndexer[0] = 2;
      MoveIndexer[1] = 1;
    }
    else if (move == 3)
    {
      MoveIndexer[0] = 2;
      MoveIndexer[1] = 2;
    }
    else
    {
      Console.WriteLine("Error! You entered a move outside the valid range!");
      MoveIndexer[0] = -1;
      MoveIndexer[1] = -1;
      indexedCorrectly = false;
    }
    return indexedCorrectly;
  }

  //check move is valid
  public bool CheckSquareNotOccupied()
  {

    Square currentSquare = this.BoardSquares[MoveIndexer[0], MoveIndexer[1]];
    if (currentSquare == Square.Null)
    {
      return true;
    }
    else
    {
      Console.WriteLine("Invalid move, the square is already occupied! Try again.");
      return false;
    }
  }

  //place a move for X or O
  public void MakeMove()
  {
    Square PlayerValue;
    if (this.WhoseTurn == "X")
    {
      PlayerValue = Square.X;
    }
    else
    {
      PlayerValue = Square.O;
    }
    this.BoardSquares[MoveIndexer[0], MoveIndexer[1]] = PlayerValue;
  }

  //update the turn
  public void UpdateTurn()
  {
    if (this.WhoseTurn == "X")
    {
      this.WhoseTurn = "O";
    }
    else
    {
      this.WhoseTurn = "X";
    }
  }

  //check if these are all the same value. Easier to loop through matrices now?
  public bool CheckWinner()
  {
    //init to 0, only set 
    bool win = false;


    //check rows
    for (int i = 0; i < 3; i++)
    {
      if ((this.BoardSquares[i, 0] == this.BoardSquares[i, 1]) &&
        (this.BoardSquares[i, 1] == this.BoardSquares[i, 2]) &&
        (this.BoardSquares[i, 0] != Square.Null))
      {
        win = true;
      }
    }
    //check columns
    for (int j = 0; j < 3; j++)
    {
      if ((this.BoardSquares[0, j] == this.BoardSquares[1, j]) &&
        (this.BoardSquares[1, j] == this.BoardSquares[2, j]) &&
        (this.BoardSquares[0, j] != Square.Null))
      {
        win = true;
      }
    }

    //check right diagonal
    if ((this.BoardSquares[0, 0] == this.BoardSquares[1, 1]) &&
      (this.BoardSquares[1, 1] == this.BoardSquares[2, 2]) &&
      (this.BoardSquares[0, 0] != Square.Null))
    {
      win = true;
    }

    //check left diagonal
    if ((this.BoardSquares[0, 2] == this.BoardSquares[1, 1]) &&
      (this.BoardSquares[1, 1] == this.BoardSquares[2, 0]) &&
      (this.BoardSquares[0, 2] != Square.Null))
    {
      win = true;
    }

    return win;
  }

  //check for cats game, i.e. every single square is occupied aka not null
  public bool CheckCatsGame()
  {
    bool isCat = true;
    for (int i = 0; i < 3; i++)
    {
      for (int j = 0; j < 3; j++)
      {
        if (this.BoardSquares[i, j] == Square.Null)
        {
          isCat = false;
        }
      }
    }
    return isCat;
  }

  //prints the board per the spec above. Tricky dicky
  public void PrintBoard()
  {
    Console.WriteLine();
    string[,] printArray = new string[3, 3];
    for (int i = 0; i < 3; i++)
    {
      for (int j = 0; j < 3; j++)
      {
        if (BoardSquares[i, j] == Square.Null)
        {
          printArray[i, j] = " ";
        }
        else
        {
          printArray[i, j] = BoardSquares[i, j].ToString();
        }
      }
      Console.WriteLine($" {printArray[i, 0]} | {printArray[i, 1]} | {printArray[i, 2]}");
      if (i != 2)
      {
        Console.WriteLine("-----------");
      }
    }
    Console.WriteLine();
  }

  //initializer, is this the same as initializing the properties? Need to research
  public Board()
  {
    this.BoardSquares = new Square[3, 3] {{Square.Null, Square.Null, Square.Null },
    { Square.Null, Square.Null, Square.Null }, {Square.Null, Square.Null, Square.Null } };
    this.MoveIndexer = new int[2] { -1, -1 };
    this.WhoseTurn = "X";
  }

}


public enum Square { Null, X, O }

