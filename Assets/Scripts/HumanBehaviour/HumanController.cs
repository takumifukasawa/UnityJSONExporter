using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using UnityJSONExporter;
using Utilities;

[ExecuteAlways]
public class HumanController : TimelineBindingObjectBase
{
    // [SerializeField]
    // private RunWhenEditingThisHierarchy _runWhenEditingThisHierarchy;

    [SerializeField]
    private bool _showWireframe = true;

    [SerializeField]
    private float _jointSize = 0.02f;

    [SerializeField]
    private Color _jointColor = Color.red;

    [SerializeField]
    private Color _boneColor = Color.white;

    [SerializeField]
    private Transform _meshAnchor;

    [SerializeField]
    private Transform _viewAnchor;

    [Space(13)]
    [Header("Pelvis Position")]
    [JsonProperty(PropertyName = "pp")]
    public Vector3 PelvisPosition = new Vector3(0, 1.05f, 0);

    [JsonProperty(PropertyName = "pr")]
    public Vector3 PelvisRotation;

    [Header("Spine1 Rotation")]
    [JsonProperty(PropertyName = "s1r")]
    public Vector3 Spine1Rotation;

    [Header("Spine2 Rotation")]
    [JsonProperty(PropertyName = "s2r")]
    public Vector3 Spine2Rotation;

    [Header("Joint Info")]
    [JsonProperty(PropertyName = "lsr")]
    public Vector3 LeftShoulderRotation;

    [Tooltip("Only Y")]
    [JsonProperty(PropertyName = "ler")]
    public Vector3 LeftElbowRotation;

    [JsonProperty(PropertyName = "lsp")]
    public Vector3 LeftShoulderPosition;

    [JsonProperty(PropertyName = "lep")]
    public Vector3 LeftElbowPosition;

    [JsonProperty(PropertyName = "lwr")]
    public Vector3 LeftWristRotation;

    [JsonProperty(PropertyName = "rsr")]
    public Vector3 RightShoulderRotation;

    [Tooltip("Only Y")]
    [JsonProperty(PropertyName = "rer")]
    public Vector3 RightElbowRotation;

    [JsonProperty(PropertyName = "rwr")]
    public Vector3 RightWristRotation;

    [JsonProperty(PropertyName = "rsp")]
    public Vector3 RightShoulderPosition;

    [JsonProperty(PropertyName = "rep")]
    public Vector3 RightElbowPosition;

    [JsonProperty(PropertyName = "lhr")]
    public Vector3 LeftHipRotation;

    [Tooltip("Only X")]
    [JsonProperty(PropertyName = "lkr")]
    public Vector3 LeftKneeRotation;

    [JsonProperty(PropertyName = "lhp")]
    public Vector3 LeftHipPosition;

    [JsonProperty(PropertyName = "lkp")]
    public Vector3 LeftKneePosition;

    [JsonProperty(PropertyName = "lfr")]
    public Vector3 LeftFootRotation;

    [JsonProperty(PropertyName = "lfp")]
    public Vector3 LeftFootPosition;

    [JsonProperty(PropertyName = "rhp")]
    public Vector3 RightHipPosition;

    [JsonProperty(PropertyName = "rhr")]
    public Vector3 RightHipRotation;

    [Tooltip("Only X")]
    [JsonProperty(PropertyName = "rkr")]
    public Vector3 RightKneeRotation;

    [JsonProperty(PropertyName = "rkp")]
    public Vector3 RightKneePosition;

    // [Tooltip("Only X")]
    [JsonProperty(PropertyName = "rfr")]
    public Vector3 RightFootRotation;

    [JsonProperty(PropertyName = "rfp")]
    public Vector3 RightFootPosition;

    [JsonProperty(PropertyName = "nr")]
    public Vector3 NeckRotation;

    [Space(13)]
    [Header("IK Controls")]
    [ExcludeExportProperty]
    [SerializeField]
    public Vector3 LeftHandIKPosition;

