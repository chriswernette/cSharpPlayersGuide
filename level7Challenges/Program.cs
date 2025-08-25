// See https://aka.ms/new-console-template for more information

/*
Console.WriteLine("What is the base:");
string triangleBaseString = Console.ReadLine();
float triangleBase =Convert.ToSingle(triangleBaseString);
Console.WriteLine("What is the height:");
string triangleHeightString = Console.ReadLine();
float triangleHeight = Convert.ToSingle(triangleHeightString);

float triangleArea = (triangleBase * triangleHeight) / 2;
Console.WriteLine("The triangle area is: " + triangleArea);
*/

/*
int a = 5;
int b = 2;
int result = a/b;
Console.WriteLine(result);
*/

/*
Console.WriteLine("How many chocolate eggs were laid today?");
int numEggs = Convert.ToInt32(Console.ReadLine());
int numEggsPerSis = numEggs / 4;
int numEggsDuckBear = numEggs % 4;
Console.WriteLine("Each sister gets " + numEggsPerSis + " eggs and " +  numEggsDuckBear + " eggs are fed to the pet duckbear");
*/

Console.WriteLine("How many provinces do you have?");
int provinces  = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("How many estates do you have?");
int estates = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("How many duchies do you have?");
int duchies = Convert.ToInt32(Console.ReadLine());

int score = estates + 3 * duchies + 6 * estates;
Console.WriteLine("Your score is: " + score);