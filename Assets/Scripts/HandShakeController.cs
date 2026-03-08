using System;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityJSONExporter;

// export const HAND_SHAKE_POSITION_AMPLITUDE_PROPERTY_NAME = NeedsShorten ? 'hs_pa' : 'handShakeAmplitude';
// export const HAND_SHAKE_POSITION_SPEED_PROPERTY_NAME = NeedsShorten ? 'hs_ps' : 'handShakeSpeed';
// export const HAND_SHAKE_POSITION_OFFSET_PROPERTY_NAME = NeedsShorten ? 'hs_os' : 'handShakeOffset';
// export const HAND_SHAKE_LOOK_AT_AMPLITUDE_PROPERTY_NAME = NeedsShorten ? 'hs_la' : 'handShakeAmplitude';
// export const HAND_SHAKE_LOOK_AT_SPEED_PROPERTY_NAME = NeedsShorten ? 'hs_ls' : 'handShakeSpeed';
// export const HAND_SHAKE_LOOK_AT_OFFSET_PROPERTY_NAME = NeedsShorten ? 'hs_lo' : 'handShakeOffset';
// export type HandShakeControllerModel = ComponentModel & {
//     positionAmplitude: Vector3;
//     positionSpeed: Vector3;
//     positionOffset: Vector3;
//     lookAtAmplitude: Vector3;
//     lookAtSpeed: Vector3;
//     lookAtOffset: Vector3;
// };


[ExecuteInEditMode]
public class HandShakeController : TimelineBindingObjectBase
{
    [JsonProperty(PropertyName = "hs_pa")] // hand shake amplitude
    public Vector3 PositionAmplitude;

    [JsonProperty(PropertyName = "hs_ps")] // hand shake speed
    public Vector3 PositionSpeed;

    [JsonProperty(PropertyName = "hs_po")] // hand shake offset
    public Vector3 PositionOffset;

    [JsonProperty(PropertyName = "hs_la")] // hand shake amplitude
    public Vector3 LookAtAmplitude;

    [JsonProperty(PropertyName = "hs_ls")] // hand shake speed
    public Vector3 LookAtSpeed;

    [JsonProperty(PropertyName = "hs_lo")] // hand shake offset
    public Vector3 LookAtOffset;

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[HandShakeController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
