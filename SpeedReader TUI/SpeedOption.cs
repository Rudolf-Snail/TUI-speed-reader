namespace SpeedReaderTextUserInterface
{
    internal static class SpeedOption
    {
        public static SpeedOptions[] ArrayOfValues() => [SpeedOptions.WordsPerSecond, SpeedOptions.WordsPerMinute, SpeedOptions.SecondsPerText, SpeedOptions.MinutesPerText];

        public static string ValueDescriptionText(SpeedOptions value)
            => value switch
            {
                SpeedOptions.WordsPerSecond => $"Type in {SpeedOptions.WordsPerSecond} or {(int)SpeedOptions.WordsPerSecond}, if you wish to set the amount of words per second the speed reader will display.",
                SpeedOptions.WordsPerMinute => $"Type in {SpeedOptions.WordsPerMinute} or {(int)SpeedOptions.WordsPerMinute}, if you wish to set the amount of words per minute the speed reader will display.",
                SpeedOptions.SecondsPerText => $"Type in {SpeedOptions.SecondsPerText} or {(int)SpeedOptions.SecondsPerText}, if you wish to set the the amount of seconds the speed reader will display the text in.",
                SpeedOptions.MinutesPerText => $"Type in {SpeedOptions.MinutesPerText} or {(int)SpeedOptions.MinutesPerText}, if you wish to set the the amount of minutes the speed reader will display the text in.",
                _ => throw new ArgumentException($"The specified option — {value} — is not valid."),
            };

        public static string DescribeAllValues()
        {
            var values = ArrayOfValues();
            var valuesLength = values.Length;
            var descriptionOfValues = new string[valuesLength];

            for (int index = 0; index < valuesLength; index++)
                descriptionOfValues[index] = ValueDescriptionText(values[index]);

            return string.Join("\n", descriptionOfValues);
        }
    }

    public enum SpeedOptions
    {
        WordsPerSecond,
        WordsPerMinute,
        SecondsPerText,
        MinutesPerText
    }
}