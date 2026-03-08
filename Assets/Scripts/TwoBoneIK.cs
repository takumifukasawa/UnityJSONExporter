// TwoBoneIK_AgnosticAxes_Full.cs
// 軸非依存 2ボーンIK + ローカル軸リマップ（aim/up）+ ストレッチ + 安全LookRotation
// 作法：root(大腿付け根) - mid(すね) - end(足首) を指定。target/pole を与える。
// 注意：親の非一様スケールは距離計算を歪ませるので、可能なら (1,1,1) を推奨。

using UnityEngine;

namespace Utilities
{
    // [ExecuteAlways]
    public class TwoBoneIK : MonoBehaviour
    {
        [Header("General")]
        public bool allowUpdate = true;

        [Tooltip("Transformの位置を実際に更新するかどうか")]
        public bool allowPositionUpdate = false;

        [Tooltip("Transformの回転を更新するかどうか")]
        public bool allowRotation = true;

        [Header("Bones")]
        public Transform root; // 大腿の付け根

        public Transform mid; // すね
        public Transform end; // 足首

        [Header("Targets")]
        public Transform target; // 先端到達点

        public Transform pole; // 膝の曲げ方向（膝の前方に置く）

        [Header("Tuning")]
        [Range(0, 1)] public float weight = 1f;

        public bool allowStretch = false;

        [Tooltip("allowStretch が true のとき、(L1+L2)*maxStretch まで伸長を許可")]
        public float maxStretch = 1.1f;

        public enum Axis
        {
            X,
            Y,
            Z,
            XNeg,
            YNeg,
            ZNeg
        }

        [Header("Bone Local Axes (ローカルどの軸が'前/上'か)")]
        [Tooltip("ボーンの“伸び方向”（前）に相当するローカル軸")]
        public Axis aimAxis = Axis.Z; // 多くの環境は +Z 前。+Y 前進リグなら Axis.Y

        [Tooltip("ボーンの“上”（ロール基準）に相当するローカル軸（aim と直交させること）")]
        public Axis upAxis = Axis.Y; // 通常は +Y

        [Header("Advanced")]
        [Tooltip("true なら初期姿勢から現在見た目に一致する軸補正を自動推定（明示指定より優先）")]
        public bool useAutoAxisRemap = false;

        // 内部状態（自動リマップ用）
        Quaternion _qAxisRemapAuto = Quaternion.identity;
        bool _remapReady = false;

        // 初期回転状態保存
        Quaternion _initialRootRotation = Quaternion.identity;
        Quaternion _initialMidRotation = Quaternion.identity;
        Quaternion _initialEndRotation = Quaternion.identity;
        bool _initialRotationsSaved = false;

        // デバッグ用理想位置（Gizmos表示用）
        Vector3 _idealMidPos;
        Vector3 _idealEndPos;
        bool _hasIdealPositions = false;

        // デバッグ用回転方向（Gizmos表示用）
        Vector3 _currentRootToMid;
        Vector3 _idealRootToMid;
        Vector3 _currentMidToEnd;
        Vector3 _idealMidToEnd;
        bool _hasRotationDebugInfo = false;

        void OnEnable()
        {
            _remapReady = false;
            _initialRotationsSaved = false;
        }

        void SaveInitialRotations()
        {
            if (!root || !mid || !end) return;

            _initialRootRotation = root.rotation;
            _initialMidRotation = mid.rotation;
            _initialEndRotation = end.rotation;
            _initialRotationsSaved = true;
        }

        public void ResetToInitialRotations()
        {
            if (!_initialRotationsSaved || !root || !mid || !end) return;

            root.rotation = _initialRootRotation;
            mid.rotation = _initialMidRotation;
            end.rotation = _initialEndRotation;
        }


        void LateUpdate()
        {
            // UpdateIKTransform();
        }

        public void SetTargetPosition(Vector3 position)
        {
            if (target)
            {
                target.position = position;
            }
        }

        public void SetTargetLocalPosition(Vector3 localPosition)
        {
            if (target)
            {
                target.localPosition = localPosition;
            }
        }

