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
    string? heroName = "";
    while (true)
    {
      RichConsole.Write("What is the true programmer's name? ");
      heroName = RichConsole.ReadLine();
      if(heroName != null && heroName != "")
        break;
    }
    
    heroName = heroName.ToUpper().Trim();
    heroes = new Character[]
    {
      new Character(heroName, 10, 5, true, true)
    };

    villains = new Character[]
    {
      new Character("SKELETON", 10, 5, false, false)
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
        if (c.HumanControlled)
          currentCommand = GetCommand(c);
        else
          currentCommand = new NoCommand();

        currentCommand.Execute(c);
        currentCommand.Display(c);
      }
      RichConsole.WriteLine();
      //print who's turn it is,
      foreach (Character c in villains)
      {
        RichConsole.WriteLine($"It is {c.Name}'s turn...");
        if (c.HumanControlled)
          currentCommand = GetCommand(c);
        else
          currentCommand = new NoCommand();

        currentCommand.Execute(c);
        currentCommand.Display(c);
      }
      RichConsole.WriteLine();

      TurnCounter++;
    }

    return;
  }

  public ICommand GetCommand(Character c)
  {
    RichConsole.Write($"What would you like {c.Name} to do? ");
    string? playerCommand = "";
    while (true)
    {
      playerCommand = RichConsole.ReadLine();
      if (playerCommand != null && playerCommand != "")
        break;
      else
        RichConsole.Write("Enter something this time! ");
    }
    playerCommand = playerCommand.Trim().ToLower();

    switch (playerCommand)
    {
      case "attack":
      case "use item":
      case "do nothing":
        return new NoCommand();
      default:
        return new NoCommand();
        break;
    }
  }

  public Character[] GetPartyFor(Character c)
  {
    if (c.IsHero)
      return this.heroes;
    else
      return this.villains;
  }

  public Character[] GetEnemyPartyFor(Character c)
  {
    if (c.IsHero)
      return this.villains;
    else
      return this.heroes;
  }
}

public class Character
{
  public string Name { get; protected set; }
  public int MaxHP { get; protected set; }
  public int HP { get; set; }
  public bool IsAlive { get; set; }
  public bool HumanControlled { get; protected set; }
  public bool IsHero {  get; protected set; }

  public Character(string name, int maxHP, int startingHP, bool humanControlled, bool isHero)
  {
    Name = name;
    MaxHP = maxHP;
    HP = startingHP;
    IsAlive = true;
    HumanControlled = humanControlled;
    IsHero = isHero;
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

public class AttackCommand : ICommand
{
  public AttackCommand()
  {

  }

  public void Execute(Character character) {
  }

  public void Display(Character character) {
  }
}

enum AttackType { Punch, BoneCrunch}