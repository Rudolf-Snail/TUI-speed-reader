using static SpeedReaderTextUserInterface.Input;

namespace SpeedReaderTextUserInterface
{
    internal static class StringInput
    {
        // Methods
        public static string? ReceiveCorrectInputValues(string messageToWriteToConsole, ReadAndProcessInput readAndProcessInput,
                                                        SuccessCondition successCondition, string incorrectDataMessage = "This option is not correct, please try again.")
        {
            string? text;

            while (true)
            {
                Console.Write(messageToWriteToConsole);

                text = readAndProcessInput();

                if (!successCondition(ref text))
                    Console.WriteLine(incorrectDataMessage);
                else
                    break;
            }

            return text;
        }

        // Delegates
        public delegate bool SuccessCondition(ref string? parsedValue);

        public static bool IsNotNullOrEmpty(ref string? parsedValue)
        {
            return !string.IsNullOrEmpty(parsedValue);
        }

        public static bool FileExists(ref string? parsedValue)
        {
            if (IsNotNullOrEmpty(ref parsedValue))
            {
                // Once again: this is checked, but the code analysis/compiler doesn't see it.
                #pragma warning disable CS8602 // Dereference of a possibly null reference.
                parsedValue = parsedValue.Trim('\"');
                #pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            return File.Exists(parsedValue);
        }
    }
}
