// See https://aka.ms/new-console-template for more information

(FoodType Food, MainIngredient Main, Seasoning Season) soup = (FoodType.Gumbo, MainIngredient.Chicken, Seasoning.Salty);

Console.WriteLine("What type of soup would you like to make? Pick the food type, main ingredient, and the seasoning.");

int foodChoice = -1;
while ((foodChoice < 0) || (foodChoice > 3))
{
  Console.Write($"Food Type Options are: {FoodType.Soup.ToString()}, {FoodType.Stew.ToString()}, or {FoodType.Gumbo.ToString()}. Select option 1, 2, or 3: ");
  foodChoice = Convert.ToInt32(Console.ReadLine());

  if (foodChoice == 1)
  {
    soup.Food = FoodType.Soup;
  }
  else if (foodChoice == 2)
  {
    soup.Food = FoodType.Stew;
  }
  else if (foodChoice == 3)
  {
    soup.Food = FoodType.Gumbo;
  }
  else
  {
    Console.WriteLine("Choose a number between 1-3 you dummy!");
  }
}

int ingredientChoice = -1;
while ((ingredientChoice < 0) || (ingredientChoice > 4))
{
  Console.Write($"Main Ingredient Options Are: {MainIngredient.Mushrooms.ToString()}, " +
    $"{MainIngredient.Chicken.ToString()}, {MainIngredient.Carrots.ToString()}, or " +
    $"{MainIngredient.Potatoes.ToString()}. Select option 1, 2, 3, or 4: ");
  ingredientChoice = Convert.ToInt32(Console.ReadLine());

  if (ingredientChoice == 1)
  {
    soup.Main = MainIngredient.Mushrooms;
  }
  else if (ingredientChoice == 2)
  {
    soup.Main = MainIngredient.Chicken;
  }
  else if (ingredientChoice == 3)
  {
    soup.Main = MainIngredient.Carrots;
  }
  else if (ingredientChoice == 4)
  {
    soup.Main = MainIngredient.Potatoes;
  }else
  {
    Console.WriteLine("Choose a number between 1-4 you dummy!");
  }
}


int seasonChoice = -1;
while ((seasonChoice < 0) || (seasonChoice > 3))
{
  Console.Write($"Seasoning Options Are: {Seasoning.Spicy.ToString()}, " +
    $"{Seasoning.Sweet.ToString()}, or {Seasoning.Salty.ToString()}, " +
    $"Select option 1, 2, or 3: ");
  seasonChoice = Convert.ToInt32(Console.ReadLine());

  if (seasonChoice == 1)
  {
    soup.Season = Seasoning.Spicy;
  }
  else if (seasonChoice == 2)
  {
    soup.Season = Seasoning.Sweet;
  }
  else if (seasonChoice == 3)
  {
    soup.Season = Seasoning.Salty;
  }else
  {
    Console.WriteLine("Choose a number between 1-3 you dummy!");
  }
}



Console.WriteLine(soup);




enum FoodType { Soup, Stew, Gumbo }
enum MainIngredient { Mushrooms, Chicken, Carrots, Potatoes }
enum Seasoning { Spicy, Sweet, Salty }