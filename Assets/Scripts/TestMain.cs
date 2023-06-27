using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
using UnityJSONExporter;

[ExecuteInEditMode]
public class TestMain : MonoBehaviour
{
    [Serializable]
    public class TimelineBinding
    {
        public GameObject TargetObject;

        // public UnityEngine.Object T;
        public int trackIndex = 0;
    }

    [SerializeField]
    private bool _syncTimeline;

    [SerializeField]
    private PlayableDirector _playableDirector;

    [SerializeField]
    private List<TimelineBinding> _timelineBindings;

    private void Update()
    {
        // Debug.Log("[TestMain.Update]");

        if (!_syncTimeline)
        {
            return;
        }

        var timelineAsset = _playableDirector.playableAsset as TimelineAsset;
        var tracks = timelineAsset.GetOutputTracks();
        // Debug.Log($"[TestMain.Update] tracks count: {tracks.Count()}");
        for (int i = 0; i < _timelineBindings.Count; i++)
        {
            var timelineBinding = _timelineBindings[i];
            var track = tracks.ToList()[timelineBinding.trackIndex];

            // for debug
            // Debug.Log($"--- track - index: {i}, name: {track.name}, muted: {track.muted}, type: {track.GetType()} --- ");
            // Debug.Log(track.GetType());
            // Debug.Log(track.GetType() == typeof(AnimationTrack));
            // Debug.Log(track.GetType() == typeof(LightControlTrack));

            var currentTime = (float)_playableDirector.time;

            if (track.muted)
            {
                continue;
            }

            // animation track
            if (track.GetType() == typeof(AnimationTrack))
            {
                // Debug.Log($"[TestMain] animation track");
                var animationTrack = track as AnimationTrack;
                var timelineClips = animationTrack.GetClips();
                foreach (var timelineClip in timelineClips)
                {
                    // Debug.Log($"[TestMain] each timeline clip");
                    var animationClip = timelineClip.animationClip;
                    if (animationClip != null)
                    {
                        var animator = _playableDirector.GetGenericBinding(track) as Animator;
                        // Debug.Log($"animator: {animator}");
                        var bindings = AnimationUtility.GetCurveBindings(animationClip);
                        var animationTrackBinder = new AnimationTrackBinder(animationClip, bindings, currentTime);
                        animationTrackBinder.AssignProperty(timelineBinding.TargetObject.transform);
                    }
                }

                continue;
            }

            // light control track
            if (track.GetType() == typeof(LightControlTrack))
            {
                // Debug.Log($"[TestMain] light control track");
                var lightControlTrack = track as LightControlTrack;
                var timelineClips = lightControlTrack.GetClips();
                foreach (var timelineClip in timelineClips)
                {
                    var animationClip = timelineClip.curves;
                    if (animationClip != null)
                    {
                        var light = _playableDirector.GetGenericBinding(track) as Light;
                        // Debug.Log($"light: {light}");
                        // Debug.Log(track.parent);
                        // Debug.Log(track.GetInstanceID());
                        // Debug.Log(GameObject.Find("Directional Light").GetInstanceID());
                        // Debug.Log(track.ToString());
                        var bindings = AnimationUtility.GetCurveBindings(animationClip);
                        var lightControlTrackPropertyBinder = new LightControlTrackPropertyBinder(animationClip, bindings, currentTime);
                        // Debug.Log(lightControlTrackPropertyBinder.Color);
                    }
                }
            }
        }
    }
}