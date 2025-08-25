// See https://aka.ms/new-console-template for more information
Console.Write("What is your name? ");
string yourName = Console.ReadLine();
float discount = 0;
if (yourName == "Chris")
{
    discount = .5F;
} else
{
    discount = 1;
}

Console.Write("What number do you want to see the price of? ");
int selection = Convert.ToInt32(Console.ReadLine());

float ropeCost = 20;
float torchCost = 15;
float climbCost = 25;
float waterCost = 1;
float macheteCost = 20;
float canoeCost = 200;
float foodCost = 1;

switch(selection)
{
  case 1:
      Console.WriteLine($"Rope costs {ropeCost*discount} gold.");
      break;
  case 2:
      Console.WriteLine($"Torches cost {torchCost*discount} gold.");
      break;
  case 3:
      Console.WriteLine($"Climbing Equipment costs {climbCost*discount} gold.");
      break;
  case 4:
      Console.WriteLine($"Clean water costs {waterCost*discount} gold.");
      break;
  case 5:
      Console.WriteLine($"A machete costs {macheteCost*discount} gold.");
      break;
  case 6:
      Console.WriteLine($"A canoe costs {canoeCost*discount} gold!");
      break;
  case 7:
      Console.WriteLine($"Food supplies cost {foodCost*discount} gold.");
      break;
  default:
      Console.WriteLine("That is not something the shop sells!");
      break;
}

