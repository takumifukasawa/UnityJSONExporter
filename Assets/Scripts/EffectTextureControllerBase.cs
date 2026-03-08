using System;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityJSONExporter;

public class EffectTextureControllerBase : TimelineBindingObjectBase
{
    [JsonProperty("et_uc")]
    public float UseComposite = 0;

    [JsonProperty("et_ti")]
    public float TilingEnabled = 0;

    [JsonProperty("et_emm")]
    public float EdgeMaskMix = 1;

    [JsonProperty("et_rmi")]
    public float RemapMin = 0;

    [JsonProperty("et_rma")]
    public float RemapMax = 1;

    [JsonProperty("et_om")]
    public float OneMinus = 0;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[EffectTextureController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
