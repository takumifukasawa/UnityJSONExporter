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
    [JsonConverter(typeof(MaterialInfoArrayConverter))]
    public class MaterialInfo
    {
        // CUSTOM_BEGIN comment out
        // [JsonProperty(PropertyName = "n")]
        // public string Name;
        // CUSTOM_END

        [JsonProperty(PropertyName = "t")]
        public MaterialType Type;

        public MaterialInfo(string name, MaterialType type)
        {
            // CUSTOM_BEGIN comment out
            // 一旦nameは使わない
            // Name = name;
            // CUSTOM_END
            Type = type;
        }
    }
}
