    using UnityEngine;

    public class ObjectMoveAndLookAtController : MonoBehaviour
    {
        public Vector3 LocalPosition = Vector3.zero;
        public static readonly string LocalPositionPropertyName = "LocalPosition";
        
        public Transform LookAtTarget;
        public static readonly string LookAtTargetPropertyName = "LookAtTarget";
        
        public void MoveAndLookAt(Vector3 p, Transform lookAt) {
            transform.localPosition = p;
            transform.LookAt(lookAt);
        }
    }
