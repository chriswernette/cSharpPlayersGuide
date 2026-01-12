// See https://aka.ms/new-console-template for more information
using System.ComponentModel.Design;

Chest myChest =Chest.Open;

while (true)
{
  Console.Write("The chest is " + myChest.ToString() + ". What do you want to do? ");
  string command = Console.ReadLine();
  if ((myChest == Chest.Open) && (command == "close"))
  {
    myChest = Chest.Closed;
  }
  else if ((myChest == Chest.Closed) && (command == "lock"))
  {
    myChest = Chest.Locked;
  } 
  else if ((myChest == Chest.Closed) && (command == "open")){
    myChest = Chest.Open;
  }
  else if ((myChest == Chest.Locked) && (command == "unlock"))
  {
    myChest = Chest.Closed;
  }
}

enum Chest { Open, Closed, Locked }