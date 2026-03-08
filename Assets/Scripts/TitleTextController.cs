using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

[ExecuteAlways]
public class TitleTextController : TimelineBindingObjectBase
{
    private void OnDidApplyAnimationProperties()
    {
    }

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[TitleTextController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
