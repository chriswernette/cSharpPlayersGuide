// See https://aka.ms/new-console-template for more information
using System.ComponentModel;

while (true)
{
  Console.WriteLine("Hello user, what is your word? Larger words may take a looooong time to guess!");
  string? userWord = Console.ReadLine();
  if (userWord == null)
  {
    Console.WriteLine("Try again next loop silly!");
  }
  else
  {
    Task<int> wordGuesserTask = RandomlyRecreateAsync(userWord);
  }
}

int RandomlyRecreate(string word)
{
  int stringLength = word.Length;
  string lowercaseWord = word.ToLower();
  char newLetter;
  char[] comparison = new char[stringLength];

  int numAttempts = 0;
  bool wordGuessed = false;

  Random randRecreater = new Random();

  //generate random string of the same length
  while(wordGuessed == false)
  {
    for (int i = 0; i < stringLength; i++)
    {
      newLetter = (char)('a' + randRecreater.Next(26));
      comparison[i] = newLetter;
    }
    string c = new string(comparison);
    if (c == lowercaseWord)
    {
      wordGuessed = true;
    }
    numAttempts++;
  }
  return numAttempts;
}

 async Task<int> RandomlyRecreateAsync(string word)
{
  Task<int> task = new Task<int>(() =>
  {
    return RandomlyRecreate(word);
  });
  DateTime startTime = DateTime.Now;
  task.Start();
  await task;
  DateTime endTime = DateTime.Now;
  TimeSpan total = endTime - startTime;
  Console.WriteLine($"It took {task.Result} tries to get guess your word {word} correctly! And {total.Minutes} minutes, {total.Seconds} seconds, {total.Milliseconds} milliseconds");
  return task.Result;
}