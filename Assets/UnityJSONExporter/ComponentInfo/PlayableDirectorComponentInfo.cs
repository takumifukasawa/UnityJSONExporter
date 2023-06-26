using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    /// <summary>
    /// 
    /// </summary>
    public class TrackInfo
    {
        [JsonProperty(PropertyName = "a")]
        public List<AnimationClipInfo> AnimationClips = new List<AnimationClipInfo>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class AnimationClipKeyframe
    {
        [JsonProperty(PropertyName = "t")]
        public float Time;

        [JsonProperty(PropertyName = "v")]
        public float Value;

        [JsonProperty(PropertyName = "i")]
        public float InTangent;

        [JsonProperty(PropertyName = "o")]
        public float OutTangent;
    }

    /// <summary>
    /// 
    /// </summary>
    public class AnimationClipBinding
    {
        [JsonProperty(PropertyName = "n")]
        public string PropertyName;

        [JsonProperty(PropertyName = "k")]
        public List<AnimationClipKeyframe> Keyframes = new List<AnimationClipKeyframe>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class AnimationClipInfo
    {
        [JsonProperty(PropertyName = "b")]
        public List<AnimationClipBinding> Bindings = new List<AnimationClipBinding>();
    }

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class PlayableDirectorComponentInfo : ComponentInfoBase
    {
        [JsonProperty(PropertyName = "n")]
        public string Name;

        [JsonProperty(PropertyName = "d")]
        public double Duration;

        [JsonProperty(PropertyName = "t")]
        public List<TrackInfo> Tracks = new List<TrackInfo>();

        public PlayableDirectorComponentInfo(PlayableDirector playableDirector, ConvertAxis convertAxis) : base(ComponentType.PlayableDirector)
        {
            var asset = playableDirector.playableAsset;

            // Debug.Log($"[PlayableDirectorComponentInfo] duration: {playableDirector.duration}");

            Duration = asset.duration;
            Name = asset.name;

            var timelineAsset = playableDirector.playableAsset as TimelineAsset;

            var fps = 60f;
            var spf = 1f / fps;
            var frameCount = playableDirector.duration / spf;


            // for (int i = 0; i < frameCount; i++)
            // {
            //     var currentTime = spf * i;

            foreach (var track in timelineAsset.GetOutputTracks())
            {
                // for debug
                // Debug.Log($"--- track - name: {track.name}, muted: {track.muted}, type: {track.GetType()} --- ");
                // Debug.Log(track.GetType());
                // Debug.Log(track.GetType() == typeof(AnimationTrack));
                // Debug.Log(track.GetType() == typeof(LightControlTrack));
                // Debug.Log(track.GetType() == typeof(ControlTrack));
                // Debug.Log(track.parent);
                // Debug.Log(track.start);
                // Debug.Log(track.end);
                // Debug.Log(track.duration);

                // var currentTime = (float)playableDirector.time;

                if (track.muted)
                {
                    continue;
                }

                var trackInfo = new TrackInfo();
                Tracks.Add(trackInfo);

                // animation track
                if (track.GetType() == typeof(AnimationTrack))
                {
                    // Debug.Log($"[TestMain] animation track");
                    var animationTrack = track as AnimationTrack;
                    var timelineClips = animationTrack.GetClips();
                    foreach (var timelineClip in timelineClips)
                    {
                        // Debug.Log($"[TestMain] each timeline clip");
                        var animationClip = timelineClip.animationClip;
                        if (animationClip == null)
                        {
                            continue;
                        }

                        var animationClipInfo = new AnimationClipInfo();
                        trackInfo.AnimationClips.Add(animationClipInfo);

                        var bindings = AnimationUtility.GetCurveBindings(animationClip);

                        foreach (var binding in bindings)
                        {
                            var animationClipBinding = new AnimationClipBinding();
                            animationClipInfo.Bindings.Add(animationClipBinding);
                            animationClipBinding.PropertyName = binding.propertyName;

                            if (binding.type.FullName != typeof(Transform).FullName)
                            {
                                continue;
                            }

                            var curve = AnimationUtility.GetEditorCurve(animationClip, binding);

                            foreach (var key in curve.keys)
                            {
                                float keyValue = key.value;
                                switch (binding.propertyName)
                                {
                                    case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_X:
                                        keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Position, TransformConverter.AxisDirection.X, keyValue);
                                        break;
                                    case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Y:
                                        keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Position, TransformConverter.AxisDirection.Y, keyValue);
                                        break;
                                    case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Z:
                                        keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Position, TransformConverter.AxisDirection.Z, keyValue);
                                        break;
                                    case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_X:
                                        keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Rotation, TransformConverter.AxisDirection.X, keyValue);
                                        break;
                                    case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Y:
                                        keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Rotation, TransformConverter.AxisDirection.Y, keyValue);
                                        break;
                                    case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Z:
                                        keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Rotation, TransformConverter.AxisDirection.Z, keyValue);
                                        break;
                                    case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_X:
                                        keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Scale, TransformConverter.AxisDirection.X, keyValue);
                                        break;
                                    case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Y:
                                        keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Scale, TransformConverter.AxisDirection.Y, keyValue);
                                        break;
                                    case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Z:
                                        keyValue = TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Scale, TransformConverter.AxisDirection.Z, keyValue);
                                        break;
                                    default:
                                        throw new Exception("invalid params");
                                }

                                var animationClipKeyframe = new AnimationClipKeyframe();
                                animationClipKeyframe.Time = key.time;
                                animationClipKeyframe.Value = keyValue;
                                animationClipKeyframe.InTangent = key.inTangent;
                                animationClipKeyframe.OutTangent = key.outTangent;
                                animationClipBinding.Keyframes.Add(animationClipKeyframe);
                            }
                        }
                    }

                    continue;
                }

                // light control track
                if (track.GetType() == typeof(LightControlTrack))
                {
                    // Debug.Log($"[TestMain] light control track");
                    var lightControlTrack = track as LightControlTrack;
                    var timelineClips = lightControlTrack.GetClips();
                    foreach (var timelineClip in timelineClips)
                    {
                        var animationClip = timelineClip.curves;
                        if (animationClip == null)
                        {
                            continue;
                        }

                        var animationClipInfo = new AnimationClipInfo();
                        trackInfo.AnimationClips.Add(animationClipInfo);

                        var bindings = AnimationUtility.GetCurveBindings(animationClip);

                        foreach (var binding in bindings)
                        {
                            var animationClipBinding = new AnimationClipBinding();
                            animationClipInfo.Bindings.Add(animationClipBinding);
                            animationClipBinding.PropertyName = binding.propertyName;

                            var curve = AnimationUtility.GetEditorCurve(animationClip, binding);

                            foreach (var key in curve.keys)
                            {
                                var animationClipKeyframe = new AnimationClipKeyframe();
                                animationClipKeyframe.Time = key.time;
                                animationClipKeyframe.Value = key.value;
                                animationClipKeyframe.InTangent = key.inTangent;
                                animationClipKeyframe.OutTangent = key.outTangent;
                                animationClipBinding.Keyframes.Add(animationClipKeyframe);
                            }
                        }
                    }

                    continue;
                }

                // control track
                if (track.GetType() == typeof(ControlTrack))
                {
                }

                // tmp

                // // animation track
                // if (track.GetType() == typeof(AnimationTrack))
                // {
                //     Debug.Log($"[TestMain] animation track");
                //     var animationTrack = track as AnimationTrack;
                //     var timelineClips = animationTrack.GetClips();
                //     foreach (var timelineClip in timelineClips)
                //     {
                //         // Debug.Log($"[TestMain] each timeline clip");
                //         var animationClip = timelineClip.animationClip;
                //         if (animationClip != null)
                //         {
                //             var bindings = AnimationUtility.GetCurveBindings(animationClip);
                //             var animationTrackBinder = new AnimationTrackBinder(animationClip, bindings, currentTime);
                //         }
                //     }

                //     continue;
                // }

                // // light control track
                // if (track.GetType() == typeof(LightControlTrack))
                // {
                //     Debug.Log($"[TestMain] light control track");
                //     var lightControlTrack = track as LightControlTrack;
                //     var timelineClips = lightControlTrack.GetClips();
                //     foreach (var timelineClip in timelineClips)
                //     {
                //         var animationClip = timelineClip.curves;
                //         if (animationClip != null)
                //         {
                //             var bindings = AnimationUtility.GetCurveBindings(animationClip);
                //             var lightControlTrackPropertyBinder = new LightControlTrackPropertyBinder(animationClip, bindings, currentTime);
                //         }
                //     }
                // }
            }
        }
    }
}
