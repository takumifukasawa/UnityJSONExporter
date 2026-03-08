using Newtonsoft.Json;
using UnityJSONExporter;

public class FbmNoiseTextureControllerComponentInfo : ComponentInfoBase
{
    public const string EFFECT_TEXTURE_GRID_SIZE_SHORTEN_PROPERTY_NAME = "fbm_gs";
    public const string EFFECT_TEXTURE_OCTAVES_SHORTEN_PROPERTY_NAME = "fbm_o";
    public const string EFFECT_TEXTURE_AMPLITUDE_SHORTEN_PROPERTY_NAME = "fbm_a";
    public const string EFFECT_TEXTURE_FREQUENCY_SHORTEN_PROPERTY_NAME = "fbm_f";
    public const string EFFECT_TEXTURE_FACTOR_SHORTEN_PROPERTY_NAME = "fbm_fa";
    public const string EFFECT_TEXTURE_SPEED_SHORTEN_PROPERTY_NAME = "fbm_s";

    [JsonProperty(PropertyName = EFFECT_TEXTURE_GRID_SIZE_SHORTEN_PROPERTY_NAME)]
    public RawVector2 GridSize;

    [JsonProperty(PropertyName = EFFECT_TEXTURE_OCTAVES_SHORTEN_PROPERTY_NAME)]
    public float Octaves;

    [JsonProperty(PropertyName = EFFECT_TEXTURE_AMPLITUDE_SHORTEN_PROPERTY_NAME)]
    public float Amplitude;

    [JsonProperty(PropertyName = EFFECT_TEXTURE_FREQUENCY_SHORTEN_PROPERTY_NAME)]
    public float Frequency;

    [JsonProperty(PropertyName = EFFECT_TEXTURE_FACTOR_SHORTEN_PROPERTY_NAME)]
    public float Factor;

    [JsonProperty(PropertyName = EFFECT_TEXTURE_SPEED_SHORTEN_PROPERTY_NAME)]
    public RawVector2 Speed;

    public FbmNoiseTextureControllerComponentInfo(EffectTextureControllerBase effectTextureController) : base(ComponentType.FbmNoiseTextureController)
    {
        if (effectTextureController is FbmNoiseTextureController fbm)
        {
            GridSize = RawVector2.fromVector2(fbm.GridSize);
            Octaves = fbm.Octaves;
            Amplitude = fbm.Amplitude;
            Frequency = fbm.Frequency;
            Factor = fbm.Factor;
            Speed = RawVector2.fromVector2(fbm.Speed);
        }
        else
        {
            LoggerProxy.LogWarning($"[EffectTextureControllerComponentInfo] Unsupported EffectTextureController type: {effectTextureController.GetType()}");
        }
    }
}
