using System;
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
        Debug.Log("[TestMain.Update]");

        var timelineAsset = _playableDirector.playableAsset as TimelineAsset;
        var tracks = timelineAsset.GetOutputTracks();
        for (int i = 0; i < _timelineBindings.Count; i++)
        {
            var timelineBinding = _timelineBindings[i];
            var output = tracks.ToList()[timelineBinding.outputIndex];

            // for debug
            // Debug.Log($"--- {output.name} --- ");
            // Debug.Log(output.GetType());
            // Debug.Log(output.GetType() == typeof(AnimationTrack));
            // Debug.Log(output.GetType() == typeof(LightControlTrack));

            var currentTime = (float)_playableDirector.time;

            if (output.muted)
            {
                continue;
            }

            // animation track
            if (output.GetType() == typeof(AnimationTrack))
            {
                var animationTrack = output as AnimationTrack;
                var timelineClips = animationTrack.GetClips();
                foreach (var timelineClip in timelineClips)
                {
                    var animationClip = timelineClip.animationClip;
                    // TODO: 要確認. animation clip になっている = transform の変更のはず
                    if (animationClip != null)
                    {
                        var bindings = AnimationUtility.GetCurveBindings(animationClip);
                        var timelinePropertyBinder = new TimelinePropertyBinder(animationClip, bindings, currentTime);
                        timelinePropertyBinder.AssignProperty(timelineBinding.TargetObject.transform);
                    }
                }

                continue;
            }

            // light control track
            if (output.GetType() == typeof(LightControlTrack))
            {
                var lightControlTrack = output as LightControlTrack;
                var timelineClips = lightControlTrack.GetClips();
                foreach (var timelineClip in timelineClips)
                {
                    var animationClip = timelineClip.curves;
                    if (animationClip != null)
                    {
                        var bindings = AnimationUtility.GetCurveBindings(timelineClip.curves);
                        var timelinePropertyBinder = new TimelinePropertyBinder(animationClip, bindings, currentTime);
                    }
                }
            }
        }
    }
}