// See https://aka.ms/new-console-template for more information
using CSharpPlayersGuide.RichConsole;

public class ItemCommand : ICommand
{
  Item item;
  Character user;
  Character target;
  
  public ItemCommand(Character user, Character target, Item item)
  {
    this.user = user;
    this.target = target;
    this.item = item;
  }

  public void Execute(Character character) 
  {
    if(item.itemData.Type == ItemType.HelpItem)
    {
      target.TakeHealing(item.itemData.HP);
      Display(character);
      this.item.UseItem();
    }
    else
    {
      //for later, maybe offensive items
    }
    
  }
  public void Display(Character character) 
  {
    RichConsole.WriteLine($"{user.Name} used {item.itemData.Name} on {target.Name}", Colors.Chartreuse);
    RichConsole.WriteLine($"{target.Name} is now at {target.HP} HP", Colors.ForestGreen);
    RichConsole.WriteLine();
  }
}

public class Item
{
  private static int HealthPotionHP = 10;
  public bool isUsed { get; private set; }
  public ItemData itemData;
  public Item() // empty item, do we really need this constructor option?
  {
    this.itemData = new ItemData("", 0, ItemType.NoType);
    isUsed = true;
  }

  public Item(ItemData itemData)
  {
    this.itemData = itemData;
    isUsed = false;
  }
  public static Item CreateHealthPotion() => new Item(new ItemData("Health Potion", HealthPotionHP, ItemType.HelpItem));

  public void UseItem() //one time use
  {
    isUsed = true;
  }
}

public record ItemData(string Name, int HP, ItemType Type);

public enum ItemType { NoType, OffensiveItem, HelpItem}