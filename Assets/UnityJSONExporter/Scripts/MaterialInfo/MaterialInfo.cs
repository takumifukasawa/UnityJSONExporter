using UnityEngine;
using Newtonsoft.Json;

namespace UnityJSONExporter
{
    public enum MaterialType
    {
        None,
        Lit,
        Unlit
    }

    [System.Serializable]
    public class MaterialInfo
    {
        [JsonProperty(PropertyName = "n")]
        public string Name;

        [JsonProperty(PropertyName = "t")]
        public MaterialType Type;

        public MaterialInfo(string name, MaterialType type)
        {
            Name = name;
            Type = type;
        }
    }
}