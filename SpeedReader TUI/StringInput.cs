using static SpeedReaderTextUserInterface.Input;

namespace SpeedReaderTextUserInterface
{
    internal static class StringInput
    {
        // Methods
        public static string? ReceiveCorrectInputValues(string messageToWriteToConsole, string incorrectDataMessage, 
                                                        ReadAndProcessInput readAndProcessInput, SuccessCondition successCondition)
        {
            string? text;
            
            while (true)
            {
                Console.Write(messageToWriteToConsole);

                text = readAndProcessInput();

                if (!successCondition(text))
                    Console.WriteLine(incorrectDataMessage);
                else
                    break;
            }

            return text;
        }

        // Delegates
        public delegate bool SuccessCondition(string? parsedValue);

        public static bool IsNullOrEmpty(string? parsedValue)
        {
            return string.IsNullOrEmpty(parsedValue);
        }
    }
}
