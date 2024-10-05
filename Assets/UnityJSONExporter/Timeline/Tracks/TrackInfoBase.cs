using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;


namespace UnityJSONExporter
{
   
    /// <summary>
    /// 
    /// </summary>
    public class TrackInfoBase
    {
        [JsonProperty(PropertyName = "t")]
        public TrackInfoType Type;

        public TrackInfoBase(TrackInfoType type)
        {
            Type = type;
        }
    }
}
