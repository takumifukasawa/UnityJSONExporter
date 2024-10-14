using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class FollowerBinder : TimelineBindingObjectBase
{
    [JsonProperty(PropertyName = "fm")]
    public int FollowMode = 0;
    
    [JsonProperty(PropertyName = "ic")]
    public float InstanceCount;

    [JsonProperty(PropertyName = "mi")]
    public int MaterialIndex;
    
    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        Debug.Log($"[FollowerBinder] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
