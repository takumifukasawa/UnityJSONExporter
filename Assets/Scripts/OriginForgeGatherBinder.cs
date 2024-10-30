using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class OriginForgeGatherBinder : TimelineBindingObjectBase
{
    [JsonProperty(PropertyName = "mr")]
    [Range(0, 1)]
    public float MorphRate = 0;
    
    [Space(13)]

    [JsonProperty(PropertyName = "gs")]
    public float GatherScaleRate = 0;

    [JsonProperty(PropertyName = "gm")]
    public float GatherMorphRate = 0;
    
    [JsonProperty(PropertyName = "rx")]
    public float RotateX = 0;
    
    [JsonProperty(PropertyName = "ry")]
    public float RotateY = 0;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[OriginForgeBinder] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
