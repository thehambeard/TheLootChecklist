using static TheLootChecklist.Main;

namespace TheLootChecklist.Utilities
{
    public static class SettingsWrapper
    {
        public static bool ToggleStackSame
        {
            get => Mod.Settings.toggleStackSame;
            set => Mod.Settings.toggleStackSame = value;
        }

        public static bool ToggleExpandOrCollapse
        {
            get => Mod.Settings.toggleExpandOrCollapse;
            set => Mod.Settings.toggleExpandOrCollapse = value;
        }

        public static bool ToggleGuids
        {
            get => Mod.Settings.toggleGuids;
            set => Mod.Settings.toggleGuids = value;
        }
        public static string LocalizationFileName
        {
            get => Mod.Settings.localizationFileName;
            set => Mod.Settings.localizationFileName = value;
        }

        public static bool ToggleShowFriendly
        {
            get => Mod.Settings.toggleShowFriendly;
            set => Mod.Settings.toggleShowFriendly = value;
        }

        public static bool ToggleLogJSON
        {
            get => Mod.Settings.toggleLogJSON;
            set => Mod.Settings.toggleLogJSON = value;
        }

        public static string TextFieldJSONFile
        {
            get => Mod.Settings.textFieldJSONFile;
            set => Mod.Settings.textFieldJSONFile = value;
        }

        public static string PathJSON
        {
            get => Mod.Settings.pathJSON;
            set => Mod.Settings.pathJSON = value;
        }

        public static string ModPath
        {
            get => Mod.Settings.modPath;
            set => Mod.Settings.modPath = value;
        }
    }
}