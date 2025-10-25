using static SpeedReaderTextUserInterface.AppSettingsConfigurationFile;
using static SpeedReaderTextUserInterface.Input;

namespace SpeedReaderTextUserInterface
{
    internal class SpeedReader
    {
        // Fields
        private decimal speed = 0;
        private SpeedOptions speedOption;

        private string? text;
        private TextOptions textOption;

        private decimal secondsPerWord = 0;
        private string[] words = [];
        private string? currentWord;

        private bool alignHorizontally;
        private bool alignVertically;
        private bool exitAfterSpeedReading;

        // Properties
        public decimal SecondsPerWord
        {
            get
            {
                return secondsPerWord;
            }
            set
            {
                secondsPerWord = value;
            }
        }

        public decimal Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        public SpeedOptions SpeedOption
        {
            get
            {
                return speedOption;
            }
            set
            {
                speedOption = value;
            }
        }

        public string? Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        public string[] Words
        {
            get
            {
                return words;
            }
            set
            {
                words = value;
            }
        }

        public string? CurrentWord
        {
            get
            {
                return currentWord;
            }
            set
            {
                currentWord = value;
            }
        }

        public bool AlignHorizontally
        {
            get
            {
                return alignHorizontally;
            }
            set
            {
                alignHorizontally = value;
            }
        }

        public bool AlignVertically
        {
            get
            {
                return alignVertically;
            }
            set
            {
                alignVertically = value;
            }
        }

        public bool ExitAfterSpeedReading
        {
            get
            {
                return exitAfterSpeedReading;
            }
            set
            {
                exitAfterSpeedReading = value;
            }
        }

        public TextOptions TextOption
        {
            get
            {
                return textOption;
            }
            set
            {
                textOption = value;
            }
        }

        // Constructors
        public SpeedReader()
        {
            SpeedOption = GetSpeedOptionConfigurationValueOrDefaultValue();
            AlignHorizontally = GetAlignmentConfigurationValueOrDefaultValue("alignHorizontally");
            AlignVertically = GetAlignmentConfigurationValueOrDefaultValue("alignVertically");
            ExitAfterSpeedReading = GetExitAfterSpeedReadingConfigurationValueOrDefaultValue();
        }

        // Methods
        public void ReadEvaluateProcessLoop()
        {
            while (true)
            {
                ProcessUserInput();

                if (TextOption == TextOptions.Exit)
                {
                    break;
                }
                else if (TextOption == TextOptions.Text || TextOption == TextOptions.File)
                {
                    ProcessSpeed();
                    SpeedReadText();

                    if (ExitAfterSpeedReading)
                        break;
                }
            }
        }

        public void ProcessUserInput()
        {
            SelectOption();
            ProcessOption();
        }

        private void SelectOption()
        {
            string message1 = "How do you wish to enter the text to speedread?\n";
            string options = "Options:\n";
            // TODO: Make this dynamic and not static 
            string text = $"Type in {TextOptions.Text} or {(int)TextOptions.Text}, if you wish to read text from the command line.\n";
            string file = $"Type in {TextOptions.File} or {(int)TextOptions.File}, if you wish to read text from a file.\n";
            string speedOption = $"Type in {TextOptions.SpeedOption} or {(int)TextOptions.SpeedOption}, if you wish to change the mode the speed the speed is using — the currently used mode is {SpeedOption}.\n";
            string alignOption = $"Type in {TextOptions.AlignOption} or {(int)TextOptions.AlignOption}, if you wish to change the alignment of the text — the currently used values are AlignHorizontally: {AlignHorizontally}, AlignVertically {AlignVertically}.\n";
            string exitOption = $"Type in {TextOptions.ExitOption} or {(int)TextOptions.ExitOption}, if you wish to exit this program after speed reading — the currently used value is {ExitAfterSpeedReading}.\n";
            string reset = $"Type in {TextOptions.Reset} or {(int)TextOptions.Reset}, if you wish to reset the alignment settings to the default values.\n";
            string exit = $"Type in {TextOptions.Exit} or {(int)TextOptions.Exit}, if you wish to exit this program.\n";
            string choice = "Type in the name or number of the option you wish to choose: ";

            string[] messages = [message1, options, text, file, speedOption, alignOption, exitOption, reset, exit, choice];

            TextOption = NonStringInput<TextOptions>.ReceiveCorrectInputValues(messages, ReadAndCapitalizeInputAndPreserveCase, NonStringInput<TextOptions>.IsParsedCorrectly);
        }

        private void ProcessOption()
        {
            switch (TextOption)
            {
                case TextOptions.Text:
                    TextUserInput();
                    break;
                case TextOptions.File:
                    FileUserInput();
                    break;
                case TextOptions.SpeedOption:
                    ConfigureSpeedOptionSettings(ref speedOption);
                    break;
                case TextOptions.AlignOption:
                    ConfigureAlignmentSettings(ref alignHorizontally, ref alignVertically);
                    break;
                case TextOptions.ExitOption:
                    ConfigureExitSettings(ref exitAfterSpeedReading);
                    break;
                case TextOptions.Reset:
                    ResetOptionSettings(out alignHorizontally, out alignVertically, out speedOption, out exitAfterSpeedReading);
                    break;
                case TextOptions.Exit:
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    throw new ArgumentException($"The specified option — {TextOption} — is not valid."); // Should not happen, but it's handled by throwing an error just in case.
            }
        }

