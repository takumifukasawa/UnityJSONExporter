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
                Debug.Log($"[PlayableDirectorComponentInfo] output name: {output.name}");
                Debug.Log($"[PlayableDirectorComponentInfo] output duration: {output.duration}");
                Debug.Log($"[PlayableDirectorComponentInfo] output has clips: {output.hasClips}");
                Debug.Log($"[PlayableDirectorComponentInfo] output curves: {output.curves}");
                var timelineClips = output.GetClips();
                Debug.Log($"[PlayableDirectorComponentInfo] output timeline clips count: {timelineClips.Count()}");
                foreach (var timelineClip in timelineClips)
                {
                    Debug.Log($"[PlayableDirectorComponentInfo] timeline clip ------------------------------");
                    var animationClip = timelineClip.animationClip;
                    var bindings = AnimationUtility.GetCurveBindings(animationClip);
                    foreach (var binding in bindings)
                    {
                        var curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                        Debug.Log($"[PlayableDirectorComponentInfo] binding ------------------------------");
                        Debug.Log($"[PlayableDirectorComponentInfo] binding type: {binding.type}, path: {binding.path}, property: {binding.propertyName}");
                        var keys = curve.keys.ToList();
                        for (var i = 0; i < keys.Count; i++)
                        {
                            var key = keys[i];
                            Debug.Log($"[PlayableDirectorComponentInfo] curve in clip - index: {i}, t: {key.time}, value: {key.value}, in-t: {key.inTangent}, in-w: {key.inWeight}, out-t: {key.outTangent}, out-w: {key.outWeight}, weighted mode: {key.weightedMode}");
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