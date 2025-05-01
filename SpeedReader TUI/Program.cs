// See https://aka.ms/new-console-template for more information
using Speed_reader;
using System.Configuration;

Console.Title = "Speed reader";

string? text;
bool alignHorizontally, alignVertically;

bool gotDefaultValueForHorizontalAlignment = bool.TryParse(ConfigurationManager.AppSettings["alignHorizontally"], out alignHorizontally);
bool gotDefaultValueForVerticalAlignment = bool.TryParse(ConfigurationManager.AppSettings["alignVertically"], out alignVertically);

if (!gotDefaultValueForHorizontalAlignment || !gotDefaultValueForVerticalAlignment)
{
    alignHorizontally = false;
    alignVertically = false;
}    

ProcessUserInput(out text, ref alignHorizontally, ref alignVertically);
decimal speed = SetReadingSpeed();

var speedReader = new SpeedReader(speed, text, alignHorizontally, alignVertically);
speedReader.SpeedReadText();

static string? ToCapital(string? text)
{
    if (string.IsNullOrEmpty(text))
        return text;
    else if (text.Length == 1)
        return text.ToUpper();
    else
        return text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
}

static void ConfigureAlignmentSettings(out bool isValidOption, out bool alignHorizontally, out bool alignVertically)
{
    do
    {
        Console.Write("Do you wish to align the text horizontally? Type in True for yes and False for no: ");
        isValidOption = bool.TryParse(Console.ReadLine(), out alignHorizontally);
    } while (!isValidOption);

    ChangeConfiguration("alignHorizontally", alignHorizontally.ToString());

    do
    {
        Console.Write("Do you wish to align the text vertically? Type in True for yes and False for no: ");
        isValidOption = bool.TryParse(Console.ReadLine(), out alignVertically);
    } while (!isValidOption);

    ChangeConfiguration("alignVertically", alignVertically.ToString());
}

static void ProcessUserInput(out string? text, ref bool alignHorizontally, ref bool alignVertically)
{
    bool isValidOption;
    TextOptions option;

    SelectOption(out isValidOption, out option);
    ProcessOption(ref isValidOption, ref option, out text, ref alignHorizontally, ref alignVertically);
}

static void SelectOption(out bool isValidOption, out TextOptions option)
{
    while (true)
    {
        Console.WriteLine("How do you wish to enter the text to speedread?");

        Console.WriteLine("Options:");
        Console.WriteLine("Type in Text, if you wish to read text from the command line.");
        Console.WriteLine("Type in File, if you wish to read text from a file.");
        Console.WriteLine("Type in Align, if you wish to change the alignment of the text.");
        Console.WriteLine("Type in Reset, if you wish to reset the alignment settings to the default values.");

        Console.Write("Type in the name of the option you wish to choose: ");
        isValidOption = Enum.TryParse<TextOptions>(ToCapital(Console.ReadLine()), out option);

        if (!isValidOption)
            Console.WriteLine("This option is not correct, please try again.");
        else
            break;
    }
}

static void ProcessOption(ref bool isValidOption, ref TextOptions option, out string? text, ref bool alignHorizontally, ref bool alignVertically)
{
    switch (option)
    {
        case TextOptions.Text:
            text = TextUserInput();
            break;
        case TextOptions.File:
            text = FileUserInput();
            break;
        case TextOptions.Align:
            ConfigureAlignmentSettings(out isValidOption, out alignHorizontally, out alignVertically);
            ProcessUserInput(out text, ref alignHorizontally, ref alignVertically);
            break;
        case TextOptions.Reset:
            ChangeConfiguration("alignHorizontally", "false");
            ChangeConfiguration("alignVertically", "false");
            ProcessUserInput(out text, ref alignHorizontally, ref alignVertically);
            break;
        default:
            throw new ArgumentException($"The specified option — {option.ToString()} — is not valid."); // Should not happen, but it's handled by throwing an error just in case.
    }
}

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

static decimal SetReadingSpeed()
{
    decimal speed;
    bool numberEntered;

    do
    {
        Console.Write("Enter the speed you wish to read the text at — in Words Per Minute (WPM) — has to be a positive value: ");
        numberEntered = decimal.TryParse(Console.ReadLine(), out speed);
    }
    while (!numberEntered || speed <= 0);

    return speed;
}

static void ChangeConfiguration(string key, string value)
{
    var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
    var settings = configFile.AppSettings.Settings;
    settings[key].Value = value;
    configFile.Save(ConfigurationSaveMode.Modified);
    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
}

enum TextOptions
{
    Text,
    File,
    Align,
    Reset
}