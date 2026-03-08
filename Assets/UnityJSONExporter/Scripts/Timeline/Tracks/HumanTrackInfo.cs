// using System;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.Timeline;
// 
// namespace UnityJSONExporter
// {
//     public class HumanTrackInfo : ClipTrackInfoBase
//     {
//         public HumanTrackInfo(
//             HumanTrack track,
//             string targetName,
//             ConvertAxis convertAxis,
//             bool minifyPropertyName,
//             Type checkType = null,
//             bool convertTransformValue = false
//         ) : base(TrackInfoType.HumanTrack, targetName)
//         {
//             var timelineClips = track.GetClips();
// 
//             foreach (var timelineClip in timelineClips)
//             {
//                 // for debug
//                 LoggerProxy.Log($"[HumanTrackInfo.constructor] timeline clip : {timelineClip}");
//                 LoggerProxy.Log($"[HumanTrackInfo.constructor] animation clip : {timelineClip.animationClip}");
//                 LoggerProxy.Log($"[HumanTrackInfo.constructor] asset : {timelineClip.asset}");
//                 var animationClip = timelineClip.animationClip;
//                 Clips.Add(GenerateHumanTrackClipInfo(
//                     timelineClip,
//                     convertAxis,
//                     minifyPropertyName,
//                     checkType,
//                     convertTransformValue
//                 ));
//             }
//         }
// 
//         /// <summary>
//         /// 
//         /// </summary>
//         /// <param name="timelineClip"></param>
//         /// <param name="convertAxis"></param>
//         /// <param name="checkType"></param>
//         /// <param name="convertTransformValue"></param>
//         /// <returns></returns>
//         ClipInfoBase GenerateHumanTrackClipInfo(
//             TimelineClip timelineClip,
//             ConvertAxis convertAxis,
//             bool minifyPropertyName,
//             Type checkType = null,
//             bool convertTransformValue = false
//         )
//         {
//             // var lightControlClip = timelineClip.asset as LightControlClip;
//             var animationClip = timelineClip.curves;
//             var bindings = AnimationUtility.GetCurveBindings(animationClip);
// 
//             var clipInfo = new HumanClipInfo(
//                 (float)timelineClip.start,
//                 (float)timelineClip.duration
//             );
// 
//             foreach (var binding in bindings)
//             {
//                 var clipBinding = new ClipBinding();
//                 clipInfo.Bindings.Add(clipBinding);
// 
//                 clipBinding.PropertyName = binding.propertyName;
// 
//                 // for debug
//                 LoggerProxy.Log($"[HumanTrackInfo.GenerateHumanTrackClipInfo] property: {binding.propertyName}");
// 
//                 // NOTE: AnimationClipの場合はいろんなpropertyの可能性があるのでガードしない（transform, material, ...)
//                 // if (checkType != null && binding.type.FullName != checkType.FullName)
//                 // {
//                 //     continue;
//                 // }
// 
//                 var curve = AnimationUtility.GetEditorCurve(animationClip, binding);
// 
//                 foreach (var key in curve.keys)
//                 {
//                     LoggerProxy.Log($"[HumanTrackInfo.GenerateHumanTrackClipInfo] property: {binding.propertyName}, key: {key.time}, {key.value}, {key.inTangent}, {key.outTangent}");
// 
//                     // tmp
//                     // float keyValue = convertTransformValue && TransformConverter.IsTransformProperty(binding.propertyName)
//                     //     ? TransformConverter.ConvertTransformCurveValue(binding, key.value, convertAxis)
//                     //     : key.value;
// 
//                     float keyValue = key.value;
// 
//                     switch (binding.propertyName)
//                     {
//                         // TODO: shorthand
//                         case "LeftShoulderRotation.x":
//                             keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Rotation, TransformConverter.AxisDirection.X, keyValue);
//                             if (minifyPropertyName)
//                             {
//                                 clipBinding.PropertyName = "lsr.x";
//                             }
//                             break;
//                         case "LeftShoulderRotation.y":
//                             keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Rotation, TransformConverter.AxisDirection.Y, keyValue);
//                             if (minifyPropertyName)
//                             {
//                                 clipBinding.PropertyName = "lsr.y";
//                             }
//                             break;
//                         case "LeftShoulderRotation.z":
//                             keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Rotation, TransformConverter.AxisDirection.Z, keyValue);
//                             if (minifyPropertyName)
//                             {
//                                 clipBinding.PropertyName = "lsr.z";
//                             }
//                             break;
//                             
//                         
//                         case "LocalPosition.x":
//                             keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Position, TransformConverter.AxisDirection.X, keyValue);
//                             if (minifyPropertyName)
//                             {
//                                 clipBinding.PropertyName = "lp.x";
//                             }
// 
//                             break;
//                         case "LocalPosition.y":
//                             keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Position, TransformConverter.AxisDirection.Y, keyValue);
//                             if (minifyPropertyName)
//                             {
//                                 clipBinding.PropertyName = "lp.y";
//                             }
// 
//                             break;
//                         case "LocalPosition.z":
//                             keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Position, TransformConverter.AxisDirection.Z, keyValue);
//                             if (minifyPropertyName)
//                             {
//                                 clipBinding.PropertyName = "lp.z";
//                             }
// 
//                             break;
//                         default:
//                             throw new Exception($"[HumanTrackInfo.GenerateHumanTrackClipInfo] invalid property: {binding.propertyName}");
//                     }
// 
// 
//                     var clipKeyframe = new ClipKeyframe();
//                     clipKeyframe.Time = key.time;
//                     clipKeyframe.Value = keyValue;
//                     clipKeyframe.InTangent = key.inTangent;
//                     clipKeyframe.OutTangent = key.outTangent;
//                     // for obj
//                     // clipBinding.Keyframes.Add(clipKeyframe);
//                     // for arr
//                     clipBinding.AddKeyframe(key.time, keyValue, key.inTangent, key.outTangent);
//                 }
//             }
// 
//             return clipInfo;
//         }
//     }
// }
