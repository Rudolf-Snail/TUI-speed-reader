// See https://aka.ms/new-console-template for more information
using System.Configuration;
using SpeedReaderTextUserInterface;
using static SpeedReaderTextUserInterface.AppSettingsConfigurationFile;
using static SpeedReaderTextUserInterface.Input;

Console.Title = "Speed reader";

// This fixes the problems with displaying non-English characters, at least for Czech. 
Console.InputEncoding = System.Text.Encoding.Unicode;
Console.OutputEncoding = System.Text.Encoding.Unicode;

/* Tests:

//Enum.TryParse(typeof(TextOptions), "Text", out object? option);
//TextOptions convertedOption = ( TextOptions ) option;

//NonStringInput<TextOptions>.SuccessCondition successCondition = NonStringInput<TextOptions>.IsParsedCorrectly;
//NonStringInput<TextOptions>.ReceiveCorrectInputValues("Please enter a TextOption enumerator: ", "That is not correct, please try again.", Input.JustReadInput, successCondition);

//NonStringInput<decimal>.SuccessCondition successCondition = NonStringInput<decimal>.IsPositiveNumber;
//NonStringInput<decimal>.GetCorrectInputValues("Please enter a positive number: ", "That is not correct, please try again.", Input.JustReadInput, successCondition);

*/

bool alignHorizontally = GetAlignmentConfigurationValueOrDefaultValue("alignHorizontally");
bool alignVertically = GetAlignmentConfigurationValueOrDefaultValue("alignVertically");

SpeedOptions speedOption = GetSpeedOptionConfigurationValueOrDefaultValue();

bool exitAfterSpeedReading = GetExitAfterSpeedReadingConfigurationValueOrDefaultValue();

TextOptions textOption;

string? text = "";

while (true)
{
    ProcessUserInput(ref text, out textOption, ref speedOption, ref alignHorizontally, ref alignVertically, ref exitAfterSpeedReading);

    if (textOption == TextOptions.Exit)
    {
        break;
    }
    else if (textOption == TextOptions.Text || textOption == TextOptions.File)
    {
        decimal speed = ProcessSpeed(speedOption);
        var speedReader = new SpeedReader(speed, speedOption, text, alignHorizontally, alignVertically);
        speedReader.SpeedReadText();

        if (exitAfterSpeedReading)
            break;
    }
}