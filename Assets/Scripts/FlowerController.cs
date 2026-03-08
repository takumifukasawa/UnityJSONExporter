using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class FlowerController : TimelineBindingObjectBase
{
    [JsonProperty(PropertyName = "f_re")]
    public float Repeat = 12f;

    [JsonProperty(PropertyName = "f_ro")]
    public float Rotate = 0f;

    [JsonProperty(PropertyName = "f_s")]
    public float Scale = 1f;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[FlowerHumanController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
