using System.Threading.Tasks;
using static SpeedReaderTextUserInterface.AppSettingsConfigurationFile;
using static SpeedReaderTextUserInterface.Input;

namespace SpeedReaderTextUserInterface
{
    internal class SpeedReader
    {
        // Fields
        private decimal speed = 0;
        private SpeedOptions currentSpeedOption;

        private string? text;
        private TextOptions currentTextOption;

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

        public SpeedOptions CurrentSpeedOption
        {
            get
            {
                return currentSpeedOption;
            }
            set
            {
                currentSpeedOption = value;
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

        public TextOptions CurrentTextOption
        {
            get
            {
                return currentTextOption;
            }
            set
            {
                currentTextOption = value;
            }
        }

        // Constructors
        public SpeedReader()
        {
            CurrentSpeedOption = GetSpeedOptionConfigurationValueOrDefaultValue();
            AlignHorizontally = GetAlignmentConfigurationValueOrDefaultValue("alignHorizontally");
            AlignVertically = GetAlignmentConfigurationValueOrDefaultValue("alignVertically");
            ExitAfterSpeedReading = GetExitAfterSpeedReadingConfigurationValueOrDefaultValue();
        }

        // Methods
        public async Task ReadEvaluateProcessLoop()
        {
            while (true)
            {
                ProcessUserInput();

                if (CurrentTextOption == TextOptions.Exit)
                {
                    break;
                }
                else if (CurrentTextOption == TextOptions.Text || CurrentTextOption == TextOptions.File)
                {
                    ProcessSpeed();
                    await SpeedReadText();

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
            string textOptionsWithDescriptions = TextOption.DescribeAllValues(this);
            string choice = "\nType in the name or number of the option you wish to choose: ";

            string[] messages = [message1, options, textOptionsWithDescriptions, choice];

            CurrentTextOption = NonStringInput<TextOptions>.ReceiveCorrectInputValues(messages, ReadAndCapitalizeInputAndPreserveCase, NonStringInput<TextOptions>.IsParsedCorrectly);
        }

        private void ProcessOption()
        {
            switch (CurrentTextOption)
            {
                case TextOptions.Text:
                    TextUserInput();
                    break;
                case TextOptions.File:
                    FileUserInput();
                    break;
                case TextOptions.SpeedOption:
                    ConfigureSpeedOptionSettings(this);
                    break;
                case TextOptions.AlignOption:
                    ConfigureAlignmentSettings(this);
                    break;
                case TextOptions.ExitOption:
                    ConfigureExitSettings(this);
                    break;
                case TextOptions.Reset:
                    ResetOptionSettings(this);
                    break;
                case TextOptions.Exit:
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    throw new ArgumentException($"The specified option — {CurrentTextOption} — is not valid."); // Should not happen, but it's handled by throwing an error just in case.
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
            string message = $"Enter the speed you wish to read the text at — the current speed option is {CurrentSpeedOption} — the number has to be a positive value: ";

            Speed = NonStringInput<decimal>.ReceiveCorrectInputValues(message, JustReadInput, NonStringInput<decimal>.IsPositiveNumber);
        }

        public string[] ConvertTextToWords()
        {
            if (Text is null)
                return [];

            string[] words = Text.Split(Array.Empty<string>(), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

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
            switch (CurrentSpeedOption)
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
                    throw new ArgumentException($"The specified option — {CurrentSpeedOption} — is not valid."); // Should not happen, but it's handled by throwing an error just in case.
            }
        }

        private decimal ConvertSpeedToWordsPerMinute() => 60 / Speed;

        private decimal ConvertSpeedToWordsPerSecond() => 1 / Speed;

        private decimal ConvertSpeedToMinutesPerText() => Speed * 60 / Words.Length;

        private decimal ConvertSpeedToSecondsPerText() => Speed / Words.Length;

        public async Task SpeedReadText()
        {
            if (Text is null)
                return;

            Words = ConvertTextToWords();

            ProcessSpeedOption();
            decimal millisecondsPerWord = MillisecondsPerWord();

            ProcessWord wordProcessor = ProcessWordAlignment();

            await SpeedReadWords(millisecondsPerWord, wordProcessor);
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

        private async Task SpeedReadWords(decimal millisecondsPerWord, ProcessWord wordProcessor)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            foreach (string word in Words)
            {
                Console.Clear();

                CurrentWord = word;
                Console.WriteLine(wordProcessor(width, height));

                await Task.Delay((int)millisecondsPerWord);
            }

            Console.Clear();
        }

        public string PadVertically(int height) => string.Concat(Enumerable.Repeat("\n", height / 2));

        public string PadHorizontally(int width)
        {
            if (CurrentWord == null)
                throw new ArgumentNullException(CurrentWord, "The CurrentWord property contains a null value.");

            return CurrentWord.PadLeft(width / 2);
        }

        // Delegates
        public delegate string ProcessWord(int width, int height);

        public string DoNotAlignWord(int width, int height)
        {
            if (CurrentWord == null)
                throw new ArgumentNullException(CurrentWord, "The CurrentWord property contains a null value.");

            return CurrentWord;
        }

        public string AlignWordHorizontally(int width, int height) => PadHorizontally(width);

        public string AlignWordVertically(int width, int height) => PadVertically(height) + CurrentWord;

        public string CenterWord(int width, int height) => PadVertically(height) + PadHorizontally(width);
    }
}
