

Sword mySword = new Sword();
ColoredItem<Sword> blueSword = new ColoredItem<Sword>(mySword, ConsoleColor.Blue);

Bow myBow = new Bow();
ColoredItem<Bow> redBow = new ColoredItem<Bow>(myBow, ConsoleColor.Red);

Axe myAxe = new Axe();
ColoredItem<Axe> greenAxe = new ColoredItem<Axe>(myAxe, ConsoleColor.Green);

blueSword.Display();
redBow.Display();
greenAxe.Display();

public class Sword { }
public class Bow { }
public class Axe { }

public class ColoredItem<T>
{
  public T Item {  get; }
  public ConsoleColor ItemColor { get; } 

  public void Display()
  {
    Console.ForegroundColor = ItemColor;
    Console.WriteLine(Item.ToString());
  }
  
  public ColoredItem(T item, ConsoleColor itemColor)
  {
    Item = item;
    ItemColor = itemColor;
  }


}