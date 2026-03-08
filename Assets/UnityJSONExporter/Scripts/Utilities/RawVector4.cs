using UnityEngine;
using Newtonsoft.Json;

namespace UnityJSONExporter
{
    [System.Serializable]
    [JsonConverter(typeof(RawVector4ArrayConverter))]
    public class RawVector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public RawVector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static RawVector4 zero
        {
            get { return new RawVector4(0, 0, 0, 0); }
        }

        public static RawVector4 fromVector4(Vector4 v)
        {
            return new RawVector4(v.x, v.y, v.z, v.w);
        }

        public static Vector4 toVector4(RawVector4 v)
        {
            return new Vector4(v.x, v.y, v.z, v.w);
        }

        public static RawVector4 FromVector2(Vector2 v, Vector2 offset)
        {
            return new RawVector4(v.x, v.y, offset.x, offset.y);
        }
        
        
        public static RawVector4 fromVector4Info(Vector4Info v)
        {
            return new RawVector4(v.X, v.Y, v.Z, v.W);
        }
    }
}
