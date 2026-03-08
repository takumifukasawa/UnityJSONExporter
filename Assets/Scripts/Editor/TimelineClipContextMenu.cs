using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

// Timeline機能の調査と実装
public static class TimelineClipMenuItems
{
    [MenuItem("Window/Custom Timeline Tools/Investigate Timeline API")]
    static void InvestigateTimelineAPI()
    {
        Debug.Log("=== Timeline API 調査結果 ===");
        
        // アセンブリの調査
        try
        {
            var timelineAssembly = Assembly.Load("Unity.Timeline");
            if (timelineAssembly != null)
            {
                Debug.Log("✓ Unity.Timeline アセンブリが見つかりました");
                
                // Timeline関連クラスの列挙
                var timelineTypes = timelineAssembly.GetTypes()
                    .Where(t => t.Name.Contains("Timeline") || t.Name.Contains("Clip"))
                    .OrderBy(t => t.Name)
                    .Take(20);
                
                Debug.Log("--- Timeline関連クラス (最初の20個) ---");
                foreach (var type in timelineTypes)
                {
                    Debug.Log($"  {type.FullName}");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Unity.Timeline アセンブリエラー: {e.Message}");
        }
        
        // Timeline Editorアセンブリの調査
        try
        {
            var timelineEditorAssembly = Assembly.Load("Unity.Timeline.Editor");
            if (timelineEditorAssembly != null)
            {
                Debug.Log("✓ Unity.Timeline.Editor アセンブリが見つかりました");
                
                var editorTypes = timelineEditorAssembly.GetTypes()
                    .Where(t => t.Name.Contains("Timeline") || t.Name.Contains("Menu") || t.Name.Contains("Context"))
                    .OrderBy(t => t.Name)
                    .Take(15);
                
                Debug.Log("--- Timeline Editor関連クラス (最初の15個) ---");
                foreach (var type in editorTypes)
                {
                    Debug.Log($"  {type.FullName}");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Unity.Timeline.Editor アセンブリエラー: {e.Message}");
        }
        
        // Selectionからの情報取得テスト
        Debug.Log("--- 現在の選択オブジェクト情報 ---");
        var selectedObjects = Selection.objects;
        Debug.Log($"選択オブジェクト数: {selectedObjects.Length}");
        
        foreach (var obj in selectedObjects)
        {
            if (obj != null)
            {
                Debug.Log($"  オブジェクト: {obj.name} (型: {obj.GetType().Name})");
            }
        }
        
        Debug.Log("===========================");
    }
    
    [MenuItem("Window/Custom Timeline Tools/Simple Timeline Test")]
    static void SimpleTimelineTest()
    {
        Debug.Log("=== 簡易Timeline連携テスト ===");
        
        // 基本的な情報収集
        var selectedObjects = Selection.objects;
        
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("何も選択されていません");
            return;
        }
        
        foreach (var obj in selectedObjects)
        {
            if (obj != null)
            {
                Debug.Log($"選択オブジェクト: {obj.name}");
                Debug.Log($"型: {obj.GetType().FullName}");
                
                // AnimationClipかどうかチェック
                if (obj is AnimationClip animClip)
                {
                    Debug.Log("--- AnimationClip情報 ---");
                    Debug.Log($"長さ: {animClip.length:F3}秒");
                    Debug.Log($"ループ: {animClip.isLooping}");
                    Debug.Log($"フレームレート: {animClip.frameRate}");
                    
                    string assetPath = AssetDatabase.GetAssetPath(animClip);
                    Debug.Log($"アセットパス: {assetPath}");
                    
                    var curveBindings = AnimationUtility.GetCurveBindings(animClip);
                    Debug.Log($"カーブバインディング数: {curveBindings.Length}");
                }
                
                Debug.Log("---");
            }
        }
        
        Debug.Log("=========================");
    }
}
