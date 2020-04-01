using Kingmaker.EntitySystem.Persistence.JsonUtility;
using ModMaker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace TheLootChecklist
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DefaultLanguage : ILanguage
    {
        [JsonProperty]
        public string Language { get; set; } = "English (Default)";

        [JsonProperty]
        public Version Version { get; set; }

        [JsonProperty]
        public string Contributors { get; set; }

        [JsonProperty]
        public string HomePage { get; set; }

        [JsonProperty]
        public Dictionary<string, string> Strings { get; set; } = new Dictionary<string, string>()
        {
            { "Menu_Tab_Checklist", "Checklist" },
            { "Menu Tab_Language", "Language" },
            { "Menu_Tab_Json", "JSON Logging" },
            { "Menu_Btn_Refresh", "Refresh" },
            { "Menu_Tog_LogJSON", "Log Data to JSON" },
            { "Menu_Tog_LoggingJSON", "Logging Data to JSON" },
            { "Menu_Tog_Friendly", "Friendly/Neutral Units Shown" },
            { "Menu_Tog_Enemy", "Friendly/Neutral Units Hidden" },
            { "Menu_Label_Json_File_Exists", "{0} already exists: Will append file" },
            { "Menu_Label_Json_File_Missing", "{0} does not exist: Will create new file." },
            { "Menu_Label_Lang_Opt", "Language Settings:" },
            { "Menu_Label_JSON_Opt", "JSON Settings:" },
            { "Menu_Label_Export_Fail", "Export of {0} failed." },
            { "Menu_Label_Language", "Current: {0}" },
            { "Menu_Label_Export_Lang", "Export the default language to JSON: " },
            { "Menu_Btn_Export_Lang", "Export" },
            { "Menu_Label_Import_Lang_Failed", "Import of {0} failed." },
            { "Menu_Btn_Default_Refresh_Lang", "Refresh Languages" },
            { "Menu_Btn_Default_Lang", "Reset to Default Language" },
            { "JSON_Txt_Loot", "Ground Loot" },
            { "Menu_Tog_Stack", "Same Units Stacked" },
            { "Menu_Tog_Unstack", "Same Units Unstacked" },
            { "Menu_Tog_Guids", "GUIDs Shown" },
            { "Menu_Tog_NoGuids", "GUIDs Hidden" },
            { "Menu_Btn_Collapse", "Collapse All" },
            { "Menu_Btn_Expand", "Expand All" },
            { "Menu_Label_ExpandCollapse", "Default on refresh: " },
            { "Menu_Tog_ExpandDefault", "Expanded" },
            { "Menu_Tog_CollapseDefault", "Collapsed" }
        };

        public T Deserialize<T>(TextReader reader)
        {
            DefaultJsonSettings.Initialize();
            return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
        }

        public void Serialize<T>(TextWriter writer, T obj)
        {
            DefaultJsonSettings.Initialize();
            writer.Write(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}