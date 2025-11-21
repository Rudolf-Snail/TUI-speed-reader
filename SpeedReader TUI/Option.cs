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
        
        public delegate  string ValueDescriptionText(T value, SpeedReader? speedReader);
    }
}