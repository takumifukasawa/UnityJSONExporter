using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

[ExecuteAlways]
public class CameraController : TimelineBindingObjectBase
{
    [SerializeField]
    private Camera _camera;

    [JsonProperty(PropertyName = "nc")]
    public float Near;

    [JsonProperty(PropertyName = "fc")]
    public float Far;

    [JsonProperty(PropertyName = "f")]
    public float Fov;

    private void OnDidApplyAnimationProperties()
    {
        // Debug.Log($"[CameraController] OnDidApplyAnimationProperties: Near={Near}, Far={Far}, Fov={Fov}");
        if (_camera)
        {
            _camera.nearClipPlane = Near;
            _camera.farClipPlane = Far;
            _camera.fieldOfView = Fov;
        }
    }


    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[CameraController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
