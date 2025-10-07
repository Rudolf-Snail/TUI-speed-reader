using static SpeedReaderTextUserInterface.AppSettingsConfigurationFile;

namespace SpeedReaderTextUserInterface
{
    internal static class Input
    {
        // Methods
        public static string? ToCapital(string? text, bool convertRestOfTextToLowerCase = true)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            else if (text.Length == 1)
                return text.ToUpper();
            else if (convertRestOfTextToLowerCase)
                return text[..1].ToUpper() + text[1..].ToLower();
            else
                return text[..1].ToUpper() + text[1..];
        }

        public static void ProcessUserInput(ref string? text, out TextOptions textOption, ref SpeedOptions speedOption, ref bool alignHorizontally, ref bool alignVertically, ref bool exitAfterSpeedReading)
        {
            SelectOption(out textOption);
            ProcessOption(ref textOption, ref speedOption, ref text, ref alignHorizontally, ref alignVertically, ref exitAfterSpeedReading);
        }

        static void SelectOption(out TextOptions textOption)
        {
            string message1 = "How do you wish to enter the text to speedread?\n";
            string options = "Options:\n";
            // TODO: Make this dynamic and not static 
            string text = $"Type in {TextOptions.Text} or {(int)TextOptions.Text}, if you wish to read text from the command line.\n";
            string file = $"Type in {TextOptions.File} or {(int)TextOptions.File}, if you wish to read text from a file.\n";
            string speedOption = $"Type in {TextOptions.SpeedOption} or {(int)TextOptions.SpeedOption}, if you wish to change the mode the speed the speed is using.\n";
            string alignOption = $"Type in {TextOptions.AlignOption} or {(int)TextOptions.AlignOption}, if you wish to change the alignment of the text.\n";
            string exitOption = $"Type in {TextOptions.ExitOption} or {(int)TextOptions.ExitOption}, if you wish to exit this program after speed reading.\n";
            string reset = $"Type in {TextOptions.Reset} or {(int)TextOptions.Reset}, if you wish to reset the alignment settings to the default values.\n";
            string exit = $"Type in {TextOptions.Exit} or {(int)TextOptions.Exit}, if you wish to exit this program.\n";
            string choice = "Type in the name or number of the option you wish to choose: ";

            string[] messages = [message1, options, text, file, speedOption, alignOption, exitOption, reset, exit, choice];

            textOption = NonStringInput<TextOptions>.ReceiveCorrectInputValues(messages, ReadAndCapitalizeInputAndPreserveCase, NonStringInput<TextOptions>.IsParsedCorrectly);
        }

        static void ProcessOption(ref TextOptions option, ref SpeedOptions speedOption, ref string? text, ref bool alignHorizontally, ref bool alignVertically, ref bool exitAfterSpeedReading)
        {
            switch (option)
            {
                case TextOptions.Text:
                    text = TextUserInput();
                    break;
                case TextOptions.File:
                    text = FileUserInput();
                    break;
                case TextOptions.SpeedOption:
                    ConfigureSpeedOptionSettings(ref speedOption);
                    break;
                case TextOptions.AlignOption:
                    ConfigureAlignmentSettings(ref alignHorizontally, ref alignVertically);
                    break;
                case TextOptions.ExitOption:
                    ConfigureExitSettings(ref exitAfterSpeedReading);
                    break;
                case TextOptions.Reset:
                    ResetOptionSettings(out alignHorizontally, out alignVertically, out speedOption, out exitAfterSpeedReading);
                    break;
                case TextOptions.Exit:
                    Console.WriteLine("Goodbye!");
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
            using (StreamReader reader = new(path, detectEncodingFromByteOrderMarks: true))
            {
                Console.OutputEncoding = reader.CurrentEncoding; // Sets the encoding to the console, otherwise the text will not display special characters of different languages.
                text = reader.ReadToEnd();
            }
#pragma warning restore CS8604 // Possible null reference argument.

            return text;
        }

        public static decimal ProcessSpeed(SpeedOptions speedOption)
        {
            string message = $"Enter the speed you wish to read the text at — the current speed option is {speedOption} — the number has to be a positive value: ";

            return NonStringInput<decimal>.ReceiveCorrectInputValues(message, JustReadInput, NonStringInput<decimal>.IsPositiveNumber);
        }

        // Delegates
        public delegate string? ReadAndProcessInput();

        public static string? JustReadInput()
        {
            return Console.ReadLine();
        }

        public static string? ReadAndTrimWhitespaces()
        {
            string? input = JustReadInput();

            return input?.Trim();
        }

        public static string? ReadAndCapitalizeInputAndConvertToLowerCase()
        {
            return ToCapital(JustReadInput());
        }

        public static string? ReadAndCapitalizeInputAndPreserveCase()
        {
            return ToCapital(JustReadInput(), false);
        }

        // Enums
        public enum TextOptions
        {
            Text,
            File,
            SpeedOption,
            AlignOption,
            Reset,
            Exit
        }

        public enum SpeedOptions
        {
            WordsPerSecond,
            WordsPerMinute,
            SecondsPerText,
            MinutesPerText
        }
    }
}
