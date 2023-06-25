using UnityEngine;
using Newtonsoft.Json;

namespace UnityJSONExporter
{
        /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class LightComponentInfo : ComponentInfoBase
    {
        [JsonProperty(PropertyName = "l")]
        public string LightType;

        [JsonProperty(PropertyName = "c")]
        public string Color;

        public LightComponentInfo(Light light) : base(ComponentType.Light)
        {
            LightType = light.type.ToString();
            Color = ColorUtilities.ConvertColorToHexString(light.color);
        }
    }

}
