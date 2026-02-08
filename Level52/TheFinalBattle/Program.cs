// See https://aka.ms/new-console-template for more information
﻿using CSharpPlayersGuide.RichConsole;
using System.Reflection.Emit;
using System.Text;


FinalBattle game = new FinalBattle();
game.RunGame();


public class FinalBattle
{
  public Character[] heroes;
  public Character[][] villains;
  public Character[] currentWave;
  public int waveIndex;
  public int finalWave;
  public ICommand currentCommand;
  public int TurnCounter { get; private set; }
  public MenuItem[] menu;
  //do we need some sort of ID of who's turn it is?

  public FinalBattle()
  {
    string? heroName = "";
    while (true)
    {
      RichConsole.Write("What is the true programmer's name? ");
      heroName = RichConsole.ReadLine();
      if (heroName != null && heroName != "")
        break;
    }

    heroName = heroName.ToUpper().Trim();
    heroes = new Character[]
    {
      new Player(heroName, 25, 25, true)
    };

    waveIndex = 0;
    finalWave = 2;
    villains = new Character[finalWave + 1][];
    villains[0] = new[] { new Skeleton(5, 5, false) };
    villains[1] = new[] { new Skeleton(5, 5, false), new Skeleton(5, 5, false) };
    villains[2] = new[] { new UncodedOne(15, 15, false) };
    currentWave = villains[waveIndex];
    currentCommand = new NoCommand();
    menu = new MenuItem[]
    {
      new MenuItem("", false, currentCommand),
    };
    TurnCounter = 1;
  }

  public void RunGame()
  {
    while(true)
    {
      Thread.Sleep(500);

      //print who's turn it is,

      IEnumerable<Character> aliveHeroes = from h in heroes
                                where h.IsAlive
                                select h;
      foreach (Character c in aliveHeroes)
      {
        //RichConsole.WriteLine($"It is {c.Name}'s turn..."); //replace with status function
        PrintStatus(c);
        if (c.HumanControlled)
          currentCommand = GetCommand(c);
        else
          currentCommand = new NoCommand();

        currentCommand.Execute(c);
      }

      IEnumerable<Character> aliveVillains = from v in currentWave
                                             where v.IsAlive
                                             select v;
      //print who's turn it is,
      foreach (Character c in aliveVillains)
      {
        //RichConsole.WriteLine($"It is {c.Name}'s turn...");
        PrintStatus(c);
        if (c.HumanControlled)
          currentCommand = GetCommand(c);
        else
          currentCommand = c.AttackList[0] switch
          {
            AttackType.BoneCrunch => new BoneCrunchAttack(heroes[0], c), //need to check for aliveHeroes if we add a party..
            AttackType.Unravel    => new UnravelAttack(heroes[0], c),
                                _ => new NoAttack(heroes[0], c)
          };

        currentCommand.Execute(c);
        Thread.Sleep(500);
      }

      //check for win or loss
      WinStatus winner = CheckForWin();
      if (winner != WinStatus.NoWinner)
        return;

      TurnCounter++;
    }

  }

  //print out the allies and enemies list with each of their health. For the current character, put them first and highlight their name in yellow
  public void PrintStatus(Character c)
  {
    Character[] allies = GetPartyFor(c);
    Character[] enemies = GetEnemyPartyFor(c);
    RichConsole.WriteLine("==========================================================================");
    RichConsole.WriteLine($"{c.Name}        ( {c.HP}/{c.MaxHP} )", Colors.Aquamarine);
    foreach(Character a in allies)
      RichConsole.WriteLine($"{a.Name}        ( {a.HP}/{a.MaxHP} )", Colors.BlueViolet);
    
    RichConsole.WriteLine("--------------------------------- VS -------------------------------------");
    foreach(Character e in enemies)
      RichConsole.WriteLine($"                                                    {e.Name}  ( {e.HP}/{e.MaxHP} )", Colors.IndianRed);
    

    RichConsole.WriteLine("==========================================================================");

  }

