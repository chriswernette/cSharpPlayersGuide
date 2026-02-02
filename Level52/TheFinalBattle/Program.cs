// See https://aka.ms/new-console-template for more information
﻿using CSharpPlayersGuide.RichConsole;

FinalBattle game = new FinalBattle();
game.RunGame();


public class FinalBattle
{
  public Character[] heroes;
  public Character[] villains;
  public ICommand currentCommand;
  public int TurnCounter { get; private set; }
  //do we need some sort of ID of who's turn it is?

  public FinalBattle()
  {
    heroes = new Character[]
    {
      new Character("SKELETON", 10, 5)
    };

    villains = new Character[]
    {
      new Character("SKELETON", 10, 5)
    };
    currentCommand = new NoCommand();
    TurnCounter = 1;
  }

  public void RunGame()
  {
    while(true)
    {
      Thread.Sleep(500);

      //print who's turn it is,
      foreach (Character c in heroes)
      {
        RichConsole.WriteLine($"It is {c.Name}'s turn..."); 
        currentCommand = new NoCommand();
        currentCommand.Execute(c);
        currentCommand.Display(c);
      }
      RichConsole.WriteLine();
      //print who's turn it is,
      foreach (Character c in villains)
      {
        RichConsole.WriteLine($"It is {c.Name}'s turn...");
        currentCommand = new NoCommand();
        currentCommand.Execute(c);
        currentCommand.Display(c);
      }
      RichConsole.WriteLine();

      TurnCounter++;
    }

    return;
  }

}

public class Character
{
  public string Name { get; protected set; }
  public int MaxHP { get; protected set; }
  public int HP { get; set; }
  public bool IsAlive { get; set; }

  public Character(string name, int maxHP, int startingHP)
  {
    Name = name;
    MaxHP = maxHP;
    HP = startingHP;
    IsAlive = true;
  }
}

public interface ICommand 
{ 
  void Execute(Character character); //need to take in the game object I thinK? Need to know the state of who's turn it is
  void Display(Character character);
}

public class NoCommand : ICommand
{
  public void Execute(Character character)
  {
    return;
  }
  public void Display(Character character)
  {
    RichConsole.WriteLine($"{character.Name} did NOTHING."); 
  }
}