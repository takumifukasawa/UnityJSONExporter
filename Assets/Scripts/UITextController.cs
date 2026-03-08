using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

[ExecuteAlways]
public class UITextController : TimelineBindingObjectBase
{
    // [JsonProperty(PropertyName = "t")]
    // public float Time;

    [JsonProperty(PropertyName = "ui_p")]
    public Vector3 Position;

    [JsonProperty(PropertyName = "ui_s")]
    public Vector3 Scale = Vector3.one;

    private void OnDidApplyAnimationProperties()
    {
    }

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[UITextController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
