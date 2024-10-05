using System.Collections.Generic;
using Newtonsoft.Json;

namespace UnityJSONExporter
{
    public class DefaultTrackInfo : TrackInfoBase
    {
        [JsonProperty(PropertyName = "cs")]
        public List<ClipInfoBase> Clips = new List<ClipInfoBase>();

        [JsonProperty(PropertyName = "tn")]
        public string TargetName;

        public DefaultTrackInfo(TrackInfoType type, string targetName, List<ClipInfoBase> clips) : base(type)
        {
            TargetName = targetName;
            Clips = clips;
        }
    }
}
