namespace SpeedReaderTextUserInterface
{
    internal static class TextOption
    {
        public static TextOptions[] ArrayOfValues() => [TextOptions.Text, TextOptions.File, TextOptions.ReadOption, TextOptions.SpeedOption, TextOptions.AlignHorizontallyOption, TextOptions.AlignVerticallyOption, TextOptions.ExitOption, TextOptions.Reset, TextOptions.Exit];

        public static string ValueDescriptionText(TextOptions value, SpeedReader speedReader)
            => value switch
            {
                TextOptions.Text => $"Type in {TextOptions.Text} or {(int)TextOptions.Text}, if you wish to read text from the command line.",
                TextOptions.File => $"Type in {TextOptions.File} or {(int)TextOptions.File}, if you wish to read text from a file.",
                TextOptions.SpeedOption => $"Type in {TextOptions.SpeedOption} or {(int)TextOptions.SpeedOption}, if you wish to change the mode the speed the speed is using — the current value is {speedReader.CurrentSpeedOption}.",
                TextOptions.AlignOption => $"Type in {TextOptions.AlignOption} or {(int)TextOptions.AlignOption}, if you wish to change the alignment of the text — the current values are AlignHorizontally: {speedReader.AlignHorizontally}, AlignVertically: {speedReader.AlignVertically}.",
                TextOptions.ExitOption => $"Type in {TextOptions.ExitOption} or {(int)TextOptions.ExitOption}, if you wish to exit this program after speed reading — the current value is {speedReader.ExitAfterSpeedReading}.",
                TextOptions.Reset => $"Type in {TextOptions.Reset} or {(int)TextOptions.Reset}, if you wish to reset the alignment settings to the default values.",
                TextOptions.Exit => $"Type in {TextOptions.Exit} or {(int)TextOptions.Exit}, if you wish to exit this program.",
                _ => throw new ArgumentException($"The specified option — {value} — is not valid."),
            };
        
        public static string DescribeAllValues(SpeedReader speedReader)
        {
            var values = ArrayOfValues();
            var valuesLength = values.Length;
            var descriptionOfValues = new string[valuesLength];

            for (int index = 0; index < valuesLength; index++)
                descriptionOfValues[index] = ValueDescriptionText(values[index], speedReader);

            return string.Join("\n", descriptionOfValues);
        }
    }


    public enum TextOptions
    {
        Text,
        File,
        ReadOption,
        SpeedOption,
        AlignHorizontallyOption,
        AlignVerticallyOption,
        ExitOption,
        Reset,
        Exit
    }
}