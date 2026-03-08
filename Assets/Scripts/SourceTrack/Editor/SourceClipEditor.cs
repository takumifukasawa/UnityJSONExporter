
// Editor/SourceClipEditor.cs
#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CustomTimelineEditor(typeof(SourceClip))] // ←操作元のクリップタイプ
public class SourceClipEditor : ClipEditor
{
    public override void OnClipChanged(TimelineClip changed)
    {
        var srcTrack = changed.GetParentTrack();
        var timeline = srcTrack?.timelineAsset;
        if (timeline == null) return;

        var srcAsset = changed.asset as SourceClip;
        if (srcAsset == null || srcAsset.links == null || srcAsset.links.Count == 0) return;

        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Sync Linked Clips");

        foreach (var link in srcAsset.links)
        {
            if (link == null || link.targetTrack == null || link.targetClipAsset == null)
                continue;

            var dst = FindClipByAsset(link.targetTrack, link.targetClipAsset);
            if (dst == null) continue;

            // Undo 対応：トラック（またはタイムライン）に対して登録
            Undo.RegisterCompleteObjectUndo(link.targetTrack, "Sync Linked Clip");

            // 同期ロジック
            if (link.syncStart)
                dst.start = changed.start + link.startOffset;

            if (link.syncDuration)
                dst.duration = Math.Max(0.0, changed.duration * Math.Max(1e-9, link.durationScale));

            if (link.syncBlend)
            {
                dst.easeInDuration  = changed.easeInDuration;
                dst.easeOutDuration = changed.easeOutDuration;
            }
        }

        EditorUtility.SetDirty(timeline);
        TimelineEditor.Refresh(RefreshReason.ContentsModified | RefreshReason.SceneNeedsUpdate);
        Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    }

    static TimelineClip FindClipByAsset(TrackAsset track, PlayableAsset asset)
    {
        if (track == null || asset == null) return null;
        foreach (var c in track.GetClips())
            if (c != null && c.asset == asset) return c;
        return null;
    }
}
#endif
