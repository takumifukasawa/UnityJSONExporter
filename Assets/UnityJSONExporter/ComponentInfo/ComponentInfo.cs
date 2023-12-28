using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class ComponentInfoBase
    {
        [JsonProperty(PropertyName = "t")]
        public ComponentType Type;

        public ComponentInfoBase(ComponentType type)
        {
            Type = type;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ComponentType
    {
        PlayableDirector, // 0
        Light, // 1
        Camera, // 2
        MeshRenderer, // 3
        MeshFilter, // 4
    }
}
