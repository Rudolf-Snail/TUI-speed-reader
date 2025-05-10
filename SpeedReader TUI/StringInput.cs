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
            return string.IsNullOrEmpty(parsedValue);
        }
    }
}
