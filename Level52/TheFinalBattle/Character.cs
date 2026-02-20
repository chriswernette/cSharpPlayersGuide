// See https://aka.ms/new-console-template for more information
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
  public Player(bool humanControlled, string name, int maxHP, int startingHP) :
    base(name, maxHP, startingHP, humanControlled, true, new AttackType[] { AttackType.Punch })
  { }
}

public class Skeleton : Character
{
  public Skeleton(bool humanControlled, string name = "SKELETON", int maxHP = 5, int startingHP = 5) :
   base(name, maxHP, startingHP, humanControlled, false, new AttackType[] { AttackType.BoneCrunch })
  { }
}

public class UncodedOne : Character
{
  public UncodedOne(bool humanControlled, string name = "THE UNCODED ONE", int maxHP = 15, int startingHP = 15) :
    base(name, maxHP, startingHP, humanControlled, false, new AttackType[] { AttackType.Unravel })
  { }
}