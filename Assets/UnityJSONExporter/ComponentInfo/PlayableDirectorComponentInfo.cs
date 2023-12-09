using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
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
        public List<AnimationClipInfoBase> AnimationClips = new List<AnimationClipInfoBase>();
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
    public class AnimationClipInfoBase
    {
        [JsonProperty(PropertyName = "s")]
        public float Start;

        [JsonProperty(PropertyName = "d")]
        public float Duration;

        [JsonProperty(PropertyName = "b")]
        public List<AnimationClipBinding> Bindings = new List<AnimationClipBinding>();

        public AnimationClipInfoBase(float s, float d)
        {
            Start = s;
            Duration = d;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AnimationClipInfo : AnimationClipInfoBase
    {
        // [JsonProperty(PropertyName = "s")]
        // public float Start;

        // [JsonProperty(PropertyName = "d")]
        // public float Duration;

        [JsonProperty(PropertyName = "op")]
        public Vector3Info OffsetPosition;

        [JsonProperty(PropertyName = "or")]
        public Vector3Info OffsetRotation;

        // [JsonProperty(PropertyName = "b")]
        // public List<AnimationClipBinding> Bindings = new List<AnimationClipBinding>();

        public AnimationClipInfo(float s, float d) : base(s, d)
        {
            OffsetPosition = new Vector3Info(0, 0, 0);
            OffsetRotation = new Vector3Info(0, 0, 0);
        }

        public AnimationClipInfo(float s, float d, Vector3 op, Vector3 or) : base(s, d)
        {
            OffsetPosition = new Vector3Info(op);
            OffsetRotation = new Vector3Info(or);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LightControlClipInfo : AnimationClipInfoBase
    {
        public LightControlClipInfo(float s, float d) : base(s, d)
        {
        }
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

            foreach (var track in timelineAsset.GetOutputTracks())
            {
                // for debug
                Debug.Log($"[PlayableDirectorComponentInfo] ===== track name: {track.name}, muted: {track.muted}, type: {track.GetType()} =====");
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

                //
                // trackの種別によって処理を追加していく
                //

                //
                // animation track
                //
                if (track.GetType() == typeof(AnimationTrack))
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] animation track");
                    var animationTrack = track as AnimationTrack;
                    var timelineClips = animationTrack.GetClips();
                    var animationClipInfoList = GenerateAnimationClipInfoList(track, timelineClips, convertAxis, typeof(Transform), true);
                    trackInfo.AnimationClips = animationClipInfoList;
                    continue;
                }

                //
                // light control track
                //
                if (track.GetType() == typeof(LightControlTrack))
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] light control track");
                    var lightControlTrack = track as LightControlTrack;
                    var timelineClips = lightControlTrack.GetClips();
                    // var animationClipInfoList = new List<AnimationClipInfoBase>();
                    var animationClipInfoList = GenerateAnimationClipInfoList(track, timelineClips, convertAxis, typeof(Light), true);
                    trackInfo.AnimationClips = animationClipInfoList;
                    continue;
                    // Debug.Log($"[PlayableDirectorComponentInfo.PlayableDirectorComponentInfo] clip len: {timelineClips.Count()}");
                    // foreach (var timelineClip in timelineClips)
                    // {
                    //     var lightControlClip = timelineClip.asset as LightControlClip;
                    //     Debug.Log($"[PlayableDirectorComponentInfo.PlayableDirectorComponentInfo] timeline clip: {timelineClip}");
                    //     Debug.Log($"[PlayableDirectorComponentInfo.PlayableDirectorComponentInfo] light control clip: {lightControlClip}");
                    //     // Debug.Log($"[PlayableDirectorComponentInfo.PlayableDirectorComponentInfo] timeline clip - animation clip: {animationClip}");
                    //     continue;
                    // }
                }


                //
                // light control track
                //
                // if (track.GetType() == typeof(TestLightControlTrack))
                // {
                //     Debug.Log($"[PlayableDirectorComponentInfo] test light control track");
                //     var lightControlTrack = track as TestLightControlTrack;
                //     var timelineClips = lightControlTrack.GetClips();
                //     Debug.Log($"[PlayableDirectorComponentInfo] timelineClips: {timelineClips}");
                //     Debug.Log($"[PlayableDirectorComponentInfo] clips count: {timelineClips.Count()}");
                //     Debug.Log($"[PlayableDirectorComponentInfo] has curves: {lightControlTrack.hasCurves}");
                //     
                //     // var animationClipInfoList = new List<AnimationClipInfoBase>();
                //     var animationClipInfoList = GenerateAnimationClipInfoList(track, timelineClips, convertAxis, typeof(Transform), true);
                //     trackInfo.AnimationClips = animationClipInfoList;
                //     continue;
                //     // Debug.Log($"[PlayableDirectorComponentInfo.PlayableDirectorComponentInfo] clip len: {timelineClips.Count()}");
                //     // foreach (var timelineClip in timelineClips)
                //     // {
                //     //     var lightControlClip = timelineClip.asset as LightControlClip;
                //     //     Debug.Log($"[PlayableDirectorComponentInfo.PlayableDirectorComponentInfo] timeline clip: {timelineClip}");
                //     //     Debug.Log($"[PlayableDirectorComponentInfo.PlayableDirectorComponentInfo] light control clip: {lightControlClip}");
                //     //     // Debug.Log($"[PlayableDirectorComponentInfo.PlayableDirectorComponentInfo] timeline clip - animation clip: {animationClip}");
                //     //     continue;
                //     // }
                // }


                //
                // control track
                //
                if (track.GetType() == typeof(ControlTrack))
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] control track");
                    // TODO: 追加
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timelineClips"></param>
        /// <param name="convertAxis"></param>
        /// <param name="checkType"></param>
        /// <param name="convertTransformValue"></param>
        /// <returns></returns>
        List<AnimationClipInfoBase> GenerateAnimationClipInfoList(
            TrackAsset track,
            IEnumerable<TimelineClip> timelineClips,
            ConvertAxis convertAxis,
            Type checkType = null,
            bool convertTransformValue = false
        )
        {
            var animationClipInfoList = new List<AnimationClipInfoBase>();
            Debug.Log($"[PlayableDirectorComponentInfo.GenerateAnimationClipInfoList] clip len: {timelineClips.Count()}");
            foreach (var timelineClip in timelineClips)
            {
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateAnimationClipInfoList] timeline clip: {timelineClip}");
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateAnimationClipInfoList] timeline clip curves: {timelineClip.curves}");
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateAnimationClipInfoList] timeline clip curves len: {timelineClip.curves.length}");
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateAnimationClipInfoList] timeline clip curves empty: {timelineClip.curves.empty}");
                // var animationClip = timelineClip.curves;
                // // Debug.Log($"[PlayableDirectorComponentInfo.GenerateAnimationClipInfoList] test light controls clip: {timelineClip is TestLightControlClip}");
                // // Debug.Log($"[PlayableDirectorComponentInfo.GenerateAnimationClipInfoList] animation clip: {timelineClip.animationClip}");
                // // Debug.Log($"[PlayableDirectorComponentInfo.GenerateAnimationClipInfoList] app: {timelineClip.asset as AnimationPlayableAsset}");

                // var bindings = AnimationUtility.GetCurveBindings(animationClip);
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateAnimationClipInfoList] bindings: {bindings}");
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateAnimationClipInfoList] bindings length: {bindings.Length}");

                var animationClip = timelineClip.animationClip;

                //
                // animation clip
                //
                if (animationClip != null)
                {
                    animationClipInfoList.Add(GenerateAnimationClipInfo(
                        timelineClip,
                        animationClip,
                        convertAxis,
                        checkType,
                        convertTransformValue
                    ));
                    continue;
                }

                //
                // light control clip
                //
                if (timelineClip.asset is LightControlClip)
                {
                    animationClipInfoList.Add(GenerateLightControlClipInfo(
                        // track as LightControlTrack,
                        timelineClip
                    ));
                }

                // TODO: light control clip
            }

            return animationClipInfoList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timelineClip"></param>
        /// <param name="convertAxis"></param>
        /// <param name="checkType"></param>
        /// <param name="convertTransformValue"></param>
        /// <returns></returns>
        AnimationClipInfoBase GenerateAnimationClipInfo(
            TimelineClip timelineClip,
            AnimationClip animationClip,
            ConvertAxis convertAxis,
            Type checkType = null,
            bool convertTransformValue = false
        )
        {
            // check timeline clip offset
            var animationPlayableAsset = timelineClip.asset as AnimationPlayableAsset;
            var offsetMatrix = new Matrix4x4();
            offsetMatrix.SetTRS(
                animationPlayableAsset.position,
                animationPlayableAsset.rotation,
                Vector3.one
            );

            var animationClipInfo = new AnimationClipInfo(
                (float)timelineClip.start,
                (float)timelineClip.duration
            );

            var bindings = AnimationUtility.GetCurveBindings(animationClip);

            foreach (var binding in bindings)
            {
                var animationClipBinding = new AnimationClipBinding();
                animationClipInfo.Bindings.Add(animationClipBinding);
                animationClipBinding.PropertyName = binding.propertyName; // TODO: property name はそのまま入っちゃうので短縮化したい

                if (checkType != null && binding.type.FullName != checkType.FullName)
                {
                    continue;
                }

                var curve = AnimationUtility.GetEditorCurve(animationClip, binding);

                foreach (var key in curve.keys)
                {
                    float keyValue = convertTransformValue
                        ? ConvertTransformCurveValue(binding, key.value, convertAxis)
                        : key.value;

                    var animationClipKeyframe = new AnimationClipKeyframe();
                    animationClipKeyframe.Time = key.time;
                    animationClipKeyframe.Value = keyValue;
                    animationClipKeyframe.InTangent = key.inTangent;
                    animationClipKeyframe.OutTangent = key.outTangent;
                    animationClipBinding.Keyframes.Add(animationClipKeyframe);
                }
            }

            return animationClipInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timelineClip"></param>
        /// <param name="animationClip"></param>
        /// <param name="convertAxis"></param>
        /// <param name="checkType"></param>
        /// <param name="convertTransformValue"></param>
        /// <returns></returns>
        AnimationClipInfoBase GenerateLightControlClipInfo(
            // LightControlTrack lightControlTrack,
            TimelineClip timelineClip
        )
        {
            // var lightControlClip = timelineClip.asset as LightControlClip;
            var animationClip = timelineClip.curves;
            var bindings = AnimationUtility.GetCurveBindings(animationClip);

            var lightControlClipInfo = new LightControlClipInfo(
                (float)timelineClip.start,
                (float)timelineClip.duration
            );

            foreach (var binding in bindings)
            {
                var animationClipBinding = new AnimationClipBinding();
                lightControlClipInfo.Bindings.Add(animationClipBinding);
                animationClipBinding.PropertyName = binding.propertyName; // TODO: property name はそのまま入っちゃうので短縮化したい

                // for debug
                // property check
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateLightControlClipInfo] binding: {binding}");
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateLightControlClipInfo] binding property name: {binding.propertyName}");

                var curve = AnimationUtility.GetEditorCurve(animationClip, binding);

                foreach (var key in curve.keys)
                {
                    float keyValue = key.value;
                    var animationClipKeyframe = new AnimationClipKeyframe();
                    animationClipKeyframe.Time = key.time;
                    animationClipKeyframe.Value = keyValue;
                    animationClipKeyframe.InTangent = key.inTangent;
                    animationClipKeyframe.OutTangent = key.outTangent;
                    animationClipBinding.Keyframes.Add(animationClipKeyframe);
                }
            }

            return lightControlClipInfo;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="keyValue"></param>
        /// <param name="convertAxis"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        float ConvertTransformCurveValue(EditorCurveBinding binding, float keyValue, ConvertAxis convertAxis)
        {
            switch (binding.propertyName)
            {
                case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_X:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Position, TransformConverter.AxisDirection.X, keyValue);
                case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Y:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Position, TransformConverter.AxisDirection.Y, keyValue);
                case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Z:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Position, TransformConverter.AxisDirection.Z, keyValue);
                case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_X:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Rotation, TransformConverter.AxisDirection.X, keyValue);
                case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Y:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Rotation, TransformConverter.AxisDirection.Y, keyValue);
                case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Z:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Rotation, TransformConverter.AxisDirection.Z, keyValue);
                case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_X:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Scale, TransformConverter.AxisDirection.X, keyValue);
                case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Y:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Scale, TransformConverter.AxisDirection.Y, keyValue);
                case Constants.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Z:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Scale, TransformConverter.AxisDirection.Z, keyValue);
                default:
                    throw new Exception("invalid params");
            }
        }
    }
}
