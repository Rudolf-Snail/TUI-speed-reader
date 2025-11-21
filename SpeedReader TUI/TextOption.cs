using static SpeedReaderTextUserInterface.Option;
using static SpeedReaderTextUserInterface.TextOptions;
    
namespace SpeedReaderTextUserInterface
{

    internal static class TextOption
    {
        public static TextOptions[] ArrayOfValues() => [Text, TextOptions.File, TextOptions.ReadOption, TextOptions.SpeedOption, AlignHorizontallyOption, AlignVerticallyOption, ExitOption, Reset, Exit];

        public static string ValueDescriptionText(TextOptions value, SpeedReader? speedReader)
        {

            if (speedReader == null)
                throw new ArgumentNullException(nameof(speedReader), "The parameter speedReader cannot be null.");

            return value switch
            {
                Text => DescriptionOfOption(Text, "read text from the command line"),
                TextOptions.File => DescriptionOfOption(TextOptions.File, "read text from a file"),
                TextOptions.ReadOption => DescriptionOfOption(TextOptions.ReadOption, "change the mode of reading", speedReader.CurrentReadOption.ToString()),
                TextOptions.SpeedOption => DescriptionOfOption(TextOptions.SpeedOption, "change the mode speed is using", speedReader.CurrentSpeedOption.ToString()),
                AlignHorizontallyOption => DescriptionOfOption(AlignHorizontallyOption, "change the horizontal alignment of the text", speedReader.CurrentAlignHorizontallyOption.ToString()),
                AlignVerticallyOption => DescriptionOfOption(AlignVerticallyOption, "change the vertical alignment of the text", speedReader.CurrentAlignVerticallyOption.ToString()),
                ExitOption => DescriptionOfOption(ExitOption, "exit this program after speed reading", speedReader.ExitAfterSpeedReading.ToString()),
                Reset => DescriptionOfOption(Reset, "reset the alignment settings to the default values"),
                Exit => DescriptionOfOption(Exit, "exit this program"),
                _ => throw new ArgumentException($"The specified option — {value} — is not valid."),
            };
        }

        public static string DescribeAllValues(SpeedReader speedReader) => Option.DescribeAllValues(ArrayOfValues(), ValueDescriptionText, speedReader);
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