    [ExcludeExportProperty]
    [SerializeField]
    public Vector3 RightHandIKPosition;

    [ExcludeExportProperty]
    [SerializeField]
    public Vector3 LeftFootIKPosition;

    [ExcludeExportProperty]
    [SerializeField]
    public Vector3 RightFootIKPosition;

    [ExcludeExportProperty]
    [SerializeField]
    public Vector3 LeftHandIKPolePosition;

    [ExcludeExportProperty]
    [SerializeField]
    public Vector3 RightHandIKPolePosition;

    [ExcludeExportProperty]
    [SerializeField]
    public Vector3 LeftFootIKPolePosition;

    [ExcludeExportProperty]
    [SerializeField]
    public Vector3 RightFootIKPolePosition;

    [Space(13)]
    [Header("IK Controller")]
    [SerializeField]
    private TwoBoneIK _leftArmIK;

    [SerializeField]
    private TwoBoneIK _rightArmIK;

    [SerializeField]
    private TwoBoneIK _leftLegIK;

    [SerializeField]
    private TwoBoneIK _rightLegIK;

    [Space(13)]
    [Header("References")]
    [Header("Body")]
    [SerializeField]
    private Transform _pelvisAnchor;

    [SerializeField]
    private Transform _spine01Anchor;

    [SerializeField]
    private Transform _spine02Anchor;

    [SerializeField]
    private Transform _neckAnchor;

    [SerializeField]
    private Transform _headAnchor;

    [Header("Left Arm")]
    [SerializeField]
    private Transform _leftShoulderAnchor;

    [SerializeField]
    private Transform _leftElbowAnchor;

    [SerializeField]
    private Transform _leftWristAnchor;

    [SerializeField]
    private Transform _leftHandAnchor;

    [SerializeField]
    private Transform _leftThumbAnchor;

    [SerializeField]
    private Transform _leftFingerBundleAnchor;

    [Header("Right Arm")]
    [SerializeField]
    private Transform _rightShoulderAnchor;

    [SerializeField]
    private Transform _rightElbowAnchor;

    [SerializeField]
    private Transform _rightWristAnchor;

    [SerializeField]
    private Transform _rightHandAnchor;

    [SerializeField]
    private Transform _rightThumbAnchor;

    [SerializeField]
    private Transform _rightFingerBundleAnchor;

    [Header("Left Leg")]
    [SerializeField]
    private Transform _leftHipAnchor;

    [SerializeField]
    private Transform _leftKneeAnchor;

    [SerializeField]
    private Transform _leftAnkleAnchor;

    [SerializeField]
    private Transform _leftFootAnchor;

    [Header("Right Leg")]
    [SerializeField]
    private Transform _rightHipAnchor;

    [SerializeField]
    private Transform _rightKneeAnchor;

    [SerializeField]
    private Transform _rightAnkleAnchor;

    [SerializeField]
    private Transform _rightFootAnchor;

    [Space(13)]
    [JsonProperty("ms_mi")] // material switcher matrial index
    public int MaterialIndex;


    void ApplyRotation(Transform anchor, Vector3 rot, Vector3 axis)
    {
        // anchor.localRotation = Quaternion.identity;
        // anchor.Rotate(0, 0, rot.z * axis.x, Space.Self); // Z
        // anchor.Rotate(rot.x * axis.y, 0, 0, Space.Self); // → そのZ後のX
        // anchor.Rotate(0, rot.y * axis.z, 0, Space.Self); // → そのX後のY

        var q = Quaternion.Euler(
            rot.x * axis.y,
            rot.y * axis.z,
            rot.z * axis.x
        );
        anchor.localRotation = q;
    }

    void ApplyPosition(Transform anchor, Vector3 pos, Vector3 axis)
    {
        anchor.localPosition = new Vector3(
            pos.x * axis.x,
            pos.y * axis.y,
            pos.z * axis.z
        );
    }

