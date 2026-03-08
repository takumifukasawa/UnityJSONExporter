using UnityEngine;

namespace UnityJSONExporter
{
    /// <summary>
    ///
    /// </summary>
    public class Vector3Info
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3Info(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3Info(Vector3 v)
        {
            X = v.x;
            Y = v.y;
            Z = v.z;
        }
    }

}
