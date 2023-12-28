using UnityEngine;
using Newtonsoft.Json;

namespace UnityJSONExporter
{
    /// <summary>
    /// NOTE: materialは1つまで対応
    /// </summary>
    [System.Serializable]
    public class MeshRendererComponentInfo : ComponentInfoBase
    {
        [JsonProperty(PropertyName = "mn")]
        public string MaterialName;

        public MeshRendererComponentInfo(MeshRenderer meshRenderer) : base(ComponentType.MeshRenderer)
        {
            MaterialName = meshRenderer.sharedMaterial.name;
        }
    }
}
