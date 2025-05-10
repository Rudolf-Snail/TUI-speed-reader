// See https://aka.ms/new-console-template for more information
using SpeedReaderTextUserInterface;
using System.Configuration;
using static SpeedReaderTextUserInterface.AppSettingsConfigurationFile;
using static SpeedReaderTextUserInterface.Input;

Console.Title = "Speed reader";

/* Tests:

//Enum.TryParse(typeof(TextOptions), "Text", out object? option);
//TextOptions convertedOption = ( TextOptions ) option;

//NonStringInput<TextOptions>.SuccessCondition successCondition = NonStringInput<TextOptions>.IsParsedCorrectly;
//NonStringInput<TextOptions>.ReceiveCorrectInputValues("Please enter a TextOption enumerator: ", "That is not correct, please try again.", Input.JustReadInput, successCondition);

//NonStringInput<decimal>.SuccessCondition successCondition = NonStringInput<decimal>.IsPositiveNumber;
//NonStringInput<decimal>.GetCorrectInputValues("Please enter a positive number: ", "That is not correct, please try again.", Input.JustReadInput, successCondition);

*/

bool gotDefaultValueForHorizontalAlignment = bool.TryParse(ConfigurationManager.AppSettings["alignHorizontally"], out bool alignHorizontally);
bool gotDefaultValueForVerticalAlignment = bool.TryParse(ConfigurationManager.AppSettings["alignVertically"], out bool alignVertically);

ProcessUserInput(out string? text, ref alignHorizontally, ref alignVertically);
decimal speed = SetReadingSpeed();

var speedReader = new SpeedReader(speed, text, alignHorizontally, alignVertically);
speedReader.SpeedReadText();

static void ProcessUserInput(out string? text, ref bool alignHorizontally, ref bool alignVertically)
{
    SelectOption(out TextOptions option);
    ProcessOption(ref option, out text, ref alignHorizontally, ref alignVertically);
}

static void SelectOption(out TextOptions option)
{
    string message1 = "How do you wish to enter the text to speedread?\n";
    string options = "Options:\n";
    // TODO: Make this dynamic and not static 
    string text = "Type in Text, if you wish to read text from the command line.\n";
    string file = "Type in File, if you wish to read text from a file.\n";
    string align = "Type in Align, if you wish to change the alignment of the text.\n";
    string reset = "Type in Reset, if you wish to reset the alignment settings to the default values.\n";
    string choice = "Type in the name of the option you wish to choose: ";

    string[] messages = [message1, options, text, file, align, reset, choice];

    option = NonStringInput<TextOptions>.ReceiveCorrectInputValues(messages, ReadAndCapitalizeInput, NonStringInput<TextOptions>.IsParsedCorrectly);
}

static void ProcessOption(ref TextOptions option, out string? text, ref bool alignHorizontally, ref bool alignVertically)
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
            ConfigureAlignmentSettings(out alignHorizontally, out alignVertically);
            ProcessUserInput(out text, ref alignHorizontally, ref alignVertically);
            break;
        case TextOptions.Reset:
            ResetAlignmentSettings(out alignHorizontally, out alignVertically);
            ProcessUserInput(out text, ref alignHorizontally, ref alignVertically);
            break;
        default:
            throw new ArgumentException($"The specified option — {option} — is not valid."); // Should not happen, but it's handled by throwing an error just in case.
    }
}

static string TextUserInput()
{
    string message = "Enter text for speedreading: ";

    // Should not be a problem as the success condition prevents an empty or null string from being returned. Sadly the code analysis/compiler does not see this. :(
    #pragma warning disable CS8603 // Possible null reference return.
    return StringInput.ReceiveCorrectInputValues(message, ReadAndTrimWhitespaces, StringInput.IsNotNullOrEmpty);
    #pragma warning restore CS8603 // Possible null reference return.
}

static string FileUserInput()
{
    string message = "Enter the file path to the file you wish to speedread: ";

    string? path = StringInput.ReceiveCorrectInputValues(message, ReadAndTrimWhitespaces, StringInput.FileExists);

    string text;

    // I WANT this to throw an error, if it doesn't find the path, as trying to catch the error and work around it is too difficult and not worth it for me, it's easier to crash the whole program and try again, than to figure out how to solve this issue. 
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
    string message = "Enter the speed you wish to read the text at — in Words Per Minute (WPM) — has to be a positive value: ";

    return NonStringInput<decimal>.ReceiveCorrectInputValues(message, JustReadInput, NonStringInput<decimal>.IsPositiveNumber);
}