// See https://aka.ms/new-console-template for more information

int[] myInts = [1, 9, 2, 8, 3, 7, 4, 6, 5];

List<int> myFilteredInts = (List<int>)ProceduralInts(myInts);

Console.WriteLine("For loops way");
foreach (int i in myFilteredInts)
{
  Console.Write(i + " ");
}

IEnumerable<int> myFilteredInts2 = KeywordInts(myInts);
Console.WriteLine("\nQuery keywords way");
foreach (int i in myFilteredInts2)
{
  Console.Write(i + " ");
}

IEnumerable<int> myFilteredInts3 = MethodInts(myInts);
Console.WriteLine("\nMethod keywords way");
foreach (int i in myFilteredInts3)
{
  Console.Write(i + " ");
}

IEnumerable<int> ProceduralInts(int[] intArray)
{
  //TODO first filter out the odds using a for loop
  //TODO next put them in order, i.e. use the Array.Sort()
  //TODO last double them

  List<int> result = new List<int>();

  //throw away odds
  foreach (int i in intArray)
  {
    if(i % 2 == 0)
    {
      result.Add(i);
    }
  }

  //list ascending
  result.Sort();

  for (int i= 0; i < result.Count(); i++)
  {
    result[i] = result[i] * 2;
  }
  return result;
}

//again but with keyword-based query expressions
IEnumerable<int> KeywordInts(int[] intArray)
{
  IEnumerable<int> result = from i in intArray
                            where i % 2 == 0
                            orderby i
                            let doubled = i * 2
                            select doubled;
  return result;
}

//again but with method call based expression
IEnumerable<int> MethodInts(int[] intArray)
{
  IEnumerable<int> result = intArray
                              .Where(i => i % 2 == 0)
                              .OrderBy(i => i)
                              .Select(i => i * 2);
  return result;
}
