// See https://aka.ms/new-console-template for more information

using System.ComponentModel.Design;
Arrow arrow = new();
int customOrStandard = -1;
while (true)
{
  Console.WriteLine("Would you like a custom arrow or a standard arrow?");
  Console.Write("Enter 0 for custom, 1 for The Elite Arrow, 2 for The Beginner Arrow, and 3 for the Marksman Arrow: ");
  customOrStandard = Convert.ToInt32(Console.ReadLine());

  if ((customOrStandard >= 0) && (customOrStandard <= 3))
  {
    break;
  }
}

if (customOrStandard == 0)
{

  Console.Write("Choose your arrowhead type, Steel, Wood, or Obsidian by entering 0, 1 or 2: ");
  ArrowheadType arrowChoice = ArrowheadType.Steel;
  int selection = -1;
  while (true)
  {
    selection = Convert.ToInt32(Console.ReadLine());

    if ((selection >= 0) && (selection <= 2))
    {
      arrowChoice = (ArrowheadType)selection;
      break;
    }
    else
    {
      Console.Write("Try again, your selection was not within the valid range! :");
    }
  }


  Console.Write("Choose your fletching type, Plastic, Turkey Feathers, or Goose Feathers by entering 0, 1 or 2: ");
  selection = -1;
  FletchingType fletchingChoice = FletchingType.Plastic;

  while (true)
  {
    selection = Convert.ToInt32(Console.ReadLine());

    if ((selection >= 0) && (selection <= 2))
    {
      fletchingChoice = (FletchingType)selection;
      break;
    }
    else
    {
      Console.Write("Try again, your selection was not within the valid range! :");
    }
  }

  Console.Write("Choose your shaft length, between 60 and 100: ");
  double length = 0;

  while (true)
  {
    length = Convert.ToDouble(Console.ReadLine());
    if ((length >= 60) && (length <= 100))
    {
      break;
    }
    else
    {
      Console.Write("Try again, your selection was not within the valid range!: ");
    }
  }

  arrow.Arrowhead = arrowChoice;
  arrow.Fletching = fletchingChoice;
  arrow.Length = length;
}

else
{
  if(customOrStandard == 1)
  {
    arrow = Arrow.CreateEliteArrow();
  }
  else if (customOrStandard == 2)
  {
    arrow = Arrow.CreateBeginnerArrow();
  }
  else
  {
    arrow = Arrow.CreateMarksmanArrow();
  }
}

Console.WriteLine($"The cost of this arrow is {arrow.GetCost()} gold!");

class Arrow
{

  private ArrowheadType _arrow;
  private FletchingType _fletching;
  private double _length;


  public Arrow() : this(ArrowheadType.Steel, FletchingType.Plastic, 60)
  {
  }

  public Arrow(ArrowheadType arrowhead, FletchingType fletching, double length)
  {
    _arrow = arrowhead;
    _fletching = fletching;
    _length = length;
  }

  public static Arrow CreateEliteArrow() => new Arrow(ArrowheadType.Steel, FletchingType.Plastic, 95);
  public static Arrow CreateBeginnerArrow() => new Arrow(ArrowheadType.Wood, FletchingType.GooseFeathers, 75);
  public static Arrow CreateMarksmanArrow() => new Arrow(ArrowheadType.Steel, FletchingType.GooseFeathers, 65);

  public ArrowheadType Arrowhead
  {
    get => _arrow;
    set => _arrow = value;
  }
  public FletchingType Fletching
  {
    get => _fletching;
    set => _fletching = value;
  }
  public double Length
  {
    get => _length;
    set
    {
      _length = value;
      if (this._length < 60)
      {
        this._length = 60;
      }
      else if (this._length > 100)
      {
        this._length = 100;
      }
    }
  }

  public double GetCost()
  {
    double price = 0;
    if (this._arrow == ArrowheadType.Steel)
    {
      price += 10;
    }
    else if (this._arrow == ArrowheadType.Wood)
    {
      price += 3;
    }else if(this._arrow == ArrowheadType.Obsidian)
    {
      price += 5;
    }
    if(this._fletching == FletchingType.Plastic)
    {
      price += 10;
    }else if(this._fletching == FletchingType.TurkeyFeathers)
    {
      price += 5;
    }else if(this._fletching== FletchingType.GooseFeathers)
    {
      price += 3;
    }
    price += this._length * .05;
    return price;
  }
}

enum ArrowheadType { Steel, Wood, Obsidian}
enum FletchingType { Plastic, TurkeyFeathers, GooseFeathers }