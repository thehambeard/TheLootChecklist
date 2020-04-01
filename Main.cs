using ModMaker;
using ModMaker.Utility;
using System.Reflection;
using UnityModManagerNet;
using static TheLootChecklist.Utilities.SettingsWrapper;


namespace TheLootChecklist
{
#if (DEBUG)
    [EnableReloading]
#endif
    static class Main
    {
        public static LocalizationManager<DefaultLanguage> Local;
        public static ModManager<Core, Settings> Mod;
        public static MenuManager MenuMan;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            Local = new LocalizationManager<DefaultLanguage>();
            Mod = new ModManager<Core, Settings>();
            MenuMan = new MenuManager();
            modEntry.OnToggle = OnToggle;
#if (DEBUG)
            modEntry.OnUnload = Unload;
            return true;
        }

        static bool Unload(UnityModManager.ModEntry modEntry)
        {
            Mod.Disable(modEntry, true);
            MenuMan = null;
            Mod = null;
            Local = null;
            return true;
        }
#else
            return true;
        }
#endif
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (value)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Local.Enable(modEntry);
                Mod.Enable(modEntry, assembly);
                MenuMan.Enable(modEntry, assembly);
                ModPath = modEntry.Path;
            }
            else
            {
                MenuMan.Disable(modEntry);
                Mod.Disable(modEntry, false);
                Local.Disable(modEntry);
                ReflectionCache.Clear();
            }
            return true;
        }
    }
}
