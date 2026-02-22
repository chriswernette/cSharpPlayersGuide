// See https://aka.ms/new-console-template for more information
public class Party
{
  public bool IsHero {  get; private set; }
  public Character[][] Characters; //we need this type of array of arrays because our array is "jagged" meaning each index could be different length
  public List<Item>[][]? Items; // this actually could be null if there's no items equipped. Also wonder if I should make this a list? Then I can add and remove items during use or loot
  public int NumWaves {  get; private set; }
  public int WaveIndex { get; private set; } = 0;

  public Party(Character[][] characters, List<Item>[][] items, bool isHero)
  {
    Characters = characters;
    Items = items;
    IsHero = isHero;
    NumWaves = characters.Length; //this is the number of rows in the array. If we did character[0].length() it would return the number of columns in row 0
  }
}
