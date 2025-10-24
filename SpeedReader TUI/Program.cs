// See https://aka.ms/new-console-template for more information
using SpeedReaderTextUserInterface;

Console.Title = "Speed reader";

// This fixes the problems with displaying non-English characters, at least for Czech. 
Console.InputEncoding = System.Text.Encoding.Unicode;
Console.OutputEncoding = System.Text.Encoding.Unicode;

SpeedReader speedReader = new();
speedReader.ReadEvaluateProcessLoop();