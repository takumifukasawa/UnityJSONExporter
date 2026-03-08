using System.Collections.Generic;
using Newtonsoft.Json;

namespace UnityJSONExporter
{
    public class DefaultTrackInfo : ClipTrackInfoBase
    {
        public DefaultTrackInfo(string name, TrackInfoType type, string targetName) : base(name, type, targetName)
        {
        }
    }
}
