using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class HeroHumanController : TimelineBindingObjectBase
{
    [JsonProperty(PropertyName = "hh_gr")]
    [Range(0, 1)]
    public float GlitchRate = 0f;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[HeroHumanController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
