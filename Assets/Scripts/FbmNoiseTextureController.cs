using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class FbmNoiseTextureController : EffectTextureControllerBase
{
    [JsonProperty(PropertyName = FbmNoiseTextureControllerComponentInfo.EFFECT_TEXTURE_GRID_SIZE_SHORTEN_PROPERTY_NAME)]
    public Vector2 GridSize = new Vector2(8, 8);

    [JsonProperty(PropertyName = FbmNoiseTextureControllerComponentInfo.EFFECT_TEXTURE_OCTAVES_SHORTEN_PROPERTY_NAME)]
    public float Octaves = 8;

    [JsonProperty(PropertyName = FbmNoiseTextureControllerComponentInfo.EFFECT_TEXTURE_AMPLITUDE_SHORTEN_PROPERTY_NAME)]
    public float Amplitude = 0.5f;

    [JsonProperty(PropertyName = FbmNoiseTextureControllerComponentInfo.EFFECT_TEXTURE_FREQUENCY_SHORTEN_PROPERTY_NAME)]
    public float Frequency = 2;

    [JsonProperty(PropertyName = FbmNoiseTextureControllerComponentInfo.EFFECT_TEXTURE_FACTOR_SHORTEN_PROPERTY_NAME)]
    public float Factor = 1.1f;

    [JsonProperty(PropertyName = FbmNoiseTextureControllerComponentInfo.EFFECT_TEXTURE_SPEED_SHORTEN_PROPERTY_NAME)]
    public Vector2 Speed = new Vector2(1, 1);

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[FbmNoiseTextureController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
