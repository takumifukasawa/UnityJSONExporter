using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace UnityJSONExporter
{
    /// <summary>
    /// 
    /// </summary>
    public enum TrackInfoType
    {
        AnimationTrack, // 0
        LightControlTrack, // 1
        ActivationTrack // 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ClipInfoType
    {
        AnimationClip,
        LightControlClip,
        ActivationControlClip
    }

    /// <summary>
    /// 
    /// </summary>
    public class TrackInfo
    {
        [JsonProperty(PropertyName = "t")]
        public TrackInfoType Type;

        [JsonProperty(PropertyName = "tn")]
        public string TargetName;

        [JsonProperty(PropertyName = "cs")]
        public List<ClipInfoBase> Clips = new List<ClipInfoBase>();

        public TrackInfo(TrackInfoType type, string targetName)
        {
            Type = type;
            TargetName = targetName;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ClipKeyframe
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
    public class ClipBinding
    {
        [JsonProperty(PropertyName = "n")]
        public string PropertyName;

        [JsonProperty(PropertyName = "k")]
        public List<ClipKeyframe> Keyframes = new List<ClipKeyframe>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class ClipInfoBase
    {
        [JsonProperty(PropertyName = "t")]
        public ClipInfoType Type;

        [JsonProperty(PropertyName = "s")]
        public float Start;

        [JsonProperty(PropertyName = "d")]
        public float Duration;

        public ClipInfoBase(ClipInfoType type, float s, float d)
        {
            Type = type;
            Start = s;
            Duration = d;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class AnimationClipInfo : ClipInfoBase
    {
        // [JsonProperty(PropertyName = "s")]
        // public float Start;

        // [JsonProperty(PropertyName = "d")]
        // public float Duration;

        [JsonProperty(PropertyName = "op")]
        public Vector3Info OffsetPosition;

        [JsonProperty(PropertyName = "or")]
        public Vector3Info OffsetRotation;

        [JsonProperty(PropertyName = "b")]
        public List<ClipBinding> Bindings = new List<ClipBinding>();

        // [JsonProperty(PropertyName = "b")]
        // public List<AnimationClipBinding> Bindings = new List<AnimationClipBinding>();

        public AnimationClipInfo(float s, float d) : base(ClipInfoType.AnimationClip, s, d)
        {
            OffsetPosition = new Vector3Info(0, 0, 0);
            OffsetRotation = new Vector3Info(0, 0, 0);
        }

        public AnimationClipInfo(float s, float d, Vector3 op, Vector3 or) : base(ClipInfoType.AnimationClip, s, d)
        {
            OffsetPosition = new Vector3Info(op);
            OffsetRotation = new Vector3Info(or);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LightControlClipInfo : ClipInfoBase
    {
        [JsonProperty(PropertyName = "b")]
        public List<ClipBinding> Bindings = new List<ClipBinding>();

        public LightControlClipInfo(float s, float d) : base(ClipInfoType.LightControlClip, s, d)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ActivationControlClipInfo : ClipInfoBase
    {
        public ActivationControlClipInfo(float s, float d) : base(ClipInfoType.ActivationControlClip, s, d)
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

        [JsonProperty(PropertyName = "ts")]
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

                var bindingObject = playableDirector.GetGenericBinding(track);
                // for debug
                // track.outputs.ToList().ForEach(o =>
                // {
                //     Debug.Log($"[PlayableDirectorComponentInfo] output source: {o.sourceObject}");
                //     Debug.Log($"[PlayableDirectorComponentInfo] output stream name: {o.streamName}");
                //     Debug.Log($"[PlayableDirectorComponentInfo] output target name: {o.outputTargetType}");
                // });

                // ミュート状態のtrackは使わないので書き出さない
                if (track.muted)
                {
                    continue;
                }

                TrackInfo trackInfo = null;

                //
                // trackの種別によって処理を追加していく
                //

                //
                // animation track
                //
                if (track.GetType() == typeof(AnimationTrack))
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] animation track: {track.name}");
                    trackInfo = new TrackInfo(TrackInfoType.AnimationTrack, bindingObject.name);
                    var animationTrack = track as AnimationTrack;
                    var timelineClips = animationTrack.GetClips();
                    // var animationClipInfoList = GenerateAnimationClipInfoList(track, timelineClips, convertAxis, typeof(Transform), true);
                    trackInfo.Clips = GenerateClipInfoList(track, timelineClips, convertAxis, typeof(Object), true);
                }

                //
                // light control track
                //
                else if (track.GetType() == typeof(LightControlTrack))
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] light control track: {track.name}");
                    trackInfo = new TrackInfo(TrackInfoType.LightControlTrack, bindingObject.name);
                    var lightControlTrack = track as LightControlTrack;
                    var timelineClips = lightControlTrack.GetClips();
                    // var animationClipInfoList = new List<AnimationClipInfoBase>();
                    trackInfo.Clips = GenerateClipInfoList(track, timelineClips, convertAxis, typeof(Light), true);
                }

                //
                // control track
                //
                else if (track.GetType() == typeof(ControlTrack))
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] control track: {track.name}");
                    // TODO: 追加
                }

                //
                // activation track
                //
                else if (track.GetType() == typeof(ActivationTrack))
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] activation track: {track.name}");
                    trackInfo = new TrackInfo(TrackInfoType.ActivationTrack, bindingObject.name);
                    var activationTrack = track as ActivationTrack;
                    var timelineClips = activationTrack.GetClips();
                    // TODO: 追加
                    trackInfo.Clips = GenerateClipInfoList(track, timelineClips, convertAxis, typeof(Object), true);
                }

                if (trackInfo != null)
                {
                    Tracks.Add(trackInfo);
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
        List<ClipInfoBase> GenerateClipInfoList(
            TrackAsset track,
            IEnumerable<TimelineClip> timelineClips,
            ConvertAxis convertAxis,
            Type checkType = null,
            bool convertTransformValue = false
        )
        {
            var clipInfoList = new List<ClipInfoBase>();

            Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] clip len: {timelineClips.Count()}");

            foreach (var timelineClip in timelineClips)
            {
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] timeline clip : {timelineClip}");
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] animation clip : {timelineClip.animationClip}");
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] asset : {timelineClip.asset}");
                // // Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] asset is activation control playable: {timelineClip.asset is UnityEngine.Timeline.ActivationPlayableAsset}");
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] asset is activation control playable: {typeof(timelineClip.asset)}");
                var animationClip = timelineClip.animationClip;

                //
                // animation clip
                //
                if (animationClip != null)
                {
                    clipInfoList.Add(GenerateAnimationClipInfo(
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
                    clipInfoList.Add(GenerateLightControlClipInfo(
                        // track as LightControlTrack,
                        timelineClip
                    ));
                }

                //
                // clip in activation track
                // 
                if (track is ActivationTrack)
                {
                    clipInfoList.Add(GenerateActivationControlClipInfo(timelineClip));
                }
            }

            return clipInfoList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timelineClip"></param>
        /// <param name="convertAxis"></param>
        /// <param name="checkType"></param>
        /// <param name="convertTransformValue"></param>
        /// <returns></returns>
        ClipInfoBase GenerateAnimationClipInfo(
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

            var clipInfo = new AnimationClipInfo(
                (float)timelineClip.start,
                (float)timelineClip.duration
            );

            var bindings = AnimationUtility.GetCurveBindings(animationClip);

            foreach (var binding in bindings)
            {
                var clipBinding = new ClipBinding();
                clipInfo.Bindings.Add(clipBinding);
                clipBinding.PropertyName = binding.propertyName; // TODO: property name はそのまま入っちゃうので短縮化したい

                // for debug
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateAnimationClipInfo] type: {checkType}, binding.propertyName: {binding.propertyName}, binding.type.FullName: {binding.type.FullName}");

                // NOTE: AnimationClipの場合はいろんなpropertyの可能性があるのでガードしない（transform, material, ...)
                // if (checkType != null && binding.type.FullName != checkType.FullName)
                // {
                //     continue;
                // }

                var curve = AnimationUtility.GetEditorCurve(animationClip, binding);

                foreach (var key in curve.keys)
                {
                    float keyValue = convertTransformValue && IsTransformProperty(binding.propertyName)
                        ? ConvertTransformCurveValue(binding, key.value, convertAxis)
                        : key.value;

                    var clipKeyframe = new ClipKeyframe();
                    clipKeyframe.Time = key.time;
                    clipKeyframe.Value = keyValue;
                    clipKeyframe.InTangent = key.inTangent;
                    clipKeyframe.OutTangent = key.outTangent;
                    clipBinding.Keyframes.Add(clipKeyframe);
                }
            }

            return clipInfo;
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
        ClipInfoBase GenerateLightControlClipInfo(
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
                var clipBinding = new ClipBinding();
                lightControlClipInfo.Bindings.Add(clipBinding);
                clipBinding.PropertyName = binding.propertyName; // TODO: property name はそのまま入っちゃうので短縮化したい

                // for debug
                // property check
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateLightControlClipInfo] binding: {binding}");
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateLightControlClipInfo] binding property name: {binding.propertyName}");

                var curve = AnimationUtility.GetEditorCurve(animationClip, binding);

                foreach (var key in curve.keys)
                {
                    float keyValue = key.value;
                    var clipKeyframe = new ClipKeyframe();
                    clipKeyframe.Time = key.time;
                    clipKeyframe.Value = keyValue;
                    clipKeyframe.InTangent = key.inTangent;
                    clipKeyframe.OutTangent = key.outTangent;
                    clipBinding.Keyframes.Add(clipKeyframe);
                }
            }

            return lightControlClipInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        ClipInfoBase GenerateActivationControlClipInfo(TimelineClip timelineClip)
        {
            var clipInfo = new ActivationControlClipInfo(
                (float)timelineClip.start,
                (float)timelineClip.duration
            );

            return clipInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        bool IsTransformProperty(string propertyName)
        {
            return
                propertyName == Constants.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_X ||
                propertyName == Constants.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Y ||
                propertyName == Constants.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Z ||
                propertyName == Constants.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_X ||
                propertyName == Constants.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Y ||
                propertyName == Constants.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Z ||
                propertyName == Constants.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_X ||
                propertyName == Constants.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Y ||
                propertyName == Constants.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Z;
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
            // for debug
            // Debug.Log($"[PlayableDirectorComponentInfo.ConvertTransformCurveValue] binding propertyName: {binding.propertyName}, keyValue: {keyValue}");

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
                    throw new Exception($"[PlayableDirectorComponentInfo.ConvertTransformCurveValue] invalid property: {binding.propertyName}");
            }
        }
    }
}
