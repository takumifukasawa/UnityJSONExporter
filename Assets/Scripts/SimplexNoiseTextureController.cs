using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class SimplexNoiseTextureController : EffectTextureControllerBase
{
    [JsonProperty(PropertyName = "smp_gs")]
    public Vector2 GridSize = new Vector2(8, 8);

    [JsonProperty(PropertyName = "smp_s")]
    public Vector2 Speed = new Vector2(1, 1);

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[SimplexNoiseTextureController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
