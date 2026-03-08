using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class CatmullRomSpline : MonoBehaviour
{
    [Header("Spline Settings")]
    [Range(5, 50)]
    public int segmentSamples = 10;

    [Header("Gizmos Settings")]
    public bool drawSpline = true;
    public bool drawControlPoints = true;
    public bool drawTangents = false;
    public bool drawNormals = false;
    public Color splineColor = Color.yellow;
    public Color controlPointColor = Color.red;
    public Color tangentColor = Color.blue;
    public Color normalColor = Color.green;
    public float controlPointRadius = 0.1f;
    public float tangentLength = 0.5f;
    public float normalLength = 0.3f;

    private List<Transform> controlPoints = new List<Transform>();

    private void OnDrawGizmos()
    {
        if (!drawSpline) return;
        DrawSplineGizmos(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawSpline) return;
        DrawSplineGizmos(true);
    }

    private void DrawSplineGizmos(bool selected)
    {
        UpdateControlPoints();

        if (controlPoints.Count < 2) return;

        // Draw control points
        if (drawControlPoints)
        {
            Gizmos.color = controlPointColor;
            foreach (var cp in controlPoints)
            {
                if (cp != null)
                {
                    Gizmos.DrawSphere(cp.position, controlPointRadius);
                }
            }
        }

        // Draw spline curve
        Gizmos.color = splineColor;

        int segments = controlPoints.Count - 1;

        for (int i = 0; i < segments; i++)
        {
            Vector3 p1 = controlPoints[i].position;
            Vector3 p2 = controlPoints[i + 1].position;

            Vector3 p0 = i > 0
                ? controlPoints[i - 1].position
                : p1 * 2 - p2;

            Vector3 p3 = i + 2 < controlPoints.Count
                ? controlPoints[i + 2].position
                : p2 * 2 - p1;

            int samples = (i == segments - 1) ? segmentSamples + 1 : segmentSamples;

            Vector3 prevPos = EvaluateCatmullRomSpline(0, p0, p1, p2, p3);

            for (int j = 1; j < samples; j++)
            {
                float t = (float)j / segmentSamples;
                Vector3 currentPos = EvaluateCatmullRomSpline(t, p0, p1, p2, p3);

                Gizmos.DrawLine(prevPos, currentPos);

                // Draw tangents and normals if enabled
                if (selected && (drawTangents || drawNormals))
                {
                    Vector3 tangent = GetCatmullRomSplineTangent(t, p0, p1, p2, p3);

                    if (drawTangents)
                    {
                        Gizmos.color = tangentColor;
                        Gizmos.DrawLine(currentPos, currentPos + tangent * tangentLength);
                    }

                    if (drawNormals)
                    {
                        Vector3 normal = GetFrenetFrameNormal(tangent);
                        Gizmos.color = normalColor;
                        Gizmos.DrawLine(currentPos, currentPos + normal * normalLength);
                    }

                    Gizmos.color = splineColor;
                }

                prevPos = currentPos;
            }
        }
    }

    private void UpdateControlPoints()
    {
        controlPoints.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                controlPoints.Add(child);
            }
        }
    }

    /// <summary>
    /// Catmull-Rom spline interpolation
    /// Based on TypeScript implementation in splineUtilities.ts
    /// </summary>
    private Vector3 EvaluateCatmullRomSpline(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        // v0 = 2 * p1
        Vector3 v0 = p1 * 2f;

        // v1 = (-p0 + p2) * t
        Vector3 v1 = (-p0 + p2) * t;

        // v2 = (2*p0 - 5*p1 + 4*p2 - p3) * t2
        Vector3 v2 = (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2;

        // v3 = (-p0 + 3*p1 - 3*p2 + p3) * t3
        Vector3 v3 = (-p0 + 3f * p1 - 3f * p2 + p3) * t3;

        return (v0 + v1 + v2 + v3) * 0.5f;
    }

    /// <summary>
    /// Get tangent at point t on Catmull-Rom spline
    /// </summary>
    private Vector3 GetCatmullRomSplineTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float t2 = t * t;

        // v0 = -p0 + p2
        Vector3 v0 = -p0 + p2;

        // v1 = (2*p0 - 5*p1 + 4*p2 - p3) * 2t
        Vector3 v1 = (2f * p0 - 5f * p1 + 4f * p2 - p3) * (2f * t);

        // v2 = (-p0 + 3*p1 - 3*p2 + p3) * 3t2
        Vector3 v2 = (-p0 + 3f * p1 - 3f * p2 + p3) * (3f * t2);

        Vector3 rawTangent = (v0 + v1 + v2) * 0.5f;

        return rawTangent.normalized;
    }

    /// <summary>
    /// Get Frenet frame normal from tangent
    /// </summary>
    private Vector3 GetFrenetFrameNormal(Vector3 tangent, Vector3? upHint = null)
    {
        Vector3 up = upHint ?? Vector3.up;

        Vector3 binormal = Vector3.Cross(up, tangent);

        // If tangent is parallel to up, try other axes
        if (binormal.magnitude < 0.01f)
        {
            binormal = Vector3.Cross(Vector3.right, tangent);
            if (binormal.magnitude < 0.01f)
            {
                binormal = Vector3.Cross(Vector3.forward, tangent);
            }
        }

        binormal = binormal.normalized;
        Vector3 normal = Vector3.Cross(tangent, binormal).normalized;

        return normal;
    }

    /// <summary>
    /// Get position at normalized t [0-1] along entire spline
    /// </summary>
    public Vector3 GetPositionAtT(float t)
    {
        UpdateControlPoints();

        if (controlPoints.Count < 2) return Vector3.zero;

        t = Mathf.Clamp01(t);

        int segments = controlPoints.Count - 1;
        float scaledT = t * segments;
        int segment = Mathf.Min(Mathf.FloorToInt(scaledT), segments - 1);
        float localT = scaledT - segment;

        Vector3 p1 = controlPoints[segment].position;
        Vector3 p2 = controlPoints[segment + 1].position;

        Vector3 p0 = segment > 0
            ? controlPoints[segment - 1].position
            : p1 * 2 - p2;

        Vector3 p3 = segment + 2 < controlPoints.Count
            ? controlPoints[segment + 2].position
            : p2 * 2 - p1;

        return EvaluateCatmullRomSpline(localT, p0, p1, p2, p3);
    }

    /// <summary>
    /// Get tangent at normalized t [0-1] along entire spline
    /// </summary>
    public Vector3 GetTangentAtT(float t)
    {
        UpdateControlPoints();

        if (controlPoints.Count < 2) return Vector3.forward;

        t = Mathf.Clamp01(t);

        int segments = controlPoints.Count - 1;
        float scaledT = t * segments;
        int segment = Mathf.Min(Mathf.FloorToInt(scaledT), segments - 1);
        float localT = scaledT - segment;

        Vector3 p1 = controlPoints[segment].position;
        Vector3 p2 = controlPoints[segment + 1].position;

        Vector3 p0 = segment > 0
            ? controlPoints[segment - 1].position
            : p1 * 2 - p2;

        Vector3 p3 = segment + 2 < controlPoints.Count
            ? controlPoints[segment + 2].position
            : p2 * 2 - p1;

        return GetCatmullRomSplineTangent(localT, p0, p1, p2, p3);
    }

    /// <summary>
    /// Sample all points along the spline
    /// </summary>
    public List<SplinePoint> SampleSplinePoints()
    {
        List<SplinePoint> points = new List<SplinePoint>();

        UpdateControlPoints();

        if (controlPoints.Count < 2) return points;

        int segments = controlPoints.Count - 1;

        for (int i = 0; i < segments; i++)
        {
            Vector3 p1 = controlPoints[i].position;
            Vector3 p2 = controlPoints[i + 1].position;

            Vector3 p0 = i > 0
                ? controlPoints[i - 1].position
                : p1 * 2 - p2;

            Vector3 p3 = i + 2 < controlPoints.Count
                ? controlPoints[i + 2].position
                : p2 * 2 - p1;

            int samples = (i == segments - 1) ? segmentSamples + 1 : segmentSamples;

            for (int j = 0; j < samples; j++)
            {
                float t = (float)j / segmentSamples;
                Vector3 position = EvaluateCatmullRomSpline(t, p0, p1, p2, p3);
                Vector3 tangent = GetCatmullRomSplineTangent(t, p0, p1, p2, p3);
                Vector3 normal = GetFrenetFrameNormal(tangent);
                Vector3 binormal = Vector3.Cross(tangent, normal).normalized;

                points.Add(new SplinePoint
                {
                    position = position,
                    tangent = tangent,
                    normal = normal,
                    binormal = binormal
                });
            }
        }

        return points;
    }
}

[System.Serializable]
public struct SplinePoint
{
    public Vector3 position;
    public Vector3 tangent;
    public Vector3 normal;
    public Vector3 binormal;
}
