using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityJSONExporter;

public class TestMain : MonoBehaviour
{
    [Serializable]
    public class TimelineBinding
    {
        public GameObject TargetObject;
        public int outputIndex = 0;
    }

    [SerializeField]
    private PlayableDirector _playableDirector;

    [SerializeField]
    private List<TimelineBinding> _timelineBindings;

    private void Update()
    {
        var timelineAsset = _playableDirector.playableAsset as TimelineAsset;
        var tracks = timelineAsset.GetOutputTracks();
        for (int i = 0; i < _timelineBindings.Count; i++)
        {
            var timelineBinding = _timelineBindings[i];
            var output = tracks.ToList()[timelineBinding.outputIndex];
            var timelineClips = output.GetClips();

            foreach (var timelineClip in timelineClips)
            {
                // Debug.Log($"[PlayableDirectorComponentInfo] timeline clip ------------------------------");
                // Debug.Log($"[PlayableDirectorComponentInfo] timeline clip asset {timelineClip.asset}");
                var animationClip = timelineClip.animationClip;
                var bindings = AnimationUtility.GetCurveBindings(animationClip);

                bool hasLocalPosition = false;
                var localPosition = Vector3.zero;
                var hasLocalRotation = false;
                var localRotationEuler = Vector3.zero;
                var hasLocalScale = false;
                var localScale = Vector3.one;

                foreach (var binding in bindings)
                {
                    var curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                    var value = CurveUtilities.EvaluateCurve((float)_playableDirector.time, curve);

                    // animated transform
                    if (binding.type.FullName == typeof(Transform).FullName)
                    {
                        switch (binding.propertyName)
                        {
                            case "m_LocalPosition.x":
                                hasLocalPosition = true;
                                localPosition.x = value;
                                break;
                            case "m_LocalPosition.y":
                                hasLocalPosition = true;
                                localPosition.y = value;
                                break;
                            case "m_LocalPosition.z":
                                hasLocalPosition = true;
                                localPosition.z = value;
                                break;
                            case "localEulerAnglesRaw.x":
                                hasLocalRotation = true;
                                localRotationEuler.x = value;
                                break;
                            case "localEulerAnglesRaw.y":
                                hasLocalRotation = true;
                                localRotationEuler.y = value;
                                break;
                            case "localEulerAnglesRaw.z":
                                hasLocalRotation = true;
                                localRotationEuler.z = value;
                                break;
                            case "m_LocalScale.x":
                                hasLocalScale = true;
                                localScale.x = value;
                                break;
                            case "m_LocalScale.y":
                                hasLocalScale = true;
                                localScale.y = value;
                                break;
                            case "m_LocalScale.z":
                                hasLocalScale = true;
                                localScale.z = value;
                                break;
                            default:
                                throw new Exception($"invalid property: {binding.propertyName}");
                        }
                    }
                }

                if (hasLocalPosition)
                {
                    timelineBinding.TargetObject.transform.localPosition = localPosition;
                }

                if (hasLocalRotation)
                {
                    timelineBinding.TargetObject.transform.localRotation = Quaternion.Euler(localRotationEuler);
                }

                if (hasLocalScale)
                {
                    timelineBinding.TargetObject.transform.localScale = localScale;
                }
            }
            // }
        }
    }
}