        public void SetTargetPolePosition(Vector3 localPosition)
        {
            if (pole)
            {
                pole.localPosition = localPosition;
            }
        }

        public void UpdateIKTransform()
        {
            if (!allowUpdate) return;
            if (!root || !mid || !end || !target) return;

            // 初期回転を保存（初回のみ）
            if (!_initialRotationsSaved)
            {
                SaveInitialRotations();
            }

            // 長さ（軸仮定なし）
            float L1 = (mid.position - root.position).magnitude;
            float L2 = (end.position - mid.position).magnitude;
            if (L1 < 1e-6f || L2 < 1e-6f) return;

            Vector3 p0 = root.position;
            Vector3 pt = target.position;

            // root→target
            Vector3 rt = pt - p0;
            float d = rt.magnitude;
            if (d < 1e-8f) rt = root.forward; // 方向が無いときの退避
            Vector3 f = rt.normalized;

            // ストレッチ/クランプ
            float maxReach = L1 + L2;
            float minReach = Mathf.Abs(L1 - L2) + 1e-6f;
            float clampedD = Mathf.Clamp(d, minReach, maxReach);
            if (allowStretch && d > maxReach)
            {
                clampedD = Mathf.Min(d, maxReach * Mathf.Max(1f, maxStretch));
            }

            // 曲げ面の法線 n（pole 優先。直線/ゼロ回避）
            Vector3 n;
            if (pole)
            {
                Vector3 pr = pole.position - p0;
                n = Vector3.Cross(f, pr);
                if (n.sqrMagnitude < 1e-10f)
                {
                    Vector3 a = (Mathf.Abs(f.x) < 0.5f) ? Vector3.right :
                        (Mathf.Abs(f.y) < 0.5f) ? Vector3.up : Vector3.forward;
                    n = Vector3.Cross(f, a);
                }
            }
            else
            {
                Vector3 pr = mid.position - p0; // 現在膝寄り方向を維持
                n = Vector3.Cross(f, pr);
                if (n.sqrMagnitude < 1e-10f)
                {
                    Vector3 a = (Mathf.Abs(f.x) < 0.5f) ? Vector3.right :
                        (Mathf.Abs(f.y) < 0.5f) ? Vector3.up : Vector3.forward;
                    n = Vector3.Cross(f, a);
                }
            }

            n = n.normalized;

            // 平面内の直交基底：u は f と n に直交（膝が飛び出す方向）
            Vector3 u = Vector3.Cross(n, f).normalized;

            // 余弦定理：膝位置（root基準で f方向 x, u方向 y）
            float aLen = L1, bLen = L2, cLen = Mathf.Clamp(clampedD, minReach, L1 + L2);
            float cosTheta = Mathf.Clamp((aLen * aLen + cLen * cLen - bLen * bLen) / (2f * aLen * cLen), -1f, 1f);
            float sinTheta = Mathf.Sqrt(Mathf.Max(0f, 1f - cosTheta * cosTheta));
            float x = cosTheta * aLen; // f方向
            float y = sinTheta * aLen; // u方向（pole が決める曲げ側）

            Vector3 newMidPos = p0 + f * x + u * y;
            Vector3 newEndPos = p0 + f * cLen;

            // ブレンド
            Vector3 midPos = Vector3.Lerp(mid.position, newMidPos, weight);
            Vector3 endPos = Vector3.Lerp(end.position, newEndPos, weight);

            // デバッグ用理想位置を保存
            _idealMidPos = midPos;
            _idealEndPos = endPos;
            _hasIdealPositions = true;

            // 位置更新（allowPositionUpdateがtrueの場合のみ）
            if (allowPositionUpdate)
            {
                mid.position = midPos;
                end.position = endPos;
            }

            // 回転更新（allowRotationがtrueの場合のみ）
            if (allowRotation)
            {
                // 軸リマップ（一時的に無効化してテスト）
                // Quaternion qAxisRemap = useAutoAxisRemap ? PrepareAxisRemapAuto() : BuildAxisRemapFromInspector();
                Quaternion qAxisRemap = Quaternion.identity; // テスト用：軸リマップを無効化

                // 回転反映（FromToRotationを使用したより直感的な方法）
                Vector3 rootToMidIdeal = midPos - root.position;
                Vector3 midToEndIdeal = endPos - midPos;

                // デバッグ情報を保存
                _currentRootToMid = mid.position - root.position;
                _idealRootToMid = rootToMidIdeal;
                _currentMidToEnd = end.position - mid.position;
                _idealMidToEnd = midToEndIdeal;
                _hasRotationDebugInfo = true;

                if (rootToMidIdeal.sqrMagnitude > 1e-10f)
                {
                    Vector3 currentRootToMid = mid.position - root.position;
                    if (currentRootToMid.sqrMagnitude > 1e-10f)
                    {
                        Quaternion fromToRotation = Quaternion.FromToRotation(currentRootToMid.normalized, rootToMidIdeal.normalized);
                        root.rotation = Quaternion.Slerp(root.rotation, fromToRotation * root.rotation, weight);
                    }
                }

                if (midToEndIdeal.sqrMagnitude > 1e-10f)
                {
                    Vector3 currentMidToEnd = end.position - mid.position;
                    if (currentMidToEnd.sqrMagnitude > 1e-10f)
                    {
                        Quaternion fromToRotation = Quaternion.FromToRotation(currentMidToEnd.normalized, midToEndIdeal.normalized);
                        mid.rotation = Quaternion.Slerp(mid.rotation, fromToRotation * mid.rotation, weight);
                    }
                }
            }

            // 末端の回転も合わせたいならここで：
            // end.rotation = Quaternion.Slerp(end.rotation, target.rotation * qAxisRemap, weight);
        }

