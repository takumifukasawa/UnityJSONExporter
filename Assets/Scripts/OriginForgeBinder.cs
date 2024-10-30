using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class OriginForgeBinder : TimelineBindingObjectBase
{
    [Space(13)]
    [Header("Morph")]
    [JsonProperty(PropertyName = "cpr")]
    public float ChildPositionRadius;

    [JsonProperty(PropertyName = "mi")]
    public int MaterialIndex = 0;

    [JsonProperty(PropertyName = "rx")]
    [Range(0, 1)]
    public float RotX = 0;

    [JsonProperty(PropertyName = "mr")]
    [Range(0, 1)]
    public float MorphRate = 0;

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

    [Space(13)]
    [Header("Gather")]
    [JsonProperty(PropertyName = "gs")]
    public float GatherScaleRate = 0;

    [JsonProperty(PropertyName = "gm")]
    public float GatherMorphRate = 0;

    [JsonProperty(PropertyName = "gec")]
    public Color GatherEmissiveColor;

    [Space(13)]
    [Header("Point Light")]
    [JsonProperty(PropertyName = "pi")]
    public float PointLightIntensity = 0;
    
    [JsonProperty(PropertyName = "pd")]
    public float PointLightDistance = 15;
    
    [JsonProperty(PropertyName = "pa")]
    public float PointLightAttenuation = 1;

    [JsonProperty(PropertyName = "pc")]
    public Color PointLightColor;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[OriginForgeBinder] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
