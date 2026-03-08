using UnityEngine;
using UnityJSONExporter;

public class ObjectMoveAndLookAtController : MonoBehaviour
{
    public static readonly string LocalPositionPropertyName = "LocalPosition";
    public static readonly string LookAtTargetPropertyName = "LookAtTarget";
    public static readonly string UpVectorPropertyName = "UpVector";

    public RawVector3 LocalPosition = RawVector3.zero;
    public RawVector3 UpVector = new RawVector3(0, 1, 0);

    public Transform LookAtTarget;
    
    public void MoveAndLookAt(Vector3 p, Transform lookAt)
    {
        transform.localPosition = p;
        transform.LookAt(lookAt, RawVector3.toVector3(UpVector));
        // Debug.Log($"[ObjectMoveAndLookAtController.MoveAndLookAt] p: {p}, look at: {lookAt}");
    }
}
