using System.Security.Cryptography.X509Certificates;

CharberryTree tree = new CharberryTree();
Notifier notif = new Notifier(tree);
Harvester harvestBuddy = new Harvester(tree);

while (true)
{
  tree.MaybeGrow();
}

public class CharberryTree
{
  private Random _random = new Random();
  public bool Ripe {  get; set; }
  public event Action<CharberryTree>? Ripened;

  public void MaybeGrow()
  {
    // Only a tiny chance of ripening each time, but we try a lot!
    if(_random.NextDouble() < 0.00000001 && !Ripe)
    {
      Ripe = true;
      Ripened(this);
    }
  }
}

public class Notifier
{
  private void OnRipened(CharberryTree charrberryTree) => AnnounceRipened();
  public void AnnounceRipened()
  {
    Console.WriteLine("A Charberry fruit has ripened!");
  }

  public Notifier(CharberryTree charberryTree)
  {
    charberryTree.Ripened += OnRipened;
  }
}

public class Harvester
{
  private void OnRipened(CharberryTree charberryTree) => Harvest(charberryTree);
  public void Harvest(CharberryTree charberryTree)
  {
    charberryTree.Ripe = false;
  }
  
  public Harvester(CharberryTree charberryTree)
  {
    charberryTree.Ripened += OnRipened;
  }
}