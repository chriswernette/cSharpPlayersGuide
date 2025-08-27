// See https://aka.ms/new-console-template for more information
int[] array = new int[] { 4, 51, -7, 13, -99, 15, -8, 45, 90 };

int total = 0;
foreach(int value in array)
  total += value;

float average = (float)total / array.Length;
Console.WriteLine(average);
