using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    public class TrackInfoBase
    {
        [JsonProperty(PropertyName = "n")]
        public string Name;
        
        [JsonProperty(PropertyName = "t")]
        public TrackInfoType Type;

        public TrackInfoBase(string name, TrackInfoType type)
        {
            Name = name;
            Type = type;
        }
    }
}
