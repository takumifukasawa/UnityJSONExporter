using UnityEngine;
using Newtonsoft.Json;

namespace UnityJSONExporter
{
    [System.Serializable]
    public class LitMaterialInfo : MaterialInfo
    {
        [JsonProperty(PropertyName = "c")]
        public string Color;

        [JsonProperty(PropertyName = "m")]
        public float Metallic;

        [JsonProperty(PropertyName = "r")]
        public float Roughness;

        [JsonProperty(PropertyName = "rs")]
        public int ReceiveShadow;

        public LitMaterialInfo(string name, Color color, float metallic, float roughness, int receiveShadow) : base(name, MaterialType.Lit)
        {
            Color = ColorUtilities.ConvertColorToHexString(color);
            Metallic = metallic;
            Roughness = roughness;
            ReceiveShadow = receiveShadow;
        }
    }
}