namespace SpeedReaderTextUserInterface
{
    internal static class Input
    {
        // Methods
        public static string? ToCapital(string? text, bool convertRestOfTextToLowerCase = true)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            else if (text.Length == 1)
                return text.ToUpper();
            else if (convertRestOfTextToLowerCase)
                return text[..1].ToUpper() + text[1..].ToLower();
            else
                return text[..1].ToUpper() + text[1..];
        }

        // Delegates
        public delegate string? ReadAndProcessInput();

        public static string? JustReadInput()
        {
            return Console.ReadLine();
        }

        public static string? ReadAndTrimWhitespaces()
        {
            string? input = JustReadInput();

            return input?.Trim();
        }

        public static string? ReadAndCapitalizeInput()
        {
            return ToCapital(JustReadInput());
        }

        // Enums
        public enum TextOptions
        {
            Text,
            File,
            Align,
            Reset
        }
    }
}
