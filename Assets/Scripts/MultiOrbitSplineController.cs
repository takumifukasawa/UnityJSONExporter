using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class MultiOrbitSplineController : OrbitSplineController
{
    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[MultiOrbitSplineController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
