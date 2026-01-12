// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;

BlockCoordinate myCoord = new BlockCoordinate(0, 0);
BlockOffset myOffset = new BlockOffset(5, 2);
Direction myDirection = Direction.East;

Console.WriteLine(myCoord +  myOffset);
Console.WriteLine(myCoord + myDirection);
Console.WriteLine(myOffset + myCoord);
Console.WriteLine(myDirection + myCoord);
myCoord = myCoord + myOffset;
Console.WriteLine(myCoord[0]);
Console.WriteLine(myCoord[1]);

myOffset = Direction.North;
Console.WriteLine(myOffset);

public record BlockCoordinate(int Row, int Column)
{
  public static BlockCoordinate operator +(BlockCoordinate a, BlockOffset b)
  {
    return new BlockCoordinate(a.Row + b.RowOffset, a.Column + b.ColumnOffset);
  }
  public static BlockCoordinate operator +(BlockOffset b, BlockCoordinate a) => a + b;
  public static BlockCoordinate operator +(BlockCoordinate a, Direction b)
  {
    if (b == Direction.North)
    {
      return a + new BlockOffset(-1, 0);
    }
    else if (b == Direction.East)
    {
      return a + new BlockOffset(0, 1);
    }
    else if (b == Direction.South)
    {
      return a + new BlockOffset(1, 0);
    }
    else
    {
      return a + new BlockOffset(0, -1);
    }
  }
  public static BlockCoordinate operator +(Direction b, BlockCoordinate a)
  {
    return a + b;
  }
  public double this[int index]
  {
    get
    {
      if (index == 0) return Row;
      else return Column;
    }
  }
}

public record BlockOffset(int RowOffset, int ColumnOffset)
{
  public static implicit operator BlockOffset(Direction dir)
  {
    if (dir == Direction.North)
    {
      return new BlockOffset(-1, 0);
    }
    else if (dir == Direction.East)
    {
      return new BlockOffset(0, 1);
    }
    else if (dir == Direction.South)
    {
      return new BlockOffset(1, 0);
    }
    else
    {
      return new BlockOffset(0, -1);
    }
  }
}
public enum Direction { North, East, South, West }

//TODO addition operator between coord and offset
//TODO addition operator between coord and Direction