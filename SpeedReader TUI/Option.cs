namespace SpeedReaderTextUserInterface
{
    internal static class Option<T>
    {
        public static string DescribeAllValues(T[] arrayOfValues, ValueDescriptionText valueDescriptionText, SpeedReader? speedReader = null)
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