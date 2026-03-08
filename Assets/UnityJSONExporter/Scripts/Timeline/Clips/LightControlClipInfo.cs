using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    /// <summary>
    /// 
    /// </summary>
    public class LightControlClipInfo : ClipInfoBase
    {
        [JsonIgnore]
        // [JsonProperty(PropertyName = "b")]
        public List<ClipBinding> Bindings = new List<ClipBinding>();

        public LightControlClipInfo(string name, float s, float d, TimelineClip.ClipExtrapolation clipExtrapolation) : base(name, ClipInfoType.LightControlClip, s, d, clipExtrapolation)
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
