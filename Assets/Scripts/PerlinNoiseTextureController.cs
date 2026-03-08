using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class PerlinNoiseTextureController : EffectTextureControllerBase
{
    [JsonProperty(PropertyName = "pn_gs")]
    public Vector2 GridSize = new Vector2(8, 8);

    [JsonProperty(PropertyName = "pn_s")]
    public Vector2 Speed = new Vector2(1, 1);

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[PerlinNoiseTextureController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
