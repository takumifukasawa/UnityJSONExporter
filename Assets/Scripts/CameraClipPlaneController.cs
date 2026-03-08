using UnityEngine;

[ExecuteAlways]
public class CameraClipPlaneController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private float _near = 0.3f;

    [SerializeField]
    private float _far = 1000f;

    private void OnValidate()
    {
        ApplyClipPlanes();
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            ApplyClipPlanes();
        }
    }

    private void ApplyClipPlanes()
    {
        if (_camera == null)
            _camera = GetComponent<Camera>();

        if (_camera != null)
        {
            _camera.nearClipPlane = _near;
            _camera.farClipPlane = _far;
        }
    }
}
