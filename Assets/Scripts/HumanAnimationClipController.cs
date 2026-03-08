using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

[ExecuteAlways]
public class HumanAnimationClipController : TimelineBindingObjectBase
{
    public enum AnimationClipName
    {
        Standing,
        Walk
    }

    [JsonProperty(PropertyName = "ac_ci")]
    public int AnimationClipIndex;

    [Space(13)]
    [SerializeField]
    private string _animationClipName;

    void OnValidate()
    {
        OnDidApplyAnimationProperties();
    }

    private void OnDidApplyAnimationProperties()
    {
        _animationClipName = Enum.GetName(typeof(AnimationClipName), AnimationClipIndex);
    }

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[HumanAnimationClipController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
