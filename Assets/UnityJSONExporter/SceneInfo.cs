using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.UI;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    // ---------------------------------------------------------------------------------------------
    // public
    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class SceneInfo
    {
        [JsonProperty("name")]
        public string Name;

        // public Hierarchy Hierarchy;
        [JsonProperty("objects")]
        public List<ObjectInfo> Objects;
    }

    // /// <summary>
    // /// 
    // /// </summary>
    // [System.Serializable]
    // public class Hierarchy
    // {
    //     public List<ObjectInfo> Objects;
    // }

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class ObjectInfo
    {
        // [JsonIgnore]
        // public GameObject InternalGameObject;
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("components")]
        public List<ComponentInfoBase> Components = new List<ComponentInfoBase>();

        [JsonProperty("children")]
        public List<ObjectInfo> Children = new List<ObjectInfo>();

        public ObjectInfo(GameObject obj)
        {
            Name = obj.name;
        }

        public void AddChild(ObjectInfo child)
        {
            Children.Add(child);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class ComponentInfoBase
    {
        [JsonProperty("type")]
        public string Type;

        public ComponentInfoBase(string type)
        {
            Type = type;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ComponentType
    {
        Light,
        PlayableDirector
    }

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class LightComponentInfo : ComponentInfoBase
    {
        [JsonProperty("lightType")]
        public string LightType;

        [JsonProperty("color")]
        public string Color;

        public LightComponentInfo(Light light) : base(ComponentType.Light.ToString())
        {
            LightType = light.type.ToString();
            Color = ColorUtilities.ConvertColorToHexString(light.color);
        }
    }

    public class TrackInfo
    {
        [JsonProperty("animationClips")]
        public List<AnimationClipInfo> AnimationClips = new List<AnimationClipInfo>();
    }

    public class AnimationClipKeyframe
    {
        [JsonProperty("time")]
        public float Time;

        [JsonProperty("value")]
        public float Value;

        [JsonProperty("inTangent")]
        public float InTangent;

        [JsonProperty("outTangent")]
        public float OutTangent;
    }

    public class AnimationClipBinding
    {
        [JsonProperty("propertyName")]
        public string PropertyName;

        [JsonProperty("keyframes")]
        public List<AnimationClipKeyframe> Keyframes = new List<AnimationClipKeyframe>();
    }

    public class AnimationClipInfo
    {
        [JsonProperty("bindings")]
        public List<AnimationClipBinding> Bindings = new List<AnimationClipBinding>();
    }

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class PlayableDirectorComponentInfo : ComponentInfoBase
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("duration")]
        public double Duration;

        [JsonProperty("tracks")]
        public List<TrackInfo> Tracks = new List<TrackInfo>();

        public PlayableDirectorComponentInfo(PlayableDirector playableDirector) : base(ComponentType.PlayableDirector.ToString())
        {
            var asset = playableDirector.playableAsset;

            Debug.Log($"[PlayableDirectorComponentInfo] duration: {playableDirector.duration}");

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
                Debug.Log($"--- track - name: {track.name}, muted: {track.muted}, type: {track.GetType()} --- ");
                // Debug.Log(track.GetType());
                // Debug.Log(track.GetType() == typeof(AnimationTrack));
                // Debug.Log(track.GetType() == typeof(LightControlTrack));

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
                    Debug.Log($"[TestMain] animation track");
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

                // light control track
                if (track.GetType() == typeof(LightControlTrack))
                {
                    Debug.Log($"[TestMain] light control track");
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
