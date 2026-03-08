using Newtonsoft.Json;
using UnityJSONExporter;

public class GBufferMaterialControllerComponentInfo : ComponentInfoBase
{
    public const string DATA_PROPERTY_NAME = "d";

    [JsonProperty(PropertyName = DATA_PROPERTY_NAME)]
    public object[] Data;

    public GBufferMaterialControllerComponentInfo(GBufferMaterialController gBufferMaterialController) : base(ComponentType.GBufferMaterialController)
    {
        Data = new object[0];
    }
}