        private void TextUserInput()
        {
            string message = "Enter text for speedreading: ";

            Text = StringInput.ReceiveCorrectInputValues(message, ReadAndTrimWhitespaces, StringInput.IsNotNullOrEmpty);
        }

        private void FileUserInput()
        {
            string message = "Enter the file path to the file you wish to speedread: ";

            string? path = StringInput.ReceiveCorrectInputValues(message, ReadAndTrimWhitespaces, StringInput.FileExists);

            // I WANT this to throw an error, if it doesn't find the path, as trying to catch the error and work around it is too difficult and not worth it for me, it's easier to crash the whole program and try again, than to figure out how to solve this issue. 
#pragma warning disable CS8604 // Possible null reference argument. Disabled because FileExists also checks if the string is null and returns false if it is, so it is (or very likely should be) impossible to get through the do-while loop with a null value.
            using (StreamReader reader = new(path, detectEncodingFromByteOrderMarks: true))
            {
                Console.OutputEncoding = reader.CurrentEncoding; // Sets the encoding to the console, otherwise the text will not display special characters of different languages.
                Text = reader.ReadToEnd();
            }
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public void ProcessSpeed()
        {
            string message = $"Enter the speed you wish to read the text at — the current speed option is {SpeedOption} — the number has to be a positive value: ";

            Speed = NonStringInput<decimal>.ReceiveCorrectInputValues(message, JustReadInput, NonStringInput<decimal>.IsPositiveNumber);
        }

        public string[] ConvertTextToWords()
        {
            if (Text is null)
                return [];

            string[] words = Text.Split(Array.Empty<string>(), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            Words = words;

            return words;
        }

        public decimal MillisecondsPerWord()
        {
            decimal millisecondsPerSecond = 1000;
            decimal millisecondsPerWord = SecondsPerWord * millisecondsPerSecond;

            return millisecondsPerWord;
        }

        public void ProcessSpeedOption()
        {
            switch (SpeedOption)
            {
                case SpeedOptions.WordsPerMinute:
                    SecondsPerWord = ConvertSpeedToWordsPerMinute();
                    break;
                case SpeedOptions.WordsPerSecond:
                    SecondsPerWord = ConvertSpeedToWordsPerSecond();
                    break;
                case SpeedOptions.MinutesPerText:
                    SecondsPerWord = ConvertSpeedToMinutesPerText();
                    break;
                case SpeedOptions.SecondsPerText:
                    SecondsPerWord = ConvertSpeedToSecondsPerText();
                    break;
                default:
                    throw new ArgumentException($"The specified option — {SpeedOption} — is not valid."); // Should not happen, but it's handled by throwing an error just in case.
            }
        }

        private decimal ConvertSpeedToWordsPerMinute() => 60 / Speed;

        private decimal ConvertSpeedToWordsPerSecond() => Speed;

        private decimal ConvertSpeedToMinutesPerText() => Speed * 60 / Words.Length;

        private decimal ConvertSpeedToSecondsPerText() => Speed / Words.Length;

        public void SpeedReadText()
        {
            if (Text is null)
                return;

            string[] words = ConvertTextToWords();

            ProcessSpeedOption();
            decimal millisecondsPerWord = MillisecondsPerWord();

            ProcessWord wordProcessor = ProcessWordAlignment();

            SpeedReadWords(words, millisecondsPerWord, wordProcessor);
        }

        private ProcessWord ProcessWordAlignment()
        {
            ProcessWord wordProcessor;

            if (!AlignHorizontally && !AlignVertically)
                wordProcessor = DoNotAlignWord;
            else if (AlignHorizontally && !AlignVertically)
                wordProcessor = AlignWordHorizontally;
            else if (!AlignHorizontally && AlignVertically)
                wordProcessor = AlignWordVertically;
            else
                wordProcessor = CenterWord;
            return wordProcessor;
        }

        private void SpeedReadWords(string[] words, decimal millisecondsPerWord, ProcessWord wordProcessor)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            foreach (string word in words)
            {
                Console.Clear();

                CurrentWord = word;
                Console.WriteLine(wordProcessor(word, width, height));

                Task.Delay((int)millisecondsPerWord).Wait();
            }

            Console.Clear();
        }

        public static string PadVertically(int height)
        {
            return string.Concat(Enumerable.Repeat("\n", height / 2));
        }

        public static string PadHorizontally(string word, int width)
        {
            return word.PadLeft(width / 2);
        }

        // Delegates
        public delegate string ProcessWord(string word, int width, int height);

        public string DoNotAlignWord(string word, int width, int height)
        {
            return word;
        }

        public string AlignWordHorizontally(string word, int width, int height)
        {
            return PadHorizontally(word, width);
        }

        public string AlignWordVertically(string word, int width, int height)
        {
            return PadVertically(height) + word;
        }

        public string CenterWord(string word, int width, int height)
        {
            return PadVertically(height) + PadHorizontally(word, width);
        }
    }
}
