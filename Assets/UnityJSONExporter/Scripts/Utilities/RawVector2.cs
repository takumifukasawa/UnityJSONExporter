using UnityEngine;
using Newtonsoft.Json;

namespace UnityJSONExporter
{

    [System.Serializable]
    [JsonConverter(typeof(RawVector2ArrayConverter))]
    public class RawVector2
    {
        public float x;
        public float y;

        public RawVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static RawVector2 zero
        {
            get { return new RawVector2(0, 0); }
        }

        public static RawVector2 fromVector2(Vector2 v)
        {
            return new RawVector2(v.x, v.y);
        }

        public static Vector2 toVector2(RawVector2 v)
        {
            return new Vector2(v.x, v.y);
        }
    }
}