  public ICommand GetCommand(Character c)
  {
    RichConsole.WriteLine($"What would you like {c.Name} to do? ");
    string? playerCommandString = "";
    int playerCommand = -1;
    bool validCommand = false;

    //action stage
    MenuBuilder(MenuStage.ActionStage, c, c);
    PrintMenu();

    //get intial command type, for now either attack (0) or do nothing (1)
    while (true)
    {
      playerCommandString = RichConsole.ReadLine();
      validCommand = Int32.TryParse(playerCommandString, out playerCommand); //check if command is a #
      if (validCommand)
      {
        validCommand = validCommand && playerCommand >= 0 && playerCommand < this.menu.Length; //check if command is in range
        validCommand = validCommand && this.menu[playerCommand].isEnabled; //check if command is enabled
      }
      if (validCommand)
        break;
      else
        RichConsole.WriteLine("Enter something valid next time!");
    }

    switch (playerCommand)
    {
      case 0:
        return GetAttackCommand(c); // attack stage
      case 1:
        return new NoCommand();
      case 2: // need to rework later to add items
      default:
        return new NoCommand();
    }
  }

  public void MenuBuilder(MenuStage stage, Character target, Character attacker)
  {

    switch (stage)
    {
      case MenuStage.ActionStage: //I maybe need to limit this based on if they have items or not? Like set the use item to false if no items, or not even add to the menu if no items?
        this.menu = new MenuItem[3]
          {
            new MenuItem("Attack", true, new NoCommand()),
            new MenuItem("Do Nothing", true, new NoCommand()),
            new MenuItem("Use Item", false, new NoCommand())
          };
        break;

      case MenuStage.TargetStage: // need to get all the targets here
        Character[] badGuys = GetEnemyPartyFor(attacker);
        this.menu = new MenuItem[badGuys.Length];
        int i = 0;
        foreach (Character badGuy in badGuys)
        {
          this.menu[i] = new MenuItem($"{badGuy.Name}", badGuy.IsAlive, new NoAttack(badGuy, attacker));
          i++;
        }
        break;

      case MenuStage.AttackTypeStage:
        AttackType[] attackList = attacker.AttackList;
        this.menu = new MenuItem[attackList.Length];
        int j = 0;
        foreach (AttackType attack in attackList)
        {
          switch (attack)
          {
            case AttackType.BoneCrunch: 
              this.menu[j] = new MenuItem($"Bone Crunch", true, new BoneCrunchAttack(target, attacker));
              break;
            case AttackType.Punch:
              this.menu[j] = new MenuItem($"Punch", true, new PunchAttack(target, attacker));
              break;
            case AttackType.Unravel:
              this.menu[j] = new MenuItem($"Unravel", true, new UnravelAttack(target, attacker));
              break;
            default:
              break;
          }
          j++;
        }
        break;

      default:
        break;
    }
  }

  public void PrintMenu()
  {
    int i = 0;
    foreach (MenuItem item in this.menu)
    {
      if (item.isEnabled)
      {
        RichConsole.WriteLine($"{i} - {this.menu[i].Description}");
        i++;
      }
    }
  }

  public AttackCommand GetAttackCommand(Character c)
  {
    Character[] badGuys = GetEnemyPartyFor(c);
    Character target;
    AttackCommand attack = new NoAttack(c, c);

    //attack stage
    MenuBuilder(MenuStage.TargetStage, c, c);
    RichConsole.WriteLine("Who do you wish to attack? Your options are as follows:");
    PrintMenu();

    target = GetAttackTarget(badGuys);

    //MenuStage.AttackTypeStage
    MenuBuilder(MenuStage.AttackTypeStage, target, c);
    RichConsole.WriteLine("What attack do you wish to use? Your options are as follows:");
    PrintMenu();
    attack = GetAttack(c);

    return attack;
  }

  public Character GetAttackTarget(Character[] enemyList)
  {
    Character target;
    int badGuyID = -1;
    bool validBadGuy = false;


    //get the integer position of the bad guy
    while (validBadGuy == false)
    {
      /* loop through enemy team to choose a target
      foreach (Character e in enemyList)
      {
        RichConsole.WriteLine($"{e.Name}: Press #{currentBadGuy}");
        currentBadGuy++;
      }
      */

      string badGuyIDString = RichConsole.ReadLine();
      validBadGuy = Int32.TryParse(badGuyIDString, out badGuyID);
      validBadGuy = validBadGuy && (badGuyID < this.menu.Length) && (badGuyID >= 0); //in valid range
      if (validBadGuy)
      {
        validBadGuy = validBadGuy && this.menu[badGuyID].isEnabled; // check if bad guy is alive
      }

      if (!validBadGuy)
      {
        RichConsole.WriteLine("Not a valid choice, try again!");
      }
    }

    target = enemyList[badGuyID];

    return target;
  }

