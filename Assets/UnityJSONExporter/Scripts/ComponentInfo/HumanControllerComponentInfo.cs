// using Newtonsoft.Json;
// using UnityEngine;
// using UnityJSONExporter;
// 
// public class HumanControllerComponentInfo : ComponentInfoBase
// {
//     [JsonProperty(PropertyName = "lsr")]
//     public Vector4Info LeftShoulderRotation;
// 
//     public HumanControllerComponentInfo(HumanController humanController) : base(ComponentType.HumanController)
//     {
//         LeftShoulderRotation = TransformConverter.ConvertRotationQuaternion(ConvertAxis.RightHand, humanController.transform.localRotation);
//     }
// }
