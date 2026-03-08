using System;
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
        public virtual object[] LightData { get; protected set; }

        // Light type constants (matching PaleGL/constants.ts)
        private const int LIGHT_TYPE_DIRECTIONAL = 0;
        private const int LIGHT_TYPE_SPOT = 1;
        private const int LIGHT_TYPE_POINT = 2;

        public LightComponentInfo(Light light) : base(ComponentType.Light)
        {
            var lightTypeValue = GetLightTypeValue(light.type);
            var intensity = light.intensity;
            var color = ColorUtilities.ConvertColorToHexString(light.color);
            LightData = new object[] { lightTypeValue, intensity, color };
        }

        protected static int GetLightTypeValue(LightType lightType)
        {
            switch (lightType)
            {
                case LightType.Directional:
                    return LIGHT_TYPE_DIRECTIONAL;
                case LightType.Spot:
                    return LIGHT_TYPE_SPOT;
                case LightType.Point:
                    return LIGHT_TYPE_POINT;
                default:
                    throw new Exception($"[LightComponentInfo.GetLightTypeValue] Unsupported light type: {lightType}");
            }
        }

        public static LightComponentInfo BuildLightComponentInfo(Light light)
        {
            switch (light.type)
            {
                case LightType.Directional:
                    return new DirectionalLightComponentInfo(light);
                // case LightType.Point:
                //     return new PointLightComponentInfo(light);
                case LightType.Spot:
                    return new SpotLightComponentInfo(light);
                default:
                    throw new Exception($"[LightComponentInfo.BuildLightComponentInfo] Unsupported light type: {light.type}");
            }
        }
    }

    public class DirectionalLightComponentInfo : LightComponentInfo
    {
        public DirectionalLightComponentInfo(Light light) : base(light)
        {
            // DirectionalLight用の配列は基底クラスで設定済み: [lightType, intensity, color]
        }
    }

    public class SpotLightComponentInfo : LightComponentInfo
    {
        public SpotLightComponentInfo(Light light) : base(light)
        {
            // SpotLight用の配列: [lightType, intensity, color, range, innerSpotAngle, spotAngle]
            var lightTypeValue = GetLightTypeValue(light.type);
            var intensity = light.intensity;
            var color = ColorUtilities.ConvertColorToHexString(light.color);
            var range = light.range;
            var innerSpotAngle = light.innerSpotAngle;
            var spotAngle = light.spotAngle;
            LightData = new object[] { lightTypeValue, intensity, color, range, innerSpotAngle, spotAngle };
        }
    }
}
