// See https://aka.ms/new-console-template for more information
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

Console.WriteLine("Hello, World!");

Point myPoint1 = new Point(2, 3);
Point myPoint2 = new Point(-4, 0);

Console.WriteLine($"( {myPoint1.X}, {myPoint1.Y} )");
Console.WriteLine($"( {myPoint2.X}, {myPoint2.Y} )");


public class Point
{
  public int X { get; private set; } = 0;
  public int Y { get; private set; } = 0;

  public Point() : this(0, 0) { }
  public Point(int x, int y)
  {
    this.X = x;
    this.Y = y;
  }
}

