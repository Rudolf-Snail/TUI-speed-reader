using System.Text;

namespace SpeedReaderTextUserInterface
{
    internal static class Option
    {
        public static string DescribeAllValues<T>(T[] arrayOfValues, ValueDescriptionText<T> valueDescriptionText, SpeedReader? speedReader = null) where T: Enum
        {
            var valuesLength = arrayOfValues.Length;
            var descriptionOfValues = new string[valuesLength];

            for (int index = 0; index < valuesLength; index++)
                descriptionOfValues[index] = valueDescriptionText(arrayOfValues[index], speedReader);

            return string.Join("\n", descriptionOfValues);
        }

        public static string DescriptionOfOption<T>(T option, string message, string? currentValue = null, bool applyIfYouWishTo = true) where T : Enum
        {
            StringBuilder descriptionOfOption = new($"{Input.ToCapital(TypeInTextOrNumber(option), false)}, ") ;

            if (applyIfYouWishTo)
                descriptionOfOption.Append($"{IfYouWishTo(message)}");
            else
                descriptionOfOption.Append(message);

            if (currentValue != null)
                descriptionOfOption.Append($" — {currentValue}");

            descriptionOfOption.Append(".");

            return descriptionOfOption.ToString();
        } 

        public static string TypeInTextOrNumber<T>(T option) where T : Enum => $"type in {TextOrNumber(option)}";

        // Workaround from https://stackoverflow.com/a/51025027/23294278
        public static string TextOrNumber<T>(T option) where T: Enum => $"{option} or {(int)(object)option}";

        public static string IfYouWishTo(string message) => $"if you wish to {message}";

        public static string CurrentValue(string currentValue) => $"the current value is {currentValue}";
    }
}