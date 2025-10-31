namespace SpeedReaderTextUserInterface
{
    internal static class TextOption
    {
        public static TextOptions[] ArrayOfValues() => [TextOptions.Text, TextOptions.File, TextOptions.ReadOption, TextOptions.SpeedOption, TextOptions.AlignHorizontallyOption, TextOptions.AlignVerticallyOption, TextOptions.ExitOption, TextOptions.Reset, TextOptions.Exit];

        public static string ValueDescriptionText(TextOptions value, SpeedReader? speedReader)
        {

            if (speedReader == null)
                throw new ArgumentNullException(nameof(speedReader), "The parameter speedReader cannot be null.");

            return value switch
            {
                TextOptions.Text => $"Type in {TextOptions.Text} or {(int)TextOptions.Text}, if you wish to read text from the command line.",
                TextOptions.File => $"Type in {TextOptions.File} or {(int)TextOptions.File}, if you wish to read text from a file.",
                TextOptions.ReadOption => $"Type in {TextOptions.ReadOption} or {(int)TextOptions.ReadOption}, if you wish to change the mode of reading — the current value is {speedReader.CurrentReadOption}.",
                TextOptions.SpeedOption => $"Type in {TextOptions.SpeedOption} or {(int)TextOptions.SpeedOption}, if you wish to change the mode speed is using — the current value is {speedReader.CurrentSpeedOption}.",
                TextOptions.AlignHorizontallyOption => $"Type in {TextOptions.AlignHorizontallyOption} or {(int)TextOptions.AlignHorizontallyOption}, if you wish to change the horizontal alignment of the text — the current value is {speedReader.CurrentAlignHorizontallyOption}.",
                TextOptions.AlignVerticallyOption => $"Type in {TextOptions.AlignVerticallyOption} or {(int)TextOptions.AlignVerticallyOption}, if you wish to change the vertical alignment of the text — the current value is {speedReader.CurrentAlignVerticallyOption}.",
                TextOptions.ExitOption => $"Type in {TextOptions.ExitOption} or {(int)TextOptions.ExitOption}, if you wish to exit this program after speed reading — the current value is {speedReader.ExitAfterSpeedReading}.",
                TextOptions.Reset => $"Type in {TextOptions.Reset} or {(int)TextOptions.Reset}, if you wish to reset the alignment settings to the default values.",
                TextOptions.Exit => $"Type in {TextOptions.Exit} or {(int)TextOptions.Exit}, if you wish to exit this program.",
                _ => throw new ArgumentException($"The specified option — {value} — is not valid."),
            };
        }

        public static string DescribeAllValues(SpeedReader speedReader) => Option<TextOptions>.DescribeAllValues(ArrayOfValues(), ValueDescriptionText, speedReader);
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