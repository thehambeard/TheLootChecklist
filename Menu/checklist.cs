using System;
using System.Linq;
using Kingmaker;
using Kingmaker.PubSubSystem;
using ModMaker;
using ModMaker.Utility;
using System.Collections.Generic;
using System.Reflection;
using TheLootChecklist.Loot;
using UnityEngine;
using UnityModManagerNet;
using static TheLootChecklist.Main;
using static TheLootChecklist.Utilities.SettingsWrapper;

namespace TheLootChecklist.Menu
{
    public class Checklist : IMenuSelectablePage,
        IModEventHandler, 
        //IAreaLoadingStagesHandler,
        ISceneHandler
    {
        LootManager lootManager = new LootManager();
        
        readonly int? DCThreshold = 100;
        public string Name => Local["Menu_Tab_Checklist"];
        public int Priority => 0;

        static Dictionary<int, bool> flags = new Dictionary<int, bool>();

        public void OnGUI(UnityModManager.ModEntry modEntry)
        {
            try
            {
                if (Mod == null || !Mod.Enabled)
                    return;

                using (new GUISubScope(Local["Menu_Tab_Checklist"]))
                    this.OnGUIChecklist();
                using (new GUISubScope())
                    this.onGUIListThatLoot((ToggleStackSame ? lootManager.Entries : lootManager.StackedEntries));
            }
            catch(Exception ex)
            {
                Mod.Log(ex.Message + ex.StackTrace);
            }
        }

        private void Update(bool fromOnGUI = false)
        {
            if (fromOnGUI && (lootManager.Entries.Count > 0 ?
                lootManager.Entries.First().Value.AreaId.Contains(Game.Instance.CurrentlyLoadedArea.AssetGuid) :

                false)) 
                return;
            Mod.Debug(MethodBase.GetCurrentMethod());    
            lootManager.Update();
            flags.Clear();
            
            for (int i = 0; i < (ToggleStackSame ? lootManager.Entries.Count : lootManager.StackedEntries.Count); i++)
            {
                flags.Add(i, ToggleExpandOrCollapse);
            }
            Mod.Debug(string.Format("{0} entries found, flag count is {1}...", lootManager.Entries.Count, flags.Count));
        }

        private void OnGUIChecklist()
        {
            try
            {
                if (Mod == null || !Mod.Enabled)
                    return;
                using (new GUILayout.VerticalScope("box"))
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button(Local["Menu_Btn_Refresh"], GUILayout.ExpandWidth(false)))
                        {
                            this.Update();
                        }

                        if (GUILayout.Button((ToggleShowFriendly ? Local["Menu_Tog_Friendly"] : Local["Menu_Tog_Enemy"]), GUILayout.ExpandWidth(false)))
                        {
                            ToggleShowFriendly = !ToggleShowFriendly;
                        }
                        if (GUILayout.Button((ToggleStackSame ? Local["Menu_Tog_Unstack"] : Local["Menu_Tog_Stack"]), GUILayout.ExpandWidth(false)))
                        {
                            ToggleStackSame = !ToggleStackSame;
                            this.Update();
                        }

                        if (GUILayout.Button((ToggleGuids ? Local["Menu_Tog_Guids"] : Local["Menu_Tog_NoGuids"]), GUILayout.ExpandWidth(false)))
                        {
                            ToggleGuids = !ToggleGuids;
                        }
                    }
                }
                using (new GUILayout.VerticalScope("box"))
                {
                    using (new GUILayout.HorizontalScope())
                    {

                        if (GUILayout.Button(Local["Menu_Btn_Expand"], GUILayout.ExpandWidth(false)))
                        {
                            for (int i = 0; i < flags.Count; i++)
                            {
                                flags[i] = true;
                            }
                        }
                        if (GUILayout.Button(Local["Menu_Btn_Collapse"], GUILayout.ExpandWidth(false)))
                        {

                            for (int i = 0; i < flags.Count; i++)
                            {
                                flags[i] = false; 
                            }
                        }

                        GUILayout.Label(Local["Menu_Label_ExpandCollapse"], GUILayout.ExpandWidth(false));

                        if (GUILayout.Button((ToggleExpandOrCollapse ? Local["Menu_Tog_ExpandDefault"] : Local["Menu_Tog_CollapseDefault"]), GUILayout.ExpandWidth(false)))
                        {
                            ToggleExpandOrCollapse = !ToggleExpandOrCollapse;
                        }
#if (DEBUG)
                        if (GUILayout.Button("Test", GUILayout.ExpandWidth(false)))
                        {
                            onGUIListThatLoot(lootManager.Entries);
                        }
#endif
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Log(ex.Message + ex.StackTrace);
            }
        }

        private void onGUIListThatLoot(Dictionary<string, LootEntry> list)
        {
            try
            {
                this.Update(true);
                using (new GUILayout.VerticalScope("box"))
                {
                    int i = 0;
                    foreach (KeyValuePair<string, LootEntry> lootEntry in list.OrderBy(o => o.Value.ObjectName))
                    {
                        if (((ToggleShowFriendly || lootEntry.Value.IsPlayerEnemy) || !lootEntry.Value.IsUnit)
                            && (!(lootEntry.Value.PCDC > DCThreshold)))
                        {
                            if (flags[i] = GUIHelper.ToggleButton(flags[i], lootEntry.Value.ObjectName, new GUIStyle(GUI.skin.button), GUILayout.ExpandWidth(false)))
                            {
                                if (ToggleGuids)
                                {
                                    using (new GUILayout.VerticalScope("box"))
                                    {
                                        using (new GUILayout.HorizontalScope())
                                        {
                                            string t = GUILayout.TextField(t = (ToggleStackSame ? string.Format("{0} - {1}", lootEntry.Value.bpGuid, lootEntry.Key) : "N/A"), new GUIStyle(GUI.skin.label));
                                        }
                                    }
                                }
                                foreach (KeyValuePair<string, LootEntry.ObjectInventory> obj in lootEntry.Value.Inventory.OrderBy(o => o.Value.ItemName))
                                {
                                    using (new GUILayout.VerticalScope("box"))
                                    {
                                        using (new GUILayout.HorizontalScope())
                                        {
                                            GUILayout.Space(10f);
                                            GUILayout.Label(string.Format("{0} x{1}", obj.Value.ItemName, obj.Value.ItemCount), GUILayout.ExpandWidth(false));
                                            if (ToggleGuids)
                                            {
                                                string t = GUILayout.TextField(t = obj.Key, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperRight });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Log(ex.Message + ex.StackTrace);
            }
        }

        public void OnAreaScenesLoaded() { }
        public void OnAreaDidLoad()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            this.Update();
            if (ToggleLogJSON) LootJSON.serialJsonToFile(lootManager.Entries, PathJSON);
        }
        public void HandleModEnable()
        {
            EventBus.Subscribe(this);
        }

        public void HandleModDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnAreaBeginUnloading()
        {
            lootManager.Dispose();
            flags.Clear();
        }
    }
}
