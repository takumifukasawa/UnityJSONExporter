using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class OriginForgeBinder : TimelineBindingObjectBase
{
    [JsonProperty(PropertyName = "cpr")]
    public float ChildPositionRadius;
    
    [JsonProperty(PropertyName = "mr")]
    [Range(0, 1)]
    public float MorphRate = 0;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        Debug.Log($"[OriginForgeBinder] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
