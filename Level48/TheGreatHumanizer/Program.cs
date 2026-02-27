// See https://aka.ms/new-console-template for more information
using Humanizer;

DateTime feastTime = DateTime.UtcNow.AddHours(30);
Console.WriteLine(Humanizer.DateHumanizeExtensions.Humanize(feastTime));

feastTime = DateTime.UtcNow.AddHours(2.5);
Console.WriteLine(Humanizer.DateHumanizeExtensions.Humanize(feastTime));

feastTime = DateTime.UtcNow.AddHours(50);
Console.WriteLine(Humanizer.DateHumanizeExtensions.Humanize(feastTime));