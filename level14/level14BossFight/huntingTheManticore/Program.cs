// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


//initialize/declare variables
int manticoreHealth = 10;
int cityHealth = 15;
int manticoreDistance = -1;
int round = 1;

//ask player 1 to enter a distance

while ((manticoreDistance < 0) || (manticoreDistance > 100))
{
  Console.Write("Player 1, how far away from the city do you want to station the Manticore? ");
  manticoreDistance = Convert.ToInt32(Console.ReadLine());
}
Console.Clear();


Console.WriteLine("Player 2, it is your turn.");
Console.WriteLine("-------------------------------------------------------------");
while ((cityHealth > 0) && (manticoreHealth > 0))
{
  //status message
  Console.WriteLine($"STATUS: Round: {round} City: {cityHealth}/15 Manticore: {manticoreHealth}/10");

  //calc damage manticore COULD take this turn if we land a hit
  int cannonDamage = cannonDamageCalculator(round);

  //status message
  Console.WriteLine($"The cannon is expected to deal {cannonDamage} damage this round.");
 
  //get user input for desired shot
  Console.Write("Enter desired cannon range: ");
  int cannonRange = Convert.ToInt32(Console.ReadLine());

  //compute if we hit or missed and output over/under
  bool didIHit = hitOrMiss(manticoreDistance, cannonRange);

  //adjust manticore health if hit
  if (didIHit == true)
  {
    manticoreHealth -= cannonDamage;
  }
  
  Console.WriteLine("-------------------------------------------------------------");


  //turn based effects
  cityHealth--;
  round++;
}

if (cityHealth <= 0)
{
  Console.WriteLine("The city of Consolas has been DESTROYED! The Uncoded one has triumphed!");
}
else
{
  Console.WriteLine("The Maticore has been destroyed! The city of Consolas has been saved!");
}

int cannonDamageCalculator(int currentRound)
{
  //determine what type of blast and calculate the damage this turn
  bool fireBlast = (currentRound % 3) == 0;
  bool electricBlast = (currentRound % 5) == 0;
  bool fireElectricBlast = fireBlast && electricBlast;
  int cannonDamage = 1;

  if (fireElectricBlast)
  {
    cannonDamage = 10;
  }
  else if (fireBlast)
  {
    cannonDamage = 3;
  }
  else if (electricBlast)
  {
    cannonDamage = 5;
  }
  return cannonDamage;
}

bool hitOrMiss(int manticoreDistance, int cannonRange)
{
  //what happens based on the users input, either hit or miss and update health if hit. Need to think of more elegant way to do with switches?
  if (cannonRange == manticoreDistance)
  {
    Console.WriteLine("That round was a DIRECT HIT!");
    return true;
  }
  else if (cannonRange > manticoreDistance)
  {
    Console.WriteLine("That round OVERSHOT the target.");
    return false;
  }
  else
  {
    Console.WriteLine("That round FELL SHORT of the target.");
    return false;
  }
}
