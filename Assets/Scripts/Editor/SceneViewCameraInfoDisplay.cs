using UnityEngine;
using UnityEditor;

public static class SceneViewCameraInfoDisplay
{
    private static bool _enabled = true;
    private static GUIStyle _boxStyle;
    private static GUIStyle _labelStyle;

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (!_enabled) return;

        InitializeStyles();

        Handles.BeginGUI();
        DrawCameraInfo(sceneView);
        Handles.EndGUI();
    }

    private static void InitializeStyles()
    {
        if (_boxStyle == null)
        {
            _boxStyle = new GUIStyle(GUI.skin.box);
            _boxStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.7f));
            _boxStyle.padding = new RectOffset(10, 10, 10, 10);
        }

        if (_labelStyle == null)
        {
            _labelStyle = new GUIStyle(GUI.skin.label);
            _labelStyle.normal.textColor = Color.white;
            _labelStyle.fontSize = 11;
            _labelStyle.richText = true;
        }
    }

    private static Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = color;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private static void DrawCameraInfo(SceneView sceneView)
    {
        Camera cam = sceneView.camera;
        if (cam == null) return;

        Vector3 position = cam.transform.position;
        Vector3 rotation = cam.transform.eulerAngles;
        Vector3 pivot = sceneView.pivot;
        float size = sceneView.size;
        bool isOrtho = sceneView.orthographic;
        float distance = Vector3.Distance(position, pivot);
        float fov = cam.fieldOfView;

        GUILayout.BeginArea(new Rect(10, 10, 320, 240));
        GUILayout.BeginVertical(_boxStyle);

        GUILayout.Label("<b>Scene View Camera Info</b>", _labelStyle);
        GUILayout.Space(5);

        GUILayout.Label($"<b>Position:</b> ({position.x:F2}, {position.y:F2}, {position.z:F2})", _labelStyle);
        GUILayout.Label($"<b>Rotation:</b> ({rotation.x:F2}, {rotation.y:F2}, {rotation.z:F2})", _labelStyle);
        GUILayout.Label($"<b>Pivot:</b> ({pivot.x:F2}, {pivot.y:F2}, {pivot.z:F2})", _labelStyle);
        GUILayout.Space(3);

        GUILayout.Label($"<b>Mode:</b> {(isOrtho ? "Orthographic" : "Perspective")}", _labelStyle);
        if (isOrtho)
        {
            GUILayout.Label($"<b>Size:</b> {size:F2}", _labelStyle);
        }
        else
        {
            GUILayout.Label($"<b>FOV:</b> {fov:F2}°", _labelStyle);
        }

        GUILayout.Label($"<b>Distance:</b> {distance:F2}", _labelStyle);
        GUILayout.Label($"<b>Near:</b> {cam.nearClipPlane:F2}, <b>Far:</b> {cam.farClipPlane:F2}", _labelStyle);

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