        // --- 軸リマップ関連 ---

        // Inspector の aimAxis/upAxis から後掛け回転を構成
        Quaternion BuildAxisRemapFromInspector()
        {
            Vector3 aim = AxisToVector(aimAxis);
            Vector3 up = AxisToVector(upAxis);

            // 直交化（ユーザーの選択ミス対策）
            up = Vector3.ProjectOnPlane(up, aim).normalized;
            if (up.sqrMagnitude < 1e-12f)
            {
                // aim に直交する適当な up を作る
                Vector3 a = (Mathf.Abs(aim.x) < 0.5f) ? Vector3.right :
                    (Mathf.Abs(aim.y) < 0.5f) ? Vector3.up : Vector3.forward;
                up = Vector3.Cross(a, aim).normalized;
            }

            return Quaternion.LookRotation(aim, up); // (+Z前,+Y上)基準への“後掛け”
        }

        // 初期姿勢から見た目一致の補正を自動推定
        Quaternion PrepareAxisRemapAuto()
        {
            if (_remapReady && _qAxisRemapAuto != default(Quaternion)) return _qAxisRemapAuto;
            if (!root || !mid)
            {
                _qAxisRemapAuto = Quaternion.identity;
                _remapReady = true;
                return _qAxisRemapAuto;
            }

            Vector3 f = (mid.position - root.position);
            if (f.sqrMagnitude < 1e-8f)
            {
                _qAxisRemapAuto = Quaternion.identity;
                _remapReady = true;
                return _qAxisRemapAuto;
            }

            f.Normalize();

            // 初期回転を使用して安定したリマップを計算
            Quaternion initialRot = _initialRotationsSaved ? _initialRootRotation : root.rotation;
            Quaternion desired = LookRotationSafe(f, Vector3.up);
            _qAxisRemapAuto = Quaternion.Inverse(desired) * initialRot;

            // 異常値チェック
            if (float.IsNaN(_qAxisRemapAuto.x) || float.IsNaN(_qAxisRemapAuto.y) ||
                float.IsNaN(_qAxisRemapAuto.z) || float.IsNaN(_qAxisRemapAuto.w))
            {
                _qAxisRemapAuto = Quaternion.identity;
            }

            _remapReady = true;
            return _qAxisRemapAuto;
        }

        static Vector3 AxisToVector(Axis a)
        {
            switch (a)
            {
                case Axis.X: return Vector3.right;
                case Axis.Y: return Vector3.up;
                case Axis.Z: return Vector3.forward;
                case Axis.XNeg: return -Vector3.right;
                case Axis.YNeg: return -Vector3.up;
                case Axis.ZNeg: return -Vector3.forward;
            }

            return Vector3.forward;
        }

