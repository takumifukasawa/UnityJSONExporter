using System;
using UnityEditor;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    public class ActivationTrackInfo : ClipTrackInfoBase
    {
        public ActivationTrackInfo(
            string name,
            ActivationTrack track,
            string targetName
        ) : base(name, TrackInfoType.ActivationTrack, targetName)
        {
            var timelineClips = track.GetClips();
            foreach (var timelineClip in timelineClips)
            {
                Clips.Add(GenerateActivationControlClipInfo(
                    // track as LightControlTrack,
                    timelineClip
                ));
            }
        }

        ClipInfoBase GenerateActivationControlClipInfo(TimelineClip timelineClip)
        {
            var clipInfo = new ActivationControlClipInfo(
                timelineClip.displayName,
                (float)timelineClip.start,
                (float)timelineClip.duration,
                timelineClip.postExtrapolationMode
            );

            return clipInfo;
        }
    }
}
