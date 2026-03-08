
// Assets/Editor/LogClipAction.cs
#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Utilities;

// Euler角の取得方法を切り替えるモード
public enum EulerAngleMode
{
    InspectorHint,      // m_LocalEulerAnglesHint優先（現在の実装）
    DirectLocalEuler    // transform.localEulerAnglesをそのまま使用
}

// グローバルなEuler角取得モード（全クラス共通、切り替えはここを変更）
public static class EulerAngleModeConfig
{
    public const EulerAngleMode Mode = EulerAngleMode.InspectorHint;
    // public const EulerAngleMode Mode = EulerAngleMode.DirectLocalEuler;
}

[MenuEntry("Clear IK Keyframes/Left Arm", 0)]
public class ClearLeftArmIKKeyframesAction : ClipAction
{
    public override ActionValidity Validate(IEnumerable<TimelineClip> clips)
    {
        return clips != null && clips.Any(c => c != null && c.asset is AnimationPlayableAsset)
            ? ActionValidity.Valid : ActionValidity.NotApplicable;
    }

    public override bool Execute(IEnumerable<TimelineClip> clips)
    {
        PlayableDirector director = GetPlayableDirector();
        if (director == null)
        {
            Debug.LogWarning("[ClearLeftArmIKKeyframes] PlayableDirectorが見つかりません");
            return false;
        }

        foreach (var clip in clips.Where(c => c != null))
        {
            ClearLeftArmIKKeyframes(clip, director);
        }
        return true;
    }

