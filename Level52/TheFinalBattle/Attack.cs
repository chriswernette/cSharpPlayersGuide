// See https://aka.ms/new-console-template for more information
using CSharpPlayersGuide.RichConsole;
using System.Diagnostics;

public class AttackCommand : ICommand
{
  public static int PunchDamage = 1;
  public static int PunchFrequency = 1; // more like a period than a frequency but oh well
  public static int BoneCrunchDamage = 1;
  public static int BoneCrunchFrequency = 2;
  public static int UnravelDamage = 2;
  public static int UnravelFrequency = 3;

  public Character Target { get; private set; }
  public Character Attacker { get; private set; }
  public AttackData AttackData { get; private set; } 
  public AttackType AttackType { get; private set; }
  public AttackCommand(Character target, Character attacker, AttackType type, AttackData attackData)
  {
    Target = target; 
    Attacker = attacker;
    AttackType = type;
    AttackData = attackData; // maybe not relevant for this base class, could make this base class more abstract?
  }

  public static AttackCommand CreatePunchAttack(Character target, Character attacker) => new AttackCommand(target, attacker, AttackType.Punch, new AttackData(PunchDamage, PunchFrequency, DamageType.Physical));
  public static AttackCommand CreateBoneCrunchAttack(Character target, Character attacker) => new AttackCommand(target, attacker, AttackType.BoneCrunch, new AttackData(BoneCrunchDamage, BoneCrunchFrequency, DamageType.Physical));
  public static AttackCommand CreateUnravelAttack(Character target, Character attacker) => new AttackCommand(target, attacker, AttackType.Unravel, new AttackData(UnravelDamage, UnravelFrequency, DamageType.Physical));

  //TODO strip character? I already have target and attacker stored in the class?
  public void Execute(Character character) {
    Random myRand = new Random();
    int attackDamage = 0;
    bool defeated = false;

    //TODO
    //calculate attack damage, special case for punch attack with 100% hit chance
    if (AttackData.Frequency == 1) // not sure it's wise to do it like this...
      attackDamage = 1;
    else 
      attackDamage = myRand.Next(AttackData.Frequency);

    //deal the damage, could be 0
    Target.TakeDamage(attackDamage);

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

    string attackName = AttackType switch
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


public class NoAttack : AttackCommand
{
  public NoAttack(Character target, Character attacker) : base(target, attacker, AttackType.NoAttack, new AttackData(0, 0, DamageType.NoType)) { }
}

//maybe this AttackData should contain the attack type, as well as the amount of damage etc.?
public record AttackData(int Damage, int Frequency, DamageType Type);

public enum AttackType { NoAttack, Punch, BoneCrunch, Unravel }
public enum DamageType { NoType, Physical }
