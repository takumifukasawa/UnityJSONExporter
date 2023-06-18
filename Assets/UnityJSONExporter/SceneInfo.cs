using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityJSONExporter
{
    [System.Serializable]
    public class SceneInfo
    {
        public string Name;
        public Hierarchy Hierarchy;
    }

    [System.Serializable]
    public class Hierarchy
    {
        public List<ObjectInfo> Objects;
    }


    [System.Serializable]
    public class ObjectInfo
    {
        public string Name;
        public List<ComponentInfoBase> Components;
    }


    [System.Serializable]
    public class ComponentInfoBase
    {
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
        public string ComponentType;
        public string LightType;
        
        public LightComponentInfo(Light light)
        {
            ComponentType = UnityJSONExporter.ComponentType.Light.ToString();
            LightType = light.type.ToString();
        }
    }
}