    private PlayableDirector GetPlayableDirector()
    {
        PlayableDirector director = null;
        try
        {
            var timelineWindow = TimelineEditor.GetWindow();
            if (timelineWindow != null)
            {
                var stateField = typeof(TimelineEditor).GetField("state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (stateField != null)
                {
                    var state = stateField.GetValue(timelineWindow);
                    if (state != null)
                    {
                        var directorProperty = state.GetType().GetProperty("editSequence");
                        if (directorProperty != null)
                        {
                            var editSequence = directorProperty.GetValue(state);
                            if (editSequence != null)
                            {
                                var directorField = editSequence.GetType().GetProperty("director");
                                if (directorField != null)
                                {
                                    director = directorField.GetValue(editSequence) as PlayableDirector;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"TimelineEditorから取得に失敗: {e.Message}");
        }
        
        if (director == null)
        {
            var directors = Object.FindObjectsOfType<PlayableDirector>();
            if (directors.Length > 0)
            {
                director = directors[0];
                Debug.Log($"シーン内のPlayableDirectorを使用: {director.name}");
            }
        }
        return director;
    }

    private void ClearLeftArmIKKeyframes(TimelineClip clip, PlayableDirector director)
    {
        Debug.Log($"=== Left Arm IK Keyframes削除: {clip.displayName} ===");
        
        var track = clip.GetParentTrack();
        if (track == null) { Debug.LogWarning("トラックが見つかりません"); return; }
        
        AnimationClip animClip = null;
        if (clip.asset is AnimationPlayableAsset animAsset) { animClip = animAsset.clip; }
        if (animClip == null) { Debug.LogWarning("AnimationClipが見つかりません"); return; }
        
        Debug.Log($"AnimationClip: {animClip.name}");
        
        // 削除対象プロパティ（Root: Shoulder, Mid: Elbow）
        string[] targetProperties = {
            "LeftShoulderRotation.x", "LeftShoulderRotation.y", "LeftShoulderRotation.z",
            "LeftShoulderPosition.x", "LeftShoulderPosition.y", "LeftShoulderPosition.z",
            "LeftElbowRotation.x", "LeftElbowRotation.y", "LeftElbowRotation.z",
            "LeftElbowPosition.x", "LeftElbowPosition.y", "LeftElbowPosition.z"
        };
        
        int clearedCount = 0;
        foreach (string propertyName in targetProperties)
        {
            if (ClearAllKeyframes(animClip, propertyName))
            {
                clearedCount++;
                Debug.Log($"✓ {propertyName} の全キーフレームを削除");
            }
        }
        
        if (clearedCount > 0)
        {
            EditorUtility.SetDirty(animClip);
            AssetDatabase.SaveAssets();
            Debug.Log($"✓ Left Arm IK Keyframes削除完了: {clearedCount}プロパティ");
        }
        else
        {
            Debug.Log("削除対象のキーフレームが見つかりませんでした");
        }
        
        Debug.Log("=====================================");
    }
    
    private bool ClearAllKeyframes(AnimationClip animClip, string propertyName)
    {
        var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), propertyName);
        var curve = AnimationUtility.GetEditorCurve(animClip, binding);
        
        if (curve == null || curve.keys.Length == 0)
        {
            return false;
        }
        
        var emptyCurve = new AnimationCurve();
        AnimationUtility.SetEditorCurve(animClip, binding, emptyCurve);
        
        return true;
    }
}

[MenuEntry("Clear IK Keyframes/Right Arm", 1)]
public class ClearRightArmIKKeyframesAction : ClipAction
{
    public override ActionValidity Validate(IEnumerable<TimelineClip> clips)
    {
        return clips != null && clips.Any(c => c != null && c.asset is AnimationPlayableAsset)
            ? ActionValidity.Valid : ActionValidity.NotApplicable;
    }

    public override bool Execute(IEnumerable<TimelineClip> clips)
    {
        PlayableDirector director = GetPlayableDirector();
        if (director == null)
        {
            Debug.LogWarning("[ClearRightArmIKKeyframes] PlayableDirectorが見つかりません");
            return false;
        }

        foreach (var clip in clips.Where(c => c != null))
        {
            ClearRightArmIKKeyframes(clip, director);
        }
        return true;
    }

    private PlayableDirector GetPlayableDirector()
    {
        PlayableDirector director = null;
        try
        {
            var timelineWindow = TimelineEditor.GetWindow();
            if (timelineWindow != null)
            {
                var stateField = typeof(TimelineEditor).GetField("state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (stateField != null)
                {
                    var state = stateField.GetValue(timelineWindow);
                    if (state != null)
                    {
                        var directorProperty = state.GetType().GetProperty("editSequence");
                        if (directorProperty != null)
                        {
                            var editSequence = directorProperty.GetValue(state);
                            if (editSequence != null)
                            {
                                var directorField = editSequence.GetType().GetProperty("director");
                                if (directorField != null)
                                {
                                    director = directorField.GetValue(editSequence) as PlayableDirector;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"TimelineEditorから取得に失敗: {e.Message}");
        }
        
        if (director == null)
        {
            var directors = Object.FindObjectsOfType<PlayableDirector>();
            if (directors.Length > 0)
            {
                director = directors[0];
                Debug.Log($"シーン内のPlayableDirectorを使用: {director.name}");
            }
        }
        return director;
    }

    private void ClearRightArmIKKeyframes(TimelineClip clip, PlayableDirector director)
    {
        Debug.Log($"=== Right Arm IK Keyframes削除: {clip.displayName} ===");
        
        var track = clip.GetParentTrack();
        if (track == null) { Debug.LogWarning("トラックが見つかりません"); return; }
        
        AnimationClip animClip = null;
        if (clip.asset is AnimationPlayableAsset animAsset) { animClip = animAsset.clip; }
        if (animClip == null) { Debug.LogWarning("AnimationClipが見つかりません"); return; }
        
        Debug.Log($"AnimationClip: {animClip.name}");
        
        // 削除対象プロパティ（Root: Shoulder, Mid: Elbow）
        string[] targetProperties = {
            "RightShoulderRotation.x", "RightShoulderRotation.y", "RightShoulderRotation.z",
            "RightShoulderPosition.x", "RightShoulderPosition.y", "RightShoulderPosition.z",
            "RightElbowRotation.x", "RightElbowRotation.y", "RightElbowRotation.z",
            "RightElbowPosition.x", "RightElbowPosition.y", "RightElbowPosition.z"
        };
        
        int clearedCount = 0;
        foreach (string propertyName in targetProperties)
        {
            if (ClearAllKeyframes(animClip, propertyName))
            {
                clearedCount++;
                Debug.Log($"✓ {propertyName} の全キーフレームを削除");
            }
        }
        
        if (clearedCount > 0)
        {
            EditorUtility.SetDirty(animClip);
            AssetDatabase.SaveAssets();
            Debug.Log($"✓ Right Arm IK Keyframes削除完了: {clearedCount}プロパティ");
        }
        else
        {
            Debug.Log("削除対象のキーフレームが見つかりませんでした");
        }
        
        Debug.Log("=====================================");
    }
    
    private bool ClearAllKeyframes(AnimationClip animClip, string propertyName)
    {
        var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), propertyName);
        var curve = AnimationUtility.GetEditorCurve(animClip, binding);
        
        if (curve == null || curve.keys.Length == 0)
        {
            return false;
        }
        
        var emptyCurve = new AnimationCurve();
        AnimationUtility.SetEditorCurve(animClip, binding, emptyCurve);
        
        return true;
    }
}

[MenuEntry("Clear IK Keyframes/Left Foot", 2)]
public class ClearLeftFootIKKeyframesAction : ClipAction
{
    public override ActionValidity Validate(IEnumerable<TimelineClip> clips)
    {
        return clips != null && clips.Any(c => c != null && c.asset is AnimationPlayableAsset)
            ? ActionValidity.Valid : ActionValidity.NotApplicable;
    }

    public override bool Execute(IEnumerable<TimelineClip> clips)
    {
        PlayableDirector director = GetPlayableDirector();
        if (director == null)
        {
            Debug.LogWarning("[ClearLeftFootIKKeyframes] PlayableDirectorが見つかりません");
            return false;
        }

        foreach (var clip in clips.Where(c => c != null))
        {
            ClearLeftFootIKKeyframes(clip, director);
        }
        return true;
    }

    private PlayableDirector GetPlayableDirector()
    {
        PlayableDirector director = null;
        try
        {
            var timelineWindow = TimelineEditor.GetWindow();
            if (timelineWindow != null)
            {
                var stateField = typeof(TimelineEditor).GetField("state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (stateField != null)
                {
                    var state = stateField.GetValue(timelineWindow);
                    if (state != null)
                    {
                        var directorProperty = state.GetType().GetProperty("editSequence");
                        if (directorProperty != null)
                        {
                            var editSequence = directorProperty.GetValue(state);
                            if (editSequence != null)
                            {
                                var directorField = editSequence.GetType().GetProperty("director");
                                if (directorField != null)
                                {
                                    director = directorField.GetValue(editSequence) as PlayableDirector;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"TimelineEditorから取得に失敗: {e.Message}");
        }
        
        if (director == null)
        {
            var directors = Object.FindObjectsOfType<PlayableDirector>();
            if (directors.Length > 0)
            {
                director = directors[0];
                Debug.Log($"シーン内のPlayableDirectorを使用: {director.name}");
            }
        }
        return director;
    }

    private void ClearLeftFootIKKeyframes(TimelineClip clip, PlayableDirector director)
    {
        Debug.Log($"=== Left Foot IK Keyframes削除: {clip.displayName} ===");
        
        var track = clip.GetParentTrack();
        if (track == null) { Debug.LogWarning("トラックが見つかりません"); return; }
        
        AnimationClip animClip = null;
        if (clip.asset is AnimationPlayableAsset animAsset) { animClip = animAsset.clip; }
        if (animClip == null) { Debug.LogWarning("AnimationClipが見つかりません"); return; }
        
        Debug.Log($"AnimationClip: {animClip.name}");
        
        // 削除対象プロパティ（Root: Hip, Mid: Knee）
        string[] targetProperties = {
            "LeftHipRotation.x", "LeftHipRotation.y", "LeftHipRotation.z",
            "LeftHipPosition.x", "LeftHipPosition.y", "LeftHipPosition.z",
            "LeftKneeRotation.x", "LeftKneeRotation.y", "LeftKneeRotation.z",
            "LeftKneePosition.x", "LeftKneePosition.y", "LeftKneePosition.z"
        };
        
        int clearedCount = 0;
        foreach (string propertyName in targetProperties)
        {
            if (ClearAllKeyframes(animClip, propertyName))
            {
                clearedCount++;
                Debug.Log($"✓ {propertyName} の全キーフレームを削除");
            }
        }
        
        if (clearedCount > 0)
        {
            EditorUtility.SetDirty(animClip);
            AssetDatabase.SaveAssets();
            Debug.Log($"✓ Left Foot IK Keyframes削除完了: {clearedCount}プロパティ");
        }
        else
        {
            Debug.Log("削除対象のキーフレームが見つかりませんでした");
        }
        
        Debug.Log("=====================================");
    }
    
    private bool ClearAllKeyframes(AnimationClip animClip, string propertyName)
    {
        var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), propertyName);
        var curve = AnimationUtility.GetEditorCurve(animClip, binding);
        
        if (curve == null || curve.keys.Length == 0)
        {
            return false;
        }
        
        var emptyCurve = new AnimationCurve();
        AnimationUtility.SetEditorCurve(animClip, binding, emptyCurve);
        
        return true;
    }
}

[MenuEntry("Clear IK Keyframes/Right Foot", 3)]
public class ClearRightFootIKKeyframesAction : ClipAction
{
    public override ActionValidity Validate(IEnumerable<TimelineClip> clips)
    {
        return clips != null && clips.Any(c => c != null && c.asset is AnimationPlayableAsset)
            ? ActionValidity.Valid : ActionValidity.NotApplicable;
    }

    public override bool Execute(IEnumerable<TimelineClip> clips)
    {
        PlayableDirector director = GetPlayableDirector();
        if (director == null)
        {
            Debug.LogWarning("[ClearRightFootIKKeyframes] PlayableDirectorが見つかりません");
            return false;
        }

        foreach (var clip in clips.Where(c => c != null))
        {
            ClearRightFootIKKeyframes(clip, director);
        }
        return true;
    }

    private PlayableDirector GetPlayableDirector()
    {
        PlayableDirector director = null;
        try
        {
            var timelineWindow = TimelineEditor.GetWindow();
            if (timelineWindow != null)
            {
                var stateField = typeof(TimelineEditor).GetField("state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (stateField != null)
                {
                    var state = stateField.GetValue(timelineWindow);
                    if (state != null)
                    {
                        var directorProperty = state.GetType().GetProperty("editSequence");
                        if (directorProperty != null)
                        {
                            var editSequence = directorProperty.GetValue(state);
                            if (editSequence != null)
                            {
                                var directorField = editSequence.GetType().GetProperty("director");
                                if (directorField != null)
                                {
                                    director = directorField.GetValue(editSequence) as PlayableDirector;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"TimelineEditorから取得に失敗: {e.Message}");
        }
        
        if (director == null)
        {
            var directors = Object.FindObjectsOfType<PlayableDirector>();
            if (directors.Length > 0)
            {
                director = directors[0];
                Debug.Log($"シーン内のPlayableDirectorを使用: {director.name}");
            }
        }
        return director;
    }

    private void ClearRightFootIKKeyframes(TimelineClip clip, PlayableDirector director)
    {
        Debug.Log($"=== Right Foot IK Keyframes削除: {clip.displayName} ===");
        
        var track = clip.GetParentTrack();
        if (track == null) { Debug.LogWarning("トラックが見つかりません"); return; }
        
        AnimationClip animClip = null;
        if (clip.asset is AnimationPlayableAsset animAsset) { animClip = animAsset.clip; }
        if (animClip == null) { Debug.LogWarning("AnimationClipが見つかりません"); return; }
        
        Debug.Log($"AnimationClip: {animClip.name}");
        
        // 削除対象プロパティ（Root: Hip, Mid: Knee）
        string[] targetProperties = {
            "RightHipRotation.x", "RightHipRotation.y", "RightHipRotation.z",
            "RightHipPosition.x", "RightHipPosition.y", "RightHipPosition.z",
            "RightKneeRotation.x", "RightKneeRotation.y", "RightKneeRotation.z",
            "RightKneePosition.x", "RightKneePosition.y", "RightKneePosition.z"
        };
        
        int clearedCount = 0;
        foreach (string propertyName in targetProperties)
        {
            if (ClearAllKeyframes(animClip, propertyName))
            {
                clearedCount++;
                Debug.Log($"✓ {propertyName} の全キーフレームを削除");
            }
        }
        
        if (clearedCount > 0)
        {
            EditorUtility.SetDirty(animClip);
            AssetDatabase.SaveAssets();
            Debug.Log($"✓ Right Foot IK Keyframes削除完了: {clearedCount}プロパティ");
        }
        else
        {
            Debug.Log("削除対象のキーフレームが見つかりませんでした");
        }
        
        Debug.Log("=====================================");
    }
    
    private bool ClearAllKeyframes(AnimationClip animClip, string propertyName)
    {
        var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), propertyName);
        var curve = AnimationUtility.GetEditorCurve(animClip, binding);
        
        if (curve == null || curve.keys.Length == 0)
        {
            return false;
        }
        
        var emptyCurve = new AnimationCurve();
        AnimationUtility.SetEditorCurve(animClip, binding, emptyCurve);
        
        return true;
    }
}

[MenuEntry("Copy IK Target to Clip/Left Arm", 0)]
public class CopyLeftArmIKTargetToClipAction : ClipAction
{
    public override ActionValidity Validate(IEnumerable<TimelineClip> clips)
    {
        return clips != null && clips.Any(c => c != null && c.asset is AnimationPlayableAsset)
            ? ActionValidity.Valid : ActionValidity.NotApplicable;
    }

    public override bool Execute(IEnumerable<TimelineClip> clips)
    {
        // Timeline Directorを取得
        PlayableDirector director = GetPlayableDirector();
        
        if (director == null)
        {
            Debug.LogWarning("[CopyLeftArmIKTarget] PlayableDirectorが見つかりません");
            return false;
        }

        foreach (var clip in clips.Where(c => c != null))
        {
            CopyLeftArmIKTargetToClip(clip, director);
        }
        return true;
    }

    private PlayableDirector GetPlayableDirector()
    {
        PlayableDirector director = null;
        
        // TimelineEditorから取得を試行
        try
        {
            var timelineWindow = TimelineEditor.GetWindow();
            if (timelineWindow != null)
            {
                var stateField = typeof(TimelineEditor).GetField("state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (stateField != null)
                {
                    var state = stateField.GetValue(timelineWindow);
                    if (state != null)
                    {
                        var directorProperty = state.GetType().GetProperty("editSequence");
                        if (directorProperty != null)
                        {
                            var editSequence = directorProperty.GetValue(state);
                            if (editSequence != null)
                            {
                                var directorField = editSequence.GetType().GetProperty("director");
                                if (directorField != null)
                                {
                                    director = directorField.GetValue(editSequence) as PlayableDirector;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"TimelineEditorから取得に失敗: {e.Message}");
        }
        
        // シーン内のPlayableDirectorを検索
        if (director == null)
        {
            var directors = Object.FindObjectsOfType<PlayableDirector>();
            if (directors.Length > 0)
            {
                director = directors[0];
                Debug.Log($"シーン内のPlayableDirectorを使用: {director.name}");
            }
        }
        
        return director;
    }

    private void CopyLeftArmIKTargetToClip(TimelineClip clip, PlayableDirector director)
    {
        Debug.Log($"=== Left Arm IK Target → Clip変換: {clip.displayName} ===");
        
        // トラック情報
        var track = clip.GetParentTrack();
        if (track == null)
        {
            Debug.LogWarning("トラックが見つかりません");
            return;
        }
        
        // AnimationClipを取得
        AnimationClip animClip = null;
        if (clip.asset is AnimationPlayableAsset animAsset)
        {
            animClip = animAsset.clip;
        }
        
        if (animClip == null)
        {
            Debug.LogWarning("AnimationClipが見つかりません");
            return;
        }
        
        Debug.Log($"AnimationClip: {animClip.name}");

        // Timeline絶対時刻からClip内相対時刻を計算
        float timelineTime = (float)director.time;
        float clipTime = timelineTime - (float)clip.start;
        Debug.Log($"Timeline時刻: {timelineTime:F3}s, Clip開始: {clip.start:F3}s, Clip内相対時間: {clipTime:F3}s");

        // バインドされたオブジェクトからHumanControllerを取得
        var humanController = GetHumanControllerFromTrack(track, director);
        if (humanController == null)
        {
            Debug.LogWarning("HumanControllerが見つかりません");
            return;
        }

        Debug.Log($"✓ HumanControllerが見つかりました: {humanController.gameObject.name}");

        // IK targetのlocalPositionを取得
        Vector3 targetLocalPosition = GetLeftArmIKTargetLocalPosition(humanController);
        Debug.Log($"Left Arm IK Target LocalPosition: {targetLocalPosition}");

        // IK root/midのlocalRotationを取得
        var ikRotations = GetLeftArmIKRotations(humanController);
        Debug.Log($"Left Arm IK Shoulder Rotation: {ikRotations.shoulderRotation}");
        Debug.Log($"Left Arm IK Elbow Rotation: {ikRotations.elbowRotation}");

        // 既存カーブのイージング情報を分析（LeftHandIKPosition.yを基準）
        var easingInfo = AnalyzeLeftArmExistingCurveEasing(animClip, clipTime);
        Debug.Log($"検出されたイージング: inTangent={easingInfo.inTangent:F3}, outTangent={easingInfo.outTangent:F3}");

        // AnimationClipのカーブを更新（イージング情報付き）
        UpdateLeftArmIKPositionCurveWithEasing(animClip, clipTime, targetLocalPosition, easingInfo);
        UpdateLeftArmRotationCurveWithEasing(animClip, "LeftShoulderRotation", clipTime, ikRotations.shoulderRotation, easingInfo);
        UpdateLeftArmRotationCurveWithEasing(animClip, "LeftElbowRotation", clipTime, ikRotations.elbowRotation, easingInfo);
        
        // アセットを保存
        EditorUtility.SetDirty(animClip);
        AssetDatabase.SaveAssets();
        
        Debug.Log("✓ Left Arm IK Target & Rotations → Clip変換完了");
        Debug.Log("=====================================");
    }
    
    private float GetCurrentTimelineTime(PlayableDirector director)
    {
        return (float)director.time;
    }
    
    private HumanController GetHumanControllerFromTrack(TrackAsset track, PlayableDirector director)
    {
        var boundObject = director.GetGenericBinding(track);
        if (boundObject == null) return null;
        
        GameObject targetGameObject = null;
        if (boundObject is GameObject go)
        {
            targetGameObject = go;
        }
        else if (boundObject is Component comp)
        {
            targetGameObject = comp.gameObject;
        }
        
        if (targetGameObject == null) return null;
        
        var humanController = targetGameObject.GetComponent<HumanController>();
        if (humanController == null)
        {
            humanController = targetGameObject.GetComponentInParent<HumanController>();
        }
        if (humanController == null)
        {
            humanController = targetGameObject.GetComponentInChildren<HumanController>();
        }
        
        return humanController;
    }
    
    private Vector3 GetLeftArmIKTargetLocalPosition(HumanController humanController)
    {
        // LeftArmIKの取得（リフレクションを使用）
        var leftArmIKField = typeof(HumanController).GetField("_leftArmIK", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (leftArmIKField == null)
        {
            Debug.LogWarning("_leftArmIKフィールドが見つかりません");
            return Vector3.zero;
        }
        
        var leftArmIK = leftArmIKField.GetValue(humanController) as TwoBoneIK;
        if (leftArmIK == null)
        {
            Debug.LogWarning("_leftArmIKが設定されていません");
            return Vector3.zero;
        }
        
        if (leftArmIK.target == null)
        {
            Debug.LogWarning("Left Arm IK targetが設定されていません");
            return Vector3.zero;
        }
        
        Debug.Log($"Left Arm IK Target: {leftArmIK.target.name}");
        return leftArmIK.target.localPosition;
    }

    private Vector3 GetInspectorEulerAngles(Transform transform)
    {
        Quaternion q = transform.localRotation;

        // モード切り替え: DirectLocalEulerの場合はlocalEulerAnglesをそのまま使用
        if (EulerAngleModeConfig.Mode == EulerAngleMode.DirectLocalEuler)
        {
            Vector3 localEuler = transform.localEulerAngles;
            Vector3 normalized = TwoBoneIK.NormalizeEulerAngles(localEuler);
            Debug.Log($"  [DirectLocalEuler] {transform.name}: raw={localEuler}, normalized={normalized}");
            return normalized;
        }

        // 単一軸回転の最適化チェック
        const float epsilon = 0.0001f;

        // X軸のみの回転をチェック
        if (Mathf.Abs(q.y) < epsilon && Mathf.Abs(q.z) < epsilon)
        {
            float angle = 2.0f * Mathf.Atan2(q.x, q.w) * Mathf.Rad2Deg;
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            Debug.Log($"  Quaternion: {q}, Single-axis X rotation: ({angle:F2}, 0, 0)");
            return new Vector3(angle, 0, 0);
        }

        // Y軸のみの回転をチェック
        if (Mathf.Abs(q.x) < epsilon && Mathf.Abs(q.z) < epsilon)
        {
            float angle = 2.0f * Mathf.Atan2(q.y, q.w) * Mathf.Rad2Deg;
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            Debug.Log($"  Quaternion: {q}, Single-axis Y rotation: (0, {angle:F2}, 0)");
            return new Vector3(0, angle, 0);
        }

        // Z軸のみの回転をチェック
        if (Mathf.Abs(q.x) < epsilon && Mathf.Abs(q.y) < epsilon)
        {
            float angle = 2.0f * Mathf.Atan2(q.z, q.w) * Mathf.Rad2Deg;
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            Debug.Log($"  Quaternion: {q}, Single-axis Z rotation: (0, 0, {angle:F2})");
            return new Vector3(0, 0, angle);
        }

        // 複雑な回転: 詳細調査
        Debug.Log($"  === Detailed rotation analysis for {transform.name} ===");
        Debug.Log($"    Quaternion (localRotation): {q}");
        Debug.Log($"    localEulerAngles (raw): {transform.localEulerAngles}");
        Debug.Log($"    Parent: {(transform.parent ? transform.parent.name : "none")}");

        // SerializedPropertyでm_LocalEulerAnglesHintを確認
        Vector3? eulerHintValue = null;
        try
        {
            SerializedObject so = new SerializedObject(transform);
            var eulerHint = so.FindProperty("m_LocalEulerAnglesHint");
            if (eulerHint != null && eulerHint.propertyType == SerializedPropertyType.Vector3)
            {
                eulerHintValue = eulerHint.vector3Value;
                Debug.Log($"    m_LocalEulerAnglesHint: {eulerHintValue.Value}");

                // m_LocalEulerAnglesHintがゼロで、実際のQuaternionが回転している場合は無視
                if (eulerHintValue.Value.sqrMagnitude < 0.01f && !Mathf.Approximately(Quaternion.Angle(q, Quaternion.identity), 0f))
                {
                    Debug.Log($"    ⚠ m_LocalEulerAnglesHint is zero but rotation exists, using Quaternion conversion");
                }
                else
                {
                    Debug.Log($"    ✓ Using m_LocalEulerAnglesHint (Inspector value)");
                    return eulerHintValue.Value;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"    m_LocalEulerAnglesHint access failed: {e.Message}");
        }

        // m_LocalEulerAnglesHintが使えない場合のフォールバック
        Debug.Log($"    Using Quaternion to Euler conversion");

        // 一時GameObjectを使ってUnity標準のEuler角変換を利用
        GameObject tempGO = new GameObject("TempEulerCalculation");
        Vector3 baseEuler;
        try
        {
            tempGO.hideFlags = HideFlags.HideAndDontSave;
            tempGO.transform.localRotation = q;
            Vector3 euler = tempGO.transform.localEulerAngles;
            baseEuler = TwoBoneIK.NormalizeEulerAngles(euler);
            Debug.Log($"    Unity Euler (raw): {euler}");
            Debug.Log($"    Unity Euler (normalized): {baseEuler}");
        }
        finally
        {
            Object.DestroyImmediate(tempGO);
        }

        // 等価なEuler角表現を生成して、元のQuaternionに最も近いものを選択
        Vector3[] equivalents = new Vector3[8];
        equivalents[0] = baseEuler;
        equivalents[1] = new Vector3(180 - baseEuler.x, baseEuler.y - 180, baseEuler.z - 180);
        equivalents[2] = new Vector3(180 - baseEuler.x, 180 - baseEuler.y, 180 + baseEuler.z);
        equivalents[3] = new Vector3(-baseEuler.x, 180 + baseEuler.y, 180 - baseEuler.z);
        equivalents[4] = new Vector3(baseEuler.x + 180, baseEuler.y + 180, baseEuler.z);
        equivalents[5] = new Vector3(baseEuler.x, baseEuler.y + 180, baseEuler.z + 180);
        equivalents[6] = new Vector3(baseEuler.x + 180, baseEuler.y, baseEuler.z + 180);
        equivalents[7] = new Vector3(baseEuler.x - 180, baseEuler.y - 180, baseEuler.z);

        float minAngle = float.MaxValue;
        Vector3 bestEuler = baseEuler;

        for (int i = 0; i < equivalents.Length; i++)
        {
            // -180～180に正規化
            Vector3 normalized = TwoBoneIK.NormalizeEulerAngles(equivalents[i]);
            Quaternion testQ = Quaternion.Euler(normalized);
            float angle = Quaternion.Angle(q, testQ);

            if (angle < minAngle)
            {
                minAngle = angle;
                bestEuler = normalized;
            }
        }

        Debug.Log($"    ✓ Best equivalent: {bestEuler} (angle diff: {minAngle:F3}°)");
        return bestEuler;
    }


    private (Vector3 shoulderRotation, Vector3 elbowRotation) GetLeftArmIKRotations(HumanController humanController)
    {
        // LeftArmIKの取得（リフレクションを使用）
        var leftArmIKField = typeof(HumanController).GetField("_leftArmIK", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (leftArmIKField == null)
        {
            Debug.LogWarning("_leftArmIKフィールドが見つかりません");
            return (Vector3.zero, Vector3.zero);
        }
        
        var leftArmIK = leftArmIKField.GetValue(humanController) as TwoBoneIK;
        if (leftArmIK == null)
        {
            Debug.LogWarning("_leftArmIKが設定されていません");
            return (Vector3.zero, Vector3.zero);
        }
        
        Vector3 shoulderRotation = Vector3.zero;
        Vector3 elbowRotation = Vector3.zero;

        if (leftArmIK.root != null)
        {
            shoulderRotation = GetInspectorEulerAngles(leftArmIK.root);
            Debug.Log($"Left Arm IK Root (Shoulder): {leftArmIK.root.name}");
            Debug.Log($"  Rotation: {shoulderRotation}");
        }
        else
        {
            Debug.LogWarning("Left Arm IK rootが設定されていません");
        }

        if (leftArmIK.mid != null)
        {
            elbowRotation = GetInspectorEulerAngles(leftArmIK.mid);
            Debug.Log($"Left Arm IK Mid (Elbow): {leftArmIK.mid.name}");
            Debug.Log($"  Rotation: {elbowRotation}");
        }
        else
        {
            Debug.LogWarning("Left Arm IK midが設定されていません");
        }

        return (shoulderRotation, elbowRotation);
    }
    
    private (float inTangent, float outTangent) AnalyzeLeftArmExistingCurveEasing(AnimationClip animClip, float time)
    {
        // LeftHandIKPosition.y カーブを基準として使用
        var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), "LeftHandIKPosition.y");
        var curve = AnimationUtility.GetEditorCurve(animClip, binding);
        
        if (curve == null || curve.keys.Length == 0)
        {
            Debug.Log("左腕基準カーブが見つからないため、デフォルトイージングを使用");
            return (0f, 0f);
        }
        
        var tangents = GetLeftArmTangentAtTime(curve, time);
        Debug.Log($"左腕基準カーブから検出したイージング: inTangent={tangents.inTangent:F3}, outTangent={tangents.outTangent:F3}");
        
        return tangents;
    }
    
    private (float inTangent, float outTangent) GetLeftArmTangentAtTime(AnimationCurve curve, float time)
    {
        var keys = curve.keys;
        
        // 完全一致するキーフレームがあるかチェック
        for (int i = 0; i < keys.Length; i++)
        {
            if (Mathf.Approximately(keys[i].time, time))
            {
                return (keys[i].inTangent, keys[i].outTangent);
            }
        }
        
        // 前後のキーフレームを見つけて補間
        int prevIndex = -1;
        int nextIndex = -1;
        
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].time < time)
            {
                prevIndex = i;
            }
            else if (keys[i].time > time && nextIndex == -1)
            {
                nextIndex = i;
                break;
            }
        }
        
        if (prevIndex == -1 && nextIndex == -1)
        {
            return (0f, 0f);
        }
        else if (prevIndex == -1)
        {
            return (keys[nextIndex].inTangent, keys[nextIndex].outTangent);
        }
        else if (nextIndex == -1)
        {
            return (keys[prevIndex].inTangent, keys[prevIndex].outTangent);
        }
        
        var prevKey = keys[prevIndex];
        var nextKey = keys[nextIndex];
        
        float t = (time - prevKey.time) / (nextKey.time - prevKey.time);
        float interpolatedInTangent = Mathf.Lerp(prevKey.outTangent, nextKey.inTangent, t);
        float interpolatedOutTangent = interpolatedInTangent;
        
        Debug.Log($"左腕補間計算: 時刻{time:F3}s (前{prevIndex}:{prevKey.time:F3}s - 次{nextIndex}:{nextKey.time:F3}s), t={t:F3}");
        
        return (interpolatedInTangent, interpolatedOutTangent);
    }
    
    private void UpdateLeftArmIKPositionCurveWithEasing(AnimationClip animClip, float time, Vector3 position, (float inTangent, float outTangent) easingInfo)
    {
        Debug.Log($"左腕カーブ更新（イージング付き): 時刻{time:F3}s に位置{position}を設定");
        
        string[] components = { ".x", ".y", ".z" };
        float[] values = { position.x, position.y, position.z };
        
        for (int i = 0; i < components.Length; i++)
        {
            var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), "LeftHandIKPosition" + components[i]);
            var curve = AnimationUtility.GetEditorCurve(animClip, binding);
            
            if (curve == null)
            {
                curve = new AnimationCurve();
            }
            
            var keyframe = CreateLeftArmKeyframeWithEasing(time, values[i], easingInfo);
            UpdateLeftArmCurveWithKeyframe(curve, keyframe);
            
            AnimationUtility.SetEditorCurve(animClip, binding, curve);
            Debug.Log($"✓ LeftHandIKPosition{components[i]} = {values[i]:F3} (時刻{time:F3}s, イージング: in={keyframe.inTangent:F3}, out={keyframe.outTangent:F3})");
        }
    }
    
    private void UpdateLeftArmRotationCurveWithEasing(AnimationClip animClip, string propertyBaseName, float time, Vector3 rotation, (float inTangent, float outTangent) easingInfo)
    {
        Debug.Log($"左腕回転カーブ更新（イージング付き): 時刻{time:F3}s に回転{rotation}を設定 ({propertyBaseName})");
        
        string[] components = { ".x", ".y", ".z" };
        float[] values = { rotation.x, rotation.y, rotation.z };
        
        for (int i = 0; i < components.Length; i++)
        {
            var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), propertyBaseName + components[i]);
            var curve = AnimationUtility.GetEditorCurve(animClip, binding);
            
            if (curve == null)
            {
                curve = new AnimationCurve();
            }
            
            var adjustedEasing = AdjustLeftArmEasingForRotation(easingInfo, values[i]);
            var keyframe = CreateLeftArmKeyframeWithEasing(time, values[i], adjustedEasing);
            UpdateLeftArmCurveWithKeyframe(curve, keyframe);
            
            AnimationUtility.SetEditorCurve(animClip, binding, curve);
            Debug.Log($"✓ {propertyBaseName}{components[i]} = {values[i]:F3} (時刻{time:F3}s, イージング: in={keyframe.inTangent:F3}, out={keyframe.outTangent:F3})");
        }
    }
    
    private Keyframe CreateLeftArmKeyframeWithEasing(float time, float value, (float inTangent, float outTangent) easingInfo)
    {
        return new Keyframe(time, value, easingInfo.inTangent, easingInfo.outTangent);
    }
    
    private void UpdateLeftArmCurveWithKeyframe(AnimationCurve curve, Keyframe keyframe)
    {
        bool foundExisting = false;
        for (int j = 0; j < curve.keys.Length; j++)
        {
            if (Mathf.Approximately(curve.keys[j].time, keyframe.time))
            {
                curve.RemoveKey(j);
                curve.AddKey(keyframe);
                foundExisting = true;
                break;
            }
        }
        
        if (!foundExisting)
        {
            curve.AddKey(keyframe);
        }
    }
    
    private (float inTangent, float outTangent) AdjustLeftArmEasingForRotation((float inTangent, float outTangent) positionEasing, float rotationValue)
    {
        float scaleFactor = 0.1f;
        float adjustedInTangent = positionEasing.inTangent * scaleFactor;
        float adjustedOutTangent = positionEasing.outTangent * scaleFactor;
        return (adjustedInTangent, adjustedOutTangent);
    }
}

[MenuEntry("Copy IK Target to Clip/Right Arm", 1)]
public class CopyRightArmIKTargetToClipAction : ClipAction
{
    public override ActionValidity Validate(IEnumerable<TimelineClip> clips)
    {
        return clips != null && clips.Any(c => c != null && c.asset is AnimationPlayableAsset)
            ? ActionValidity.Valid : ActionValidity.NotApplicable;
    }

    public override bool Execute(IEnumerable<TimelineClip> clips)
    {
        PlayableDirector director = GetPlayableDirector();
        if (director == null)
        {
            Debug.LogWarning("[CopyRightArmIKTarget] PlayableDirectorが見つかりません");
            return false;
        }

        foreach (var clip in clips.Where(c => c != null))
        {
            CopyRightArmIKTargetToClip(clip, director);
        }
        return true;
    }

    private PlayableDirector GetPlayableDirector()
    {
        PlayableDirector director = null;
        try
        {
            var timelineWindow = TimelineEditor.GetWindow();
            if (timelineWindow != null)
            {
                var stateField = typeof(TimelineEditor).GetField("state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (stateField != null)
                {
                    var state = stateField.GetValue(timelineWindow);
                    if (state != null)
                    {
                        var directorProperty = state.GetType().GetProperty("editSequence");
                        if (directorProperty != null)
                        {
                            var editSequence = directorProperty.GetValue(state);
                            if (editSequence != null)
                            {
                                var directorField = editSequence.GetType().GetProperty("director");
                                if (directorField != null)
                                {
                                    director = directorField.GetValue(editSequence) as PlayableDirector;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"TimelineEditorから取得に失敗: {e.Message}");
        }
        
        if (director == null)
        {
            var directors = Object.FindObjectsOfType<PlayableDirector>();
            if (directors.Length > 0)
            {
                director = directors[0];
                Debug.Log($"シーン内のPlayableDirectorを使用: {director.name}");
            }
        }
        return director;
    }

    private void CopyRightArmIKTargetToClip(TimelineClip clip, PlayableDirector director)
    {
        Debug.Log($"=== Right Arm IK Target → Clip変換: {clip.displayName} ===");
        
        var track = clip.GetParentTrack();
        if (track == null) { Debug.LogWarning("トラックが見つかりません"); return; }
        
        AnimationClip animClip = null;
        if (clip.asset is AnimationPlayableAsset animAsset) { animClip = animAsset.clip; }
        if (animClip == null) { Debug.LogWarning("AnimationClipが見つかりません"); return; }
        
        Debug.Log($"AnimationClip: {animClip.name}");

        // Timeline絶対時刻からClip内相対時刻を計算
        float timelineTime = (float)director.time;
        float clipTime = timelineTime - (float)clip.start;
        Debug.Log($"Timeline時刻: {timelineTime:F3}s, Clip開始: {clip.start:F3}s, Clip内相対時間: {clipTime:F3}s");

        var humanController = GetHumanControllerFromTrack(track, director);
        if (humanController == null) { Debug.LogWarning("HumanControllerが見つかりません"); return; }
        
        Debug.Log($"✓ HumanControllerが見つかりました: {humanController.gameObject.name}");
        
        Vector3 targetLocalPosition = GetRightArmIKTargetLocalPosition(humanController);
        Debug.Log($"Right Arm IK Target LocalPosition: {targetLocalPosition}");
        
        var ikRotations = GetRightArmIKRotations(humanController);
        Debug.Log($"Right Arm IK Shoulder Rotation: {ikRotations.shoulderRotation}");
        Debug.Log($"Right Arm IK Elbow Rotation: {ikRotations.elbowRotation}");

        var easingInfo = AnalyzeRightArmExistingCurveEasing(animClip, clipTime);
        Debug.Log($"検出されたイージング: inTangent={easingInfo.inTangent:F3}, outTangent={easingInfo.outTangent:F3}");

        UpdateRightArmIKPositionCurveWithEasing(animClip, clipTime, targetLocalPosition, easingInfo);
        UpdateRightArmRotationCurveWithEasing(animClip, "RightShoulderRotation", clipTime, ikRotations.shoulderRotation, easingInfo);
        UpdateRightArmRotationCurveWithEasing(animClip, "RightElbowRotation", clipTime, ikRotations.elbowRotation, easingInfo);
        
        EditorUtility.SetDirty(animClip);
        AssetDatabase.SaveAssets();
        
        Debug.Log("✓ Right Arm IK Target & Rotations → Clip変換完了");
        Debug.Log("=====================================");
    }
    
    private HumanController GetHumanControllerFromTrack(TrackAsset track, PlayableDirector director)
    {
        var boundObject = director.GetGenericBinding(track);
        if (boundObject == null) return null;
        
        GameObject targetGameObject = null;
        if (boundObject is GameObject go) { targetGameObject = go; }
        else if (boundObject is Component comp) { targetGameObject = comp.gameObject; }
        if (targetGameObject == null) return null;
        
        var humanController = targetGameObject.GetComponent<HumanController>();
        if (humanController == null) { humanController = targetGameObject.GetComponentInParent<HumanController>(); }
        if (humanController == null) { humanController = targetGameObject.GetComponentInChildren<HumanController>(); }
        return humanController;
    }
    
    private Vector3 GetRightArmIKTargetLocalPosition(HumanController humanController)
    {
        var rightArmIKField = typeof(HumanController).GetField("_rightArmIK", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (rightArmIKField == null) { Debug.LogWarning("_rightArmIKフィールドが見つかりません"); return Vector3.zero; }
        
        var rightArmIK = rightArmIKField.GetValue(humanController) as TwoBoneIK;
        if (rightArmIK == null) { Debug.LogWarning("_rightArmIKが設定されていません"); return Vector3.zero; }
        if (rightArmIK.target == null) { Debug.LogWarning("Right Arm IK targetが設定されていません"); return Vector3.zero; }
        
        Debug.Log($"Right Arm IK Target: {rightArmIK.target.name}");
        return rightArmIK.target.localPosition;
    }

    private Vector3 GetInspectorEulerAngles(Transform transform)
    {
        Quaternion q = transform.localRotation;

        // モード切り替え: DirectLocalEulerの場合はlocalEulerAnglesをそのまま使用
        if (EulerAngleModeConfig.Mode == EulerAngleMode.DirectLocalEuler)
        {
            Vector3 localEuler = transform.localEulerAngles;
            Vector3 normalized = TwoBoneIK.NormalizeEulerAngles(localEuler);
            Debug.Log($"  [DirectLocalEuler] {transform.name}: raw={localEuler}, normalized={normalized}");
            return normalized;
        }

        // 単一軸回転の最適化チェック
        const float epsilon = 0.0001f;

        // X軸のみの回転をチェック
        if (Mathf.Abs(q.y) < epsilon && Mathf.Abs(q.z) < epsilon)
        {
            float angle = 2.0f * Mathf.Atan2(q.x, q.w) * Mathf.Rad2Deg;
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            Debug.Log($"  Quaternion: {q}, Single-axis X rotation: ({angle:F2}, 0, 0)");
            return new Vector3(angle, 0, 0);
        }

        // Y軸のみの回転をチェック
        if (Mathf.Abs(q.x) < epsilon && Mathf.Abs(q.z) < epsilon)
        {
            float angle = 2.0f * Mathf.Atan2(q.y, q.w) * Mathf.Rad2Deg;
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            Debug.Log($"  Quaternion: {q}, Single-axis Y rotation: (0, {angle:F2}, 0)");
            return new Vector3(0, angle, 0);
        }

        // Z軸のみの回転をチェック
        if (Mathf.Abs(q.x) < epsilon && Mathf.Abs(q.y) < epsilon)
        {
            float angle = 2.0f * Mathf.Atan2(q.z, q.w) * Mathf.Rad2Deg;
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            Debug.Log($"  Quaternion: {q}, Single-axis Z rotation: (0, 0, {angle:F2})");
            return new Vector3(0, 0, angle);
        }

        // 複雑な回転: 詳細調査
        Debug.Log($"  === Detailed rotation analysis for {transform.name} ===");
        Debug.Log($"    Quaternion (localRotation): {q}");
        Debug.Log($"    localEulerAngles (raw): {transform.localEulerAngles}");
        Debug.Log($"    Parent: {(transform.parent ? transform.parent.name : "none")}");

        // SerializedPropertyでm_LocalEulerAnglesHintを確認
        Vector3? eulerHintValue = null;
        try
        {
            SerializedObject so = new SerializedObject(transform);
            var eulerHint = so.FindProperty("m_LocalEulerAnglesHint");
            if (eulerHint != null && eulerHint.propertyType == SerializedPropertyType.Vector3)
            {
                eulerHintValue = eulerHint.vector3Value;
                Debug.Log($"    m_LocalEulerAnglesHint: {eulerHintValue.Value}");

                // m_LocalEulerAnglesHintがゼロで、実際のQuaternionが回転している場合は無視
                if (eulerHintValue.Value.sqrMagnitude < 0.01f && !Mathf.Approximately(Quaternion.Angle(q, Quaternion.identity), 0f))
                {
                    Debug.Log($"    ⚠ m_LocalEulerAnglesHint is zero but rotation exists, using Quaternion conversion");
                }
                else
                {
                    Debug.Log($"    ✓ Using m_LocalEulerAnglesHint (Inspector value)");
                    return eulerHintValue.Value;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"    m_LocalEulerAnglesHint access failed: {e.Message}");
        }

        // m_LocalEulerAnglesHintが使えない場合のフォールバック
        Debug.Log($"    Using Quaternion to Euler conversion");

        // 一時GameObjectを使ってUnity標準のEuler角変換を利用
        GameObject tempGO = new GameObject("TempEulerCalculation");
        Vector3 baseEuler;
        try
        {
            tempGO.hideFlags = HideFlags.HideAndDontSave;
            tempGO.transform.localRotation = q;
            Vector3 euler = tempGO.transform.localEulerAngles;
            baseEuler = TwoBoneIK.NormalizeEulerAngles(euler);
            Debug.Log($"    Unity Euler (raw): {euler}");
            Debug.Log($"    Unity Euler (normalized): {baseEuler}");
        }
        finally
        {
            Object.DestroyImmediate(tempGO);
        }

        // 等価なEuler角表現を生成して、元のQuaternionに最も近いものを選択
        Vector3[] equivalents = new Vector3[8];
        equivalents[0] = baseEuler;
        equivalents[1] = new Vector3(180 - baseEuler.x, baseEuler.y - 180, baseEuler.z - 180);
        equivalents[2] = new Vector3(180 - baseEuler.x, 180 - baseEuler.y, 180 + baseEuler.z);
        equivalents[3] = new Vector3(-baseEuler.x, 180 + baseEuler.y, 180 - baseEuler.z);
        equivalents[4] = new Vector3(baseEuler.x + 180, baseEuler.y + 180, baseEuler.z);
        equivalents[5] = new Vector3(baseEuler.x, baseEuler.y + 180, baseEuler.z + 180);
        equivalents[6] = new Vector3(baseEuler.x + 180, baseEuler.y, baseEuler.z + 180);
        equivalents[7] = new Vector3(baseEuler.x - 180, baseEuler.y - 180, baseEuler.z);

        float minAngle = float.MaxValue;
        Vector3 bestEuler = baseEuler;

        for (int i = 0; i < equivalents.Length; i++)
        {
            // -180～180に正規化
            Vector3 normalized = TwoBoneIK.NormalizeEulerAngles(equivalents[i]);
            Quaternion testQ = Quaternion.Euler(normalized);
            float angle = Quaternion.Angle(q, testQ);

            if (angle < minAngle)
            {
                minAngle = angle;
                bestEuler = normalized;
            }
        }

        Debug.Log($"    ✓ Best equivalent: {bestEuler} (angle diff: {minAngle:F3}°)");
        return bestEuler;
    }


    private (Vector3 shoulderRotation, Vector3 elbowRotation) GetRightArmIKRotations(HumanController humanController)
    {
        var rightArmIKField = typeof(HumanController).GetField("_rightArmIK", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (rightArmIKField == null) { Debug.LogWarning("_rightArmIKフィールドが見つかりません"); return (Vector3.zero, Vector3.zero); }
        
        var rightArmIK = rightArmIKField.GetValue(humanController) as TwoBoneIK;
        if (rightArmIK == null) { Debug.LogWarning("_rightArmIKが設定されていません"); return (Vector3.zero, Vector3.zero); }
        
        Vector3 shoulderRotation = Vector3.zero, elbowRotation = Vector3.zero;

        if (rightArmIK.root != null)
        {
            shoulderRotation = GetInspectorEulerAngles(rightArmIK.root);
            Debug.Log($"Right Arm IK Root (Shoulder): {rightArmIK.root.name}");
            Debug.Log($"  Rotation: {shoulderRotation}");
        }
        else { Debug.LogWarning("Right Arm IK rootが設定されていません"); }

        if (rightArmIK.mid != null)
        {
            elbowRotation = GetInspectorEulerAngles(rightArmIK.mid);
            Debug.Log($"Right Arm IK Mid (Elbow): {rightArmIK.mid.name}");
            Debug.Log($"  Rotation: {elbowRotation}");
        }
        else { Debug.LogWarning("Right Arm IK midが設定されていません"); }

        return (shoulderRotation, elbowRotation);
    }
    
    private (float inTangent, float outTangent) AnalyzeRightArmExistingCurveEasing(AnimationClip animClip, float time)
    {
        var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), "RightHandIKPosition.y");
        var curve = AnimationUtility.GetEditorCurve(animClip, binding);
        
        if (curve == null || curve.keys.Length == 0)
        {
            Debug.Log("右腕基準カーブが見つからないため、デフォルトイージングを使用");
            return (0f, 0f);
        }
        
        var tangents = GetRightArmTangentAtTime(curve, time);
        Debug.Log($"右腕基準カーブから検出したイージング: inTangent={tangents.inTangent:F3}, outTangent={tangents.outTangent:F3}");
        return tangents;
    }
    
    private (float inTangent, float outTangent) GetRightArmTangentAtTime(AnimationCurve curve, float time)
    {
        var keys = curve.keys;
        for (int i = 0; i < keys.Length; i++)
        {
            if (Mathf.Approximately(keys[i].time, time)) { return (keys[i].inTangent, keys[i].outTangent); }
        }
        
        int prevIndex = -1, nextIndex = -1;
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].time < time) { prevIndex = i; }
            else if (keys[i].time > time && nextIndex == -1) { nextIndex = i; break; }
        }
        
        if (prevIndex == -1 && nextIndex == -1) { return (0f, 0f); }
        else if (prevIndex == -1) { return (keys[nextIndex].inTangent, keys[nextIndex].outTangent); }
        else if (nextIndex == -1) { return (keys[prevIndex].inTangent, keys[prevIndex].outTangent); }
        
        var prevKey = keys[prevIndex];
        var nextKey = keys[nextIndex];
        float t = (time - prevKey.time) / (nextKey.time - prevKey.time);
        float interpolatedInTangent = Mathf.Lerp(prevKey.outTangent, nextKey.inTangent, t);
        
        Debug.Log($"右腕補間計算: 時刻{time:F3}s (前{prevIndex}:{prevKey.time:F3}s - 次{nextIndex}:{nextKey.time:F3}s), t={t:F3}");
        return (interpolatedInTangent, interpolatedInTangent);
    }
    
    private void UpdateRightArmIKPositionCurveWithEasing(AnimationClip animClip, float time, Vector3 position, (float inTangent, float outTangent) easingInfo)
    {
        Debug.Log($"右腕カーブ更新（イージング付き): 時刻{time:F3}s に位置{position}を設定");
        
        string[] components = { ".x", ".y", ".z" };
        float[] values = { position.x, position.y, position.z };
        
        for (int i = 0; i < components.Length; i++)
        {
            var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), "RightHandIKPosition" + components[i]);
            var curve = AnimationUtility.GetEditorCurve(animClip, binding);
            if (curve == null) { curve = new AnimationCurve(); }
            
            var keyframe = new Keyframe(time, values[i], easingInfo.inTangent, easingInfo.outTangent);
            UpdateRightArmCurveWithKeyframe(curve, keyframe);
            
            AnimationUtility.SetEditorCurve(animClip, binding, curve);
            Debug.Log($"✓ RightHandIKPosition{components[i]} = {values[i]:F3} (時刻{time:F3}s, イージング: in={keyframe.inTangent:F3}, out={keyframe.outTangent:F3})");
        }
    }
    
    private void UpdateRightArmRotationCurveWithEasing(AnimationClip animClip, string propertyBaseName, float time, Vector3 rotation, (float inTangent, float outTangent) easingInfo)
    {
        Debug.Log($"右腕回転カーブ更新（イージング付き): 時刻{time:F3}s に回転{rotation}を設定 ({propertyBaseName})");
        
        string[] components = { ".x", ".y", ".z" };
        float[] values = { rotation.x, rotation.y, rotation.z };
        
        for (int i = 0; i < components.Length; i++)
        {
            var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), propertyBaseName + components[i]);
            var curve = AnimationUtility.GetEditorCurve(animClip, binding);
            if (curve == null) { curve = new AnimationCurve(); }
            
            float scaleFactor = 0.1f;
            var adjustedEasing = (easingInfo.inTangent * scaleFactor, easingInfo.outTangent * scaleFactor);
            var keyframe = new Keyframe(time, values[i], adjustedEasing.Item1, adjustedEasing.Item2);
            UpdateRightArmCurveWithKeyframe(curve, keyframe);
            
            AnimationUtility.SetEditorCurve(animClip, binding, curve);
            Debug.Log($"✓ {propertyBaseName}{components[i]} = {values[i]:F3} (時刻{time:F3}s, イージング: in={keyframe.inTangent:F3}, out={keyframe.outTangent:F3})");
        }
    }
    
    private void UpdateRightArmCurveWithKeyframe(AnimationCurve curve, Keyframe keyframe)
    {
        bool foundExisting = false;
        for (int j = 0; j < curve.keys.Length; j++)
        {
            if (Mathf.Approximately(curve.keys[j].time, keyframe.time))
            {
                curve.RemoveKey(j);
                curve.AddKey(keyframe);
                foundExisting = true;
                break;
            }
        }
        
        if (!foundExisting) { curve.AddKey(keyframe); }
    }
}

[MenuEntry("Copy IK Target to Clip/Left Foot", 2)]
public class CopyLeftFootIKTargetToClipAction : ClipAction
{
    public override ActionValidity Validate(IEnumerable<TimelineClip> clips)
    {
        return clips != null && clips.Any(c => c != null && c.asset is AnimationPlayableAsset)
            ? ActionValidity.Valid : ActionValidity.NotApplicable;
    }

    public override bool Execute(IEnumerable<TimelineClip> clips)
    {
        // Timeline Directorを取得
        PlayableDirector director = GetPlayableDirector();
        
        if (director == null)
        {
            Debug.LogWarning("[CopyLeftFootIKTarget] PlayableDirectorが見つかりません");
            return false;
        }

        foreach (var clip in clips.Where(c => c != null))
        {
            CopyLeftFootIKTargetToClip(clip, director);
        }
        return true;
    }

    private PlayableDirector GetPlayableDirector()
    {
        PlayableDirector director = null;
        
        // TimelineEditorから取得を試行
        try
        {
            var timelineWindow = TimelineEditor.GetWindow();
            if (timelineWindow != null)
            {
                var stateField = typeof(TimelineEditor).GetField("state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (stateField != null)
                {
                    var state = stateField.GetValue(timelineWindow);
                    if (state != null)
                    {
                        var directorProperty = state.GetType().GetProperty("editSequence");
                        if (directorProperty != null)
                        {
                            var editSequence = directorProperty.GetValue(state);
                            if (editSequence != null)
                            {
                                var directorField = editSequence.GetType().GetProperty("director");
                                if (directorField != null)
                                {
                                    director = directorField.GetValue(editSequence) as PlayableDirector;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"TimelineEditorから取得に失敗: {e.Message}");
        }
        
        // シーン内のPlayableDirectorを検索
        if (director == null)
        {
            var directors = Object.FindObjectsOfType<PlayableDirector>();
            if (directors.Length > 0)
            {
                director = directors[0];
                Debug.Log($"シーン内のPlayableDirectorを使用: {director.name}");
            }
        }
        
        return director;
    }

    private void CopyLeftFootIKTargetToClip(TimelineClip clip, PlayableDirector director)
    {
        Debug.Log($"=== Left Foot IK Target → Clip変換: {clip.displayName} ===");
        
        // トラック情報
        var track = clip.GetParentTrack();
        if (track == null)
        {
            Debug.LogWarning("トラックが見つかりません");
            return;
        }
        
        // AnimationClipを取得
        AnimationClip animClip = null;
        if (clip.asset is AnimationPlayableAsset animAsset)
        {
            animClip = animAsset.clip;
        }
        
        if (animClip == null)
        {
            Debug.LogWarning("AnimationClipが見つかりません");
            return;
        }
        
        Debug.Log($"AnimationClip: {animClip.name}");

        // Timeline絶対時刻からClip内相対時刻を計算
        float timelineTime = (float)director.time;
        float clipTime = timelineTime - (float)clip.start;
        Debug.Log($"Timeline時刻: {timelineTime:F3}s, Clip開始: {clip.start:F3}s, Clip内相対時間: {clipTime:F3}s");

        // バインドされたオブジェクトからHumanControllerを取得
        var humanController = GetHumanControllerFromTrack(track, director);
        if (humanController == null)
        {
            Debug.LogWarning("HumanControllerが見つかりません");
            return;
        }

        Debug.Log($"✓ HumanControllerが見つかりました: {humanController.gameObject.name}");

        // IK targetのlocalPositionを取得
        Vector3 targetLocalPosition = GetLeftFootIKTargetLocalPosition(humanController);
        Debug.Log($"Left Foot IK Target LocalPosition: {targetLocalPosition}");

        // IK root/midのlocalRotationを取得
        var ikRotations = GetLeftFootIKRotations(humanController);
        Debug.Log($"Left Foot IK Hip Rotation: {ikRotations.hipRotation}");
        Debug.Log($"Left Foot IK Knee Rotation: {ikRotations.kneeRotation}");

        // 既存カーブのイージング情報を分析（LeftFootIKPosition.yを基準）
        var easingInfo = AnalyzeLeftFootExistingCurveEasing(animClip, clipTime);
        Debug.Log($"検出されたイージング: inTangent={easingInfo.inTangent:F3}, outTangent={easingInfo.outTangent:F3}");

        // AnimationClipのカーブを更新（イージング情報付き）
        UpdateLeftFootIKPositionCurveWithEasing(animClip, clipTime, targetLocalPosition, easingInfo);
        UpdateLeftFootRotationCurveWithEasing(animClip, "LeftHipRotation", clipTime, ikRotations.hipRotation, easingInfo);
        UpdateLeftFootRotationCurveWithEasing(animClip, "LeftKneeRotation", clipTime, ikRotations.kneeRotation, easingInfo);
        
        // アセットを保存
        EditorUtility.SetDirty(animClip);
        AssetDatabase.SaveAssets();
        
        Debug.Log("✓ Left Foot IK Target & Rotations → Clip変換完了");
        Debug.Log("=====================================");
    }
    
    private float GetCurrentTimelineTime(PlayableDirector director)
    {
        // PlayableDirectorの現在時刻を取得
        return (float)director.time;
    }
    
    private HumanController GetHumanControllerFromTrack(TrackAsset track, PlayableDirector director)
    {
        // バインドされたオブジェクトを取得
        var boundObject = director.GetGenericBinding(track);
        if (boundObject == null) return null;
        
        // GameObjectを取得
        GameObject targetGameObject = null;
        if (boundObject is GameObject go)
        {
            targetGameObject = go;
        }
        else if (boundObject is Component comp)
        {
            targetGameObject = comp.gameObject;
        }
        
        if (targetGameObject == null) return null;
        
        // HumanControllerコンポーネントを検索
        var humanController = targetGameObject.GetComponent<HumanController>();
        if (humanController == null)
        {
            humanController = targetGameObject.GetComponentInParent<HumanController>();
        }
        if (humanController == null)
        {
            humanController = targetGameObject.GetComponentInChildren<HumanController>();
        }
        
        return humanController;
    }
    
    private Vector3 GetLeftFootIKTargetLocalPosition(HumanController humanController)
    {
        // LeftLegIKの取得（リフレクションを使用）
        var leftLegIKField = typeof(HumanController).GetField("_leftLegIK", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (leftLegIKField == null)
        {
            Debug.LogWarning("_leftLegIKフィールドが見つかりません");
            return Vector3.zero;
        }
        
        var leftLegIK = leftLegIKField.GetValue(humanController) as TwoBoneIK;
        if (leftLegIK == null)
        {
            Debug.LogWarning("_leftLegIKが設定されていません");
            return Vector3.zero;
        }
        
        if (leftLegIK.target == null)
        {
            Debug.LogWarning("Left Foot IK targetが設定されていません");
            return Vector3.zero;
        }
        
        Debug.Log($"Left Foot IK Target: {leftLegIK.target.name}");
        return leftLegIK.target.localPosition;
    }

    private Vector3 GetInspectorEulerAngles(Transform transform)
    {
        Quaternion q = transform.localRotation;

        // モード切り替え: DirectLocalEulerの場合はlocalEulerAnglesをそのまま使用
        if (EulerAngleModeConfig.Mode == EulerAngleMode.DirectLocalEuler)
        {
            Vector3 localEuler = transform.localEulerAngles;
            Vector3 normalized = TwoBoneIK.NormalizeEulerAngles(localEuler);
            Debug.Log($"  [DirectLocalEuler] {transform.name}: raw={localEuler}, normalized={normalized}");
            return normalized;
        }

        // 単一軸回転の最適化チェック
        const float epsilon = 0.0001f;

        // X軸のみの回転をチェック
        if (Mathf.Abs(q.y) < epsilon && Mathf.Abs(q.z) < epsilon)
        {
            float angle = 2.0f * Mathf.Atan2(q.x, q.w) * Mathf.Rad2Deg;
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            Debug.Log($"  Quaternion: {q}, Single-axis X rotation: ({angle:F2}, 0, 0)");
            return new Vector3(angle, 0, 0);
        }

        // Y軸のみの回転をチェック
        if (Mathf.Abs(q.x) < epsilon && Mathf.Abs(q.z) < epsilon)
        {
            float angle = 2.0f * Mathf.Atan2(q.y, q.w) * Mathf.Rad2Deg;
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            Debug.Log($"  Quaternion: {q}, Single-axis Y rotation: (0, {angle:F2}, 0)");
            return new Vector3(0, angle, 0);
        }

        // Z軸のみの回転をチェック
        if (Mathf.Abs(q.x) < epsilon && Mathf.Abs(q.y) < epsilon)
        {
            float angle = 2.0f * Mathf.Atan2(q.z, q.w) * Mathf.Rad2Deg;
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            Debug.Log($"  Quaternion: {q}, Single-axis Z rotation: (0, 0, {angle:F2})");
            return new Vector3(0, 0, angle);
        }

        // 複雑な回転: 詳細調査
        Debug.Log($"  === Detailed rotation analysis for {transform.name} ===");
        Debug.Log($"    Quaternion (localRotation): {q}");
        Debug.Log($"    localEulerAngles (raw): {transform.localEulerAngles}");
        Debug.Log($"    Parent: {(transform.parent ? transform.parent.name : "none")}");

        // SerializedPropertyでm_LocalEulerAnglesHintを確認
        Vector3? eulerHintValue = null;
        try
        {
            SerializedObject so = new SerializedObject(transform);
            var eulerHint = so.FindProperty("m_LocalEulerAnglesHint");
            if (eulerHint != null && eulerHint.propertyType == SerializedPropertyType.Vector3)
            {
                eulerHintValue = eulerHint.vector3Value;
                Debug.Log($"    m_LocalEulerAnglesHint: {eulerHintValue.Value}");

                // m_LocalEulerAnglesHintがゼロで、実際のQuaternionが回転している場合は無視
                if (eulerHintValue.Value.sqrMagnitude < 0.01f && !Mathf.Approximately(Quaternion.Angle(q, Quaternion.identity), 0f))
                {
                    Debug.Log($"    ⚠ m_LocalEulerAnglesHint is zero but rotation exists, using Quaternion conversion");
                }
                else
                {
                    Debug.Log($"    ✓ Using m_LocalEulerAnglesHint (Inspector value)");
                    return eulerHintValue.Value;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"    m_LocalEulerAnglesHint access failed: {e.Message}");
        }

        // m_LocalEulerAnglesHintが使えない場合のフォールバック
        Debug.Log($"    Using Quaternion to Euler conversion");

        // 一時GameObjectを使ってUnity標準のEuler角変換を利用
        GameObject tempGO = new GameObject("TempEulerCalculation");
        Vector3 baseEuler;
        try
        {
            tempGO.hideFlags = HideFlags.HideAndDontSave;
            tempGO.transform.localRotation = q;
            Vector3 euler = tempGO.transform.localEulerAngles;
            baseEuler = TwoBoneIK.NormalizeEulerAngles(euler);
            Debug.Log($"    Unity Euler (raw): {euler}");
            Debug.Log($"    Unity Euler (normalized): {baseEuler}");
        }
        finally
        {
            Object.DestroyImmediate(tempGO);
        }

        // 等価なEuler角表現を生成して、元のQuaternionに最も近いものを選択
        Vector3[] equivalents = new Vector3[8];
        equivalents[0] = baseEuler;
        equivalents[1] = new Vector3(180 - baseEuler.x, baseEuler.y - 180, baseEuler.z - 180);
        equivalents[2] = new Vector3(180 - baseEuler.x, 180 - baseEuler.y, 180 + baseEuler.z);
        equivalents[3] = new Vector3(-baseEuler.x, 180 + baseEuler.y, 180 - baseEuler.z);
        equivalents[4] = new Vector3(baseEuler.x + 180, baseEuler.y + 180, baseEuler.z);
        equivalents[5] = new Vector3(baseEuler.x, baseEuler.y + 180, baseEuler.z + 180);
        equivalents[6] = new Vector3(baseEuler.x + 180, baseEuler.y, baseEuler.z + 180);
        equivalents[7] = new Vector3(baseEuler.x - 180, baseEuler.y - 180, baseEuler.z);

        float minAngle = float.MaxValue;
        Vector3 bestEuler = baseEuler;

        for (int i = 0; i < equivalents.Length; i++)
        {
            // -180～180に正規化
            Vector3 normalized = TwoBoneIK.NormalizeEulerAngles(equivalents[i]);
            Quaternion testQ = Quaternion.Euler(normalized);
            float angle = Quaternion.Angle(q, testQ);

            if (angle < minAngle)
            {
                minAngle = angle;
                bestEuler = normalized;
            }
        }

        Debug.Log($"    ✓ Best equivalent: {bestEuler} (angle diff: {minAngle:F3}°)");
        return bestEuler;
    }


    private (Vector3 hipRotation, Vector3 kneeRotation) GetLeftFootIKRotations(HumanController humanController)
    {
        // LeftLegIKの取得（リフレクションを使用）
        var leftLegIKField = typeof(HumanController).GetField("_leftLegIK", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (leftLegIKField == null)
        {
            Debug.LogWarning("_leftLegIKフィールドが見つかりません");
            return (Vector3.zero, Vector3.zero);
        }
        
        var leftLegIK = leftLegIKField.GetValue(humanController) as TwoBoneIK;
        if (leftLegIK == null)
        {
            Debug.LogWarning("_leftLegIKが設定されていません");
            return (Vector3.zero, Vector3.zero);
        }
        
        Vector3 hipRotation = Vector3.zero;
        Vector3 kneeRotation = Vector3.zero;

        if (leftLegIK.root != null)
        {
            hipRotation = GetInspectorEulerAngles(leftLegIK.root);
            Debug.Log($"Left Foot IK Root (Hip): {leftLegIK.root.name}");
            Debug.Log($"  Rotation: {hipRotation}");
        }
        else
        {
            Debug.LogWarning("Left Foot IK rootが設定されていません");
        }

        if (leftLegIK.mid != null)
        {
            kneeRotation = GetInspectorEulerAngles(leftLegIK.mid);
            Debug.Log($"Left Foot IK Mid (Knee): {leftLegIK.mid.name}");
            Debug.Log($"  Rotation: {kneeRotation}");
        }
        else
        {
            Debug.LogWarning("Left Foot IK midが設定されていません");
        }

        return (hipRotation, kneeRotation);
    }
    
    private (float inTangent, float outTangent) AnalyzeLeftFootExistingCurveEasing(AnimationClip animClip, float time)
    {
        // LeftFootIKPosition.y カーブを基準として使用（足の上下動が最も重要）
        var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), "LeftFootIKPosition.y");
        var curve = AnimationUtility.GetEditorCurve(animClip, binding);
        
        if (curve == null || curve.keys.Length == 0)
        {
            Debug.Log("左足基準カーブが見つからないため、デフォルトイージングを使用");
            return (0f, 0f); // Auto tangent equivalent
        }
        
        // 指定時刻における tangent を算出
        var tangents = GetLeftFootTangentAtTime(curve, time);
        Debug.Log($"左足基準カーブから検出したイージング: inTangent={tangents.inTangent:F3}, outTangent={tangents.outTangent:F3}");
        
        return tangents;
    }
    
    private (float inTangent, float outTangent) GetLeftFootTangentAtTime(AnimationCurve curve, float time)
    {
        var keys = curve.keys;
        
        // 完全一致するキーフレームがあるかチェック
        for (int i = 0; i < keys.Length; i++)
        {
            if (Mathf.Approximately(keys[i].time, time))
            {
                return (keys[i].inTangent, keys[i].outTangent);
            }
        }
        
        // 前後のキーフレームを見つけて補間
        int prevIndex = -1;
        int nextIndex = -1;
        
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].time < time)
            {
                prevIndex = i;
            }
            else if (keys[i].time > time && nextIndex == -1)
            {
                nextIndex = i;
                break;
            }
        }
        
        // 前後のキーフレームが見つからない場合
        if (prevIndex == -1 && nextIndex == -1)
        {
            return (0f, 0f); // デフォルト
        }
        else if (prevIndex == -1)
        {
            // 最初のキーフレームより前
            return (keys[nextIndex].inTangent, keys[nextIndex].outTangent);
        }
        else if (nextIndex == -1)
        {
            // 最後のキーフレームより後
            return (keys[prevIndex].inTangent, keys[prevIndex].outTangent);
        }
        
        // 前後のキーフレーム間での補間
        var prevKey = keys[prevIndex];
        var nextKey = keys[nextIndex];
        
        float t = (time - prevKey.time) / (nextKey.time - prevKey.time);
        float interpolatedInTangent = Mathf.Lerp(prevKey.outTangent, nextKey.inTangent, t);
        float interpolatedOutTangent = interpolatedInTangent; // 連続性を保持
        
        Debug.Log($"左足補間計算: 時刻{time:F3}s (前{prevIndex}:{prevKey.time:F3}s - 次{nextIndex}:{nextKey.time:F3}s), t={t:F3}");
        
        return (interpolatedInTangent, interpolatedOutTangent);
    }
    
    private void UpdateLeftFootIKPositionCurveWithEasing(AnimationClip animClip, float time, Vector3 position, (float inTangent, float outTangent) easingInfo)
    {
        Debug.Log($"左足カーブ更新（イージング付き): 時刻{time:F3}s に位置{position}を設定");
        
        string[] components = { ".x", ".y", ".z" };
        float[] values = { position.x, position.y, position.z };
        
        for (int i = 0; i < components.Length; i++)
        {
            var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), "LeftFootIKPosition" + components[i]);
            var curve = AnimationUtility.GetEditorCurve(animClip, binding);
            
            // カーブが存在しない場合は新規作成
            if (curve == null)
            {
                curve = new AnimationCurve();
            }
            
            // イージング情報付きキーフレームを作成
            var keyframe = CreateLeftFootKeyframeWithEasing(time, values[i], easingInfo);
            
            // 既存のキーフレームがあるかチェックして更新
            UpdateLeftFootCurveWithKeyframe(curve, keyframe);
            
            // カーブをAnimationClipに設定
            AnimationUtility.SetEditorCurve(animClip, binding, curve);
            Debug.Log($"✓ LeftFootIKPosition{components[i]} = {values[i]:F3} (時刻{time:F3}s, イージング: in={keyframe.inTangent:F3}, out={keyframe.outTangent:F3})");
        }
    }
    
    private void UpdateLeftFootRotationCurveWithEasing(AnimationClip animClip, string propertyBaseName, float time, Vector3 rotation, (float inTangent, float outTangent) easingInfo)
    {
        Debug.Log($"左足回転カーブ更新（イージング付き): 時刻{time:F3}s に回転{rotation}を設定 ({propertyBaseName})");
        
        string[] components = { ".x", ".y", ".z" };
        float[] values = { rotation.x, rotation.y, rotation.z };
        
        for (int i = 0; i < components.Length; i++)
        {
            var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), propertyBaseName + components[i]);
            var curve = AnimationUtility.GetEditorCurve(animClip, binding);
            
            // カーブが存在しない場合は新規作成
            if (curve == null)
            {
                curve = new AnimationCurve();
            }
            
            // 位置と回転の値域差を考慮してイージングを調整
            var adjustedEasing = AdjustLeftFootEasingForRotation(easingInfo, values[i]);
            
            // イージング情報付きキーフレームを作成
            var keyframe = CreateLeftFootKeyframeWithEasing(time, values[i], adjustedEasing);
            
            // 既存のキーフレームがあるかチェックして更新
            UpdateLeftFootCurveWithKeyframe(curve, keyframe);
            
            // カーブをAnimationClipに設定
            AnimationUtility.SetEditorCurve(animClip, binding, curve);
            Debug.Log($"✓ {propertyBaseName}{components[i]} = {values[i]:F3} (時刻{time:F3}s, イージング: in={keyframe.inTangent:F3}, out={keyframe.outTangent:F3})");
        }
    }
    
    private Keyframe CreateLeftFootKeyframeWithEasing(float time, float value, (float inTangent, float outTangent) easingInfo)
    {
        var keyframe = new Keyframe(time, value, easingInfo.inTangent, easingInfo.outTangent);
        return keyframe;
    }
    
    private void UpdateLeftFootCurveWithKeyframe(AnimationCurve curve, Keyframe keyframe)
    {
        // 既存のキーフレームがあるかチェック
        bool foundExisting = false;
        for (int j = 0; j < curve.keys.Length; j++)
        {
            if (Mathf.Approximately(curve.keys[j].time, keyframe.time))
            {
                // 既存のキーフレームを更新
                curve.RemoveKey(j);
                curve.AddKey(keyframe);
                foundExisting = true;
                break;
            }
        }
        
        // 新しいキーフレームを追加
        if (!foundExisting)
        {
            curve.AddKey(keyframe);
        }
    }
    
    private (float inTangent, float outTangent) AdjustLeftFootEasingForRotation((float inTangent, float outTangent) positionEasing, float rotationValue)
    {
        // 位置と回転の値域差を考慮した調整
        // 位置は通常 -1～1 程度、回転は 0～360 度
        // 大きな値域差があるため、tangent値をスケール調整
        
        float scaleFactor = 0.1f; // 回転用のスケール調整係数
        
        float adjustedInTangent = positionEasing.inTangent * scaleFactor;
        float adjustedOutTangent = positionEasing.outTangent * scaleFactor;
        
        return (adjustedInTangent, adjustedOutTangent);
    }
}

[MenuEntry("Copy IK Target to Clip/Right Foot", 3)]
public class CopyRightFootIKTargetToClipAction : ClipAction
{
    public override ActionValidity Validate(IEnumerable<TimelineClip> clips)
    {
        return clips != null && clips.Any(c => c != null && c.asset is AnimationPlayableAsset)
            ? ActionValidity.Valid : ActionValidity.NotApplicable;
    }

    public override bool Execute(IEnumerable<TimelineClip> clips)
    {
        // Timeline Directorを取得
        PlayableDirector director = GetPlayableDirector();
        
        if (director == null)
        {
            Debug.LogWarning("[CopyRightFootIKTarget] PlayableDirectorが見つかりません");
            return false;
        }

        foreach (var clip in clips.Where(c => c != null))
        {
            CopyIKTargetToClip(clip, director);
        }
        return true;
    }

    private PlayableDirector GetPlayableDirector()
    {
        PlayableDirector director = null;
        
        // TimelineEditorから取得を試行
        try
        {
            var timelineWindow = TimelineEditor.GetWindow();
            if (timelineWindow != null)
            {
                var stateField = typeof(TimelineEditor).GetField("state", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (stateField != null)
                {
                    var state = stateField.GetValue(timelineWindow);
                    if (state != null)
                    {
                        var directorProperty = state.GetType().GetProperty("editSequence");
                        if (directorProperty != null)
                        {
                            var editSequence = directorProperty.GetValue(state);
                            if (editSequence != null)
                            {
                                var directorField = editSequence.GetType().GetProperty("director");
                                if (directorField != null)
                                {
                                    director = directorField.GetValue(editSequence) as PlayableDirector;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"TimelineEditorから取得に失敗: {e.Message}");
        }
        
        // シーン内のPlayableDirectorを検索
        if (director == null)
        {
            var directors = Object.FindObjectsOfType<PlayableDirector>();
            if (directors.Length > 0)
            {
                director = directors[0];
                Debug.Log($"シーン内のPlayableDirectorを使用: {director.name}");
            }
        }
        
        return director;
    }

    private void CopyIKTargetToClip(TimelineClip clip, PlayableDirector director)
    {
        Debug.Log($"=== Right Foot IK Target → Clip変換: {clip.displayName} ===");
        
        // トラック情報
        var track = clip.GetParentTrack();
        if (track == null)
        {
            Debug.LogWarning("トラックが見つかりません");
            return;
        }
        
        // AnimationClipを取得
        AnimationClip animClip = null;
        if (clip.asset is AnimationPlayableAsset animAsset)
        {
            animClip = animAsset.clip;
        }
        
        if (animClip == null)
        {
            Debug.LogWarning("AnimationClipが見つかりません");
            return;
        }
        
        Debug.Log($"AnimationClip: {animClip.name}");

        // Timeline絶対時刻からClip内相対時刻を計算
        float timelineTime = (float)director.time;
        float clipTime = timelineTime - (float)clip.start;
        Debug.Log($"Timeline時刻: {timelineTime:F3}s, Clip開始: {clip.start:F3}s, Clip内相対時間: {clipTime:F3}s");

        // バインドされたオブジェクトからHumanControllerを取得
        var humanController = GetHumanControllerFromTrack(track, director);
        if (humanController == null)
        {
            Debug.LogWarning("HumanControllerが見つかりません");
            return;
        }
        
        Debug.Log($"✓ HumanControllerが見つかりました: {humanController.gameObject.name}");
        
        // IK targetのlocalPositionを取得
        Vector3 targetLocalPosition = GetIKTargetLocalPosition(humanController);
        Debug.Log($"IK Target LocalPosition: {targetLocalPosition}");
        
        // IK root/midのlocalRotationを取得
        var ikRotations = GetIKRotations(humanController);
        Debug.Log($"IK Hip Rotation: {ikRotations.hipRotation}");
        Debug.Log($"IK Knee Rotation: {ikRotations.kneeRotation}");

        // 既存カーブのイージング情報を分析
        var easingInfo = AnalyzeExistingCurveEasing(animClip, clipTime);
        Debug.Log($"検出されたイージング: inTangent={easingInfo.inTangent:F3}, outTangent={easingInfo.outTangent:F3}");

        // AnimationClipのカーブを更新（イージング情報付き）
        UpdateRightFootIKPositionCurveWithEasing(animClip, clipTime, targetLocalPosition, easingInfo);
        UpdateRotationCurveWithEasing(animClip, "RightHipRotation", clipTime, ikRotations.hipRotation, easingInfo);
        UpdateRotationCurveWithEasing(animClip, "RightKneeRotation", clipTime, ikRotations.kneeRotation, easingInfo);
        
        // アセットを保存
        EditorUtility.SetDirty(animClip);
        AssetDatabase.SaveAssets();
        
        Debug.Log("✓ IK Target & Rotations → Clip変換完了");
        Debug.Log("=====================================");
    }
    
    private float GetCurrentTimelineTime(PlayableDirector director)
    {
        // PlayableDirectorの現在時刻を取得
        return (float)director.time;
    }
    
    private HumanController GetHumanControllerFromTrack(TrackAsset track, PlayableDirector director)
    {
        // バインドされたオブジェクトを取得
        var boundObject = director.GetGenericBinding(track);
        if (boundObject == null) return null;
        
        // GameObjectを取得
        GameObject targetGameObject = null;
        if (boundObject is GameObject go)
        {
            targetGameObject = go;
        }
        else if (boundObject is Component comp)
        {
            targetGameObject = comp.gameObject;
        }
        
        if (targetGameObject == null) return null;
        
        // HumanControllerコンポーネントを検索
        var humanController = targetGameObject.GetComponent<HumanController>();
        if (humanController == null)
        {
            humanController = targetGameObject.GetComponentInParent<HumanController>();
        }
        if (humanController == null)
        {
            humanController = targetGameObject.GetComponentInChildren<HumanController>();
        }
        
        return humanController;
    }
    
    private Vector3 GetIKTargetLocalPosition(HumanController humanController)
    {
        // RightLegIKの取得（リフレクションを使用）
        var rightLegIKField = typeof(HumanController).GetField("_rightLegIK", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (rightLegIKField == null)
        {
            Debug.LogWarning("_rightLegIKフィールドが見つかりません");
            return Vector3.zero;
        }
        
        var rightLegIK = rightLegIKField.GetValue(humanController) as TwoBoneIK;
        if (rightLegIK == null)
        {
            Debug.LogWarning("_rightLegIKが設定されていません");
            return Vector3.zero;
        }
        
        if (rightLegIK.target == null)
        {
            Debug.LogWarning("IK targetが設定されていません");
            return Vector3.zero;
        }
        
        Debug.Log($"IK Target: {rightLegIK.target.name}");
        return rightLegIK.target.localPosition;
    }

    private Vector3 GetInspectorEulerAngles(Transform transform)
    {
        Quaternion q = transform.localRotation;

        // モード切り替え: DirectLocalEulerの場合はlocalEulerAnglesをそのまま使用
        if (EulerAngleModeConfig.Mode == EulerAngleMode.DirectLocalEuler)
        {
            Vector3 localEuler = transform.localEulerAngles;
            Vector3 normalized = TwoBoneIK.NormalizeEulerAngles(localEuler);
            Debug.Log($"  [DirectLocalEuler] {transform.name}: raw={localEuler}, normalized={normalized}");
            return normalized;
        }

        // 単一軸回転の最適化チェック
        const float epsilon = 0.0001f;

        // X軸のみの回転をチェック
        if (Mathf.Abs(q.y) < epsilon && Mathf.Abs(q.z) < epsilon)
        {
            float angle = 2.0f * Mathf.Atan2(q.x, q.w) * Mathf.Rad2Deg;
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            Debug.Log($"  Quaternion: {q}, Single-axis X rotation: ({angle:F2}, 0, 0)");
            return new Vector3(angle, 0, 0);
        }

        // Y軸のみの回転をチェック
        if (Mathf.Abs(q.x) < epsilon && Mathf.Abs(q.z) < epsilon)
        {
            float angle = 2.0f * Mathf.Atan2(q.y, q.w) * Mathf.Rad2Deg;
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            Debug.Log($"  Quaternion: {q}, Single-axis Y rotation: (0, {angle:F2}, 0)");
            return new Vector3(0, angle, 0);
        }

        // Z軸のみの回転をチェック
        if (Mathf.Abs(q.x) < epsilon && Mathf.Abs(q.y) < epsilon)
        {
            float angle = 2.0f * Mathf.Atan2(q.z, q.w) * Mathf.Rad2Deg;
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            Debug.Log($"  Quaternion: {q}, Single-axis Z rotation: (0, 0, {angle:F2})");
            return new Vector3(0, 0, angle);
        }

        // 複雑な回転: 詳細調査
        Debug.Log($"  === Detailed rotation analysis for {transform.name} ===");
        Debug.Log($"    Quaternion (localRotation): {q}");
        Debug.Log($"    localEulerAngles (raw): {transform.localEulerAngles}");
        Debug.Log($"    Parent: {(transform.parent ? transform.parent.name : "none")}");

        // SerializedPropertyでm_LocalEulerAnglesHintを確認
        Vector3? eulerHintValue = null;
        try
        {
            SerializedObject so = new SerializedObject(transform);
            var eulerHint = so.FindProperty("m_LocalEulerAnglesHint");
            if (eulerHint != null && eulerHint.propertyType == SerializedPropertyType.Vector3)
            {
                eulerHintValue = eulerHint.vector3Value;
                Debug.Log($"    m_LocalEulerAnglesHint: {eulerHintValue.Value}");

                // m_LocalEulerAnglesHintがゼロで、実際のQuaternionが回転している場合は無視
                if (eulerHintValue.Value.sqrMagnitude < 0.01f && !Mathf.Approximately(Quaternion.Angle(q, Quaternion.identity), 0f))
                {
                    Debug.Log($"    ⚠ m_LocalEulerAnglesHint is zero but rotation exists, using Quaternion conversion");
                }
                else
                {
                    Debug.Log($"    ✓ Using m_LocalEulerAnglesHint (Inspector value)");
                    return eulerHintValue.Value;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"    m_LocalEulerAnglesHint access failed: {e.Message}");
        }

        // m_LocalEulerAnglesHintが使えない場合のフォールバック
        Debug.Log($"    Using Quaternion to Euler conversion");

        // 一時GameObjectを使ってUnity標準のEuler角変換を利用
        GameObject tempGO = new GameObject("TempEulerCalculation");
        Vector3 baseEuler;
        try
        {
            tempGO.hideFlags = HideFlags.HideAndDontSave;
            tempGO.transform.localRotation = q;
            Vector3 euler = tempGO.transform.localEulerAngles;
            baseEuler = TwoBoneIK.NormalizeEulerAngles(euler);
            Debug.Log($"    Unity Euler (raw): {euler}");
            Debug.Log($"    Unity Euler (normalized): {baseEuler}");
        }
        finally
        {
            Object.DestroyImmediate(tempGO);
        }

        // 等価なEuler角表現を生成して、元のQuaternionに最も近いものを選択
        Vector3[] equivalents = new Vector3[8];
        equivalents[0] = baseEuler;
        equivalents[1] = new Vector3(180 - baseEuler.x, baseEuler.y - 180, baseEuler.z - 180);
        equivalents[2] = new Vector3(180 - baseEuler.x, 180 - baseEuler.y, 180 + baseEuler.z);
        equivalents[3] = new Vector3(-baseEuler.x, 180 + baseEuler.y, 180 - baseEuler.z);
        equivalents[4] = new Vector3(baseEuler.x + 180, baseEuler.y + 180, baseEuler.z);
        equivalents[5] = new Vector3(baseEuler.x, baseEuler.y + 180, baseEuler.z + 180);
        equivalents[6] = new Vector3(baseEuler.x + 180, baseEuler.y, baseEuler.z + 180);
        equivalents[7] = new Vector3(baseEuler.x - 180, baseEuler.y - 180, baseEuler.z);

        float minAngle = float.MaxValue;
        Vector3 bestEuler = baseEuler;

        for (int i = 0; i < equivalents.Length; i++)
        {
            // -180～180に正規化
            Vector3 normalized = TwoBoneIK.NormalizeEulerAngles(equivalents[i]);
            Quaternion testQ = Quaternion.Euler(normalized);
            float angle = Quaternion.Angle(q, testQ);

            if (angle < minAngle)
            {
                minAngle = angle;
                bestEuler = normalized;
            }
        }

        Debug.Log($"    ✓ Best equivalent: {bestEuler} (angle diff: {minAngle:F3}°)");
        return bestEuler;
    }


    private (Vector3 hipRotation, Vector3 kneeRotation) GetIKRotations(HumanController humanController)
    {
        // RightLegIKの取得（リフレクションを使用）
        var rightLegIKField = typeof(HumanController).GetField("_rightLegIK", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (rightLegIKField == null)
        {
            Debug.LogWarning("_rightLegIKフィールドが見つかりません");
            return (Vector3.zero, Vector3.zero);
        }
        
        var rightLegIK = rightLegIKField.GetValue(humanController) as TwoBoneIK;
        if (rightLegIK == null)
        {
            Debug.LogWarning("_rightLegIKが設定されていません");
            return (Vector3.zero, Vector3.zero);
        }
        
        Vector3 hipRotation = Vector3.zero;
        Vector3 kneeRotation = Vector3.zero;

        if (rightLegIK.root != null)
        {
            hipRotation = GetInspectorEulerAngles(rightLegIK.root);
            Debug.Log($"Right Foot IK Root (Hip): {rightLegIK.root.name}");
            Debug.Log($"  Rotation: {hipRotation}");
        }
        else
        {
            Debug.LogWarning("IK rootが設定されていません");
        }

        if (rightLegIK.mid != null)
        {
            kneeRotation = GetInspectorEulerAngles(rightLegIK.mid);
            Debug.Log($"Right Foot IK Mid (Knee): {rightLegIK.mid.name}");
            Debug.Log($"  Rotation: {kneeRotation}");
        }
        else
        {
            Debug.LogWarning("IK midが設定されていません");
        }

        return (hipRotation, kneeRotation);
    }
    
    private (float inTangent, float outTangent) AnalyzeExistingCurveEasing(AnimationClip animClip, float time)
    {
        // RightFootIKPosition.y カーブを基準として使用（足の上下動が最も重要）
        var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), "RightFootIKPosition.y");
        var curve = AnimationUtility.GetEditorCurve(animClip, binding);
        
        if (curve == null || curve.keys.Length == 0)
        {
            Debug.Log("基準カーブが見つからないため、デフォルトイージングを使用");
            return (0f, 0f); // Auto tangent equivalent
        }
        
        // 指定時刻における tangent を算出
        var tangents = GetTangentAtTime(curve, time);
        Debug.Log($"基準カーブから検出したイージング: inTangent={tangents.inTangent:F3}, outTangent={tangents.outTangent:F3}");
        
        return tangents;
    }
    
    private (float inTangent, float outTangent) GetTangentAtTime(AnimationCurve curve, float time)
    {
        var keys = curve.keys;
        
        // 完全一致するキーフレームがあるかチェック
        for (int i = 0; i < keys.Length; i++)
        {
            if (Mathf.Approximately(keys[i].time, time))
            {
                return (keys[i].inTangent, keys[i].outTangent);
            }
        }
        
        // 前後のキーフレームを見つけて補間
        int prevIndex = -1;
        int nextIndex = -1;
        
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].time < time)
            {
                prevIndex = i;
            }
            else if (keys[i].time > time && nextIndex == -1)
            {
                nextIndex = i;
                break;
            }
        }
        
        // 前後のキーフレームが見つからない場合
        if (prevIndex == -1 && nextIndex == -1)
        {
            return (0f, 0f); // デフォルト
        }
        else if (prevIndex == -1)
        {
            // 最初のキーフレームより前
            return (keys[nextIndex].inTangent, keys[nextIndex].outTangent);
        }
        else if (nextIndex == -1)
        {
            // 最後のキーフレームより後
            return (keys[prevIndex].inTangent, keys[prevIndex].outTangent);
        }
        
        // 前後のキーフレーム間での補間
        var prevKey = keys[prevIndex];
        var nextKey = keys[nextIndex];
        
        float t = (time - prevKey.time) / (nextKey.time - prevKey.time);
        float interpolatedInTangent = Mathf.Lerp(prevKey.outTangent, nextKey.inTangent, t);
        float interpolatedOutTangent = interpolatedInTangent; // 連続性を保持
        
        Debug.Log($"補間計算: 時刻{time:F3}s (前{prevIndex}:{prevKey.time:F3}s - 次{nextIndex}:{nextKey.time:F3}s), t={t:F3}");
        
        return (interpolatedInTangent, interpolatedOutTangent);
    }
    
    private void UpdateRightFootIKPositionCurveWithEasing(AnimationClip animClip, float time, Vector3 position, (float inTangent, float outTangent) easingInfo)
    {
        Debug.Log($"カーブ更新（イージング付き): 時刻{time:F3}s に位置{position}を設定");
        
        string[] components = { ".x", ".y", ".z" };
        float[] values = { position.x, position.y, position.z };
        
        for (int i = 0; i < components.Length; i++)
        {
            var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), "RightFootIKPosition" + components[i]);
            var curve = AnimationUtility.GetEditorCurve(animClip, binding);
            
            // カーブが存在しない場合は新規作成
            if (curve == null)
            {
                curve = new AnimationCurve();
            }
            
            // イージング情報付きキーフレームを作成
            var keyframe = CreateKeyframeWithEasing(time, values[i], easingInfo);
            
            // 既存のキーフレームがあるかチェックして更新
            UpdateCurveWithKeyframe(curve, keyframe);
            
            // カーブをAnimationClipに設定
            AnimationUtility.SetEditorCurve(animClip, binding, curve);
            Debug.Log($"✓ RightFootIKPosition{components[i]} = {values[i]:F3} (時刻{time:F3}s, イージング: in={keyframe.inTangent:F3}, out={keyframe.outTangent:F3})");
        }
    }
    
    private void UpdateRotationCurveWithEasing(AnimationClip animClip, string propertyBaseName, float time, Vector3 rotation, (float inTangent, float outTangent) easingInfo)
    {
        Debug.Log($"回転カーブ更新（イージング付き): 時刻{time:F3}s に回転{rotation}を設定 ({propertyBaseName})");
        
        string[] components = { ".x", ".y", ".z" };
        float[] values = { rotation.x, rotation.y, rotation.z };
        
        for (int i = 0; i < components.Length; i++)
        {
            var binding = EditorCurveBinding.FloatCurve("", typeof(HumanController), propertyBaseName + components[i]);
            var curve = AnimationUtility.GetEditorCurve(animClip, binding);
            
            // カーブが存在しない場合は新規作成
            if (curve == null)
            {
                curve = new AnimationCurve();
            }
            
            // 位置と回転の値域差を考慮してイージングを調整
            var adjustedEasing = AdjustEasingForRotation(easingInfo, values[i]);
            
            // イージング情報付きキーフレームを作成
            var keyframe = CreateKeyframeWithEasing(time, values[i], adjustedEasing);
            
            // 既存のキーフレームがあるかチェックして更新
            UpdateCurveWithKeyframe(curve, keyframe);
            
            // カーブをAnimationClipに設定
            AnimationUtility.SetEditorCurve(animClip, binding, curve);
            Debug.Log($"✓ {propertyBaseName}{components[i]} = {values[i]:F3} (時刻{time:F3}s, イージング: in={keyframe.inTangent:F3}, out={keyframe.outTangent:F3})");
        }
    }
    
    private Keyframe CreateKeyframeWithEasing(float time, float value, (float inTangent, float outTangent) easingInfo)
    {
        var keyframe = new Keyframe(time, value, easingInfo.inTangent, easingInfo.outTangent);
        return keyframe;
    }
    
    private void UpdateCurveWithKeyframe(AnimationCurve curve, Keyframe keyframe)
    {
        // 既存のキーフレームがあるかチェック
        bool foundExisting = false;
        for (int j = 0; j < curve.keys.Length; j++)
        {
            if (Mathf.Approximately(curve.keys[j].time, keyframe.time))
            {
                // 既存のキーフレームを更新
                curve.RemoveKey(j);
                curve.AddKey(keyframe);
                foundExisting = true;
                break;
            }
        }
        
        // 新しいキーフレームを追加
        if (!foundExisting)
        {
            curve.AddKey(keyframe);
        }
    }
    
    private (float inTangent, float outTangent) AdjustEasingForRotation((float inTangent, float outTangent) positionEasing, float rotationValue)
    {
        // 位置と回転の値域差を考慮した調整
        // 位置は通常 -1～1 程度、回転は 0～360 度
        // 大きな値域差があるため、tangent値をスケール調整

        float scaleFactor = 0.1f; // 回転用のスケール調整係数

        float adjustedInTangent = positionEasing.inTangent * scaleFactor;
        float adjustedOutTangent = positionEasing.outTangent * scaleFactor;

        return (adjustedInTangent, adjustedOutTangent);
    }

    /// <summary>
    /// オイラー角を-180～180度の範囲に正規化
    /// UnityのInspectorと同じ表示にする
    /// </summary>
    private float NormalizeAngle(float angle)
    {
        // Mathf.DeltaAngleを使用して-180～180度に正規化
        return Mathf.DeltaAngle(0f, angle);
    }

    /// <summary>
    /// Vector3の各成分を-180～180度に正規化
    /// </summary>
    private Vector3 NormalizeEulerAngles(Vector3 euler)
    {
        return new Vector3(
            NormalizeAngle(euler.x),
            NormalizeAngle(euler.y),
            NormalizeAngle(euler.z)
        );
    }
}
#endif
