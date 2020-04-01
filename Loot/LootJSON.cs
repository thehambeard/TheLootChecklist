using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static TheLootChecklist.Main;

namespace TheLootChecklist.Loot
{
    class LootJSON
    {
        public static void serialJsonToFile(Dictionary<string, LootEntry> entries, string path)
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            string output = JsonConvert.SerializeObject(entries);
            try
            {
                if (!File.Exists(path))
                    File.Create(path).Close();
                
                if (entries.Count > 0 ? !isAlreadyLogged(entries.First().Value.AreaId, path) : false)
                {
                    using (TextWriter tw = File.AppendText(path))
                    {   
                        tw.Write(output + "\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Error(ex.Message + ex.StackTrace);
            }
        }

        public static Dictionary<string, LootEntry> deserialFileToEntries(string path)
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            Dictionary<string, LootEntry> result = new Dictionary<string, LootEntry>();
            try
            {
                
                if (!File.Exists(path) || new FileInfo(path).Length == 0)
                        return (new Dictionary<string, LootEntry>());

                using (TextReader tr = File.OpenText(path))
                {
                    string input = tr.ReadToEnd();
                    result = JsonConvert.DeserializeObject<Dictionary<string, LootEntry>>(input);
                }
            }
            catch (Exception ex)
            {
                Mod.Error(ex.Message + ex.StackTrace);
            }
            return (result);
        }

        private static bool isAlreadyLogged(string areaId, string path)
        {
            try
            {
                Mod.Debug(MethodBase.GetCurrentMethod());
                if (!File.Exists(path + ".AreaData"))
                    File.Create(path + ".AreaData").Close();
                using (TextReader tr = File.OpenText(path + ".AreaData"))
                {
                    if (tr.ReadToEnd().Contains(areaId)) return (true);
                }
                using (TextWriter tw = File.AppendText(path + ".AreaData"))
                {
                    tw.WriteLine(areaId);
                    return (false);
                }
            }
            catch(Exception ex)
            {
                Mod.Error(ex.Message + ex.StackTrace);
                return false;
            }
        }
    }
}
