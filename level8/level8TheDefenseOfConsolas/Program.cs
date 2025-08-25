// See https://aka.ms/new-console-template for more information
//set up the title and color scheme
Console.Title = "The Defense Of Consolas";
Console.Clear();
Console.BackgroundColor = ConsoleColor.Green;
Console.ForegroundColor = ConsoleColor.Magenta;

//ask user for inputs and store them for calc
Console.Write("Target Row? ");
var targetRow = Convert.ToInt32(Console.ReadLine());
Console.Write("Target Column? ");
var targetCol = Convert.ToInt32(Console.ReadLine());

Console.WriteLine("Deploy to:");
Console.WriteLine($"({targetRow},{targetCol - 1})");
Console.WriteLine($"({targetRow - 1},{targetCol})");
Console.WriteLine($"({targetRow},{targetCol + 1})");
Console.WriteLine($"({targetRow + 1},{targetCol})");


Console.Beep(440, 1000);