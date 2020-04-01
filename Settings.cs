using UnityModManagerNet;

namespace TheLootChecklist
{
    public class Settings : UnityModManager.ModSettings
    {
        public string lastModVersion;

        public string localizationFileName;

        public bool toggleShowFriendly;

        public bool toggleStackSame;

        public bool toggleGuids;

        public string modPath;

        public bool toggleExpandOrCollapse;

        public bool toggleLogJSON;

        public string textFieldJSONFile;

        public string pathJSON;
    }
}