namespace SpeedReaderTextUserInterface
{
    internal static class Option<T>
    {
        public delegate  string ValueDescriptionText(T value, SpeedReader? speedReader);
    }
}