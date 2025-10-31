using System.Configuration;
using static SpeedReaderTextUserInterface.Input;

namespace SpeedReaderTextUserInterface
{
    internal static class AppSettingsConfigurationFile
    {
        public static void ConfigureReadingOptionSetting(SpeedReader speedReader)
        {
            string message = "What reading option do you wish to use?\n";
            string options = "Options:\n";
            string readOptionsWithDescriptions = ReadOption.DescribeAllValues();
            string choice = $"\nType in the name or number of the option you wish to choose — current option is {speedReader.CurrentReadOption} — and press the Enter key: ";

            string[] messages = [message, options, readOptionsWithDescriptions, choice];

            ConfigureSetting(speedReader, messages, SetCurrentReadOptionPropertyAndChangeSetting);
        }

        public static void ConfigureSpeedOptionSetting(SpeedReader speedReader)
        {
            string message1 = "What speed option do you wish to use?\n";
            string options = "Options:\n";
            string speedOptionsWithDescriptions = SpeedOption.DescribeAllValues();
            string choice = $"\nType in the name or number of the option you wish to choose — current option is {speedReader.CurrentSpeedOption} — and press the Enter key: ";

            string[] messages = [message1, options, speedOptionsWithDescriptions, choice];

            ConfigureSetting(speedReader, messages, SetCurrentSpeedOptionPropertyAndChangeSetting);
        }

        public static void ConfigureHorizontalAlignmentOptionSetting(SpeedReader speedReader)
        {
            string message = $"Do you wish to align the text horizontally? Type in True for yes and False for no — current value is {speedReader.CurrentAlignHorizontallyOption} — and press the Enter key: ";

            ConfigureSetting(speedReader, message, SetCurrentAlignHorizontallyOptionPropertyAndChangeSetting);
        }


        public static void ConfigureVerticalAlignmentOptionSetting(SpeedReader speedReader)
        {
            string message = $"Do you wish to align the text vertically? Type in True for yes and False for no — current value is {speedReader.CurrentAlignVerticallyOption} — and press the Enter key: ";

            ConfigureSetting(speedReader, message, SetCurrentAlignVerticallyOptionPropertyAndChangeSetting);
        }

        public static void ConfigureExitOptionSetting(SpeedReader speedReader)
        {
            string message = $"Do you wish to exit after speed reading through text? Type in True for yes and False for no — current value is {speedReader.ExitAfterSpeedReading} — and press the Enter key: ";

            ConfigureSetting(speedReader, message, SetCurrentExitAfterSpeedReadingOptionPropertyAndChangeSetting);
        }

        public static void ConfigureSetting(SpeedReader speedReader, string[] messagesToJoinTogether, SetPropertyAndChangeSetting setProperty)
        {
            string message = string.Concat(messagesToJoinTogether);

            ConfigureSetting(speedReader, message, setProperty);
        }

        public static void ConfigureSetting(SpeedReader speedReader, string message, SetPropertyAndChangeSetting setProperty)
        {
            ConfigurationFileAppSettings(out Configuration configFile, out KeyValueConfigurationCollection settings);

            setProperty(speedReader, settings, message);

            SaveSettings(configFile);
            ReloadSettings(configFile);
        }

        public static void ResetOptionSettings(SpeedReader speedReader)
        {
            ConfigurationFileAppSettings(out Configuration configFile, out KeyValueConfigurationCollection settings);

            ChangeSetting("readOption", ReadOptions.Manual.ToString(), settings);
            
            ChangeSetting("alignVertically", "false", settings);
            speedReader.CurrentAlignHorizontallyOption = false;
            ChangeSetting("alignVertically", "false", settings);
            speedReader.CurrentAlignVerticallyOption = false;

            ChangeSetting("speedOption", SpeedOptions.WordsPerSecond.ToString(), settings);
            speedReader.CurrentSpeedOption = SpeedOptions.WordsPerSecond;

            ChangeSetting("exitAfterSpeedReading", "false", settings);
            speedReader.ExitAfterSpeedReading = false;

            SaveSettings(configFile);
            ReloadSettings(configFile);
        }

        public static void ConfigurationFileAppSettings(out Configuration configFile, out KeyValueConfigurationCollection settings)
        {
            configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            settings = configFile.AppSettings.Settings;
        }

        public static void ChangeSetting(string key, string value, KeyValueConfigurationCollection settings)
        {
            settings[key].Value = value;
        }

