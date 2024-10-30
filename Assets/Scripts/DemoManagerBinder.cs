using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class DemoManagerBinder : TimelineBindingObjectBase
{
    [JsonProperty(PropertyName = "ga")]
    public float GreetingAlpha = 0f;
    
    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[DemoManagerBinder] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
