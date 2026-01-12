// See https://aka.ms/new-console-template for more information
Potion userPotion = Potion.Water;
Ingredient userIngredient = Ingredient.Stardust;
string? userIngredientInput;
int userIngredientInt = 0;
bool convertToInt;

while (true)
{
  Console.WriteLine($"\nCurrently you have a {userPotion}. Press C to complete the potion. Or, you can add: ");

  //give list of ingredients corresponding to key press and convert from integer back to enum type
  foreach (Ingredient ingredient in (Ingredient[])Enum.GetValues(typeof(Ingredient)))
  {
    Console.WriteLine($"{ingredient} - press {(int)ingredient}");
  }
  
  userIngredientInput = Console.ReadLine();
  if (userIngredientInput != null && userIngredientInput.Trim().ToUpper() == "C")
  {
    break;
  }
  if (userIngredientInput != null && userIngredientInput != "")
  {
    convertToInt = Int32.TryParse(userIngredientInput, out userIngredientInt);
    if (convertToInt && userIngredientInt >= 0 && userIngredientInt <= 4)
    {
      userIngredient = (Ingredient)userIngredientInt;
      userPotion = Mixer(userPotion, userIngredient);
    }
    else
    {
      Console.WriteLine("Enter an appropriate value for one of the possible ingredients next time good sir. \n");
    }
  }
  else
  {
    Console.WriteLine("Enter something next time silly! \n");
  }
  if(userPotion == Potion.RuinedPotion)
  {
    Console.WriteLine("Welp, another potion ruined... starting over again with water. \n");
    userPotion = Potion.Water;
  }
}
  


Potion Mixer(Potion userPotion, Ingredient userIngredient)
{
  return (userPotion, userIngredient) switch
  {
    (Potion.Water, Ingredient.Stardust) => Potion.Elixir,
    (Potion.Elixir, Ingredient.SnakeVenom) => Potion.PoisonPotion,
    (Potion.Elixir, Ingredient.DragonBreath) => Potion.FlyingPotion,
    (Potion.Elixir, Ingredient.ShadowGlass) => Potion.InvisibilityPotion,
    (Potion.Elixir, Ingredient.EyeshineGem) => Potion.NightSightPotion,
    (Potion.NightSightPotion, Ingredient.ShadowGlass) => Potion.CloudyBrew,
    (Potion.InvisibilityPotion, Ingredient.EyeshineGem) => Potion.CloudyBrew,
    (Potion.CloudyBrew, Ingredient.Stardust) => Potion.WraithPotion,
    (_, _) => Potion.RuinedPotion,
  };
}


enum Potion
{
  Water,
  Elixir,
  PoisonPotion,
  FlyingPotion,
  InvisibilityPotion,
  NightSightPotion,
  CloudyBrew,
  WraithPotion,
  RuinedPotion
}

enum Ingredient
{
  Stardust,
  SnakeVenom,
  DragonBreath,
  ShadowGlass,
  EyeshineGem,
}