using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(ClipInfoBaseAsArrayConverter))]
    public class ClipInfoBase
    {
        [JsonIgnore]
        public string Name;
        
        [JsonIgnore]
        // [JsonProperty(PropertyName = "t")]
        public ClipInfoType Type;

        [JsonIgnore]
        // [JsonProperty(PropertyName = "s")]
        public float Start;

        [JsonIgnore]
        // [JsonProperty(PropertyName = "d")]
        public float Duration;

        [JsonIgnore]
        public TimelineClip.ClipExtrapolation PostExtrapolation = TimelineClip.ClipExtrapolation.None;

        public ClipInfoBase(string name, ClipInfoType type, float s, float d, TimelineClip.ClipExtrapolation postExtrapolation)
        {
            Name = name;
            Type = type;
            Start = s;
            Duration = d;
            PostExtrapolation = postExtrapolation;
        }

        protected internal virtual void WriteSubclassArray(JsonWriter w, JsonSerializer s)
        {
        }
    }


    public sealed class ClipInfoBaseAsArrayConverter : JsonConverter<ClipInfoBase>
    {
        public override void WriteJson(JsonWriter w, ClipInfoBase v, JsonSerializer s)
        {
            w.WriteStartArray();
            w.WriteValue(v.Name);
            w.WriteValue((int)v.Type);
            JsonUtilities.WriteRoundedValue(w, v.Start, JsonUtilities.DefaultRoundingDecimals);
            JsonUtilities.WriteRoundedValue(w, v.Duration, JsonUtilities.DefaultRoundingDecimals);
            w.WriteValue((int)v.PostExtrapolation);

            v.WriteSubclassArray(w, s); // ← サブクラスの追記
            w.WriteEndArray();
        }

        public override bool CanRead => false;

        public override ClipInfoBase ReadJson(JsonReader r, Type t, ClipInfoBase ex, bool hasEx, JsonSerializer s)
            => throw new NotSupportedException();
    }
}
