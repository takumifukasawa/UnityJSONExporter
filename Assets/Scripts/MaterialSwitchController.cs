using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class MaterialSwitchController : TimelineBindingObjectBase
{
    [JsonProperty("ms_mi")]
    public int MaterialIndex;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[MaterialSwitchController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