  //TODO can I make this code shared with GetAttackTarget? Yes, using "generic methods" page 237 I think
  public AttackCommand GetAttack(Character c)
  {

    int attackID = -1;
    bool validAttack = false;

    while (validAttack == false)
    {
      /* TAKEN CARE OF BY MENU AND PRINT MENU
      foreach (AttackType a in c.AttackList)
      {
        attackString = a switch
        {
          AttackType.BoneCrunch => "Bone Crunch",
          AttackType.Punch => "Punch",
          AttackType.Unravel => "Unravel",
          _ => "Not a valid attack"
        };
        RichConsole.WriteLine($"{attackString}: Press #{currentAttack}");
        currentAttack++;
      }
      */

      string attackIDstring = RichConsole.ReadLine();
      validAttack = Int32.TryParse(attackIDstring, out attackID);
      validAttack = validAttack && (attackID < this.menu.Length) && (attackID >= 0); //in valid range

      if (validAttack)
      {
        validAttack = validAttack && this.menu[attackID].isEnabled; // check if attack is enabled, which it should be if it's in the attack list
      }

      if (!validAttack)
      {
        RichConsole.WriteLine("Not a valid choice, try again!");
      }
    }

    return (AttackCommand) menu[attackID].Action;

  }

  public Character[] GetPartyFor(Character c)
  {
    Character[] party;
    IEnumerable<Character> partyEnumerable;
    if (c.IsHero)
      partyEnumerable = from p in this.heroes
                        where p.IsAlive
                        where p != c
                        select p;
    else
      partyEnumerable = from p in this.villains[waveIndex]
                        where p.IsAlive
                        where p!= c
                        select p;

    party = partyEnumerable.ToArray();
    return party;
  }

  public Character[] GetEnemyPartyFor(Character c)
  {
    Character[] party;
    IEnumerable<Character> partyEnumerable;
    if (c.IsHero)
      partyEnumerable = from p in this.villains[waveIndex]
                        where p.IsAlive
                        select p;

    else
      partyEnumerable = from p in this.heroes
                        where p.IsAlive
                        select p;

    party = partyEnumerable.ToArray();
    return party;
  }

  public WinStatus CheckForWin()
  {

    IEnumerable<Character> aliveHeroes = from h in heroes
                                         where h.IsAlive
                                         select h;

    IEnumerable<Character> aliveVillains = from v in currentWave
                                           where v.IsAlive
                                           select v;
    if (!aliveHeroes.Any() || aliveHeroes == null)
    {
      RichConsole.WriteLine("The heroes have lost.. The Uncoded One's forces have prevailed.");
      return WinStatus.VillainsWon;
    }
    else if (!aliveVillains.Any() || aliveVillains == null)
    {
      if(waveIndex == finalWave)
      {
        RichConsole.WriteLine("The heroes have won! The Uncoded one was defeated!");
        return WinStatus.HeroesWon;
      }
      else
      {
        RichConsole.WriteLine("The heroes have defeated the current wave of bad guys, but the Uncoded One is rallying more forces!");
        waveIndex++;
        currentWave = villains[waveIndex];
        return WinStatus.NoWinner;
      }
    }
    else
      return WinStatus.NoWinner;

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
  public AttackType[] AttackList { get; protected set;  } 

  public Character(string name, int maxHP, int startingHP, bool humanControlled, bool isHero, AttackType[] attacks)
  {
    Name = name;
    MaxHP = maxHP;
    HP = startingHP;
    IsAlive = true;
    HumanControlled = humanControlled;
    IsHero = isHero;
    AttackList = attacks;
  }
}

public class Player : Character
{
  public Player(string name, int maxHP, int startingHP, bool humanControlled) :
    base(name, maxHP, startingHP, humanControlled, true, new AttackType[] { AttackType.Punch })
  {}
}

public class Skeleton : Character
{
  public Skeleton(int maxHP, int startingHP, bool humanControlled, string name = "SKELETON") :
   base(name, maxHP, startingHP, humanControlled, false, new AttackType[] { AttackType.BoneCrunch })
  { }
}

public class UncodedOne : Character
{
  public UncodedOne(int maxHP, int startingHP, bool humanControlled, string name = "THE UNCODED ONE"):
    base(name, maxHP, startingHP, humanControlled, false, new AttackType[] { AttackType.Unravel }){ }
}

public interface ICommand 
{ 
  public void Execute(Character character); //need to take in the game object I thinK? Need to know the state of who's turn it is
  public virtual void Display(Character character){ } //TODO remove character from the display command, not needed. Make Display into one method with variable # of inputs but passing in optional args?
  public virtual void Display(int AttackDamage, bool Defeated) { }
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
  public Character Target { get; private set; }
  public Character Attacker { get; private set; }
  public AttackData AttackData { get; private set; } 
  public AttackType Attack { get; private set; }
  public AttackCommand(Character target, Character attacker, AttackType type, AttackData attackData)
  {
    Target = target; 
    Attacker = attacker;
    Attack = type;
    AttackData = attackData; // maybe not relevant for this base class, could make this base class more abstract?
  }

