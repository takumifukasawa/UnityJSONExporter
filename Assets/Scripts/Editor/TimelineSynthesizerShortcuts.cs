using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TimelineSynthesizer;
using UnityJSONExporter;

/// <summary>
/// TimelineSynthesizerManagerのショートカット機能を提供するエディター拡張
/// </summary>
public static class TimelineSynthesizerShortcuts
{

    /// <summary>
    /// ショートカットでTimeline再生を実行
    /// </summary>
    [MenuItem("Tools/Timeline Synthesizer/Play Timeline %#,", false, 1)]
    public static void PlayTimelineFromShortcut()
    {
        var manager = Object.FindObjectOfType<TimelineSynthesizerManager>();
        if (manager != null)
        {
            manager.SendPlay();
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] Timeline Play executed");
        }
        else
        {
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] TimelineSynthesizerManager not found in scene");
        }
    }

    /// <summary>
    /// ショートカットでTimeline停止を実行
    /// </summary>
    [MenuItem("Tools/Timeline Synthesizer/Stop Timeline %#.", false, 2)]
    public static void StopTimelineFromShortcut()
    {
        var manager = Object.FindObjectOfType<TimelineSynthesizerManager>();
        if (manager != null)
        {
            manager.SendStop();
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] Timeline Stop executed");
        }
        else
        {
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] TimelineSynthesizerManager not found in scene");
        }
    }

    // /// <summary>
    // /// ショートカットでScene View Data送信を実行
    // /// </summary>
    // [MenuItem("Tools/Timeline Synthesizer/Send Scene View Data %#x", false, 3)]
    // public static void SendSceneViewDataFromShortcut()
    // {
    //     var manager = Object.FindObjectOfType<TimelineSynthesizerManager>();
    //     if (manager != null)
    //     {
    //         manager.SendSceneViewData();
    //         LoggerProxy.Log("[TimelineSynthesizerShortcuts] Scene View Data sent");
    //     }
    //     else
    //     {
    //         LoggerProxy.Log("[TimelineSynthesizerShortcuts] TimelineSynthesizerManager not found in scene");
    //     }
    // }

    /// <summary>
    /// ショートカットでSendBeginPlayerを実行
    /// </summary>
    [MenuItem("Tools/Timeline Synthesizer/Send Begin Player %#i", false, 31)]
    public static void SendBeginPlayerFromShortcut()
    {
        var manager = Object.FindObjectOfType<TimelineSynthesizerManager>();
        if (manager != null)
        {
            manager.SendBeginPlayer();
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] SendBeginPlayer executed");
        }
        else
        {
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] TimelineSynthesizerManager not found in scene");
        }
    }


    /// <summary>
    /// ショートカットでSendReloadを実行
    /// </summary>
    [MenuItem("Tools/Timeline Synthesizer/Send Reload %#o", false, 31)]
    public static void SendReloadFromShortcut()
    {
        var manager = Object.FindObjectOfType<TimelineSynthesizerManager>();
        if (manager != null)
        {
            manager.SendReload();
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] SendReload executed");
        }
        else
        {
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] TimelineSynthesizerManager not found in scene");
        }
    }

    /// <summary>
    /// ショートカットでTimeline位置を1フレーム進める
    /// </summary>
    [MenuItem("Tools/Timeline Synthesizer/Next Frame %RIGHT", false, 10)]
    public static void NextFrameFromShortcut()
    {
        var playableDirector = GetPlayableDirector();
        if (playableDirector != null)
        {
            MoveTimelineByFrames(playableDirector, 1);
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] Timeline moved to next frame");
        }
        else
        {
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] PlayableDirector not found in scene");
        }
    }

    /// <summary>
    /// ショートカットでTimeline位置を1フレーム戻す
    /// </summary>
    [MenuItem("Tools/Timeline Synthesizer/Previous Frame %LEFT", false, 11)]
    public static void PreviousFrameFromShortcut()
    {
        var playableDirector = GetPlayableDirector();
        if (playableDirector != null)
        {
            MoveTimelineByFrames(playableDirector, -1);
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] Timeline moved to previous frame");
        }
        else
        {
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] PlayableDirector not found in scene");
        }
    }

    /// <summary>
    /// PlayableDirectorを取得する
    /// </summary>
    private static PlayableDirector GetPlayableDirector()
    {
        // まずTimelineSynthesizerManagerから取得を試行
        var manager = Object.FindObjectOfType<TimelineSynthesizerManager>();
        if (manager != null)
        {
            var managerType = manager.GetType();
            var field = managerType.GetField("_playableDirector", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                var director = field.GetValue(manager) as PlayableDirector;
                if (director != null)
                {
                    return director;
                }
            }
        }

        // 見つからない場合はシーン内で直接検索
        return Object.FindObjectOfType<PlayableDirector>();
    }

    /// <summary>
    /// Timeline位置を指定したフレーム数だけ移動する
    /// </summary>
    private static void MoveTimelineByFrames(PlayableDirector director, int frames)
    {
        if (director.playableAsset is TimelineAsset timelineAsset)
        {
            double frameRate = timelineAsset.editorSettings.fps;
            double frameTime = 1.0 / frameRate;
            double currentTime = director.time;
            double newTime = currentTime + (frameTime * frames);

            // 範囲チェック
            newTime = System.Math.Max(0, System.Math.Min(newTime, director.duration));

            // Timeline時間を設定
            director.time = newTime;

            // Timelineを強制的に評価してアニメーション状態を更新
            director.Evaluate();

            // エディターとScene Viewを更新
            EditorUtility.SetDirty(director);
            SceneView.RepaintAll();
            EditorApplication.QueuePlayerLoopUpdate();
        }
    }

    /// <summary>
    /// ショートカットで次のキーフレームにジャンプ
    /// </summary>
    [MenuItem("Tools/Timeline Synthesizer/Next Keyframe %#RIGHT", false, 20)]
    public static void NextKeyframeFromShortcut()
    {
        var playableDirector = GetPlayableDirector();
        if (playableDirector != null)
        {
            var nextKeyTime = GetNextKeyframeTime(playableDirector);
            if (nextKeyTime.HasValue)
            {
                JumpToTime(playableDirector, nextKeyTime.Value);
                LoggerProxy.Log($"[TimelineSynthesizerShortcuts] Jumped to next keyframe at {nextKeyTime.Value:F3}");
            }
            else
            {
                LoggerProxy.Log("[TimelineSynthesizerShortcuts] No next keyframe found");
            }
        }
        else
        {
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] PlayableDirector not found in scene");
        }
    }

    /// <summary>
    /// ショートカットで前のキーフレームにジャンプ
    /// </summary>
    [MenuItem("Tools/Timeline Synthesizer/Previous Keyframe %#LEFT", false, 21)]
    public static void PreviousKeyframeFromShortcut()
    {
        var playableDirector = GetPlayableDirector();
        if (playableDirector != null)
        {
            var prevKeyTime = GetPreviousKeyframeTime(playableDirector);
            if (prevKeyTime.HasValue)
            {
                JumpToTime(playableDirector, prevKeyTime.Value);
                LoggerProxy.Log($"[TimelineSynthesizerShortcuts] Jumped to previous keyframe at {prevKeyTime.Value:F3}");
            }
            else
            {
                LoggerProxy.Log("[TimelineSynthesizerShortcuts] No previous keyframe found");
            }
        }
        else
        {
            LoggerProxy.Log("[TimelineSynthesizerShortcuts] PlayableDirector not found in scene");
        }
    }

    /// <summary>
    /// 次のキーフレーム時刻を取得
    /// </summary>
    private static double? GetNextKeyframeTime(PlayableDirector director)
    {
        var currentTime = director.time;
        var keyframeTimes = GetAllKeyframeTimes(director);

        return keyframeTimes
            .Where(time => time > currentTime + 0.001) // 現在時刻より少し後の時刻
            .OrderBy(time => time)
            .FirstOrDefault();
    }

    /// <summary>
    /// 前のキーフレーム時刻を取得
    /// </summary>
    private static double? GetPreviousKeyframeTime(PlayableDirector director)
    {
        var currentTime = director.time;
        var keyframeTimes = GetAllKeyframeTimes(director);

        return keyframeTimes
            .Where(time => time < currentTime - 0.001) // 現在時刻より少し前の時刻
            .OrderByDescending(time => time)
            .FirstOrDefault();
    }

    /// <summary>
    /// Timeline内の全キーフレーム時刻を取得
    /// </summary>
    private static List<double> GetAllKeyframeTimes(PlayableDirector director)
    {
        var keyframeTimes = new List<double>();

        if (!(director.playableAsset is TimelineAsset timelineAsset))
            return keyframeTimes;

        // 選択中のクリップから優先的にキーフレームを取得
        var selectedClips = GetSelectedTimelineClips();
        if (selectedClips.Any())
        {
            foreach (var clip in selectedClips)
            {
                ExtractKeyframesFromClip(clip, keyframeTimes);
            }
        }
        else
        {
            // 選択がない場合は全トラックから取得
            foreach (var track in timelineAsset.GetOutputTracks())
            {
                ExtractKeyframesFromTrack(track, keyframeTimes);
            }
        }

        return keyframeTimes.Distinct().ToList();
    }

    /// <summary>
    /// 選択中のTimelineClipを取得
    /// </summary>
    private static List<TimelineClip> GetSelectedTimelineClips()
    {
        var selectedClips = new List<TimelineClip>();

        try
        {
            // Timeline Editor内部API使用を試行
            var timelineEditorType = System.Type.GetType("UnityEditor.Timeline.TimelineEditor, Unity.Timeline.Editor");
            if (timelineEditorType != null)
            {
                var selectedClipsProperty = timelineEditorType.GetProperty("selectedClips",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (selectedClipsProperty != null)
                {
                    var clips = selectedClipsProperty.GetValue(null) as IEnumerable<TimelineClip>;
                    if (clips != null)
                    {
                        selectedClips.AddRange(clips);
                    }
                }
            }
        }
        catch
        {
            // 内部API失敗時は空リストを返す
        }

        return selectedClips;
    }

    /// <summary>
    /// トラックからキーフレームを抽出
    /// </summary>
    private static void ExtractKeyframesFromTrack(TrackAsset track, List<double> keyframeTimes)
    {
        foreach (var clip in track.GetClips())
        {
            ExtractKeyframesFromClip(clip, keyframeTimes);
        }
    }

    /// <summary>
    /// クリップからキーフレームを抽出
    /// </summary>
    private static void ExtractKeyframesFromClip(TimelineClip clip, List<double> keyframeTimes)
    {
        // クリップの開始・終了時刻を追加
        keyframeTimes.Add(clip.start);
        keyframeTimes.Add(clip.end);

        // AnimationClipの場合はキーフレーム時刻も取得
        if (clip.asset is AnimationPlayableAsset animationAsset && animationAsset.clip != null)
        {
            var animClip = animationAsset.clip;
            var bindings = AnimationUtility.GetCurveBindings(animClip);

            foreach (var binding in bindings)
            {
                var curve = AnimationUtility.GetEditorCurve(animClip, binding);
                if (curve != null)
                {
                    foreach (var key in curve.keys)
                    {
                        // ローカル時刻をグローバル時刻に変換
                        var globalTime = clip.start + (key.time * clip.timeScale);
                        if (globalTime >= clip.start && globalTime <= clip.end)
                        {
                            keyframeTimes.Add(globalTime);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 指定時刻にジャンプ
    /// </summary>
    private static void JumpToTime(PlayableDirector director, double time)
    {
        // 範囲チェック
        time = System.Math.Max(0, System.Math.Min(time, director.duration));

        director.time = time;
        director.Evaluate();
        EditorUtility.SetDirty(director);
        SceneView.RepaintAll();
        EditorApplication.QueuePlayerLoopUpdate();
    }
}
