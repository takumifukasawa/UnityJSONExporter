using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectMoveAndLookAtClipInfo : ClipInfoBase
    {
        [JsonIgnore]
        // [JsonProperty(PropertyName = "b")]
        // [CanBeNull]
        public List<ClipBinding> Bindings = null;

        public ObjectMoveAndLookAtClipInfo(string name, float s, float d, TimelineClip.ClipExtrapolation clipExtrapolation) : base(name, ClipInfoType.ObjectMoveAndLookAtClip, s, d, clipExtrapolation)
        {
        }

        public void AddBinding(ClipBinding binding)
        {
            if (Bindings == null) Bindings = new List<ClipBinding>();
            Bindings.Add(binding);
        }

        protected internal override void WriteSubclassArray(JsonWriter w, JsonSerializer s)
        {
            // [親の情報, bindings]

            if (Bindings != null && Bindings.Count > 0) s.Serialize(w, Bindings);
            else w.WriteNull();
        }
    }
}
