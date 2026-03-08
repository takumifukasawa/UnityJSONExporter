using UnityEngine;
using Newtonsoft.Json;

namespace UnityJSONExporter
{
    [System.Serializable]
    [JsonConverter(typeof(MaterialInfoArrayConverter))]
    public class UnlitMaterialInfo : MaterialInfo
    {
        [JsonProperty(PropertyName = "c")]
        public string Color;

        public UnlitMaterialInfo(string name, Color color) : base(name, MaterialType.Unlit)
        {
            Color = ColorUtilities.ConvertColorToHexString(color);
        }
    }
}