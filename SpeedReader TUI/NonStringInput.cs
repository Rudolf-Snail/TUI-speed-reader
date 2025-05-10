using static SpeedReaderTextUserInterface.Input;

namespace SpeedReaderTextUserInterface
{
    internal static class NonStringInput<T> where T : struct
    {
        // Methods
        public static T ReceiveCorrectInputValues(string messageToWriteToConsole, ReadAndProcessInput readAndProcessInput,
                                                  SuccessCondition successCondition, string incorrectDataMessage = "This option is not correct, please try again.")
        {
            System.Reflection.MethodInfo? TryParseMethod = GetTryParseMethodBasedOnType();

            if (TryParseMethod == null)
                throw new MissingMethodException("The generic struct type you used for the Input class does not have a TryParse method.");

            T parsedValue;

            while (true)
            {
                Console.Write(messageToWriteToConsole);

                ParseValueBasedOnType(out bool parsedSuccessfully, out parsedValue, readAndProcessInput, TryParseMethod);

                if (!successCondition(parsedSuccessfully, parsedValue))
                    Console.WriteLine(incorrectDataMessage);
                else
                    break;
            }

            return parsedValue;
        }

        private static System.Reflection.MethodInfo? GetTryParseMethodBasedOnType()
        {
            System.Reflection.MethodInfo? TryParseMethod;

            if (!typeof(T).IsSubclassOf(typeof(Enum)))
                // Need to make the second parameter referential with MakeByRefType, because otherwise it won't find it, since it is an out parameter
                TryParseMethod = typeof(T).GetMethod("TryParse", [typeof(string), typeof(T).MakeByRefType()]);
            else
                // Need to make the third parameter referential with MakeByRefType, because otherwise it won't find it, since it is an out parameter
                TryParseMethod = typeof(Enum).GetMethod("TryParse", [typeof(Type), typeof(string), typeof(object).MakeByRefType()]);
            
            return TryParseMethod;
        }

        private static void ParseValueBasedOnType(out bool parsedSuccessfully, out T parsedValue,
                                                  Input.ReadAndProcessInput readAndProcessInput, System.Reflection.MethodInfo TryParseMethod)
        {
            object?[] parameters;
            int parsedValueIndex;

            if (!typeof(T).IsSubclassOf(typeof(Enum)))
            {
                parameters = [readAndProcessInput(), null];
                parsedValueIndex = 1;
            }
            else
            {
                parameters = [typeof(T), readAndProcessInput(), null];
                parsedValueIndex = 2;
            }

            parsedValue = ParseValue(readAndProcessInput, out parsedSuccessfully, TryParseMethod, parameters, parsedValueIndex);
        }

        private static T ParseValue(Input.ReadAndProcessInput readAndProcessInput, out bool parsedSuccessfully,
                                        System.Reflection.MethodInfo TryParseMethod, object?[] parameters, int parsedValueIndex)
        {
            parsedSuccessfully = ( bool? ) TryParseMethod.Invoke(null, parameters) ?? false;

            if (parsedSuccessfully)
                // This should not be a null value, because the method parsed successfully and I can't think of a good solution without suppressing the issue.
#pragma warning disable CS8605 // Unboxing a possibly null value.
                return ( T ) parameters[parsedValueIndex];
#pragma warning restore CS8605 // Unboxing a possibly null value. 
            else
                return default;
        }

        // Delegates
        public delegate bool SuccessCondition(bool parsedSuccessfully, T parsedValue);

        public static bool IsParsedCorrectly(bool parsedSuccessfully, T parsedValue)
        {
            return parsedSuccessfully;
        }

        public static bool IsPositiveNumber(bool parsedSuccessfully, decimal parsedValue)
        {
            return parsedSuccessfully && parsedValue > 0;
        }
    }
}