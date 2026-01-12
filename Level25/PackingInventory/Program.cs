// See https://aka.ms/new-console-template for more information


int userItems;
float userWeight;
float userVolume;

while (true)
{
  string? input;
  Console.Write("How many items can your pack fit? ");
  input = Console.ReadLine();


  if(int.TryParse(input, out userItems))
  {
    break;
  }
  else
  {
    Console.WriteLine("Not a valid input, must be an even integer. Try again!");
  }
}

while (true)
{
  string? input;
  Console.Write("And how much weight are you willing to carry? ");
  input = Console.ReadLine();


  if (float.TryParse(input, out userWeight))
  {
    break;
  }
  else
  {
    Console.WriteLine("Not a valid input, must be a number. Try again!");
  }
}


while (true)
{
  string? input;
  Console.Write("And now how volumous is your pack? ");
  input = Console.ReadLine();


  if (float.TryParse(input, out userVolume))
  {
    break;
  }
  else
  {
    Console.WriteLine("Not a valid input, must be a number. Try again!");
  }
}


Pack myPack = new Pack(userItems, userWeight, userVolume);

while (true)
{
  string? userSelection;
  Console.WriteLine(myPack.ToString());
  
  Console.Write("What would you like to add to the pack? A = arrow, B = bow, " +
    "R = rope, W = water, F = food rations, S = sword. RS to report the pack specs. " +
    "RC to report the current pack stats. Q to quit packing: ");
  userSelection = Console.ReadLine();

  InventoryItem userItem;

  if(userSelection == null)
  {
    Console.WriteLine("You didn't enter anything! Try again.");
    continue;
  }
  else
  {
    userSelection = userSelection.ToLower();
  }
  
  if (userSelection == "a")
  {
    userItem = new Arrow();
  }
  else if (userSelection == "b")
  {
    userItem = new Bow();
  }
  else if (userSelection == "r")
  {
    userItem = new Rope();
  }
  else if (userSelection == "w")
  {
    userItem = new Water();
  }
  else if (userSelection == "f")
  {
    userItem = new FoodRations();
  }
  else if (userSelection == "s")
  {
    userItem = new Sword();
  }
  else if (userSelection == "q")
  {
    break;
  }
  else if (userSelection == "rs")
  {
    string weightFormatted = string.Format("{0:0.##}", myPack.WeightCarryable);
    string volumeFormatted = string.Format("{0:0.##}", myPack.VolumeFillable);

    Console.WriteLine($"The pack can hold {myPack.NumberOfItemsContainable} items!");
    Console.WriteLine($"The pack can carry " + weightFormatted + " lbs of weight.");
    Console.WriteLine($"The pack can hold " + volumeFormatted + " liters of volume.");
    continue;
  }
  else if (userSelection == "rc")
  {
    string weightFormatted = string.Format("{0:0.##}", myPack.CurrentWeight);
    string volumeFormatted = string.Format("{0:0.##}", myPack.CurrentVolume);

    Console.WriteLine($"The pack is currently holding {myPack.CurrentItems} items!");
    Console.WriteLine("The pack currently weighs " + weightFormatted + " lbs.");
    Console.WriteLine("The pack currently is filled " + volumeFormatted + " liters of volume.");
    continue;
  }
  else
  {
    Console.WriteLine("You made an invalid selection! Try again.");
    continue;
  }
  bool addItemSuccessful;
  addItemSuccessful = myPack.Add(userItem);

  if (!addItemSuccessful)
  {
    Console.WriteLine("The pack is full! Time to call it quits packing...");
  }
}




class Pack(int numberOfItemsContainable, float weightCarryable, float volumeFillable)
{
  public int NumberOfItemsContainable { get; private set; } = numberOfItemsContainable;
  public float WeightCarryable { get; private set; } = weightCarryable;
  public float VolumeFillable { get; private set; } = volumeFillable;
  public InventoryItem[] Inventory {  get; private set; } = new InventoryItem[numberOfItemsContainable];

  public int CurrentItems {  get; private set; } = 0;
  public float CurrentWeight { get; private set; } = 0;
  public float CurrentVolume {  get; private set; } = 0;

  //return false if exceeded #items, weight, or volume
  public bool Add(InventoryItem item)
  {
    if(CurrentItems == NumberOfItemsContainable)
    {
      return false;
    }
    else if((CurrentWeight + item.ItemWeight) >= WeightCarryable)
    {
      return false;
    }
    else if ( (CurrentVolume + item.ItemVolume) >= VolumeFillable)
    {
      return false;
    }
    else
    {
      Inventory[CurrentItems] = item;
      CurrentItems++;
      CurrentWeight += item.ItemWeight;
      CurrentVolume += item.ItemVolume;
      return true;
    }
  }

  public override string ToString()
  {
    string printString;

    if(CurrentItems > 0)
    {
      printString = "Pack containing ";
      for(int i = 0; i < CurrentItems;  i++)
      {
        printString += Inventory[i].ToString();
        printString += " ";
      }
    }
    else
    {
      printString = "Your pack is empty!";
    }
    return printString;
  }


}

public class InventoryItem (float itemWeight, float itemVolume)
{
  public float ItemWeight { get; private set; } = itemWeight;
  public float ItemVolume { get; private set; } = itemVolume;
}

public class Arrow : InventoryItem
{
  public Arrow() : base(.1f, .05f)
  {
  }
  public override string ToString()
  {
    return "Arrow";
  }
}

public class Bow : InventoryItem
{
  public Bow() : base(1f, 4f)
  {
  }
  public override string ToString()
  {
    return "Bow";
  }
}

public class Rope : InventoryItem
{
  public Rope() : base(1f, 1.5f)
  {
  }
  public override string ToString()
  {
    return "Rope";
  }
}

public class Water : InventoryItem
{
  public Water() : base(2f, 3f)
  {
  }
  public override string ToString()
  {
    return "Water";
  }
}

public class FoodRations : InventoryItem
{
  public FoodRations() : base(1f, .5f)
  {
  }
  public override string ToString()
  {
    return "Food Rations";
  }
}

public class Sword : InventoryItem
{
  public Sword() : base(5f, 3f)
  {
  }
  public override string ToString()
  {
    return "Sword";
  }
}