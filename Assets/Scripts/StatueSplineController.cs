using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class StatueSplineController : StraightSplineController
{
    [Space(13)]
    [JsonProperty(PropertyName = "s_io")]
    public Vector3 InstanceOffset = new Vector3(0, 0, 5);

    [JsonProperty(PropertyName = "s_is")]
    public Vector3 InstanceScale = new Vector3(1, 1, 1);

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[StatueSplineController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
