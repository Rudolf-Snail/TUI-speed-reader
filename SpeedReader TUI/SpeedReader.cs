using static SpeedReaderTextUserInterface.Input;

namespace SpeedReaderTextUserInterface
{
    internal class SpeedReader
    {
        // Fields
        private decimal speed = 0;
        private SpeedOptions speedOption;
        private string? text;

        private decimal secondsPerWord = 0;
        private string[] words = [];
        private string? currentWord;

        private bool alignHorizontally;
        private bool alignVertically;

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

        // Constructors
        public SpeedReader(decimal speed, SpeedOptions speedOption, string? text = null, bool alignHorizontally = false, bool alignVertically = false)
        {
            Speed = speed;
            SpeedOption = speedOption;
            Text = text;
            AlignHorizontally = alignHorizontally;
            AlignVertically = alignVertically;
        }

        // Methods
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
            decimal millisecondsPerWord = millisecondsPerSecond / SecondsPerWord;

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

        public void SpeedReadText()
        {
            if (Text is null)
                return;

            string[] words = ConvertTextToWords();
            decimal millisecondsPerWord = MillisecondsPerWord();

            ProcessWord wordProcessor;

            if (!AlignHorizontally && !AlignVertically)
                wordProcessor = DoNotAlignWord;
            else if (AlignHorizontally && !AlignVertically)
                wordProcessor = AlignWordHorizontally;
            else if (!AlignHorizontally && AlignVertically)
                wordProcessor = AlignWordVertically;
            else
                wordProcessor = CenterWord;

            SpeedReadWords(words, millisecondsPerWord, wordProcessor);
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

                Task.Delay(( int ) millisecondsPerWord).Wait();
            }
        }

        public static string PadVertically(int height)
        {
            return String.Concat(Enumerable.Repeat("\n", height / 2));
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
