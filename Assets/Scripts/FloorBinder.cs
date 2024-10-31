using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class FloorBinder : TimelineBindingObjectBase
{
    [JsonProperty(PropertyName = "ec")]
    public Color EmissionColor;
    
    [JsonProperty(PropertyName = "di")]
    public float DistanceIndex = 0f;
    
    [JsonProperty(PropertyName = "mr")]
    public float MorphRate = 0f;
 
    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[FloorBinder] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
