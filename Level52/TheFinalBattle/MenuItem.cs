// See https://aka.ms/new-console-template for more information
//Not expecting to use this class except for default cases

using CSharpPlayersGuide.RichConsole;

public class Menu
{
  public MenuItem[] MenuItems {  get; private set; }

  public Menu(MenuItem[] menuItems) => MenuItems = menuItems;

  public void PrintMenu()
  {
    int i = 0;
    foreach (MenuItem item in MenuItems)
    {
      if (item.isEnabled)
      {
        RichConsole.WriteLine($"{i} - {item.Description}");
        i++;
      }
    }
  }

}

public record MenuItem(string Description, bool isEnabled = true);

public enum MenuStage { ActionStage, TargetStage, AttackTypeStage, ItemStage }