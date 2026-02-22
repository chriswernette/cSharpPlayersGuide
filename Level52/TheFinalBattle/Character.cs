// See https://aka.ms/new-console-template for more information
public class Character
{
  private static int HeroHP = 25;
  private static int SkeletonHP = 5;
  private static int UncodedOneHP = 15;

  public string Name { get; protected set; }
  public int MaxHP { get; protected set; }
  public int HP { get; set; }
  public bool IsAlive { get; set; }
  public bool HumanControlled { get; protected set; }
  public bool IsHero {  get; protected set; }
  public AttackType[] BaseAttackList { get; protected set;  } 
  public AttackType[]? ItemAttackList { get; set; } //TODO add this later when we add items that can be equipped to attack such as sword, bow, etc.

  public Character(string name, int maxHP, int startingHP, AttackType[] attacks)
  {
    Name = name;
    MaxHP = maxHP;
    HP = startingHP;
    IsAlive = true;
    BaseAttackList = attacks;
  }

  public static Character CreatePlayer(string trueProgrammerName) => new Character(trueProgrammerName, HeroHP, HeroHP, [AttackType.Punch]);
  public static Character CreateSkeleton() => new Character("SKELETON", SkeletonHP, SkeletonHP, [AttackType.BoneCrunch]);
  public static Character CreateUncodedOne() => new Character("THE UNCODED ONE", UncodedOneHP, UncodedOneHP, [AttackType.Unravel]);
}