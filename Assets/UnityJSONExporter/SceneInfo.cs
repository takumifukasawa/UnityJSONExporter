using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.UI;
using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    // ---------------------------------------------------------------------------------------------
    // public
    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class SceneInfo
    {
        [JsonProperty(PropertyName = "n")]
        public string Name;

        // public Hierarchy Hierarchy;
        [JsonProperty(PropertyName = "o")]
        public List<ObjectInfo> Objects;
    }

    // /// <summary>
    // /// 
    // /// </summary>
    // [System.Serializable]
    // public class Hierarchy
    // {
    //     public List<ObjectInfo> Objects;
    // }

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class ObjectInfo
    {
        // [JsonIgnore]
        // public GameObject InternalGameObject;
        [JsonProperty(PropertyName = "n")]
        public string Name;

        [JsonProperty(PropertyName = "t")]
        public TransformInfo Transform;

        [JsonProperty(PropertyName = "c")]
        public List<ComponentInfoBase> Components = new List<ComponentInfoBase>();

        [JsonProperty(PropertyName = "o")]
        public List<ObjectInfo> Children = new List<ObjectInfo>();

        public ObjectInfo(GameObject obj, ConvertAxis axis)
        {
            Name = obj.name;
            var localPosition = obj.transform.localPosition;
            var localRotation = obj.transform.localRotation;
            var localScale = obj.transform.localScale;

            Transform = new TransformInfo()
            {
                LocalPosition = TransformConverter.ConvertPosition(axis, localPosition),
                LocalRotation = TransformConverter.ConvertRotation(axis, localRotation.eulerAngles),
                LocalScale = TransformConverter.ConvertScale(axis, localScale)
            };
        }

        public void AddChild(ObjectInfo child)
        {
            Children.Add(child);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Vector3Info
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3Info(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3Info(Vector3 v)
        {
            X = v.x;
            Y = v.y;
            Z = v.z;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TransformInfo
    {
        [JsonProperty(PropertyName = "lp")]
        public Vector3Info LocalPosition;
        [JsonProperty(PropertyName = "lr")]
        public Vector3Info LocalRotation;
        [JsonProperty(PropertyName = "ls")]
        public Vector3Info LocalScale;
    }
}
