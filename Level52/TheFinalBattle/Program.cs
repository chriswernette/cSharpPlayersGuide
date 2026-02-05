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
      new Player(heroName, 25, 25, true)
    };

    villains = new Character[]
    {
      new Skeleton(5, 5, false)
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

      IEnumerable<Character> aliveHeroes = from h in heroes
                                where h.IsAlive
                                select h;
      foreach (Character c in aliveHeroes)
      {
        RichConsole.WriteLine($"It is {c.Name}'s turn...");
        if (c.HumanControlled)
          currentCommand = GetCommand(c);
        else
          currentCommand = new NoCommand();

        currentCommand.Execute(c);
      }
      RichConsole.WriteLine();

      IEnumerable<Character> aliveVillains = from v in villains
                                             where v.IsAlive
                                             select v;
      //print who's turn it is,
      foreach (Character c in aliveVillains)
      {
        RichConsole.WriteLine($"It is {c.Name}'s turn...");
        if (c.HumanControlled)
          currentCommand = GetCommand(c);
        else
          currentCommand = new BoneCrunchAttack(heroes[0],c); // need to figure out how to make the computer "choose" the attackee and attack type in the future? Using rand?

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
        return GetAttackCommand(c);
      case "use item":
      case "do nothing":
        return new NoCommand();
      default:
        return new NoCommand();
        break;
    }
  }

  public AttackCommand GetAttackCommand(Character c)
  {
    //TODO get list of enemy team
    Character[] badGuys = GetEnemyPartyFor(c);
    Character attackee;
    AttackCommand attack = new NoAttack(c, c);
    AttackType chosenAttack;

    attackee = GetAttackTarget(badGuys);
    chosenAttack = GetAttackType(c);
      
    attack = chosenAttack switch
    {
      AttackType.Punch      => new PunchAttack(attackee, c),
      AttackType.BoneCrunch => new BoneCrunchAttack(attackee, c),
      _                     => new NoAttack(attackee, c)
    };

    return attack;
  }


  //TODO figure out how to make sure the guy chosen here is the one we attack.
  //Will C# pass by reference kinda handle this for me? Or should I make some sort of character ID that is returned?
  public Character GetAttackTarget(Character[] enemyList)
  {
    Character target;
    int currentBadGuy = 0;
    int badGuyID = -1;
    bool validBadGuy = false;

    RichConsole.WriteLine("Who do you wish to attack? Your options are as follows:");

    //get the integer position of the bad guy
    while (validBadGuy == false)
    {
      // loop through enemy team to choose a target
      foreach (Character e in enemyList)
      {
        RichConsole.WriteLine($"{e.Name}: Press #{currentBadGuy}");
        currentBadGuy++;
      }

      string badGuyIDString = RichConsole.ReadLine();
      validBadGuy = Int32.TryParse(badGuyIDString, out badGuyID);
      validBadGuy = validBadGuy && (badGuyID <= currentBadGuy) && (badGuyID >= 0); //in valid range
      
      if (!validBadGuy)
        RichConsole.WriteLine("Not a valid choice, try again!");
    }

    target = enemyList[badGuyID];

    return target;
  }

  //TODO can I make this code shared with ChooseAttackee, it seems very copy-paste except the types are different. Can I make one function with type <T>?
  //The only downside is I use a switch expression for the attack types to translate it from enum to string. Maybe I can wrap that with if(<T>.isType(AttackType))?
  public AttackType GetAttackType(Character c)
  {

    string attackString = "";
    int attackID = -1;
    int currentAttack = 0;
    bool validAttack = false;

    while (validAttack == false)
    {
      RichConsole.WriteLine("What attack do you wish to use? Your options are as follows:");
      foreach (AttackType a in c.AttackList)
      {
        attackString = a switch
        {
          AttackType.BoneCrunch => "Bone Crunch",
          AttackType.Punch => "Punch",
          _ => "Not a valid attack"
        };
        RichConsole.WriteLine($"{attackString}: Press #{currentAttack}");
        currentAttack++;
      }
      string attackIDstring = RichConsole.ReadLine();
      validAttack = Int32.TryParse(attackIDstring, out attackID);
      validAttack = validAttack && (attackID <= currentAttack) && (attackID >= 0); //in valid range

      if (!validAttack)
        RichConsole.WriteLine("Not a valid choice, try again!");
    }

    return c.AttackList[attackID];

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

public interface ICommand 
{ 
  public void Execute(Character character); //need to take in the game object I thinK? Need to know the state of who's turn it is
  public virtual void Display(Character character){ }
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
    
    //calculate if the attack landed or not
    int randResult = myRand.Next((int)(1 / AttackData.AttackFrequency));
    bool attackLanded = randResult == 0;
    int attackDamage = 0;
    bool defeated = false;

    //deal the damage only if the attack landed
    if (attackLanded)
    {
      attackDamage = AttackData.AttackDamage;
      Target.HP = Math.Clamp(Target.HP - attackDamage, 0, Target.HP);
    }

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
      _ => "INVALID ATTACK"
    };

    //display who used the attack, how much damage it dealt, and update the console about the target's current HP
    RichConsole.WriteLine($"{Attacker.Name} used {attackName} on {Target.Name}");
    RichConsole.WriteLine($"{attackName} dealt {AttackDamage} damage to {Target.Name}.");
    RichConsole.WriteLine($"{Target.Name} is now at {Target.HP}/{Target.MaxHP} HP.");
    if (Defeated)
      RichConsole.WriteLine($"{Target.Name} has been defeated!");

  }
}

//Not expecting to use this class except for default cases
public class NoAttack : AttackCommand
{
  public NoAttack(Character target, Character attacker) : base(target, attacker, AttackType.NoAttack, new AttackData(0,0,DamageType.NoType)) { }
}

public class PunchAttack : AttackCommand
{
  public PunchAttack(Character target, Character attacker) : base(target, attacker, AttackType.Punch, new AttackData(1, 1.0F, DamageType.Physical)) { }

}

public class BoneCrunchAttack : AttackCommand
{
  public BoneCrunchAttack(Character target, Character attacker): base(target, attacker, AttackType.BoneCrunch, new AttackData(1, 0.5F, DamageType.Physical)) { }
}

//maybe this AttackData should contain the attack type, as well as the amount of damage etc.?
public class AttackData
{
  public int AttackDamage { get; private set; }
  public float AttackFrequency { get; private set; } // this is from 0 to 100% 
  public DamageType Type { get; private set; }

  public AttackData(int damage, float frequency, DamageType type)
  {
    AttackDamage = damage;
    AttackFrequency = frequency;
    Type = type;
  }

}

public enum AttackType { NoAttack, Punch, BoneCrunch}
public enum DamageType { NoType, Physical}