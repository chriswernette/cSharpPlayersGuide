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
    if(this.item.Type == ItemType.HelpItem)
    {
      this.target.HP = Math.Clamp(this.target.HP + item.HP, 0, this.target.MaxHP);
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
    RichConsole.WriteLine($"{user.Name} used {item.itemName} on {target.Name}");
    RichConsole.WriteLine($"{target.Name} is now at {target.HP} HP");
    RichConsole.WriteLine();
  }
}

public class Item
{
  private static int HealthPotionHP = 10;
  public bool isUsed { get; private set; }
  public int HP { get; private set; }
  public ItemType Type { get; private set; }
  public string itemName { get; private set; }

  public Item()
  {
    HP = 0;
    Type = ItemType.NoType;
    isUsed = true;
    this.itemName = "";
  }

  public Item(int hp, ItemType type, string itemName)
  {
    HP = hp;
    Type = type;
    isUsed = false;
    this.itemName = itemName;
  }
  public static Item CreateHealthPotion() => new Item(HealthPotionHP, ItemType.HelpItem, "Health Potion");

  public void UseItem() //one time use
  {
    isUsed = true;
  }
}

public enum ItemType { NoType, OffensiveItem, HelpItem}