namespace SpeedReaderTextUserInterface
{
    internal static class ReadOption
    {
        public static ReadOptions[] ArrayOfValues() => [ReadOptions.Manual, ReadOptions.Automatic];

        public static string ValueDescriptionText(ReadOptions value, SpeedReader? _ = null)
        => value switch
        {
            ReadOptions.Manual => $"Type in {ReadOptions.Manual} or {(int)ReadOptions.Manual}, if you wish to read text manually.",
            ReadOptions.Automatic => $"Type in {ReadOptions.Automatic} or {(int)ReadOptions.Automatic}, if you wish to read text automatically.",
            _ => throw new ArgumentException($"The specified option — {value} — is not valid."),
        };

        public static string DescribeAllValues() => Option<ReadOptions>.DescribeAllValues(ArrayOfValues(), ValueDescriptionText);
    }

    public enum ReadOptions
    {
        Manual,
        Automatic
    }
}