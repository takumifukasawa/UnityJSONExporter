using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class OrbitSplineController : TimelineBindingObjectBase
{
    [JsonProperty(PropertyName = "s_h")]
    public float Height = 2;

    [JsonProperty(PropertyName = "s_hs")]
    public float HeightStep = 0.2f;

    [JsonProperty(PropertyName = "s_phc")]
    public float PivotHeightCenteringEnabled = 1f;

    [JsonProperty(PropertyName = "s_r")]
    public float Radius = 3;

    [JsonProperty(PropertyName = "s_ras")]
    public float RadiusStep = 0;
    
    [JsonProperty(PropertyName = "s_rao")]
    public float RadiusOffset = 0;

    [JsonProperty(PropertyName = "s_d")]
    public float Divide = 12;

    [JsonProperty(PropertyName = "s_wa")]
    public Vector3 WaveAmp = new Vector3(0.5f, 0.5f, 0.5f);

    [JsonProperty(PropertyName = "s_wp")]
    public Vector3 WavePeriod = new Vector3(1f, 2f, 3f);

    [JsonProperty(PropertyName = "s_wpo")]
    public Vector3 WavePeriodOffset = new Vector3(1f, 2f, 3f);

    [JsonProperty(PropertyName = "s_ro")]
    public Vector3 Rotation;

    [JsonProperty(PropertyName = "s_ros")]
    public Vector3 RotationSpeed;

    [JsonProperty(PropertyName = "s_dc")]
    public int DrawCount = 32;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[OrbitSplineController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
