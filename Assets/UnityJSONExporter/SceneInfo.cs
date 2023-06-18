using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityJSONExporter
{
    [System.Serializable]
    public class SceneInfo
    {
        public string Name;
        // public Hierarchy Hierarchy;
        public List<ObjectInfo> Objects;
    }

    [System.Serializable]
    public class Hierarchy
    {
        public List<ObjectInfo> Objects;
    }


    [System.Serializable]
    public class ObjectInfo
    {
        // [JsonIgnore]
        // public GameObject InternalGameObject;
        public string Name;
        public List<ComponentInfoBase> Components = new List<ComponentInfoBase>();
        public List<ObjectInfo> Children = new List<ObjectInfo>();

        public ObjectInfo(GameObject obj)
        {
            Name = obj.name;
        }

        public void AddChild(ObjectInfo child)
        {
            Children.Add(child);
        }
    }


    [System.Serializable]
    public class ComponentInfoBase
    {
        public string Type;
    }

    public enum ComponentType
    {
        Light,
    }

    // [System.Serializable]
    // public class ComponentInfo
    // {
    //     public ComponentType ComponentType;

    //     // public static ComponentBase BuildComponentInfo()
    //     // {
    //     //     return new LightComponent();
    //     // }
    // }
    [System.Serializable]
    public class LightComponentInfo : ComponentInfoBase
    {
        public string LightType;
        public string Color;
        
        public LightComponentInfo(Light light)
        {
            Type = ComponentType.Light.ToString();
            LightType = light.type.ToString();
            Color = ColorUtilities.ConvertColorToHexString(light.color);
        }
    }

    public static class ColorUtilities
    {
        public static string ConvertColorToHexString(Color color)
        {
            return $"0x{color.r.ToString("X2")}{color.g.ToString("X2")}{color.b.ToString("X2")}";
        }
    }
}
