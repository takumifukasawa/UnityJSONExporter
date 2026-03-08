using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class GBufferMaterialController : TimelineBindingObjectBase
{
    public override AnimationClipType AnimationClipType => AnimationClipType.GBufferMaterial;

    // NOTE: 一旦、プロパティの書き出し自体はしない. と思ったがダメかも
    
    // [JsonIgnore]
    // CUSTOM_BEGIN comment out
    [JsonProperty(PropertyName = "gbm_bc")]
    // CUSTOM_END
    public Color BaseColor;

    // [JsonIgnore]
    // CUSTOM_BEGIN comment out
    [JsonProperty(PropertyName = "gbm_m")]
    // CUSTOM_END
    public float Metallic;

    // [JsonIgnore]
    // CUSTOM_BEGIN comment out
    [JsonProperty(PropertyName = "gbm_r")]
    // CUSTOM_END
    public float Roughness;

    // [JsonIgnore]
    // CUSTOM_BEGIN comment out
    [JsonProperty(PropertyName = "gbm_ec")]
    // CUSTOM_END
    public Color EmissiveColor;

    // [JsonIgnore]
    // CUSTOM_BEGIN comment out
    [JsonProperty(PropertyName = "gbm_to")]
    // CUSTOM_END
    public Vector4 TextureTilingOffset = new Vector4(1, 1, 0, 0);  // xy: tiling, zw: offset

    // [JsonIgnore]
    // CUSTOM_BEGIN comment out
    [JsonProperty(PropertyName = "gbm_hs")]
    // CUSTOM_END
    public float HeightScale = 0f;

    // [JsonIgnore]
    // CUSTOM_BEGIN comment out
    [JsonProperty(PropertyName = "gbm_p0")]
    // CUSTOM_END
    public Vector4 CustomProperties0;

    // TODO: 複数テクスチャ対応用
    // [JsonProperty(PropertyName = "gbm_bc_to")]
    // public Vector4 BaseColorTextureTilingOffset;
    // [JsonProperty(PropertyName = "gbm_n_to")]
    // public Vector4 NormalTextureTilingOffset;
    // [JsonProperty(PropertyName = "gbm_mr_to")]
    // public Vector4 MetallicRoughnessTextureTilingOffset;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[GBufferMaterialControllerBinder] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
