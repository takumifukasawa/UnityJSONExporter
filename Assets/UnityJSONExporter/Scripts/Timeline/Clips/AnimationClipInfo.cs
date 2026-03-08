using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    /// <summary>
    /// 
    /// </summary>
    public class AnimationClipInfo : ClipInfoBase
    {
        // [JsonProperty(PropertyName = "s")]
        // public float Start;

        // [JsonProperty(PropertyName = "d")]
        // public float Duration;

        [JsonIgnore]
        // [JsonProperty(PropertyName = "act")]
        public AnimationClipType AnimationClipType = AnimationClipType.Default;

        [JsonIgnore]
        // [JsonProperty(PropertyName = "b")]
        public List<ClipBinding> Bindings = null;

        [JsonIgnore]
        // [JsonProperty(PropertyName = "op")]
        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Vector3Info OffsetPosition = null;

        [JsonIgnore]
        // [JsonProperty(PropertyName = "or")]
        // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Vector3Info OffsetRotation = null;

        // [JsonProperty(PropertyName = "b")]
        // public List<AnimationClipBinding> Bindings = new List<AnimationClipBinding>();

        public AnimationClipInfo(string name, AnimationClipType act, float s, float d, TimelineClip.ClipExtrapolation clipExtrapolation) : base(name, ClipInfoType.AnimationClip, s, d, clipExtrapolation)
        {
            AnimationClipType = act;
            // OffsetPosition = new Vector3Info(0, 0, 0);
            // OffsetRotation = new Vector3Info(0, 0, 0);
        }

        // public AnimationClipInfo(float s, float d, Vector3 op, Vector3 or) : base(ClipInfoType.AnimationClip, s, d)
        // {
        //     OffsetPosition = new Vector3Info(op);
        //     OffsetRotation = new Vector3Info(or);
        // }

        public void AddBinding(ClipBinding binding)
        {
            if (Bindings == null) Bindings = new List<ClipBinding>();
            Bindings.Add(binding);
        }

        protected internal override void WriteSubclassArray(JsonWriter w, JsonSerializer s)
        {
            // [親の情報, bindings, animationClipType, offsetPosition?, offsetRotation?]

            if (Bindings != null && Bindings.Count > 0) s.Serialize(w, Bindings);
            else
            {
                w.WriteStartArray();
                w.WriteEndArray();
            }

            w.WriteValue((int)AnimationClipType);

            if (OffsetPosition != null && OffsetRotation != null)
            {
                s.Serialize(w, OffsetPosition);
                s.Serialize(w, OffsetRotation);
            }
            // else
            // {
            //     w.WriteNull();
            //     w.WriteNull();
            // }
        }
    }
}
