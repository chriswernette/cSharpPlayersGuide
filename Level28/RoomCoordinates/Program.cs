// See https://aka.ms/new-console-template for more information
using System.Data;

Console.WriteLine("Hello, World!");

Coordinate coord1 = new Coordinate(1, 5);
Coordinate coord2 = new Coordinate(2, 6);
Coordinate coord3 = new Coordinate(3, 7);

bool adj12 = coord1.IsAdjacent(coord2);
bool adj13 = coord1.IsAdjacent(coord3);
bool adj23 = coord2.IsAdjacent(coord3);

Console.WriteLine($"Coordinate 1 is adjacent to Coordinate 2: {adj12}");
Console.WriteLine($"Coordinate 2 is adjacent to Coordinate 3: {adj23}");
Console.WriteLine($"Coordinate 1 is not adjacent to Coordinate 3: {adj13}");

public struct Coordinate
{
  public int Row { get; }
  public int Column { get; }
  public Coordinate (int row, int col)
  {
    Row = row;
    Column = col;
  }
  public bool IsAdjacent(Coordinate other)
  {
    if (Math.Abs(this.Column - other.Column) < 2)
    {
      return true;
    }
    else if (Math.Abs(this.Row - other.Row) < 2)
    {
      return true;
    }
    else
    {
      return false;
    }
  }
}