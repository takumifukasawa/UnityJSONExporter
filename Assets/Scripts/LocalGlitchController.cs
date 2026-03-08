using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class LocalGlitchController : TimelineBindingObjectBase
{
    [JsonProperty(PropertyName = "lg_br")]
    [Range(0, 1)]
    public float BlendRate = 0f;

    [JsonProperty(PropertyName = "lg_bs")]
    public Vector2 BillboardSize = Vector2.one;

    [JsonProperty(PropertyName = "lg_bo")]
    public Vector3 BillboardOffset = Vector3.zero;
    
    [JsonProperty(PropertyName = "lg_be")]
    public float BillboardEnabled = 1;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[LocalGlitchController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
