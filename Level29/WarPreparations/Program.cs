// See https://aka.ms/new-console-template for more information

Sword mySword = new Sword(SwordMaterials.Iron, Gemstone.None, 10, 1);
Sword legendarySword = mySword with { swordType = SwordMaterials.Binarium, gemstoneType = Gemstone.Bitstone };
Sword plainSword = mySword with { swordType = SwordMaterials.Wood };

Console.WriteLine(mySword.ToString());
Console.WriteLine(legendarySword.ToString());
Console.WriteLine(plainSword.ToString());

public record Sword(SwordMaterials swordType, Gemstone gemstoneType, float length, float crossguardWidth);


public enum SwordMaterials
{
  Wood,
  Bronze,
  Iron,
  Steel, 
  Binarium
}

public enum Gemstone
{
  None,
  Emerald,
  Amber,
  Sapphire,
  Diamond,
  Bitstone
}