  //TODO strip character? I already have target and attacker stored in the class?
  public void Execute(Character character) {
    Random myRand = new Random();
    int attackDamage = 0;
    bool defeated = false;

    //calculate attack damage, special case for punch attack with 100% hit chance
    if (AttackData.AttackFrequency == 1) // not sure it's wise to do it like this...
      attackDamage = 1;
    else 
      attackDamage = myRand.Next(AttackData.AttackFrequency);

    //deal the damage, could be 0
    Target.HP = Math.Clamp(Target.HP - attackDamage, 0, Target.HP);

    //kill target
    if(Target.HP == 0)
    {
      Target.IsAlive = false;
      defeated = true;
    }
    //calling display within execute is cleaner I think...
    Display(attackDamage, defeated);
  }

  public void Display(int AttackDamage, bool Defeated) {

    string attackName = Attack switch
    {
      AttackType.Punch => "PUNCH",
      AttackType.BoneCrunch => "BONE CRUNCH",
      AttackType.Unravel => "UNRAVEL",
      _ => "INVALID ATTACK"
    };

    //display who used the attack, how much damage it dealt, and update the console about the target's current HP
    RichConsole.WriteLine($"{Attacker.Name} used {attackName} on {Target.Name}");
    RichConsole.WriteLine($"{attackName} dealt {AttackDamage} damage to {Target.Name}.");
    RichConsole.WriteLine($"{Target.Name} is now at {Target.HP}/{Target.MaxHP} HP.");
    if (Defeated)
      RichConsole.WriteLine($"{Target.Name} has been defeated!");
    RichConsole.WriteLine();
  }
}

//Not expecting to use this class except for default cases
public class NoAttack : AttackCommand
{
  public NoAttack(Character target, Character attacker) : base(target, attacker, AttackType.NoAttack, new AttackData(0,0,DamageType.NoType)) { }
}

public class PunchAttack : AttackCommand
{
  public PunchAttack(Character target, Character attacker) : base(target, attacker, AttackType.Punch, new AttackData(1, 1, DamageType.Physical)) { }

}
public class BoneCrunchAttack : AttackCommand
{
  public BoneCrunchAttack(Character target, Character attacker): base(target, attacker, AttackType.BoneCrunch, new AttackData(1, 2, DamageType.Physical)) { }
}

public class UnravelAttack : AttackCommand
{
  public UnravelAttack(Character target, Character attacker): base(target, attacker, AttackType.Unravel, new AttackData(2, 3, DamageType.Physical)) { }
}

//maybe this AttackData should contain the attack type, as well as the amount of damage etc.?
public class AttackData
{
  public int AttackDamage { get; private set; }
  public int AttackFrequency { get; private set; } // this is the max range fed into rand command
  public DamageType Type { get; private set; }

  public AttackData(int damage, int frequency, DamageType type)
  {
    AttackDamage = damage;
    AttackFrequency = frequency;
    Type = type;
  }

}

public record MenuItem(string Description, bool isEnabled, ICommand Action);

public enum AttackType { NoAttack, Punch, BoneCrunch, Unravel}
public enum DamageType { NoType, Physical}
public enum WinStatus { NoWinner, HeroesWon, VillainsWon}
public enum MenuStage { ActionStage, TargetStage, AttackTypeStage}