    void OnDidApplyAnimationProperties()
    {
        // Debug.Log($"[HumanController.OnValidate] ===============================");
        // Debug.Log($"[HumanController.OnValidate] leftShoulderRotation: {LeftShoulderRotation}");
        // Debug.Log($"[HumanController.OnValidate] leftElbowRotation: {LeftElbowRotation}");

        // tmp
        // ApplyRotation(_leftShoulderAnchor, LeftShoulderRotation, new Vector3(1, 1, 1));
        // ApplyRotation(_leftElbowAnchor, LeftElbowRotation, new Vector3(-1, -1, -1)); // only y
        // ApplyRotation(_rightShoulderAnchor, RightShoulderRotation, new Vector3(1, 1, 1));
        // ApplyRotation(_rightElbowAnchor, RightElbowRotation, new Vector3(1, 1, 1)); // only y
        // ApplyRotation(_leftHipAnchor, LeftHipRotation, new Vector3(1, 1, 1));
        // ApplyRotation(_leftKneeAnchor, LeftKneeRotation, new Vector3(-1, -1, -1)); // only x
        // ApplyRotation(_rightHipAnchor, RightHipRotation, new Vector3(1, 1, 1));
        // ApplyRotation(_rightKneeAnchor, RightKneeRotation, new Vector3(1, 1, 1)); // only x

        // pelvis
        _pelvisAnchor.localPosition = PelvisPosition;
        ApplyRotation(_pelvisAnchor, PelvisRotation, new Vector3(1, 1, 1));

        // pelvis2
        ApplyRotation(_spine01Anchor, Spine1Rotation, new Vector3(1, -1, 1));
        ApplyRotation(_spine02Anchor, Spine2Rotation, new Vector3(1, -1, 1));

        // 左腕IK処理
        _leftArmIK.SetTargetPolePosition(LeftHandIKPolePosition);
        _leftArmIK.SetTargetLocalPosition(LeftHandIKPosition);
        _leftArmIK.UpdateIKTransform();

        // 右腕IK処理
        _rightArmIK.SetTargetPolePosition(RightHandIKPolePosition);
        _rightArmIK.SetTargetLocalPosition(RightHandIKPosition);
        _rightArmIK.UpdateIKTransform();

        // 左足IK処理
        _leftLegIK.SetTargetPolePosition(LeftFootIKPolePosition);
        _leftLegIK.SetTargetLocalPosition(LeftFootIKPosition);
        _leftLegIK.UpdateIKTransform();

        // 右足IK処理
        _rightLegIK.SetTargetPolePosition(RightFootIKPolePosition);
        _rightLegIK.SetTargetLocalPosition(RightFootIKPosition);
        _rightLegIK.UpdateIKTransform();

        ApplyRotation(_neckAnchor, NeckRotation, new Vector3(1, 1, 1));

        // Debug.Log($"hogehoge: {RightFootIKPosition}");
    }

    private void OnValidate()
    {
        if (_meshAnchor && _viewAnchor)
        {
            var scale = _meshAnchor.localScale;
            var p = new Vector3(
                _meshAnchor.localPosition.x / scale.x,
                _meshAnchor.localPosition.y / scale.y,
                _meshAnchor.localPosition.z / scale.z
            ) * -1;
            var s = new Vector3(
                1f / scale.x,
                1f / scale.y,
                1f / scale.z
            );

            _viewAnchor.localPosition = p;
            _viewAnchor.localScale = s;
        }
    }

