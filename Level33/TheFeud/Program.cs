// See https://aka.ms/new-console-template for more information
using IField;
using McDroid;
using IPig = IField.Pig;
using McPig = McDroid.Pig;

Sheep mySheep = new Sheep();
IField.Pig myPig = new IField.Pig();
Cow myCow = new Cow();
McPig mySecondPig = new McPig();

Console.WriteLine("Hello, World!");


namespace IField
{
  class Sheep { }
  class Pig { }
}

namespace McDroid
{
  class Cow { }
  class Pig { }
}