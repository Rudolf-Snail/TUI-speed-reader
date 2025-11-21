using static SpeedReaderTextUserInterface.Option;
using static SpeedReaderTextUserInterface.SpeedOptions;

namespace SpeedReaderTextUserInterface
{
    internal static class SpeedOption
    {
        public static SpeedOptions[] ArrayOfValues() => [WordsPerSecond, WordsPerMinute, SecondsPerText, MinutesPerText];

        public static string ValueDescriptionText(SpeedOptions value, SpeedReader? _ = null)
            => value switch
            {
                WordsPerSecond => DescriptionOfOption(WordsPerSecond, "set the amount of words per second the speed reader will display"),
                WordsPerMinute => DescriptionOfOption(WordsPerMinute, "set the amount of words per minute the speed reader will display"),
                SecondsPerText => DescriptionOfOption(SecondsPerText, "set the the amount of seconds the speed reader will display the text in"),
                MinutesPerText => DescriptionOfOption(MinutesPerText, "set the the amount of minutes the speed reader will display the text in"),
                _ => throw new ArgumentException($"The specified option — {value} — is not valid."),
            };

        public static string DescribeAllValues() => Option.DescribeAllValues(ArrayOfValues(), ValueDescriptionText);
    }

    public enum SpeedOptions
    {
        WordsPerSecond,
        WordsPerMinute,
        SecondsPerText,
        MinutesPerText
    }
}