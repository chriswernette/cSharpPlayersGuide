// See https://aka.ms/new-console-template for more information
//Not expecting to use this class except for default cases

public record MenuItem(string Description, ICommand Action, bool isEnabled = true);
