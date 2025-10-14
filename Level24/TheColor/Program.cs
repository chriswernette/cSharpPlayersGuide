// See https://aka.ms/new-console-template for more information

Color myColor1 = new Color(0, 230, 47);
Color myRed = Color.Red;


Console.WriteLine($"( {myColor1._red}, {myColor1._green}, {myColor1._blue} )");
Console.WriteLine($"( {myRed._red}, {myRed._green}, {myRed._blue} )");

public class Color
{
  public int _red { get; private set; } = 0;
  public int _green { get; private set; } = 0;
  public int _blue { get; private set; } = 0;

  public static Color White = new(255, 255, 255);
  public static Color Black = new(0, 0, 0);
  public static Color Red = new(255, 0, 0);
  public static Color Orange = new(255, 165, 0);
  public static Color Yellow = new(255, 255, 0);
  public static Color Green = new(0, 128, 0);
  public static Color Blue = new(0, 0, 255);
  public static Color Purple = new(128, 0, 128);

  public Color(int red, int green, int blue)
  {
    _red = red;
    _green = green;
    _blue = blue;
  }
}
