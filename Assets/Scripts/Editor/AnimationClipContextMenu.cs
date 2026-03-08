using UnityEngine;
using UnityEditor;

public static class AnimationClipContextMenu
{
    [MenuItem("Assets/Custom Animation Tools/Test Log", false, 2000)]
    static void TestLogAnimationClip()
    {
        AnimationClip selectedClip = Selection.activeObject as AnimationClip;
        
        if (selectedClip == null)
        {
            Debug.LogWarning("AnimationClipが選択されていません");
            return;
        }
        
        string assetPath = AssetDatabase.GetAssetPath(selectedClip);
        
        Debug.Log($"=== AnimationClip情報 ===");
        Debug.Log($"名前: {selectedClip.name}");
        Debug.Log($"パス: {assetPath}");
        Debug.Log($"長さ: {selectedClip.length:F3}秒");
        Debug.Log($"フレームレート: {selectedClip.frameRate}");
        Debug.Log($"ループ: {selectedClip.isLooping}");
        Debug.Log($"レガシー: {selectedClip.legacy}");
        
        // カーブバインディング数も表示
        var curveBindings = AnimationUtility.GetCurveBindings(selectedClip);
        Debug.Log($"カーブバインディング数: {curveBindings.Length}");
        
        Debug.Log("======================");
    }
    
    [MenuItem("Assets/Custom Animation Tools/Test Log", true, 2000)]
    static bool ValidateTestLogAnimationClip()
    {
        return Selection.activeObject is AnimationClip;
    }
}