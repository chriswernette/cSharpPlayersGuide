// See https://aka.ms/new-console-template for more information

Console.WriteLine("What kind of thing are we talking about?");
//string a is the thing we're talking about
string a = Console.ReadLine();
Console.WriteLine("How would you describe it? Big? Azure? Tattered?");
//string b is how we describe string a
string b = Console.ReadLine();
//string c makes it ominous
string c = "of doom";
//string d is another boring literal to make it extra important
string d = "3000";
Console.WriteLine("The " + b + " " + a + " of " + c + " " + d + "!");
