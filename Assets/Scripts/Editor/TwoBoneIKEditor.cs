using UnityEngine;
using UnityEditor;
using Utilities;

[CustomEditor(typeof(TwoBoneIK))]
public class TwoBoneIKEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GUILayout.Space(10);
        
        TwoBoneIK twoBoneIK = (TwoBoneIK)target;
        
        if (GUILayout.Button("Reset to Initial Rotations"))
        {
            twoBoneIK.ResetToInitialRotations();
        }
        
        EditorGUILayout.HelpBox("回転がおかしくなった場合は、allowRotationをオフにしてから「Reset to Initial Rotations」ボタンを押してください。", MessageType.Info);
    }
}