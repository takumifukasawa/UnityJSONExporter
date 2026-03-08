using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ClipLink
{
    public TrackAsset targetTrack;
    public PlayableAsset targetClipAsset;
    public bool syncStart = true;
    public double startOffset = 0.0;
    public bool syncDuration = true;
    public double durationScale = 1.0;
    public bool syncBlend = false;
}

[Serializable]
public class SourceClip : PlayableAsset, ITimelineClipAsset
{
    public List<ClipLink> links = new();

    public ClipCaps clipCaps =>
        ClipCaps.Blending | ClipCaps.ClipIn | ClipCaps.Extrapolation | ClipCaps.SpeedMultiplier;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        // 再生時の処理が不要なら Null でもOK（編集時同期が目的ならこれで十分）
        return Playable.Null;
    }
}
