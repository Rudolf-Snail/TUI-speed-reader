using System;
using System.Text;
using Speed_reader;

namespace MyApp
{
    internal class Program
    {
        static int Main(string[] args)
        {
            string? text;
            decimal speed;

            if (args.Length == 0)
            {
                do
                {
                    Console.Write("Enter text for speedreading: ");
                    text = Console.ReadLine();
                }
                while (text is null);

                bool numberEntered;

                do
                {
                    Console.Write("Enter the speed you wish to read the text at — in Words Per Minute (WPM): ");
                    numberEntered = decimal.TryParse(Console.ReadLine(), out speed);
                }
                while (!numberEntered);
            }
            else
            {
                int amountOfArguments = args.Length;

                // Speed is first, and text is second
                bool isNumber = Decimal.TryParse(( string? ) args[0], out speed);

                if (amountOfArguments < 2)
                {
                    return 1;
                    throw new ArgumentException("The text to be read is empty.");
                }

                StringBuilder getText = new(amountOfArguments - 1);

                for (int index = 1; index < amountOfArguments; index++)
                    getText.Append(args[index]);

                text = getText.ToString();
            }

            SpeedReader speedReader = new(speed, text);

            speedReader.SpeedReadText();

            return 0;
        }
    }
}