using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class FollowerBinder : TimelineBindingObjectBase
{
    // None: 0,
    // Jump: 1,
    // Attract: 2
    [JsonProperty(PropertyName = "fm")]
    public int FollowMode = 0;
    [Range(0, 256)]
    [JsonProperty(PropertyName = "ic")]
    public float InstanceCount;

    [JsonProperty(PropertyName = "mi")]
    public int MaterialIndex;
    
    [JsonProperty(PropertyName = "mr")]
    [Range(0, 1)]
    public float MorphRate;

    [JsonProperty(PropertyName = "abp")]
    public float AttractBasePower;

    [JsonProperty(PropertyName = "amp")]
    public float AttractMinPower;

    // [JsonProperty(PropertyName = "ap")]
    // public float AttractPower;

    [JsonProperty(PropertyName = "aa")]
    public float AttractAmplitude;

    [JsonProperty(PropertyName = "dm")]
    public float DiffuseMixer = 0;
    
    [JsonProperty(PropertyName = "em")]
    public float EmissionMixer = 0;
    
    [JsonProperty(PropertyName = "ffr")]
    public float FlowerFloorRange = 10;
 
    [Space(13)]
    [Header("Surface")]
    [JsonProperty(PropertyName = "sm")]
    [Range(0, 1)]
    public float Metallic = 0;

    [JsonProperty(PropertyName = "sr")]
    [Range(0, 1)]
    public float Roughness = 0;

    [JsonProperty(PropertyName = "sdc")]
    public Color DiffuseColor;

    [JsonProperty(PropertyName = "sec")]
    public Color EmissiveColor;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[FollowerBinder] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
