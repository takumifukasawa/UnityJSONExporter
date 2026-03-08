using UnityEngine;
using Newtonsoft.Json;

namespace UnityJSONExporter
{

    [System.Serializable]
    [JsonConverter(typeof(RawVector3ArrayConverter))]
    public class RawVector3
    {
        public float x;
        public float y;
        public float z;

        public RawVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static RawVector3 zero
        {
            get { return new RawVector3(0, 0, 0); }
        }

        public static RawVector3 fromVector3(Vector3 v)
        {
            return new RawVector3(v.x, v.y, v.z);
        }

        public static Vector3 toVector3(RawVector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static RawVector3 fromVector3Info(Vector3Info v)
        {
            return new RawVector3(v.X, v.Y, v.Z);
        }
    }
}
