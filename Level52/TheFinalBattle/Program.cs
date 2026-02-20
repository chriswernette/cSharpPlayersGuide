// See https://aka.ms/new-console-template for more information
﻿using CSharpPlayersGuide.RichConsole;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;


FinalBattle game = new FinalBattle();
game.RunGame();


public class FinalBattle
{
  public Character[] heroes;
  public Item[] heroItems;
  public Character[][] villains;
  public Item[][] villainItems;
  public Character[] currentWave;
  private int _heroHP = 25;
  public int waveIndex;
  public int finalWave;
  public ICommand currentCommand;
  public int TurnCounter { get; private set; }
  public MenuItem[] menu;
  ComputerControlledOption computerControlledEnum = ComputerControlledOption.ComputerVsComputer;
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


    string computerControlled = "1"; // default to comp vs comp
    int computerControlledInt = 1;
    while (true)
    {
      RichConsole.WriteLine("What game setup would you like to play?");
      RichConsole.WriteLine("0 - human (good guys) vs computer (villains)");
      RichConsole.WriteLine("1 - computer vs computer");
      RichConsole.WriteLine("2 - human vs human");
      computerControlled = RichConsole.ReadLine();

      if (computerControlled != null && computerControlled != "")
      {
        if (Int32.TryParse(computerControlled, out computerControlledInt))
        {
          computerControlledEnum = (ComputerControlledOption)computerControlledInt;
          if (Enum.IsDefined(typeof(ComputerControlledOption), computerControlledEnum))
            break;
        }
      }
    }

    bool heroControlled = computerControlledEnum == ComputerControlledOption.HumanVsHuman || computerControlledEnum == ComputerControlledOption.HumanVsComputer;
    bool villainControlled = computerControlledEnum == ComputerControlledOption.HumanVsHuman;

    heroName = heroName.ToUpper().Trim();
    heroes = new Character[]
    {
      new Player(heroControlled, heroName, _heroHP, _heroHP)
    };
    heroItems = new Item[]
    {
      new HealthPotion(),
      new HealthPotion(),
      new HealthPotion()
    };

    waveIndex = 0;
    finalWave = 2;

    villains = new Character[finalWave + 1][];
    villains[0] = new[] { new Skeleton(villainControlled) };
    villains[1] = new[] { new Skeleton(villainControlled), new Skeleton(villainControlled) };
    villains[2] = new[] { new UncodedOne(villainControlled) };

    villainItems = new Item[finalWave + 1][];
    villainItems[0] = new[] { new HealthPotion() };
    villainItems[1] = new[] { new HealthPotion() };
    villainItems[2] = new[] { new HealthPotion() };

