using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TheLootChecklist.Loot
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public class LootEntry
    {
        [JsonProperty]
        public string ObjectName { get; set; }
        [JsonProperty]
        public string bpGuid { get; set; }
        [JsonProperty]
        public string AreaId { get; set; }
        [JsonProperty]
        public string AreaName { get; set; }
        [JsonProperty]
        public bool IsUnit { get; set; }
        [JsonProperty]
        public bool IsPlayerEnemy { get; set; }
        [JsonProperty]
        public int? PCDC { get; set; }
        [JsonProperty]
        public Dictionary<string, ObjectInventory> Inventory { get; set; } = new Dictionary<string, ObjectInventory>();
        
        
        [JsonObject(MemberSerialization.OptIn)]
        [Serializable]
        public class ObjectInventory
        {
            [JsonProperty]
            public string ItemName;
            [JsonProperty]
            public int ItemCount;
        }
    }
}
