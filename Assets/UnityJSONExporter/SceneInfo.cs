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
    public abstract class PropertyBinder
    {
        public PropertyBinder(
            AnimationClip animationClip,
            EditorCurveBinding[] bindings,
            float time
        )
        {
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class AnimationTrackBinder : PropertyBinder
    {
        // ---------------------------------------------------------------------------
        // constants
        // ---------------------------------------------------------------------------

        const string PROPERTY_LOCAL_POSITION_X = "m_LocalPosition.x";
        const string PROPERTY_LOCAL_POSITION_Y = "m_LocalPosition.y";
        const string PROPERTY_LOCAL_POSITION_Z = "m_LocalPosition.z";
        const string PROPERTY_LOCAL_EULER_ANGLES_RAW_X = "localEulerAnglesRaw.x";
        const string PROPERTY_LOCAL_EULER_ANGLES_RAW_Y = "localEulerAnglesRaw.y";
        const string PROPERTY_LOCAL_EULER_ANGLES_RAW_Z = "localEulerAnglesRaw.z";
        const string PROPERTY_LOCAL_SCALE_X = "m_LocalScale.x";
        const string PROPERTY_LOCAL_SCALE_Y = "m_LocalScale.y";
        const string PROPERTY_LOCAL_SCALE_Z = "m_LocalScale.z";

        // ---------------------------------------------------------------------------
        // public
        // ---------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animationClip"></param>
        /// <param name="bindings"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public AnimationTrackBinder(
            AnimationClip animationClip,
            EditorCurveBinding[] bindings,
            float time
        ) : base(animationClip, bindings, time)
        {
            foreach (var binding in bindings)
            {
                // Debug.Log(binding.type.FullName);
                // animated transform
                if (binding.type.FullName == typeof(Transform).FullName)
                {
                    var curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                    var value = CurveUtilities.EvaluateCurve(time, curve);

                    // // animated transform
                    // if (binding.type.FullName == typeof(Transform).FullName)
                    // {
                    switch (binding.propertyName)
                    {
                        case PROPERTY_LOCAL_POSITION_X:
                            _hasLocalPosition = true;
                            _localPosition.x = value;
                            break;
                        case PROPERTY_LOCAL_POSITION_Y:
                            _hasLocalPosition = true;
                            _localPosition.y = value;
                            break;
                        case PROPERTY_LOCAL_POSITION_Z:
                            _hasLocalPosition = true;
                            _localPosition.z = value;
                            break;
                        case PROPERTY_LOCAL_EULER_ANGLES_RAW_X:
                            _hasLocalRotationEuler = true;
                            _localRotationEuler.x = value;
                            break;
                        case PROPERTY_LOCAL_EULER_ANGLES_RAW_Y:
                            _hasLocalRotationEuler = true;
                            _localRotationEuler.y = value;
                            break;
                        case PROPERTY_LOCAL_EULER_ANGLES_RAW_Z:
                            _hasLocalRotationEuler = true;
                            _localRotationEuler.z = value;
                            break;
                        case PROPERTY_LOCAL_SCALE_X:
                            _hasLocalScale = true;
                            _localScale.x = value;
                            break;
                        case PROPERTY_LOCAL_SCALE_Y:
                            _hasLocalScale = true;
                            _localScale.y = value;
                            break;
                        case PROPERTY_LOCAL_SCALE_Z:
                            _hasLocalScale = true;
                            _localScale.z = value;
                            break;
                        default:
                            throw new Exception($"invalid property: {binding.propertyName}");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public void AssignProperty(Transform t)
        {
            // Debug.Log("==========");
            // Debug.Log(LocalPosition);
            // Debug.Log(LocalRotationEuler);
            // Debug.Log(LocalScale);
            if (_hasLocalPosition)
            {
                t.localPosition = _localPosition;
            }

            if (_hasLocalRotationEuler)
            {
                t.localRotation = Quaternion.Euler(_localRotationEuler);
            }

            if (_hasLocalScale)
            {
                t.localScale = _localScale;
            }
        }

        // ---------------------------------------------------------------------------
        // private
        // ---------------------------------------------------------------------------

        private bool _hasLocalPosition;
        private bool _hasLocalRotationEuler;
        private bool _hasLocalScale;

        private Vector3 _localPosition = Vector3.zero;
        private Vector3 _localRotationEuler = Vector3.zero;
        private Vector3 _localScale = Vector3.one;
    }

    /// <summary>
    /// 
    /// </summary>
    public class LightControlTrackPropertyBinder : PropertyBinder
    {
        public LightControlTrackPropertyBinder(
            AnimationClip animationClip,
            EditorCurveBinding[] bindings,
            float time
        ) : base(animationClip, bindings, time)
        {
            foreach (var binding in bindings)
            {
                Debug.Log($"binding type: {binding.type}, path: {binding.path}, property name: {binding.propertyName}");
                // // animated transform
                // if (binding.type.FullName == typeof(Transform).FullName)
                // {
                // }
            }
        }
    }
}