        // --- 角度正規化 ---
        // 角度を-180～180度の範囲に正規化
        public static float NormalizeAngle(float angle)
        {
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            return angle;
        }

        // Vector3の各要素を正規化
        public static Vector3 NormalizeEulerAngles(Vector3 eulerAngles)
        {
            return new Vector3(
                NormalizeAngle(eulerAngles.x),
                NormalizeAngle(eulerAngles.y),
                NormalizeAngle(eulerAngles.z)
            );
        }

        // --- 安全 LookRotation ---
        static Quaternion LookRotationSafe(Vector3 forward, Vector3 up)
        {
            if (forward.sqrMagnitude < 1e-12f) return Quaternion.identity;
            forward.Normalize();

            // up が forward と平行だとねじれるので直交化
            if (up.sqrMagnitude < 1e-12f) up = Vector3.up;
            up = Vector3.ProjectOnPlane(up, forward).normalized;
            if (up.sqrMagnitude < 1e-12f)
            {
                // それでもダメなら強制直交 up
                Vector3 a = (Mathf.Abs(forward.x) < 0.5f) ? Vector3.right :
                    (Mathf.Abs(forward.y) < 0.5f) ? Vector3.up : Vector3.forward;
                up = Vector3.Cross(a, forward).normalized;
            }

            return Quaternion.LookRotation(forward, up);
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (!root) return;

            // ターゲットライン
            Gizmos.color = Color.cyan;
            if (target) Gizmos.DrawLine(root.position, target.position);

            // ポール
            if (pole)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(pole.position, 0.02f);
            }

            // 理想位置の可視化（allowPositionUpdateがfalseの時のみ）
            if (!allowPositionUpdate && _hasIdealPositions && mid && end)
            {
                // 理想位置を緑色で表示
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_idealMidPos, 0.015f);
                Gizmos.DrawSphere(_idealEndPos, 0.015f);

                // 理想的なボーンライン
                Gizmos.color = Color.green;
                Gizmos.DrawLine(root.position, _idealMidPos);
                Gizmos.DrawLine(_idealMidPos, _idealEndPos);

                // 現在位置と理想位置の差を赤線で表示
                Gizmos.color = Color.red;
                Gizmos.DrawLine(mid.position, _idealMidPos);
                Gizmos.DrawLine(end.position, _idealEndPos);
            }

            // 回転方向のデバッグ可視化
            if (_hasRotationDebugInfo && root && mid && end)
            {
                float arrowLength = 0.1f;

                // 現在の方向を青色の矢印で表示
                Gizmos.color = Color.blue;
                if (_currentRootToMid.sqrMagnitude > 1e-10f)
                {
                    Vector3 currentDir = _currentRootToMid.normalized * arrowLength;
                    Gizmos.DrawRay(root.position, currentDir);
                    Gizmos.DrawSphere(root.position + currentDir, 0.01f);
                }

                if (_currentMidToEnd.sqrMagnitude > 1e-10f)
                {
                    Vector3 currentDir = _currentMidToEnd.normalized * arrowLength;
                    Gizmos.DrawRay(mid.position, currentDir);
                    Gizmos.DrawSphere(mid.position + currentDir, 0.01f);
                }

                // 理想の方向を緑色の矢印で表示
                Gizmos.color = Color.green;
                if (_idealRootToMid.sqrMagnitude > 1e-10f)
                {
                    Vector3 idealDir = _idealRootToMid.normalized * arrowLength;
                    Gizmos.DrawRay(root.position, idealDir);
                    Gizmos.DrawSphere(root.position + idealDir, 0.008f);
                }

                if (_idealMidToEnd.sqrMagnitude > 1e-10f)
                {
                    Vector3 idealDir = _idealMidToEnd.normalized * arrowLength;
                    Gizmos.DrawRay(mid.position, idealDir);
                    Gizmos.DrawSphere(mid.position + idealDir, 0.008f);
                }
            }
        }
#endif
    }
}
