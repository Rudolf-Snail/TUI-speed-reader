namespace Speed_reader
{
    internal class SpeedReader
    {
        // Fields
        private decimal wordsPerMinute = 0;
        private string? text;
        private string? currentWord;

        private bool alignHorizontally;
        private bool alignVertically;

        // Properties
        public decimal WordsPerMinute
        {
            get
            {
                return wordsPerMinute;
            }
            set
            {
                wordsPerMinute = value;
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
        public SpeedReader(decimal wordsPerMinute, string? text = null, bool alignHorizontally = false, bool alignVertically = false)
        {
            WordsPerMinute = wordsPerMinute;
            Text = text;
            AlignHorizontally = alignHorizontally;
            AlignVertically = alignVertically;
        }

        // Methods
        public string[] ConvertTextToWords()
        {
            if (Text is null)
                return Array.Empty<string>();

            string[] words = Text.Split(Array.Empty<string>(), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            return words;
        }

        public decimal MilisecondsPerWord()
        {
            decimal milisecondsInAMinute = 60 * 1000;
            decimal milisecondsPerWord = milisecondsInAMinute / WordsPerMinute;

            return milisecondsPerWord;
        }

        public void SpeedReadText()
        {
            if (Text is null)
                return;

            string[] words = ConvertTextToWords();
            decimal milisecondsPerWord = MilisecondsPerWord();

            ProcessWord wordProcessor;

            if (!AlignHorizontally && !AlignVertically)
            {
                wordProcessor = DoNotAlignWord;
                SpeedReadWords(words, milisecondsPerWord, wordProcessor);
            }
            else if (AlignHorizontally && !AlignVertically)
            {
                wordProcessor = AlignWordHorizontally;
                SpeedReadWords(words, milisecondsPerWord, wordProcessor);
            }
            else if (!AlignHorizontally && AlignVertically)
            {
                wordProcessor = AlignWordVertically;
                SpeedReadWords(words, milisecondsPerWord, wordProcessor);
            }
            else
            {
                wordProcessor = CenterWord;
                SpeedReadWords(words, milisecondsPerWord, wordProcessor);
            } 
        }

        private void SpeedReadWords(string[] words, decimal milisecondsPerWord, ProcessWord wordProcessor)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            foreach (string word in words)
            {
                Console.Clear();

                CurrentWord = word;
                Console.WriteLine(wordProcessor(word, width, height));

                Task.Delay(( int ) milisecondsPerWord).Wait();
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
