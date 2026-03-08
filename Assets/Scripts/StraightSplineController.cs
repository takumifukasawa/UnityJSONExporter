using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class StraightSplineController : TimelineBindingObjectBase
{
    [JsonProperty(PropertyName = "s_sp")]
    public Vector3 StartPoint = new Vector3(10, 1, 0);

    [JsonProperty(PropertyName = "s_ep")]
    public Vector3 EndPoint = new Vector3(-10, 1, 0);

    [JsonProperty(PropertyName = "s_wa")]
    public Vector3 WaveAmp = new Vector3(0.5f, 0.5f, 0.5f);

    [JsonProperty(PropertyName = "s_wp")]
    public Vector3 WavePeriod = new Vector3(1f, 2f, 3f);

    [JsonProperty(PropertyName = "s_wpo")]
    public Vector3 WavePeriodOffset = new Vector3(1f, 2f, 3f);

    [JsonProperty(PropertyName = "s_dc")]
    public int DrawCount = 32;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[StraightSplineController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
