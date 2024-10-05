using UnityEngine;

public class ObjectMoveAndLookAtController : MonoBehaviour
{
    public static readonly string LocalPositionPropertyName = "LocalPosition";
    public static readonly string LookAtTargetPropertyName = "LookAtTarget";

    [SerializeField]
    public Vector3 LocalPosition = Vector3.zero;

    [SerializeField]
    public Transform LookAtTarget;

    public void MoveAndLookAt(Vector3 p, Transform lookAt)
    {
        transform.localPosition = p;
        transform.LookAt(lookAt);
    }
}
