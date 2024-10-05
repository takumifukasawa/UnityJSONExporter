using System;
using System.Collections.Generic;
using System.Linq;
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
    [System.Serializable]
    public class PlayableDirectorComponentInfo : ComponentInfoBase
    {
        [JsonProperty(PropertyName = "n")]
        public string Name;

        [JsonProperty(PropertyName = "d")]
        public double Duration;

        [JsonProperty(PropertyName = "ts")]
        public List<TrackInfoBase> Tracks = new List<TrackInfoBase>();

        public PlayableDirectorComponentInfo(PlayableDirector playableDirector, ConvertAxis convertAxis, bool minifyPropertyName) : base(ComponentType.PlayableDirector)
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

                TrackInfoBase trackInfo = null;

                //
                // trackの種別によって処理を追加していく
                //

                //
                // marker track
                //
                if (track.GetType() == typeof(MarkerTrack))
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] marker track: {track.name}");
                    var markerTrack = track as MarkerTrack;
                    var outputs = markerTrack.outputs;
                    var markers = markerTrack.GetMarkers();
                    Debug.Log($"[PlayableDirectorComponentInfo] marker track: {track.name}, marker track output count: {markerTrack.outputs.Count()}, outputs count: {outputs.Count()}, marker count: {markers.Count()}");
                    var signalEmitterInfo = new List<SignalEmitterInfo>();
                    foreach (var marker in markers)
                    {
                        var signalEmitter = marker as SignalEmitter;
                        Debug.Log($"[PlayableDirectorComponentInfo] signal emitter: {signalEmitter.name}");
                        signalEmitterInfo.Add(new SignalEmitterInfo(signalEmitter.name, (float)signalEmitter.time));
                    }

                    trackInfo = new MarkerTrackInfo(signalEmitterInfo);
                }


                //
                // animation track
                //
                if (track.GetType() == typeof(AnimationTrack))
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] animation track: {track.name}");
                    var animationTrack = track as AnimationTrack;
                    var timelineClips = animationTrack.GetClips();
                    // var animationClipInfoList = GenerateAnimationClipInfoList(track, timelineClips, convertAxis, typeof(Transform), true);
                    trackInfo = new DefaultTrackInfo(
                        TrackInfoType.AnimationTrack,
                        bindingObject.name,
                        GenerateClipInfoList(
                            track,
                            timelineClips,
                            convertAxis,
                            minifyPropertyName,
                            typeof(UnityEngine.Object),
                            true
                        )
                    );
                    // trackInfo.Clips = GenerateClipInfoList(track, timelineClips, convertAxis, typeof(Object), true);
                }

                //
                // light control track
                //
                else if (track.GetType() == typeof(LightControlTrack))
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] light control track: {track.name}");
                    var lightControlTrack = track as LightControlTrack;
                    var timelineClips = lightControlTrack.GetClips();
                    // var animationClipInfoList = new List<AnimationClipInfoBase>();
                    trackInfo = new DefaultTrackInfo(
                        TrackInfoType.LightControlTrack,
                        bindingObject.name,
                        GenerateClipInfoList(
                            track,
                            timelineClips,
                            convertAxis,
                            minifyPropertyName,
                            typeof(Light),
                            true
                        )
                    );
                    // trackInfo.Clips = GenerateClipInfoList(track, timelineClips, convertAxis, typeof(Light), true);
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
                    // trackInfo = new TrackInfo(TrackInfoType.ActivationTrack, bindingObject.name);
                    var activationTrack = track as ActivationTrack;
                    var timelineClips = activationTrack.GetClips();
                    // TODO: 追加
                    // trackInfo.Clips = GenerateClipInfoList(track, timelineClips, convertAxis, typeof(Object), true);
                    trackInfo = new DefaultTrackInfo(
                        TrackInfoType.ActivationTrack,
                        bindingObject.name,
                        GenerateClipInfoList(
                            track,
                            timelineClips,
                            convertAxis,
                            minifyPropertyName,
                            typeof(UnityEngine.Object),
                            true
                        )
                    );
                }

                // object move and look at track
                else if (track.GetType() == typeof(ObjectMoveAndLookAtTrack))
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] object move and look at track: {track.name}");
                    // trackInfo = new TrackInfo(TrackInfoType.ActivationTrack, bindingObject.name);
                    var objectMoveAndLookAtTrack = track as ObjectMoveAndLookAtTrack;
                    var timelineClips = track.GetClips();
                    trackInfo = new DefaultTrackInfo(
                        TrackInfoType.ObjectMoveAndLookAtTrack,
                        bindingObject.name,
                        GenerateClipInfoList(
                            track,
                            timelineClips,
                            convertAxis,
                            minifyPropertyName,
                            typeof(UnityEngine.Object),
                            true
                        )
                    );
                }

                else
                {
                    Debug.LogError($"[PlayableDirectorComponentInfo] unknown track type: {track.GetType()}");
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
            bool minifyPropertyName,
            Type checkType = null,
            bool convertTransformValue = false
        )
        {
            var clipInfoList = new List<ClipInfoBase>();

            // for debug
            // Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] ==================================");
            // Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] clip len: {timelineClips.Count()}");

            foreach (var timelineClip in timelineClips)
            {
                // for debug
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] timeline clip : {timelineClip}");
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] animation clip : {timelineClip.animationClip}");
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] asset : {timelineClip.asset}");
                // // Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] asset is activation control playable: {timelineClip.asset is UnityEngine.Timeline.ActivationPlayableAsset}");
                // // Debug.Log($"[PlayableDirectorComponentInfo.GenerateClipInfoList] asset is activation control playable: {typeof(timelineClip.asset)}");
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
                        minifyPropertyName,
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
            bool minifyPropertyName,
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

                var targetComponent = FindComponentInScene(binding.type);
                Debug.Log(
                    $"[PlayableDirectorComponentInfo.GenerateAnimationClipInfo] timeline clip name: {timelineClip.displayName}, type: {checkType}, binding.propertyName: {binding.propertyName}, binding.type.FullName: {binding.type.FullName}, target component: {targetComponent}");

                if (targetComponent is PostProcessController)
                {
                    clipBinding.PropertyName = ResolveJsonProperty(
                        targetComponent as PostProcessController,
                        binding.propertyName,
                        minifyPropertyName
                    );
                }
                else
                {
                    clipBinding.PropertyName = binding.propertyName;
                }

                // for debug
                // Debug.Log($"[PlayableDirectorComponentInfo.GenerateAnimationClipInfo] timeline clip name: {timelineClip.displayName}, type: {checkType}, binding.propertyName: {binding.propertyName}, binding.type.FullName: {binding.type.FullName}");

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

        Component FindComponentInScene(Type componentType)
        {
            var allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (var obj in allObjects)
            {
                var component = obj.GetComponent(componentType);
                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }

        string ResolveJsonProperty<T>(
            T component,
            string targetProperty,
            bool needsMinify
        ) where T : Component
        {
            if (!needsMinify)
            {
                return targetProperty;
            }

            var type = typeof(T);
            var fields = type.GetFields();
            // for debug
            // Debug.Log($"[PlayableDirectorComponentInfo.ResolveJsonProperty] type: {type}, type2: {type2}, properties count: {properties.Length}");
            foreach (var field in fields)
            {
                var jsonProperty = field
                    .GetCustomAttributes(typeof(JsonPropertyAttribute), false)
                    .Cast<JsonPropertyAttribute>()
                    .FirstOrDefault();
                // for debug
                // Debug.Log($"[PlayableDirectorComponentInfo.ResolveJsonProperty] property name: {property.Name}, target property: {targetProperty}, jsonProperty: {jsonProperty}, jsonPropertyName: {jsonProperty.PropertyName}");
                if (field.Name == targetProperty && needsMinify)
                {
                    return jsonProperty.PropertyName;
                }
            }

            // fallback
            return targetProperty;
        }
    }
}
