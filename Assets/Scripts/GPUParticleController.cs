using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

[ExecuteAlways]
public class GPUParticleController : TimelineBindingObjectBase
{
    [JsonProperty(PropertyName = "gp_ui")]
    public int UpdaterIndex;

    [JsonProperty(PropertyName = "gp_ic")]
    public int InstanceCount = 32;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[GPUParticleController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
