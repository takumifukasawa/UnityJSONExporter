using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class CustomPostProcessController : TimelineBindingObjectBase
{
    [Space(13)]
    [Header("Bloom")]
    [Range(0, 10)]
    [JsonProperty(PropertyName = "bl_i")]
    public float BloomIntensity;
    
    [Header("Volumetric Fog")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = "vl_rs")]
    public float VolumetricLightRayStep;
    
    [Header("Color Cover Pass")]
    [JsonProperty(PropertyName = "cbr")]
    [Range(0, 1)]
    public float ColorCoverPassBlendRate = 0;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[CustomPostProcessController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}