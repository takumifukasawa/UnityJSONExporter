using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
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

            Duration = asset.duration;
            Name = asset.name;

            var timelineAsset = playableDirector.playableAsset as TimelineAsset;

            foreach (var output in timelineAsset.GetOutputTracks())
            {
                Debug.Log($"[PlayableDirectorComponentInfo] output ==============================");
                Debug.Log($"[PlayableDirectorComponentInfo] output name: {output.name}, start: {output.start}, end: {output.end}, duration: {output.duration}, has clip: {output.hasClips}, curves: {output.curves}");
                Debug.Log($"[PlayableDirectorComponentInfo] output to string: {output.ToString()}, instance id: {output.GetInstanceID()}");
                
                var timelineClips = output.GetClips();
                Debug.Log($"[PlayableDirectorComponentInfo] output timeline clips count: {timelineClips.Count()}");
                foreach (var timelineClip in timelineClips)
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] timeline clip ------------------------------");
                    Debug.Log($"[PlayableDirectorComponentInfo] timeline clip asset {timelineClip.asset}");
                    var animationClip = timelineClip.animationClip;
                    var animationPlayableAsset = timelineClip.asset as AnimationPlayableAsset;
                    var bindings = AnimationUtility.GetCurveBindings(animationClip);
                    
                    bool hasLocalPosition = false;
                    var localPosition = Vector3.zero;
                    var hasLocalRotation = false;
                    var localRotationEuler = Vector3.zero;
                    foreach (var binding in bindings)
                    {
                        var curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                        Debug.Log($"[PlayableDirectorComponentInfo] binding ------------------------------");
                        Debug.Log($"[PlayableDirectorComponentInfo] binding type base type: {binding.type.FullName}, is transform: {binding.type.FullName == typeof(Transform).FullName}");
                        Debug.Log($"[PlayableDirectorComponentInfo] binding type: {binding.type}, path: {binding.path}, property: {binding.propertyName}");

                        // animated transform
                        if (binding.type.FullName == typeof(Transform).FullName)
                        {
                            switch (binding.path)
                            {
                                case "m_LocalPosition.x":
                                    hasLocalPosition = true;
                                    break;
                                case "m_LocalPosition.y":
                                    hasLocalPosition = true;
                                    break;
                                case "m_LocalPosition.z":
                                    hasLocalPosition = true;
                                    break;
                                case "localEulerAnglesRaw.x":
                                    hasLocalRotation = true;
                                    break;
                                case "localEulerAnglesRaw.y":
                                    hasLocalRotation = true;
                                    break;
                                case "localEulerAnglesRaw.z":
                                    hasLocalRotation = true;
                                    break;
                                default:
                                    throw new Exception("invalid property");
                            }
                        }

                        var keys = curve.keys.ToList();
                        for (var i = 0; i < keys.Count; i++)
                        {
                            var key = keys[i];
                            Debug.Log(
                                $"[PlayableDirectorComponentInfo] curve in clip - index: {i}, t: {key.time}, value: {key.value}, in-t: {key.inTangent}, in-w: {key.inWeight}, out-t: {key.outTangent}, out-w: {key.outWeight}, weighted mode: {key.weightedMode}");
                        }
                    }
                }
                // Debug.Log($"[PlayableDirectorComponentInfo] output curves obj name: {output.curves.GameObject().name}");
                // Debug.Log($"[PlayableDirectorComponentInfo] output curves obj name: {output.curves.}");
                // output.curves.GameObject()
                // AnimationUtility.
                // output.curves
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ColorUtilities
    {
        public static string ConvertColorToHexString(Color color)
        {
            var r = Mathf.RoundToInt(color.r * 255);
            var g = Mathf.RoundToInt(color.g * 255);
            var b = Mathf.RoundToInt(color.b * 255);
            var a = Mathf.RoundToInt(color.a * 255);

            // for debug
            // Debug.Log(r.ToString("X2"));
            // Debug.Log(g.ToString("X2"));
            // Debug.Log(b.ToString("X2"));
            // Debug.Log(a.ToString("X2"));

            string hexColor = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);

            return hexColor;
        }
    }
}