    private void OnDrawGizmos()
    {
        if (!_showWireframe) return;

        DrawJoint(_pelvisAnchor, _jointColor, _jointSize);
        DrawJoint(_spine01Anchor, _jointColor, _jointSize);
        DrawJoint(_spine02Anchor, _jointColor, _jointSize);
        DrawJoint(_neckAnchor, _jointColor, _jointSize);
        DrawJoint(_headAnchor, _jointColor, _jointSize);

        DrawJoint(_leftShoulderAnchor, _jointColor, _jointSize);
        DrawJoint(_leftElbowAnchor, _jointColor, _jointSize);
        DrawJoint(_leftWristAnchor, _jointColor, _jointSize);
        DrawJoint(_leftHandAnchor, _jointColor, _jointSize);
        DrawJoint(_leftThumbAnchor, _jointColor, _jointSize);
        DrawJoint(_leftFingerBundleAnchor, _jointColor, _jointSize);

        DrawJoint(_rightShoulderAnchor, _jointColor, _jointSize);
        // DrawJoint(_rightUpperArmAnchor, _jointColor, _jointSize);
        DrawJoint(_rightElbowAnchor, _jointColor, _jointSize);
        // DrawJoint(_rightForearmAnchor, _jointColor, _jointSize);
        DrawJoint(_rightWristAnchor, _jointColor, _jointSize);
        DrawJoint(_rightHandAnchor, _jointColor, _jointSize);
        DrawJoint(_rightThumbAnchor, _jointColor, _jointSize);
        DrawJoint(_rightFingerBundleAnchor, _jointColor, _jointSize);

        DrawJoint(_leftHipAnchor, _jointColor, _jointSize);
        DrawJoint(_leftKneeAnchor, _jointColor, _jointSize);
        DrawJoint(_leftAnkleAnchor, _jointColor, _jointSize);
        DrawJoint(_leftFootAnchor, _jointColor, _jointSize);

        DrawJoint(_rightHipAnchor, _jointColor, _jointSize);
        DrawJoint(_rightKneeAnchor, _jointColor, _jointSize);
        DrawJoint(_rightAnkleAnchor, _jointColor, _jointSize);
        DrawJoint(_rightFootAnchor, _jointColor, _jointSize);

        DrawBones();
    }

    private void DrawBones()
    {
        Gizmos.color = _boneColor;

        DrawBone(_pelvisAnchor, _spine01Anchor);
        DrawBone(_spine01Anchor, _spine02Anchor);
        DrawBone(_spine02Anchor, _neckAnchor);
        DrawBone(_neckAnchor, _headAnchor);

        DrawBone(_spine02Anchor, _leftShoulderAnchor);
        DrawBone(_leftShoulderAnchor, _leftElbowAnchor);
        DrawBone(_leftElbowAnchor, _leftWristAnchor);
        DrawBone(_leftWristAnchor, _leftHandAnchor);
        DrawBone(_leftHandAnchor, _leftThumbAnchor);
        DrawBone(_leftHandAnchor, _leftFingerBundleAnchor);

        DrawBone(_spine02Anchor, _rightShoulderAnchor);
        DrawBone(_rightShoulderAnchor, _rightElbowAnchor);
        DrawBone(_rightElbowAnchor, _rightWristAnchor);
        DrawBone(_rightWristAnchor, _rightHandAnchor);
        DrawBone(_rightHandAnchor, _rightThumbAnchor);
        DrawBone(_rightHandAnchor, _rightFingerBundleAnchor);

        DrawBone(_pelvisAnchor, _leftHipAnchor);
        DrawBone(_leftHipAnchor, _leftKneeAnchor);
        DrawBone(_leftKneeAnchor, _leftAnkleAnchor);
        DrawBone(_leftAnkleAnchor, _leftFootAnchor);

        DrawBone(_pelvisAnchor, _rightHipAnchor);
        DrawBone(_rightHipAnchor, _rightKneeAnchor);
        DrawBone(_rightKneeAnchor, _rightAnkleAnchor);
        DrawBone(_rightAnkleAnchor, _rightFootAnchor);
    }

    private void DrawBone(Transform from, Transform to)
    {
        if (from == null || to == null) return;

        Gizmos.DrawLine(from.position, to.position);
    }

    private void DrawJoint(Transform joint, Color color, float size)
    {
        if (joint == null) return;

        Gizmos.color = color;
        Gizmos.DrawSphere(joint.position, size);
    }

    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[HumanController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }

    public void OnEditingInHierarchy()
    {
        // if (_runWhenEditingThisHierarchy != null)
        // {
        //     _runWhenEditingThisHierarchy.Run();
        // }
    }
}
