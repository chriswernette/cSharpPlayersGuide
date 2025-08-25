// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//get user x input
Console.Write("What is the x position of the enemy? ");
int xPos = Convert.ToInt32(Console.ReadLine());

//get user y input
Console.Write("What is the y position of the enemy? ");
int yPos = Convert.ToInt32(Console.ReadLine());

//determine if they are east or west
if (xPos > 0) //Eastern
{
  if (yPos > 0)
  {
    Console.WriteLine("The enemy is to the northeast!");
  } else if (yPos == 0) 
  {
    Console.WriteLine("The enemy is to the east!");
  }
  else
  {
    Console.WriteLine("The enemy is to the southeast!");
  }
}
else if (xPos == 0) //Neither east nor west, but could be north or south
{
  if (yPos > 0)
  {
    Console.WriteLine("The enemy is to the north!");
  }
  else if (yPos == 0)
  {
    Console.WriteLine("The enemy is here!");
  }
  else
  {
    Console.WriteLine("The enemy is to the south!");
  }
}
else //Western
{
  if (yPos > 0)
  {
    Console.WriteLine("The enemy is to the northwest!");
  }
  else if (yPos == 0)
  {
    Console.WriteLine("The enemy is to the west!");
  }
  else
  {
    Console.WriteLine("The enemy is to the southwest!");
  }
}