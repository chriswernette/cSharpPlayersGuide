// See https://aka.ms/new-console-template for more information

int storedNum;

//runs at least once to ask for number, if out of range, keeps asking
do
{
  Console.Write("User 1, enter a number between 0 and 100: ");
  storedNum = Convert.ToInt32(Console.ReadLine());
}
while ((storedNum < 0) || (storedNum > 100));

//clear screen once good number is picked, so no cheating!
Console.Clear();

do
{
  Console.WriteLine("User 2, guess the number. ");
  Console.Write("What is your next guess? ");
  int guessNum = Convert.ToInt32(Console.ReadLine());

  if(guessNum == storedNum)
  {
    Console.WriteLine("You guessed the number!");
    break;
  } else if(guessNum < storedNum){
    Console.WriteLine("You guessed too low!");
  } else
  {
    Console.WriteLine("You guessed too high!");
  }
}
while (true);
