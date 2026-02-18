// See https://aka.ms/new-console-template for more information
using CSharpPlayersGuide.RichConsole;

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


public class NoAttack : AttackCommand
{
  public NoAttack(Character target, Character attacker) : base(target, attacker, AttackType.NoAttack, new AttackData(0, 0, DamageType.NoType)) { }
}

public class PunchAttack : AttackCommand
{
  public PunchAttack(Character target, Character attacker) : base(target, attacker, AttackType.Punch, new AttackData(1, 1, DamageType.Physical)) { }

}
public class BoneCrunchAttack : AttackCommand
{
  public BoneCrunchAttack(Character target, Character attacker) : base(target, attacker, AttackType.BoneCrunch, new AttackData(1, 2, DamageType.Physical)) { }
}

public class UnravelAttack : AttackCommand
{
  public UnravelAttack(Character target, Character attacker) : base(target, attacker, AttackType.Unravel, new AttackData(2, 3, DamageType.Physical)) { }
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
