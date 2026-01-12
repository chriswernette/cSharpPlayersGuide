// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;

Console.Write("Please enter your name: ");
string? userName = null;
int playerScore = 0;
while(userName == null || userName == "")
{
  userName = Console.ReadLine().Trim();
  if (userName != null && userName != "")
  {
    Console.WriteLine($"Username {userName} accepted.");
  }
  else
  {
    Console.WriteLine("Enter a valid username next time!");
  }
}

if (File.Exists($"{userName}.txt"))
{
  string previous = File.ReadAllText($"{userName}.txt");
  bool success = Int32.TryParse(previous, out playerScore);
}

Console.WriteLine($"{userName} go crazy and type as long as you can. Hitting enter means you are done.");
ConsoleKeyInfo cki;
while (true)
{
  cki = Console.ReadKey(false);
  if(cki.Key == ConsoleKey.Enter)
  {
    break;
  }
  else
  {
    playerScore++;
    Console.WriteLine($" Score: {playerScore}");
  }
}

Console.WriteLine($"{userName} final score: {playerScore}");

File.WriteAllText($"{userName}.txt", playerScore.ToString());