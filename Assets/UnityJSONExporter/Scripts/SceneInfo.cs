using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityJSONExporter
{
    // ---------------------------------------------------------------------------------------------
    // public
    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// 一個だけプロパティを持たせる
    /// </summary>
    [System.Serializable]
    public class SceneInfo
    {
        // [JsonProperty(PropertyName = "n")]
        // public string Name;

        [JsonProperty(PropertyName = "o")]
        public List<ObjectInfo> Objects;
    }



    // /// <summary>
    // /// 
    // /// </summary>
    // public class TransformInfo
    // {
    //     [JsonProperty(PropertyName = "lp")]
    //     public Vector3Info LocalPosition;

    //     [JsonProperty(PropertyName = "lr")]
    //     public Vector4Info LocalRotation;

    //     [JsonProperty(PropertyName = "ls")]
    //     public Vector3Info LocalScale;
    // }


}
