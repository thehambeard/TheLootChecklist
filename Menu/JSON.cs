using ModMaker;
using ModMaker.Utility;
using System.IO;
using UnityEngine;
using UnityModManagerNet;
using static TheLootChecklist.Main;
using static TheLootChecklist.Utilities.SettingsWrapper;

namespace TheLootChecklist.Menu
{
    class JSON : IMenuSelectablePage
    {
        public string Name => Local["Menu_Tab_Json"];

        public int Priority => 400;

        public void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (Mod == null || !Mod.Enabled)
                return;

            using (new GUISubScope(Local["Menu_Tab_Json"]))
                OnGUIJson();
        }

        private void OnGUIJson()
        {
            using (new GUILayout.VerticalScope("box"))
            {
                //string apdp = (Application.persistentDataPath + "/");
                string apdp = (ModPath);
                if (string.IsNullOrEmpty(TextFieldJSONFile))
                    TextFieldJSONFile = "LootList.json";

                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button((ToggleLogJSON ? Local["Menu_Tog_LoggingJSON"] : Local["Menu_Tog_LogJSON"]), GUILayout.ExpandWidth(false)))
                    {

                        ToggleLogJSON = !ToggleLogJSON;
                    }
                    if (ToggleLogJSON)
                    {
                        if (File.Exists(apdp + TextFieldJSONFile))
                            GUILayout.Label(string.Format(Local["Menu_Label_Json_File_Exists"], TextFieldJSONFile));
                        else
                            GUILayout.Label(string.Format(Local["Menu_Label_Json_File_Missing"], TextFieldJSONFile));
                    }
                }
                GUILayout.Label(string.Format("Save Folder: {0}", apdp), GUILayout.ExpandWidth(false));
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("Save File: ", GUILayout.ExpandWidth(false));
                    TextFieldJSONFile = GUILayout.TextField(TextFieldJSONFile);
                    PathJSON = (apdp + TextFieldJSONFile);
                }
            }
        }
    }
}