        public static void SaveSettings(Configuration configFile)
        {
            configFile.Save(ConfigurationSaveMode.Modified);
        }

        public static void ReloadSettings(Configuration configFile)
        {
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

        public static ReadOptions GetReadOptionConfigurationValueOrDefaultValue() => GetConfigurationManagerValueOrDefaultValue<ReadOptions>("readOption");

        public static bool GetHorizontalAlignmentConfigurationValueOrDefaultValue() => GetConfigurationManagerValueOrDefaultValue<bool>("alignHorizontally");

        public static bool GetVerticalAlignmentConfigurationValueOrDefaultValue() => GetConfigurationManagerValueOrDefaultValue<bool>("alignVertically");

        public static SpeedOptions GetSpeedOptionConfigurationValueOrDefaultValue() => GetConfigurationManagerValueOrDefaultValue<SpeedOptions>("speedOption");

        public static bool GetExitAfterSpeedReadingConfigurationValueOrDefaultValue() => GetConfigurationManagerValueOrDefaultValue<bool>("exitAfterSpeedReading");

        public static T GetConfigurationManagerValueOrDefaultValue<T>(string value) where T : struct
        {
            var TryParseMethod = NonStringInput<T>.GetTryParseMethodBasedOnType($"Type {nameof(T)} does not have a TryParse method.");

            object?[] parameters;
            int parsedValueIndex;

            if (!typeof(T).IsSubclassOf(typeof(Enum)))
            {
                parameters = [ConfigurationManager.AppSettings[value], null];
                parsedValueIndex = 1;
            }
            else
            {
                parameters = [typeof(T), ConfigurationManager.AppSettings[value], null];
                parsedValueIndex = 2;
            }

            return NonStringInput<T>.ParseValue(out bool parsedSuccessfully, TryParseMethod, parameters, parsedValueIndex);
        }

        // Delegates
        public delegate void SetPropertyAndChangeSetting(SpeedReader speedReader, KeyValueConfigurationCollection settings, string message);

        public static void SetCurrentReadOptionPropertyAndChangeSetting(SpeedReader speedReader, KeyValueConfigurationCollection settings, string message)
        {
            speedReader.CurrentReadOption = NonStringInput<ReadOptions>.ReceiveCorrectInputValues(message, ReadAndCapitalizeInputAndPreserveCase, NonStringInput<ReadOptions>.IsParsedCorrectly);
            ChangeSetting("readOption", speedReader.CurrentReadOption.ToString(), settings);
        }

        public static void SetCurrentSpeedOptionPropertyAndChangeSetting(SpeedReader speedReader, KeyValueConfigurationCollection settings, string message)
        {
            speedReader.CurrentSpeedOption = NonStringInput<SpeedOptions>.ReceiveCorrectInputValues(message, ReadAndCapitalizeInputAndPreserveCase, NonStringInput<SpeedOptions>.IsParsedCorrectly);
            ChangeSetting("speedOption", speedReader.CurrentSpeedOption.ToString(), settings);
        }

        public static void SetCurrentAlignHorizontallyOptionPropertyAndChangeSetting(SpeedReader speedReader, KeyValueConfigurationCollection settings, string message)
        {
            speedReader.CurrentAlignHorizontallyOption = NonStringInput<bool>.ReceiveCorrectInputValues(message, JustReadInput, NonStringInput<bool>.IsParsedCorrectly);
            ChangeSetting("alignHorizontally", speedReader.CurrentAlignHorizontallyOption.ToString(), settings);
        }

        public static void SetCurrentAlignVerticallyOptionPropertyAndChangeSetting(SpeedReader speedReader, KeyValueConfigurationCollection settings, string message)
        {
            speedReader.CurrentAlignVerticallyOption = NonStringInput<bool>.ReceiveCorrectInputValues(message, JustReadInput, NonStringInput<bool>.IsParsedCorrectly);
            ChangeSetting("alignVertically", speedReader.CurrentAlignVerticallyOption.ToString(), settings);
        }

        public static void SetCurrentExitAfterSpeedReadingOptionPropertyAndChangeSetting(SpeedReader speedReader, KeyValueConfigurationCollection settings, string message)
        {
            speedReader.ExitAfterSpeedReading = NonStringInput<bool>.ReceiveCorrectInputValues(message, JustReadInput, NonStringInput<bool>.IsParsedCorrectly);
            ChangeSetting("exitAfterSpeedReading", speedReader.ExitAfterSpeedReading.ToString(), settings);
        }
    }
}
