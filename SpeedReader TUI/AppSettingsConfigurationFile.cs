using System.Configuration;
using static SpeedReaderTextUserInterface.Input;


namespace SpeedReaderTextUserInterface
{
    internal static class AppSettingsConfigurationFile
    {
        public static void ConfigureAlignmentSettings(out bool alignHorizontally, out bool alignVertically)
        {
            ConfigurationFileAppSettings(out Configuration configFile, out KeyValueConfigurationCollection settings);

            string message1 = "Do you wish to align the text horizontally? Type in True for yes and False for no: ";
            alignHorizontally = NonStringInput<bool>.ReceiveCorrectInputValues(message1, JustReadInput, NonStringInput<bool>.IsParsedCorrectly);
            ChangeSetting("alignHorizontally", alignHorizontally.ToString(), settings);

            string message2 = "Do you wish to align the text vertically? Type in True for yes and False for no: ";
            alignVertically = NonStringInput<bool>.ReceiveCorrectInputValues(message2, JustReadInput, NonStringInput<bool>.IsParsedCorrectly);
            ChangeSetting("alignVertically", alignVertically.ToString(), settings);

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
