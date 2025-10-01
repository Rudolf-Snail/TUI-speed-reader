using System.Configuration;
using static SpeedReaderTextUserInterface.Input;


namespace SpeedReaderTextUserInterface
{
    internal static class AppSettingsConfigurationFile
    {
        public static void ConfigureSpeedOptionSettings(ref SpeedOptions speedOption)
        {
            ConfigurationFileAppSettings(out Configuration configFile, out KeyValueConfigurationCollection settings);

            string message1 = "What speed option do you wish to use?\n";
            string options = "Options:\n";
            // TODO: Make this dynamic and not static 
            string wordsPerSecond = $"Type in WordsPerSecond or {(int)SpeedOptions.WordsPerSecond}, if you wish to set the amount of words per second the speed reader will display.\n";
            string wordsPerMinute = $"Type in WordsPerMinute or {(int)SpeedOptions.WordsPerMinute}, if you wish to set the amount of words per minute the speed reader will display.\n";
            string secondsPerText = $"Type in SecondsPerText or {(int)SpeedOptions.SecondsPerText}, if you wish to set the the amount of seconds the speed reader will display the text in.\n";
            string minutesPerText = $"Type in MinutesPerText or {(int)SpeedOptions.MinutesPerText}, if you wish to set the the amount of minutes the speed reader will display the text in.\n";
            string choice = $"Type in the name or number of the option you wish to choose — current option is {speedOption}: ";

            string[] messages = [message1, options, wordsPerSecond, wordsPerMinute, secondsPerText, minutesPerText, choice];

            speedOption = NonStringInput<SpeedOptions>.ReceiveCorrectInputValues(messages, ReadAndCapitalizeInputAndPreserveCase, NonStringInput<SpeedOptions>.IsParsedCorrectly);
            ChangeSetting("speedOption", speedOption.ToString(), settings);

            SaveSettings(configFile);
            ReloadSettings(configFile);
        }

        public static void ConfigureAlignmentSettings(ref bool alignHorizontally, ref bool alignVertically)
        {
            ConfigurationFileAppSettings(out Configuration configFile, out KeyValueConfigurationCollection settings);

            string message1 = $"Do you wish to align the text horizontally? Type in True for yes and False for no — current value is {alignHorizontally}: ";
            alignHorizontally = NonStringInput<bool>.ReceiveCorrectInputValues(message1, JustReadInput, NonStringInput<bool>.IsParsedCorrectly);
            ChangeSetting("alignHorizontally", alignHorizontally.ToString(), settings);

            string message2 = $"Do you wish to align the text vertically? Type in True for yes and False for no — current value is {alignVertically}: ";
            alignVertically = NonStringInput<bool>.ReceiveCorrectInputValues(message2, JustReadInput, NonStringInput<bool>.IsParsedCorrectly);
            ChangeSetting("alignVertically", alignVertically.ToString(), settings);

            SaveSettings(configFile);
            ReloadSettings(configFile);
        }

        public static void ConfigureExitSettings(ref bool exitAfterSpeedReading)
        {
            ConfigurationFileAppSettings(out Configuration configFile, out KeyValueConfigurationCollection settings);

            string message = $"Do you wish to exit after speed reading through text? Type in True for yes and False for no — current vallue is {exitAfterSpeedReading}: ";
            exitAfterSpeedReading = NonStringInput<bool>.ReceiveCorrectInputValues(message, JustReadInput, NonStringInput<bool>.IsParsedCorrectly);
            ChangeSetting("exit", exitAfterSpeedReading.ToString(), settings);

            SaveSettings(configFile);
            ReloadSettings(configFile);
        }

        public static void ResetAlignmentSettings(out bool alignHorizontally, out bool alignVertically)
        {
            ConfigurationFileAppSettings(out Configuration configFile, out KeyValueConfigurationCollection settings);

            ChangeSetting("alignVertically", "false", settings);
            alignHorizontally = false;
            ChangeSetting("alignVertically", "false", settings);
            alignVertically = false;

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
    }
}
