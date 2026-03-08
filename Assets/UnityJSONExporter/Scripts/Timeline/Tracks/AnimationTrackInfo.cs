using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    public class AnimationTrackInfo : ClipTrackInfoBase
    {
        public AnimationTrackInfo(
            string name,
            AnimationTrack track,
            string targetName,
            ConvertAxis convertAxis,
            bool minifyPropertyName,
            Type checkType = null,
            bool convertTransformValue = false
        ) : base(name, TrackInfoType.AnimationTrack, targetName)
        {
            var timelineClips = track.GetClips();

            // track名が "Uniq_" で始まる場合、重複排除を有効化
            bool shouldDeduplicateBindings = name.StartsWith("Uniq_");
            HashSet<string> processedAnimationNames = shouldDeduplicateBindings
                ? new HashSet<string>()
                : null;

            foreach (var timelineClip in timelineClips)
            {
                // for debug
                // LoggerProxy.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] timeline clip : {timelineClip}");
                // LoggerProxy.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] animation clip : {timelineClip.animationClip}");
                // LoggerProxy.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] asset : {timelineClip.asset}");
                // // LoggerProxy.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] asset is activation control playable: {timelineClip.asset is UnityEngine.Timeline.ActivationPlayableAsset}");
                // // LoggerProxy.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] asset is activation control playable: {typeof(timelineClip.asset)}");
                var animationClip = timelineClip.animationClip;

                bool shouldGenerateBindings = true;
                if (shouldDeduplicateBindings)
                {
                    if (processedAnimationNames.Contains(timelineClip.displayName))
                    {
                        shouldGenerateBindings = false;
                    }
                    else
                    {
                        processedAnimationNames.Add(timelineClip.displayName);
                    }
                }

                Clips.Add(GenerateAnimationClipInfo(
                    timelineClip,
                    animationClip,
                    convertAxis,
                    minifyPropertyName,
                    checkType,
                    convertTransformValue,
                    shouldGenerateBindings
                ));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="timelineClip"></param>
        /// <param name="convertAxis"></param>
        /// <param name="checkType"></param>
        /// <param name="convertTransformValue"></param>
        /// <param name="shouldGenerateBindings"></param>
        /// <returns></returns>
        ClipInfoBase GenerateAnimationClipInfo(
            TimelineClip timelineClip,
            AnimationClip animationClip,
            ConvertAxis convertAxis,
            bool minifyPropertyName,
            Type checkType = null,
            bool convertTransformValue = false,
            bool shouldGenerateBindings = true
        )
        {
            // TODO: offsetの適用
            // check timeline clip offset
            var animationPlayableAsset = timelineClip.asset as AnimationPlayableAsset;
            var offsetMatrix = new Matrix4x4();
            offsetMatrix.SetTRS(
                animationPlayableAsset.position,
                animationPlayableAsset.rotation,
                Vector3.one
            );

            var bindings = AnimationUtility.GetCurveBindings(animationClip);

            if (bindings.Length < 1)
            {
                Debug.LogError($"[AnimationTrackInfo.GenerateAnimationClipInfo] bindings length is 0. timeline clip name: {timelineClip.displayName}");
            }

            // TODO: propertyごとにfindしたいが・・・
            Component targetComponent = null;
            for (int i = 0; i < bindings.Length; i++)
            {
                targetComponent = SceneUtilities.FindComponentInScene(bindings[i].type);
                Debug.Log($"fugafuga [AnimationTrackInfo.GenerateAnimationClipInfo] checking binding type: {bindings[i].type}, checkType: {checkType}, targetComponent: {targetComponent}");
                if (targetComponent)
                {
                    break;
                }
            }

            // Debug.Log($"fugafuga [AnimationTrackInfo.GenerateAnimationClipInfo] timeline clip name: {timelineClip.displayName}, type: {targetComponent.GetType()}");

            if (!targetComponent)
            {
                Debug.LogError($"[AnimationTrackInfo.GenerateAnimationClipInfo] target component not found in scene. timeline clip name: {timelineClip.displayName}");
            }

            // var targetComponent = SceneUtilities.FindComponentInScene(bindings[0].type);
            var animationClipType = AnimationClipType.Default;
            if (targetComponent is TimelineBindingObjectBase)
            {
                animationClipType = (targetComponent as TimelineBindingObjectBase).AnimationClipType;
            }

            var clipInfo = new AnimationClipInfo(
                timelineClip.displayName,
                animationClipType,
                (float)timelineClip.start,
                (float)timelineClip.duration,
                timelineClip.postExtrapolationMode
            );

            // timeScale の逆数でkeyframe時間を変換
            float timeScaleInverse = 1.0f / (float)timelineClip.timeScale;
            // Debug.Log(
            //     $"fugafuga [AnimationTrackInfo.GenerateAnimationClipInfo] timeline clip name: {timelineClip.displayName}, target component: {targetComponent}, component type: {targetComponent.GetType()}, animationClipType: {animationClipType}, post-e: {timelineClip.postExtrapolationMode == TimelineClip.ClipExtrapolation.Loop}");

            if (shouldGenerateBindings)
            {
                foreach (var binding in bindings)
            {
                // var sp = binding.propertyName.Split(".");
                // Debug.Log($"fugafuga, {binding.propertyName}, {sp}, {sp.Length}");

                // TODO: propertyごとにfindしたいが・・・
                // targetComponent = null;
                // var keyProperty = "";
                // if (binding.propertyName.Split(".").Length > 1)
                // {
                //     keyProperty = binding.propertyName.Split(".")[0];
                // }
                // else
                // {
                //     keyProperty = binding.propertyName;
                // }
                // Debug.Log($"fugafuga, {keyProperty}");

                // for (int i = 0; i < bindings.Length; i++)
                // {
                //     targetComponent = SceneUtilities.FindComponentInScene(bindings[i].type);
                //     if (targetComponent.GetType().GetProperties().ToList().Find(property => property.Name == keyProperty) != null)
                //     {
                //         break;
                //     }
                // }

                // if (targetComponent == null)
                // {
                //     Debug.LogError(
                //         $"[AnimationTrackInfo.GenerateAnimationClipInfo] target component for binding not found in scene. timeline clip name: {timelineClip.displayName}, binding property name: {binding.propertyName}, binding type: {binding.type}");
                // }


                var clipBinding = new ClipBinding();

                clipBinding.PropertyName = binding.propertyName;

                var curve = AnimationUtility.GetEditorCurve(animationClip, binding);

                LoggerProxy.Log(
                    $"[AnimationTrackInfo.GenerateAnimationClipInfo] timeline clip name: {timelineClip.displayName}, type: {checkType}, binding.propertyName: {binding.propertyName}, binding.type: {binding.type}, target component: {targetComponent}, component type: {targetComponent.GetType()}");

                // var type = targetComponent.GetType();
                clipBinding.PropertyName = binding.propertyName;

                if (minifyPropertyName)
                {
                    if (targetComponent is TimelineBindingObjectBase)
                    {
                        clipBinding.PropertyName = (targetComponent as TimelineBindingObjectBase).ResolvePropertyName(binding.propertyName);
                        // Debug.Log($"hogehoge, {targetComponent.GetType()}, {clipBinding.PropertyName}, {binding.propertyName}");
                        // standard property name
                        var excludeProperty = FieldAttributeLookup.HasFieldAttribute<ExcludeExportPropertyAttribute>(targetComponent, binding.propertyName);
                        // hoge.x -> hoge
                        var splitPropertyName = clipBinding.PropertyName.Split(".");
                        if (splitPropertyName.Length > 1)
                        {
                            excludeProperty = FieldAttributeLookup.HasFieldAttribute<ExcludeExportPropertyAttribute>(targetComponent, splitPropertyName[0]);
                        }

                        Debug.Log(
                            $"[AnimationTrackInfo.GenerateAnimationClipInfo] target component type: {targetComponent.GetType()}, binding.propertyName: {binding.propertyName}, resolved property name: {clipBinding.PropertyName}, excludeProperty: {excludeProperty}");
                        // clipBinding.PropertyName = JsonUtilities.ResolveJsonProperty<type>(
                        //     // targetComponent as PostProcessController,
                        //     targetComponent as type,
                        //     binding.propertyName,
                        //     minifyPropertyName
                        // );
                        if (excludeProperty)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        // clipBinding.PropertyName = binding.propertyName;
                        clipBinding.PropertyName = PropertyNameResolver.ResolveUnityBuiltinPropertyName(binding.propertyName);
                    }
                }


                foreach (var key in curve.keys)
                {
                    float keyValue = convertTransformValue && TransformConverter.IsTransformProperty(binding.propertyName)
                        ? TransformConverter.ConvertTransformCurveValue(binding, key.value, convertAxis)
                        : key.value;

                    // keyframe時間にtimeScaleの逆数を適用
                    float adjustedTime = key.time * timeScaleInverse;

                    var clipKeyframe = new ClipKeyframe();
                    clipKeyframe.Time = adjustedTime;
                    clipKeyframe.Value = keyValue;
                    clipKeyframe.InTangent = key.inTangent;
                    clipKeyframe.OutTangent = key.outTangent;
                    // for obj
                    // clipBinding.Keyframes.Add(clipKeyframe);
                    // for arr
                    clipBinding.AddKeyframe(adjustedTime, keyValue, key.inTangent, key.outTangent);
                }

                // 処理が終わったら追加
                clipInfo.AddBinding(clipBinding);

                LoggerProxy.Log($"[AnimationTrackInfo.GenerateAnimationClipInfo] clipBinding.PropertyName: {clipBinding.PropertyName}, keyframe num: {clipBinding.Keyframes.Count}");
                }
            }

            return clipInfo;
        }
    }
}
