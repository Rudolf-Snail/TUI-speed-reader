using static SpeedReaderTextUserInterface.Option;
using static SpeedReaderTextUserInterface.ReadOptions;

namespace SpeedReaderTextUserInterface
{
    internal static class ReadOption
    {
        public static ReadOptions[] ArrayOfValues() => [Manual, Automatic];

        public static string ValueDescriptionText(ReadOptions value, SpeedReader? _ = null)
        => value switch
        {
            Manual => DescriptionOfOption(Manual, "if you wish to read text manually"),
            Automatic => DescriptionOfOption(Automatic, "read text automatically"),
            _ => throw new ArgumentException($"The specified option — {value} — is not valid."),
        };

        public static string DescribeAllValues() => Option.DescribeAllValues(ArrayOfValues(), ValueDescriptionText);
    }

    public enum ReadOptions
    {
        Manual,
        Automatic
    }
}