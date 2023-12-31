using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TestCustomClip : PlayableAsset, ITimelineClipAsset
{
    public TestCustomBehaviour template = new TestCustomBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TestCustomBehaviour>.Create (graph, template);
        return playable;
    }
}
