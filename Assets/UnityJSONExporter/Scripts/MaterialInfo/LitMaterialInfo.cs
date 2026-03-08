using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace UnityJSONExporter
{
    [System.Serializable]
    [JsonConverter(typeof(MaterialInfoArrayConverter))]
    public class LitMaterialInfo : MaterialInfo
    {
        [JsonProperty(PropertyName = "c")]
        public string Color;

        [JsonProperty(PropertyName = "ti")]
        public RawVector4 Tiling;

        [JsonProperty(PropertyName = "m")]
        public float Metallic;

        [JsonProperty(PropertyName = "r")]
        public float Roughness;

        [JsonProperty(PropertyName = "e")]
        public string EmissionColor;

        [JsonProperty(PropertyName = "rs")]
        public int ReceiveShadow;

        public LitMaterialInfo(
            string name,
            Color color,
            Vector2 tilingScale,
            Vector2 tilingOffset,
            float metallic,
            float roughness,
            Color emissionColor,
            int receiveShadow
        ) : base(name, MaterialType.Lit)
        {
            Color = ColorUtilities.ConvertColorToHexString(color);
            Tiling = RawVector4.FromVector2(tilingScale, tilingOffset);
            Metallic = metallic;
            Roughness = roughness;
            EmissionColor = ColorUtilities.ConvertEmissionToHexString(emissionColor);
            ReceiveShadow = receiveShadow;
        }
    }
}
