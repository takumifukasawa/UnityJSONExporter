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

                // foreach (var binding in bindings)
                // {
                //     // animated transform
                //     if (binding.type.FullName == typeof(Transform).FullName)
                //     {
                //         var transformPropertyBinder = TimelineBindingUtilities.GetCurrentTransformPropertyBinder(animationClip, bindings, (float)_playableDirector.time);
                //         transformPropertyBinder.AssignProperty(timelineBinding.TargetObject.transform);
                //     }
                // }
                var transformPropertyBinder = TimelineBindingUtilities.GetCurrentTransformPropertyBinder(animationClip, bindings, (float)_playableDirector.time);
                transformPropertyBinder.AssignProperty(timelineBinding.TargetObject.transform);
            }
            // }
        }
    }
}