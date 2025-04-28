// See https://aka.ms/new-console-template for more information
using Speed_reader;

bool isValidOption;
TextOptions option;

do
{
    Console.WriteLine("How do you wish to enter the text to speedread?");

    Console.WriteLine("Options:");
    Console.WriteLine("Type in Text, if you wish to read text from the command line.");
    Console.WriteLine("Type in File, if you wish to read text from a file.");
    //TODO: Add option for padding text. 

    Console.Write("Type in the name of the option you wish to choose: ");
    isValidOption = Enum.TryParse<TextOptions>(ToCapital(Console.ReadLine()), out option);
} while (!isValidOption);

string? text;

switch (option)
{
    case TextOptions.Text:
        text = TextUserInput();
        break;
    case TextOptions.File:
        text = FileUserInput();
        break;
    //TODO: Add option for padding text. 
    default:
        throw new ArgumentException($"The specified option — {option.ToString()} — is not valid."); // Should not happen, but it's handled by throwing an error just in case.
}

decimal speed;
bool numberEntered;

do
{
    Console.Write("Enter the speed you wish to read the text at — in Words Per Minute (WPM) — has to be a positive value: ");
    numberEntered = decimal.TryParse(Console.ReadLine(), out speed);
}
while (!numberEntered || speed <= 0);

//TODO: Appropriately factor in the padding variables for text. 
var speedReader = new SpeedReader(speed, text, false, false);

speedReader.SpeedReadText();

static string TextUserInput()
{
    string? text;

    do
    {
        Console.Write("Enter text for speedreading: ");
        text = Console.ReadLine();
    }
    while (string.IsNullOrEmpty(text));

    return text;
}

static string FileUserInput()
{
    string text;

    bool fileAvailable;
    string? path;

    do
    {
        Console.Write("Enter the file path to the file you wish to speedread: ");
        path = Console.ReadLine();

        if (!string.IsNullOrEmpty(path))
            path = path.Trim('\"');

        fileAvailable = File.Exists(path);
    } while (!fileAvailable);

#pragma warning disable CS8604 // Possible null reference argument. Disabled becaused FileExists also checks if the string is null and returns false if it is, so it is (or very likely should be) impossible to get through the do-while loop with a null value.
    using (StreamReader reader = new(path))
    {
        text = reader.ReadToEnd();
    }
#pragma warning restore CS8604 // Possible null reference argument.

    return text;
}

static string? ToCapital(string? text)
{
    if (string.IsNullOrEmpty(text))
        return text;
    else if (text.Length == 1)
        return text.ToUpper();
    else
        return text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
}

enum TextOptions
{
    Text,
    File
}