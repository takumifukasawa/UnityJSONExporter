
// Editor/SourceClipEditor_Inspector.cs
#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

[CustomEditor(typeof(SourceClip))]
public class SourceClipInspector : Editor
{
    SerializedProperty _links;

    void OnEnable()
    {
        _links = serializedObject.FindProperty("links");
    }

    public override void OnInspectorGUI()
    {
        // 現在編集中の Timeline を取得（ここが同一 TL 制限のカギ）
        var timeline = TimelineEditor.inspectedAsset as TimelineAsset;

        if (timeline == null)
        {
            EditorGUILayout.HelpBox("Timeline ウィンドウでこのクリップが載っている Timeline を開いた状態で編集してください。", MessageType.Info);
            base.OnInspectorGUI(); // フォールバック（通常の表示）
            return;
        }

        serializedObject.Update();

        // 出力トラック一覧（表示順）
        var tracks = timeline.GetOutputTracks().Where(t => t != null).ToList();
        if (GUILayout.Button("+ Add Link")) _links.InsertArrayElementAtIndex(_links.arraySize);

        for (int i = 0; i < _links.arraySize; i++)
        {
            var link = _links.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Link #{i+1}", EditorStyles.boldLabel);

            // === Track 選択（同一 Timeline 限定） ===
            var spTrack = link.FindPropertyRelative("targetTrack");
            var currentTrack = spTrack.objectReferenceValue as TrackAsset;
            int trackIndex = Mathf.Max(0, tracks.IndexOf(currentTrack));
            var trackNames = tracks.Select(t => t.name).ToArray();
            int newTrackIndex = EditorGUILayout.Popup("Target Track", trackIndex, trackNames);

            if (newTrackIndex != trackIndex || spTrack.objectReferenceValue == null)
            {
                spTrack.objectReferenceValue = tracks[newTrackIndex];
                // トラックが変わったらクリップもクリア
                link.FindPropertyRelative("targetClipAsset").objectReferenceValue = null;
            }

            // === クリップ選択（選んだトラック内だけ）===
            var spClipAsset = link.FindPropertyRelative("targetClipAsset");
            var selectedTrack = spTrack.objectReferenceValue as TrackAsset;

            var clips = new List<TimelineClip>();
            if (selectedTrack != null) clips = selectedTrack.GetClips().ToList();

            int clipIndex = Mathf.Max(0, clips.FindIndex(c => c != null && c.asset == spClipAsset.objectReferenceValue));
            var clipNames = clips.Select(c => c.displayName).ToArray();

            EditorGUI.BeginDisabledGroup(clips.Count == 0);
            int newClipIndex = clips.Count == 0 ? 0 : EditorGUILayout.Popup("Target Clip", Mathf.Clamp(clipIndex,0,clips.Count-1), clipNames);
            if (clips.Count > 0 && (newClipIndex != clipIndex || spClipAsset.objectReferenceValue == null))
            {
                spClipAsset.objectReferenceValue = clips[newClipIndex].asset; // 同一 TL の sub-asset を参照
            }
            EditorGUI.EndDisabledGroup();

            // === オプション ===
            EditorGUILayout.PropertyField(link.FindPropertyRelative("syncStart"));
            if (link.FindPropertyRelative("syncStart").boolValue)
                EditorGUILayout.PropertyField(link.FindPropertyRelative("startOffset"));
            EditorGUILayout.PropertyField(link.FindPropertyRelative("syncDuration"));
            if (link.FindPropertyRelative("syncDuration").boolValue)
                EditorGUILayout.PropertyField(link.FindPropertyRelative("durationScale"));
            EditorGUILayout.PropertyField(link.FindPropertyRelative("syncBlend"));

            // 削除
            if (GUILayout.Button("Remove")) { _links.DeleteArrayElementAtIndex(i); EditorGUILayout.EndVertical(); break; }
            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
