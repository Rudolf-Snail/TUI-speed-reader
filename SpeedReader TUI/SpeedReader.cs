namespace Speed_reader
{
    internal class SpeedReader
    {
        // Fields
        private decimal wordsPerMinute = 0;
        private string? text;
        private string? currentWord;

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

        // Constructors
        public SpeedReader(decimal wordsPerMinute, string? text = null)
        {
            WordsPerMinute = wordsPerMinute;
            Text = text;
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

            foreach (string word in words)
            {
                Console.Clear();

                CurrentWord = word;
                Console.WriteLine(CurrentWord);

                Task.Delay(( int ) milisecondsPerWord).Wait();
            }
        }

    }
}
