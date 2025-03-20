// See https://aka.ms/new-console-template for more information
using Speed_reader;

string? text;

do
{
    Console.Write("Enter text for speedreading: ");
    text = Console.ReadLine();
}
while (text is null);

decimal speed;
bool numberEntered;

do
{
    Console.Write("Enter the speed you wish to read the text at — in Words Per Minute (WPM): ");
    numberEntered = decimal.TryParse(Console.ReadLine(), out speed);
}
while (!numberEntered);

var speedReader = new SpeedReader(speed, text);

speedReader.SpeedReadText();