    currentWave = villains[waveIndex];
    currentCommand = new NoCommand();
    menu = new MenuItem[]
    {
      new MenuItem("", currentCommand, false),
    };
    TurnCounter = 1;
  }

  public void RunGame()
  {
    while (true)
    {
      Thread.Sleep(500);

      //print who's turn it is,

      IEnumerable<Character> aliveHeroes = from h in heroes
                                           where h.IsAlive
                                           select h;

      IEnumerable<Item> availableHeroItems = from i in heroItems
                                             where i.isUsed == false
                                             select i;

      IEnumerable<Character> aliveVillains = from v in currentWave
                                                where v.IsAlive
                                                select v;

      IEnumerable<Item> availableVillainItems = from i in villainItems[waveIndex]
                                                where i.isUsed == false
                                                select i;

      bool usePotion;

      //TODO this foreach and the foreach below could ber wrapped into a function
      foreach (Character c in aliveHeroes)
      {
        PrintStatus(c);
        if (c.HumanControlled)
          currentCommand = GetCommand(c);
        else
        {
          usePotion = ComputerUseHealthPotion(c, availableHeroItems.ToArray());
          if (usePotion)
          {
            currentCommand = new ItemCommand(c, c, availableHeroItems.ToArray()[0]);
          }
          else //TODO make more generic, don't have to use specific attack types
          {
            currentCommand = c.AttackList[0] switch
            {
              AttackType.Punch => new PunchAttack(aliveVillains.ToArray()[0], c),
              _ => new NoAttack(heroes[0], c)
            };
          }
        }
        currentCommand.Execute(c);
      }

      
      //print who's turn it is,
      foreach (Character c in aliveVillains)
      {
        PrintStatus(c);
        if (c.HumanControlled)
          currentCommand = GetCommand(c);
        else
        {
          usePotion = ComputerUseHealthPotion(c, availableVillainItems.ToArray());
          if (usePotion)
          {
            currentCommand = new ItemCommand(c, c, availableVillainItems.ToArray()[0]);
          }
          else
          {
            currentCommand = c.AttackList[0] switch
            {
              AttackType.BoneCrunch => new BoneCrunchAttack(heroes[0], c), //need to check for aliveHeroes if we add a party..
              AttackType.Unravel => new UnravelAttack(heroes[0], c),
              _ => new NoAttack(heroes[0], c)
            };
          }
        }

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
    Item[] itemList;
    IEnumerable<Item> itemOptionsEnumerable;

    if (c.IsHero)
    {
      itemOptionsEnumerable = from h in heroItems
                              where h.isUsed == false
                              select h;
    }
    else
    {
      itemOptionsEnumerable = from v in villainItems[waveIndex]
                              where v.isUsed == false
                              select v;
    }
    itemList = itemOptionsEnumerable.ToArray();


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

    playerCommandString = this.menu[playerCommand].Description;

    switch (playerCommandString)
    {
      case "Attack":
        return GetAttackCommand(c); // attack stage
      case "Use Item":
        return GetItemCommand(c,itemList); // item stage
      case "Do Nothing": // need to rework later to add items
      default:
        return new NoCommand();
    }
  }

  public void MenuBuilder(MenuStage stage, Character target, Character attacker)
  {
    bool itemsAvailable = false;
    IEnumerable<Item> hasItemsChecker;


    switch (stage)
      {
        case MenuStage.ActionStage: //I maybe need to limit this based on if they have items or not? Like set the use item to false if no items, or not even add to the menu if no items?
          if (attacker.IsHero)
            hasItemsChecker = from h in heroItems
                              where h.isUsed == false
                              select h;
          else
            hasItemsChecker = from v in villainItems[waveIndex]
                              where v.isUsed == false
                              select v;

          itemsAvailable = hasItemsChecker.Any(); // check if any items left  

          if (itemsAvailable)
            this.menu = new MenuItem[3]
              {
              new MenuItem("Attack", new NoCommand()),
              new MenuItem("Use Item", new NoCommand()),
              new MenuItem("Do Nothing", new NoCommand())
              };
          else
            this.menu = new MenuItem[2]
              {
              new MenuItem("Attack", new NoCommand()),
              new MenuItem("Do Nothing", new NoCommand())
              };
          break;

        case MenuStage.TargetStage: // need to get all the targets here
          Character[] badGuys = GetEnemyPartyFor(attacker);
          this.menu = new MenuItem[badGuys.Length];
          int i = 0;
          foreach (Character badGuy in badGuys)
          {
            this.menu[i] = new MenuItem($"{badGuy.Name}", new NoAttack(badGuy, attacker), badGuy.IsAlive);
            i++;
          }
          break;

        case MenuStage.ItemStage:
          Item[] itemOptions;
          IEnumerable<Item> itemOptionsEnumerable;
          if (attacker.IsHero)
          {
            itemOptionsEnumerable = from h in heroItems
                                    where h.isUsed == false
                                    select h;
            itemOptions = itemOptionsEnumerable.ToArray();
          }
          else
          {
            itemOptionsEnumerable = from v in villainItems[waveIndex]
                                    where v.isUsed == false
                                    select v;
            itemOptions = itemOptionsEnumerable.ToArray();
          }

          this.menu = new MenuItem[itemOptions.Length];
          i = 0;
          foreach (Item item in itemOptions)
          {
            this.menu[i] = new MenuItem($"{item.itemName}", new ItemCommand(attacker, attacker, item), item.isUsed == false);
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
                this.menu[j] = new MenuItem($"Bone Crunch", new BoneCrunchAttack(target, attacker));
                break;
              case AttackType.Punch:
                this.menu[j] = new MenuItem($"Punch", new PunchAttack(target, attacker));
                break;
              case AttackType.Unravel:
                this.menu[j] = new MenuItem($"Unravel", new UnravelAttack(target, attacker));
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

  public ItemCommand GetItemCommand(Character c, Item[] itemList) // itemlist will have to be the filtered list of items that aren't used
  {
    Character[] badGuys = GetEnemyPartyFor(c);
    Character[] goodGuys = GetPartyForIncludingCharacter(c);
    Character target;
    ItemCommand itemCommand;
    string itemIDString = "";
    int itemID = 0;
    bool validItemChoice = false;

    //item stage
    MenuBuilder(MenuStage.ItemStage, c, c);
    RichConsole.WriteLine("What item do you wish to use? Your options are as follows:");
    PrintMenu();

    //this should be a function, copy and pasting this code around instead of reusing!
    //figure out what item out of the list of items they want to use
    while (validItemChoice == false)
    {
      itemIDString = RichConsole.ReadLine();
      validItemChoice = Int32.TryParse(itemIDString, out itemID);
      validItemChoice = validItemChoice && (itemID < this.menu.Length) && (itemID >= 0); //in valid range
      if (validItemChoice)
      {
        validItemChoice = validItemChoice && this.menu[itemID].isEnabled; // check if bad guy is alive
      }

      if (!validItemChoice)
      {
        RichConsole.WriteLine("Not a valid choice, try again!");
      }
    }

    Item itemChoice = itemList[itemID];

    //get all the characters in c's party if it's a help item, all characters in enemy party if it's an offensive item
    RichConsole.WriteLine("Who do you want to use the item on?");
    if (itemChoice.Type == ItemType.HelpItem)
    {
      MenuBuilder(MenuStage.TargetStage, badGuys[0], badGuys[0]);
      PrintMenu();
      target = GetAttackTarget(goodGuys);
    }
    else
    {
      MenuBuilder(MenuStage.TargetStage, goodGuys[0], goodGuys[0]);
      PrintMenu();
      target = GetAttackTarget(badGuys);
    }

    itemCommand = new ItemCommand(c, target, itemChoice);
    return itemCommand;
  }

  //rename or make more generic. It is just getting a list of characters to attack or use an item on, not necessarily to attack
  public Character GetAttackTarget(Character[] enemyList)
  {
    Character target;
    int badGuyID = -1;
    bool validBadGuy = false;


    //get the integer position of the bad guy
    while (validBadGuy == false)
    {

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

  public Character[] GetPartyForIncludingCharacter(Character c)
  {
    Character[] party;
    IEnumerable<Character> partyEnumerable;
    if (c.IsHero)
      partyEnumerable = from p in this.heroes
                        where p.IsAlive
                        select p;
    else
      partyEnumerable = from p in this.villains[waveIndex]
                        where p.IsAlive
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

  //when the character c is < 50% and there is a potion in their inventory
  public bool ComputerUseHealthPotion(Character c, Item[] ItemList)
  {
    bool usePotion = false;
    bool potionInItemList = false;
    bool lessThanFifty = false;
    float frequency = 0.25F;
    int period = (int) (1 / frequency); // should equal 4
    int threshold = 1; //only 0 will be chosen
    int randomRoll = -1;
    Random potionRand = new Random();

    //check if there is a potion available in inventory
    foreach (Item i in ItemList)
    {
      if (i.isUsed == false)
      {
        if(i.itemName == "Health Potion")
        {
          potionInItemList = true;
        }
      }
    }
    //check if less than 50% HP
    if ((float)(c.HP) < (float)c.MaxHP / 2.0F)
    {
      lessThanFifty = true;
    }

    if (potionInItemList && lessThanFifty)
    {
      //dice roll 25%
      randomRoll = potionRand.Next(0, period);
      if (randomRoll < threshold)
      {
        usePotion = true;
      }
    }

    return usePotion;
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


public enum AttackType { NoAttack, Punch, BoneCrunch, Unravel}
public enum DamageType { NoType, Physical}
public enum WinStatus { NoWinner, HeroesWon, VillainsWon}
public enum MenuStage { ActionStage, TargetStage, AttackTypeStage, ItemStage}
public enum ComputerControlledOption { HumanVsComputer, ComputerVsComputer, HumanVsHuman}