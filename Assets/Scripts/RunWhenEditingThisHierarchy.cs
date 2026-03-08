
// RunWhenEditingThisHierarchy.cs
// ルートに付ける。選択/操作中のみイベントを発火。

using System;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utilities
{
    [ExecuteAlways]
    public class RunWhenEditingThisHierarchy : MonoBehaviour
    {
        [Tooltip("子孫まで対象にするか")]
        public bool includeChildren = true;

        [Tooltip("対象が編集中になった瞬間")]
        public UnityEvent onBeginInteract;
        [Tooltip("対象の編集中に毎フレーム（Editor Update）呼ばれる")]
        public UnityEvent onEditingTick;
        [Tooltip("対象の編集中が終わった瞬間")]
        public UnityEvent onEndInteract;
        
        public Action onEditingTickAction;

#if UNITY_EDITOR
        bool _isRelevantSelected;  // 自分/子孫が選択されている
        bool _isInteracting;       // つまんで動かしている等（ハンドル掴み中）
        bool _wasInteracting;

        // 直近フレームのTRSを覚えて変化検知（最小限）
        struct TRS { public Vector3 p; public Quaternion r; public Vector3 s; }
        System.Collections.Generic.Dictionary<Transform, TRS> _last = new();

        void OnEnable()
        {
            EditorApplication.update += EditorUpdate;
            Selection.selectionChanged += OnSelectionChanged;
            SceneView.duringSceneGui += OnSceneGUI;

            CacheTransforms();
            OnSelectionChanged(); // 初期判定
        }

        void OnDisable()
        {
            EditorApplication.update -= EditorUpdate;
            Selection.selectionChanged -= OnSelectionChanged;
            SceneView.duringSceneGui -= OnSceneGUI;
            _last.Clear();
            _isInteracting = _wasInteracting = _isRelevantSelected = false;
        }

        void CacheTransforms()
        {
            _last.Clear();
            foreach (var t in EnumerateTargets())
                _last[t] = Snapshot(t);
        }

        System.Collections.Generic.IEnumerable<Transform> EnumerateTargets()
        {
            if (!includeChildren) { yield return transform; yield break; }
            foreach (var t in GetComponentsInChildren<Transform>(true))
                yield return t;
        }

        static TRS Snapshot(Transform t) => new TRS { p = t.position, r = t.rotation, s = t.localScale };

        bool AnyTransformChangedThisFrame()
        {
            bool changed = false;
            foreach (var t in EnumerateTargets())
            {
                var now = Snapshot(t);
                if (_last.TryGetValue(t, out var prev))
                {
                    if (now.p != prev.p || now.r != prev.r || now.s != prev.s)
                        changed = true;
                }
                _last[t] = now; // 更新
            }
            return changed;
        }

        void OnSelectionChanged()
        {
            _isRelevantSelected = IsAnySelectionInsideThisHierarchy();
            // 選択が外れたらインタラクト終了を即時扱い
            if (!_isRelevantSelected && _wasInteracting)
                EndInteract();
        }

        bool IsAnySelectionInsideThisHierarchy()
        {
            var sels = Selection.transforms;
            if (sels == null || sels.Length == 0) return false;
            foreach (var sel in sels)
            {
                if (!sel) continue;
                if (sel == transform) return true;
                if (includeChildren && sel.IsChildOf(transform)) return true;
            }
            return false;
        }

        void OnSceneGUI(SceneView sv)
        {
            // ハンドルを“掴んでいる”間は hotControl が非0
            var e = Event.current;
            bool dragging = GUIUtility.hotControl != 0; // Move/Rotate/Scale/Rect/CustomHandle等すべて
            // SceneView上でのドラッグのみ拾いたい時は e.type==MouseDrag も併用可
            _isInteracting = _isRelevantSelected && dragging;
        }

        void EditorUpdate()
        {
            // Inspectorでの値変更等、ハンドルを掴んでいなくても Transform が動いたらインタラクト扱い
            bool changedByInspector = _isRelevantSelected && AnyTransformChangedThisFrame();

            bool nowInteracting = _isInteracting || changedByInspector;

            if (nowInteracting && !_wasInteracting) BeginInteract();
            if (nowInteracting)
            {
                onEditingTick?.Invoke();
                onEditingTickAction?.Invoke();
            }
            if (!nowInteracting && _wasInteracting) EndInteract();

            _wasInteracting = nowInteracting;
        }

        void BeginInteract() => onBeginInteract?.Invoke();

        void EndInteract()
        {
            onEndInteract?.Invoke();
            // 終了時点の状態を基準化
            CacheTransforms();
        }
#endif
    }
}
