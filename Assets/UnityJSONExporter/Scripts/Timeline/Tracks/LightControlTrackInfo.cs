#define LIGHT_CONTROL_TRACK_ENABLED

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
#if LIGHT_CONTROL_TRACK_ENABLED
    public class LightControlTrackInfo : ClipTrackInfoBase
    {
        public LightControlTrackInfo(
            string name,
            LightControlTrack track,
            string targetName,
            bool minifyPropertyName
        ) : base(name, TrackInfoType.LightControlTrack, targetName)
        {
            var timelineClips = track.GetClips();
            foreach (var timelineClip in timelineClips)
            {
                Clips.Add(GenerateLightControlClipInfo(
                    // track as LightControlTrack,
                    timelineClip,
                    minifyPropertyName
                ));
            }
        }

        ClipInfoBase GenerateLightControlClipInfo(
            TimelineClip timelineClip,
            bool minifyPropertyName
        )
        {
            // var lightControlClip = timelineClip.asset as LightControlClip;
            var animationClip = timelineClip.curves;
            var bindings = AnimationUtility.GetCurveBindings(animationClip);

            var lightControlClipInfo = new LightControlClipInfo(
                timelineClip.displayName,
                (float)timelineClip.start,
                (float)timelineClip.duration,
                timelineClip.postExtrapolationMode
            );

            // timeScale の逆数でkeyframe時間を変換
            float timeScaleInverse = 1.0f / (float)timelineClip.timeScale;

            foreach (var binding in bindings)
            {
                var clipBinding = new ClipBinding();
                lightControlClipInfo.Bindings.Add(clipBinding);
                lightControlClipInfo.AddBinding(clipBinding);

                // TODO: property name はそのまま入っちゃうので短縮化したい
                clipBinding.PropertyName =
                    minifyPropertyName
                        ? PropertyNameResolver.ResolveUnityBuiltinPropertyName(binding.propertyName)
                        : binding.propertyName;

                // // for debug
                // // property check
                // Debug.Log($"[LightControlTrackInfo.GenerateLightControlClipInfo] binding: {binding}");
                // Debug.Log($"[LightControlTrackInfo.GenerateLightControlClipInfo] binding property name: {binding.propertyName}");

                var curve = AnimationUtility.GetEditorCurve(animationClip, binding);

                foreach (var key in curve.keys)
                {
                    float keyValue = key.value;
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
            }

            return lightControlClipInfo;
        }
    }
#endif
}
