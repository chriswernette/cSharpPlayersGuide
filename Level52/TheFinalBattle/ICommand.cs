// See https://aka.ms/new-console-template for more information
using CSharpPlayersGuide.RichConsole;

public interface ICommand 
{ 
  public void Execute(Character character); //need to take in the game object I thinK? Need to know the state of who's turn it is
  public virtual void Display(Character character){ } //TODO remove character from the display command, not needed. Make Display into one method with variable # of inputs but passing in optional args?
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
