// See https://aka.ms/new-console-template for more information

using System.Drawing;

foreach (CardColor color in Enum.GetValues(typeof(CardColor)))
{
  foreach(CardRank rank in Enum.GetValues(typeof(CardRank)))
  {
    Card myCard = new Card(color, rank);
    Console.WriteLine($"The {myCard.Color} {myCard.Rank}");
  }
}


public class Card
{
  public CardColor Color { get; private set; } = CardColor.Blue;
  public CardRank Rank { get; private set; } = CardRank.Two;
  public Card(CardColor color, CardRank rank)
  {
    Color = color;
    Rank = rank;
  }
  public bool IsNumber()
  {
    return this.Rank <= CardRank.Ten;
  }

  public bool IsFace()
  {
    return this.Rank > CardRank.Ten;
  }

}

public enum CardColor
{
  Red,
  Green,
  Blue,
  Yellow
}

public enum CardRank
{
  One,
  Two,
  Three,
  Four,
  Five,
  Six,
  Seven,
  Eight,
  Nine,
  Ten,
  Dollar,
  Percent,
  Caret,
  Ampersand
}