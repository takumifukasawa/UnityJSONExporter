using System;
using System.Collections.Generic;
using System.Linq;
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
        public string Name;

        // public Hierarchy Hierarchy;
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
        public string Name;
        public List<ComponentInfoBase> Components = new List<ComponentInfoBase>();
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
        public string LightType;
        public string Color;

        public LightComponentInfo(Light light) : base(ComponentType.Light.ToString())
        {
            LightType = light.type.ToString();
            Color = ColorUtilities.ConvertColorToHexString(light.color);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class PlayableDirectorComponentInfo : ComponentInfoBase
    {
        public string Name;
        public double Duration;

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

            for (int i = 0; i < frameCount; i++)
            {
                var currentTime = spf * i;

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
                            if (animationClip != null)
                            {
                                var bindings = AnimationUtility.GetCurveBindings(animationClip);
                                var animationTrackBinder = new AnimationTrackBinder(animationClip, bindings, currentTime);
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
                            if (animationClip != null)
                            {
                                var bindings = AnimationUtility.GetCurveBindings(animationClip);
                                var lightControlTrackPropertyBinder = new LightControlTrackPropertyBinder(animationClip, bindings, currentTime);
                            }
                        }
                    }
                }


                // foreach (var track in timelineAsset.GetOutputTracks())
                // {
                //     Debug.Log($"[PlayableDirectorComponentInfo] track ==============================");
                //     Debug.Log($"[PlayableDirectorComponentInfo] track name: {track.name}, start: {track.start}, end: {track.end}, duration: {track.duration}, has clip: {track.hasClips}, curves: {track.curves}");
                //     Debug.Log($"[PlayableDirectorComponentInfo] track to string: {track.ToString()}, instance id: {track.GetInstanceID()}");

                //     var timelineClips = track.GetClips();
                //     Debug.Log($"[PlayableDirectorComponentInfo] track timeline clips count: {timelineClips.Count()}");
                //     foreach (var timelineClip in timelineClips)
                //     {
                //         Debug.Log($"[PlayableDirectorComponentInfo] timeline clip ------------------------------");
                //         Debug.Log($"[PlayableDirectorComponentInfo] timeline clip asset {timelineClip.asset}");
                //         var animationClip = timelineClip.animationClip;
                //         var animationPlayableAsset = timelineClip.asset as AnimationPlayableAsset;
                //         var bindings = AnimationUtility.GetCurveBindings(animationClip);

                //         bool hasLocalPosition = false;
                //         var localPosition = Vector3.zero;
                //         var hasLocalRotation = false;
                //         var localRotationEuler = Vector3.zero;
                //         foreach (var binding in bindings)
                //         {
                //             var curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                //             Debug.Log($"[PlayableDirectorComponentInfo] binding ------------------------------");
                //             Debug.Log($"[PlayableDirectorComponentInfo] binding type base type: {binding.type.FullName}, is transform: {binding.type.FullName == typeof(Transform).FullName}");
                //             Debug.Log($"[PlayableDirectorComponentInfo] binding type: {binding.type}, path: {binding.path}, property: {binding.propertyName}");

                //             // animated transform
                //             if (binding.type.FullName == typeof(Transform).FullName)
                //             {
                //                 switch (binding.path)
                //                 {
                //                     case "m_LocalPosition.x":
                //                         hasLocalPosition = true;
                //                         break;
                //                     case "m_LocalPosition.y":
                //                         hasLocalPosition = true;
                //                         break;
                //                     case "m_LocalPosition.z":
                //                         hasLocalPosition = true;
                //                         break;
                //                     case "localEulerAnglesRaw.x":
                //                         hasLocalRotation = true;
                //                         break;
                //                     case "localEulerAnglesRaw.y":
                //                         hasLocalRotation = true;
                //                         break;
                //                     case "localEulerAnglesRaw.z":
                //                         hasLocalRotation = true;
                //                         break;
                //                     default:
                //                         throw new Exception("invalid property");
                //                 }
                //             }

                //             var keys = curve.keys.ToList();
                //             for (var i = 0; i < keys.Count; i++)
                //             {
                //                 var key = keys[i];
                //                 Debug.Log(
                //                     $"[PlayableDirectorComponentInfo] curve in clip - index: {i}, t: {key.time}, value: {key.value}, in-t: {key.inTangent}, in-w: {key.inWeight}, out-t: {key.outTangent}, out-w: {key.outWeight}, weighted mode: {key.weightedMode}");
                //             }
                //         }
                //     }
                //     // Debug.Log($"[PlayableDirectorComponentInfo] track curves obj name: {track.curves.GameObject().name}");
                //     // Debug.Log($"[PlayableDirectorComponentInfo] track curves obj name: {track.curves.}");
                //     // track.curves.GameObject()
                //     // AnimationUtility.
                //     // track.curves
                // }
            }
        }
    }
}
