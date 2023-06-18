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
            var r = Mathf.RoundToInt(color.r * 255);
            var g = Mathf.RoundToInt(color.g * 255);
            var b = Mathf.RoundToInt(color.b * 255);
            var a = Mathf.RoundToInt(color.a * 255);

            // for debug
            // Debug.Log(r.ToString("X2"));
            // Debug.Log(g.ToString("X2"));
            // Debug.Log(b.ToString("X2"));
            // Debug.Log(a.ToString("X2"));
            
            string hexColor = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);

            return hexColor;
        }
    }
}
