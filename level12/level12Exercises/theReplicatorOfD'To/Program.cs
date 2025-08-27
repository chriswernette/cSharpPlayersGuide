// See https://aka.ms/new-console-template for more information
int[] array1 = new int[5];

for (int i = 0; i < array1.Length; i++)
{
  array1[i] = AskForNumber("Enter a number to store in the array: ");
}

int[] array2 = new int[5];
for (int i = 0; i < array2.Length; i++)
{
  array2[i] = array1[i];
  Console.WriteLine($"The value of array1 at {i} is {array1[i]}");
  Console.WriteLine($"The value of array2 at {i} is {array1[i]}");
}

int AskForNumber(string text)
{
  Console.Write(text);
  int result = Convert.ToInt32(Console.ReadLine());
  return result;
}
int AskForNumberInRange(string text, int min, int max)
{
  while (true)
  {
    Console.WriteLine(text);
    int result = Convert.ToInt32(Console.ReadLine());
    if ((result >= min) && (result <= max))
      return result;
  }

}