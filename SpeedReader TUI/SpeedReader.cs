using System.Diagnostics;
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
        private int index = 0;

        private ReadOptions currentReadOption;
        private bool currentAlignHorizontallyOption;
        private bool currentAlignVerticallyOption;
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

        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        public ReadOptions CurrentReadOption
        {
            get
            {
                return currentReadOption;
            }
            set
            {
                currentReadOption = value;
            }
        }

        public bool CurrentAlignHorizontallyOption
        {
            get
            {
                return currentAlignHorizontallyOption;
            }
            set
            {
                currentAlignHorizontallyOption = value;
            }
        }

        public bool CurrentAlignVerticallyOption
        {
            get
            {
                return currentAlignVerticallyOption;
            }
            set
            {
                currentAlignVerticallyOption = value;
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
            CurrentReadOption = GetReadOptionConfigurationValueOrDefaultValue();
            CurrentSpeedOption = GetSpeedOptionConfigurationValueOrDefaultValue();
            CurrentAlignHorizontallyOption = GetHorizontalAlignmentConfigurationValueOrDefaultValue();
            CurrentAlignVerticallyOption = GetVerticalAlignmentConfigurationValueOrDefaultValue();
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

        private void ProcessUserInput()
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
                case TextOptions.ReadOption:
                    ConfigureReadingOptionSetting(this);
                    break;
                case TextOptions.SpeedOption:
                    ConfigureSpeedOptionSetting(this);
                    break;
                case TextOptions.AlignHorizontallyOption:
                    ConfigureHorizontalAlignmentOptionSetting(this);
                    break;
                case TextOptions.AlignVerticallyOption:
                    ConfigureVerticalAlignmentOptionSetting(this);
                    break;
                case TextOptions.ExitOption:
                    ConfigureExitOptionSetting(this);
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

        private void ProcessSpeed()
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

        private decimal MillisecondsPerWord()
        {
            decimal millisecondsPerSecond = 1000;
            decimal millisecondsPerWord = SecondsPerWord * millisecondsPerSecond;

            return millisecondsPerWord;
        }

        private void ProcessSpeedOption()
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
        private void SetSpeedIfItIsNotSet()
        {
            if (Speed != 0)
                return;

            ProcessSpeed();
            ProcessSpeedOption();
        }

        private ProcessWord ProcessWordAlignment()
        {
            ProcessWord wordProcessor;

            if (!CurrentAlignHorizontallyOption && !CurrentAlignVerticallyOption)
                wordProcessor = DoNotAlignWord;
            else if (CurrentAlignHorizontallyOption && !CurrentAlignVerticallyOption)
                wordProcessor = AlignWordHorizontally;
            else if (!CurrentAlignHorizontallyOption && CurrentAlignVerticallyOption)
                wordProcessor = AlignWordVertically;
            else
                wordProcessor = CenterWord;
            return wordProcessor;
        }

        private async Task SpeedReadWords(decimal millisecondsPerWord, ProcessWord wordProcessor)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            Stopwatch? timer; 

            foreach (string word in Words)
            {
                timer = Stopwatch.StartNew();
                
                Console.Clear();

                CurrentWord = word;

                Console.WriteLine(wordProcessor(width, height));

                timer.Stop();

                await Task.Delay(Math.Max((int)millisecondsPerWord - (int)timer.ElapsedMilliseconds, 0));

                timer.Reset();
            }


        private void ConsoleCleanUp()
        {
            Console.Clear();
            Console.Title = "Speed reader";
        }

        private string PadVertically() => string.Concat(Enumerable.Repeat("\n", Console.WindowHeight / 2));

        private string PadHorizontally(string? text = null)
        {
            CheckIfCurrentWordAndTextIsNull(text);

            if (text != null)
                return text.PadLeft((Console.WindowWidth / 2) + (text.Length / 2));
            else
#pragma warning disable CS8602 // Dereference of a possibly null reference. CurrentWord CANNOT be null here, but the compiler doesn't see it. 
                return CurrentWord.PadLeft((Console.WindowWidth / 2) + (CurrentWord.Length / 2));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private void CheckIfCurrentWordAndTextIsNull(string? text)
        {
            if (CurrentWord == null && text == null)
                throw new NullReferenceException("The CurrentWord property and the text argument are null.");
        }

        // Delegates
        private delegate string ProcessWord(string? text = null);

        private string DoNotAlignWord(string? text = null)
        {
            CheckIfCurrentWordAndTextIsNull(text);

            if (text != null)
                return text;
            else
#pragma warning disable CS8603 // Possible null reference return. CurrentWord CANNOT be null here, but the compiler doesn't see it. 
                return CurrentWord;
#pragma warning restore CS8603 // Possible null reference return.
        }

        private string AlignWordHorizontally(string? text = null) => PadHorizontally(text);

        private string AlignWordVertically(string? text = null)
        {
            CheckIfCurrentWordAndTextIsNull(text);

            if (text != null)
                return PadVertically() + text;
            else
                return PadVertically() + CurrentWord;
        }

        private string CenterWord(string? text = null) => PadVertically() + PadHorizontally(text);
    }
}
