using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.View.MapObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using static TheLootChecklist.Main;

namespace TheLootChecklist.Loot
{
    
    public class LootManager
    {
        public int Priority => 0;
        private LootEntry CurrentLootEntry { get; set; }

        public Dictionary<string, LootEntry> Entries { get; private set; }
        public Dictionary<string, LootEntry> StackedEntries { get; private set; }

        public LootManager()
        {
            CurrentLootEntry = new LootEntry();
            Entries = new Dictionary<string, LootEntry>();
            StackedEntries = new Dictionary<string, LootEntry>();
        }

        public void Dispose()
        {
            Entries.Clear();
            StackedEntries.Clear();
        }

        public void Update()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            this.Entries.Clear();
            this.StackedEntries.Clear();
            this.getAllData();
            this.StackEntries();
        }

        private void getAllData()
        {
            getAreaData();
            getUnitData();
            getMapObjectData();
        }
        private void AddCurrentEntry(string key)
        {
            if (!Entries.ContainsKey(key))
            {
                Entries.Add(key, CurrentLootEntry);
            }
            CurrentLootEntry = new LootEntry();
        }

        public object DeepClone(object obj)
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            object objResult = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                
                bf.Serialize(ms, obj);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }
            return objResult;
        }

        public void StackEntries()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            if (!(Entries.Count > 0)) return;
            Dictionary<string, LootEntry> copy = DeepClone(Entries) as Dictionary<string, LootEntry>;
            foreach (KeyValuePair<string, LootEntry> ent in copy)
            {
                string key = ent.Value.ObjectName;
                if (!StackedEntries.ContainsKey(key))
                {
                    StackedEntries.Add(key, ent.Value);
                }
                else
                {
                    foreach (KeyValuePair<string, LootEntry.ObjectInventory> item in ent.Value.Inventory)
                    {
                        if (!StackedEntries[key].Inventory.ContainsKey(item.Key))
                        {
                            StackedEntries[key].Inventory.Add(item.Key, item.Value);
                        }
                        else
                        {
                            StackedEntries[key].Inventory[item.Key].ItemCount += item.Value.ItemCount;
                        }
                    }
                }
            }
        }
        private void getAreaData()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            CurrentLootEntry.AreaId = Game.Instance.CurrentlyLoadedArea.AssetGuidThreadSafe;
            CurrentLootEntry.AreaName = Game.Instance.CurrentlyLoadedArea.AreaDisplayName;
        }

        private void getUnitData()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            try
            {
                foreach (UnitEntityData unit in Game.Instance.State.Units)
                {
                    if (!unit.Descriptor.State.HasCondition(UnitCondition.Unlootable) && !unit.IsPlayerFaction)
                    {
                        
                        CurrentLootEntry.bpGuid = unit.Blueprint.AssetGuid;
                        CurrentLootEntry.ObjectName = unit.CharacterName;
                        CurrentLootEntry.IsPlayerEnemy = unit.IsPlayersEnemy;
                        CurrentLootEntry.IsUnit = true;
                        foreach (ItemEntity item in from i in unit.Inventory
                                                    where i.IsLootable
                                                    select i)
                        {
                            if (!CurrentLootEntry.Inventory.ContainsKey(item.Blueprint.AssetGuid))
                            {
                                CurrentLootEntry.Inventory.Add(item.Blueprint.AssetGuid, new LootEntry.ObjectInventory()
                                {
                                    ItemCount = item.Count,
                                    ItemName = item.Blueprint.Name,
                                });
                            }
                            else
                            {
                                CurrentLootEntry.Inventory[item.Blueprint.AssetGuid].ItemCount += item.Count;
                            }
                        }

                        if (CurrentLootEntry.Inventory.Count > 0)
                            AddCurrentEntry(unit.UniqueId);
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Error(ex.StackTrace);
            }
        }

        private void getMapObjectData()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());
            try
            {
                foreach (MapObjectEntityData mapObjectEntityData in Game.Instance.State.MapObjects)
                {
                    foreach (InteractionComponent ic in mapObjectEntityData.View.Interactions)
                    {
                        if (ic.GetType() == typeof(LootComponent))
                        {
                            LootComponent playerChest = ic as LootComponent;
                            if (playerChest.LootContainerType != LootContainerType.PlayerChest)
                            {
                                LootComponent.LootPersistentData lootComponent = mapObjectEntityData.View.Data.GetComponentData<LootComponent.LootPersistentData>();
                                CurrentLootEntry.bpGuid = mapObjectEntityData.View.name;
                                CurrentLootEntry.PCDC = mapObjectEntityData.PerceptionCheckDC;
                                CurrentLootEntry.ObjectName = Local["JSON_Txt_Loot"];
                                CurrentLootEntry.IsPlayerEnemy = false;
                                CurrentLootEntry.IsUnit = false;

                                foreach (ItemEntity item in lootComponent.Loot)
                                {
                                    if (!CurrentLootEntry.Inventory.ContainsKey(item.Blueprint.AssetGuid))
                                    {
                                        CurrentLootEntry.Inventory.Add(item.Blueprint.AssetGuid, new LootEntry.ObjectInventory()
                                        {
                                            ItemCount = item.Count,
                                            ItemName = item.Blueprint.Name,
                                        });
                                    }
                                    else
                                    {
                                        CurrentLootEntry.Inventory[item.Blueprint.AssetGuid].ItemCount += item.Count;
                                    }
                                }
                                if (CurrentLootEntry.Inventory.Count > 0)
                                    AddCurrentEntry(mapObjectEntityData.UniqueId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Error(ex.StackTrace);
            }
        }

    }
}
