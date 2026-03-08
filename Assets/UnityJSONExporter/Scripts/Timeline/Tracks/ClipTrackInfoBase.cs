using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    [JsonConverter(typeof(ClipTrackInfoBaseConverter))]
    public class ClipTrackInfoBase : TrackInfoBase
    {
        public string TargetName;
        public List<ClipInfoBase> Clips = new List<ClipInfoBase>();

        public ClipTrackInfoBase(string name, TrackInfoType type, string targetName) : base(name, type)
        {
            TargetName = targetName;
        }
    }

    public class ClipTrackInfoBaseConverter : JsonConverter<ClipTrackInfoBase>
    {
        public override void WriteJson(JsonWriter writer, ClipTrackInfoBase value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.Name);
            writer.WriteValue((int)value.Type);
            writer.WriteValue(value.TargetName);
            serializer.Serialize(writer, value.Clips);
            writer.WriteEndArray();
        }

        public override ClipTrackInfoBase ReadJson(JsonReader reader, Type objectType, ClipTrackInfoBase existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
