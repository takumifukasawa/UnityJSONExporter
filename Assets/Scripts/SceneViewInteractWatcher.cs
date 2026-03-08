using TimelineSynthesizer;
using UnityEngine;

[ExecuteAlways]
public class SceneViewActivitySensor : MonoBehaviour
{
    [SerializeField]
    private TimelineSynthesizerManager _timelineSynthesizerManager;
#if UNITY_EDITOR
    [Header("States")]
    // 状態をインスペクタで見たい場合は public/SerializeField のままでOK（保存したくなければ [NonSerialized] + [HideInInspector] など）
    [SerializeField, Tooltip("SceneView がフォーカス中か")]
    bool isSceneViewFocused;

    [SerializeField, Tooltip("マウスが SceneView 上にあるか")]
    bool isMouseOverSceneView;

    [SerializeField, Tooltip("Alt/中/右ドラッグやホイールなどで SceneView を操作中か")]
    bool isNavigating;

    [SerializeField, Tooltip("前フレームから SceneView カメラが動いたか")]
    bool isCameraMoving;

    Vector3 _prevPivot;
    Quaternion _prevRot;
    float _prevSize;
    bool _initialized;

    void OnEnable()
    {
        UnityEditor.EditorApplication.update += Tick;
        UnityEditor.SceneView.duringSceneGui += OnDuringSceneGUI;
        _initialized = false;
    }

    void OnDisable()
    {
        UnityEditor.EditorApplication.update -= Tick;
        UnityEditor.SceneView.duringSceneGui -= OnDuringSceneGUI;
        isNavigating = false;
        isCameraMoving = false;
    }

    void OnDestroy()
    {
        // Domain Reload 無効時のリーク対策で念のため
        UnityEditor.EditorApplication.update -= Tick;
        UnityEditor.SceneView.duringSceneGui -= OnDuringSceneGUI;
    }

    void OnDuringSceneGUI(UnityEditor.SceneView sv)
    {
        var e = Event.current;

        // フォーカス / ホバー
        isSceneViewFocused = UnityEditor.EditorWindow.focusedWindow is UnityEditor.SceneView;
        isMouseOverSceneView = UnityEditor.EditorWindow.mouseOverWindow is UnityEditor.SceneView;

        // ナビゲーション操作判定（右/中ドラッグ、Alt+左、ホイール等）
        switch (e.type)
        {
            case EventType.MouseDown:
            case EventType.MouseDrag:
            case EventType.ScrollWheel:
                if (e.button == 1 || e.button == 2 || e.alt || e.type == EventType.ScrollWheel)
                    isNavigating = true;
                break;

            case EventType.MouseUp:
            case EventType.KeyUp:
                isNavigating = false;
                break;
        }

        // （補助）現在 ViewTool モードかどうか
        // var isViewTool = UnityEditor.Tools.viewToolActive;
    }

    void Tick()
    {
        var sv = UnityEditor.SceneView.lastActiveSceneView;
        if (sv == null)
        {
            isCameraMoving = false;
            return;
        }

        var pivot = sv.pivot;
        var rot = sv.rotation;
        var size = sv.size;

        if (!_initialized)
        {
            _prevPivot = pivot;
            _prevRot = rot;
            _prevSize = size;
            _initialized = true;
            isCameraMoving = false;
            return;
        }

        isCameraMoving =
            (_prevPivot - pivot).sqrMagnitude > 1e-8f ||
            Quaternion.Angle(_prevRot, rot) > 0.01f ||
            Mathf.Abs(_prevSize - size) > 1e-6f;

        _prevPivot = pivot;
        _prevRot = rot;
        _prevSize = size;

        // if (_timelineSynthesizerManager && isNavigating)
        if (_timelineSynthesizerManager)
        {
            _timelineSynthesizerManager.SendSceneViewData();
        }
    }
#endif

    // ほかのスクリプトから読みたい場合の public プロパティ（Editor 以外では常に false を返す）
    public bool IsSceneViewFocused => isSceneViewFocused;
    public bool IsMouseOverSceneView => isMouseOverSceneView;
    public bool IsNavigating => isNavigating;
    public bool IsCameraMoving => isCameraMoving;
}
