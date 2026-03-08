using System;
using UnityEngine;
using Newtonsoft.Json;
using UnityJSONExporter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[System.Serializable]
// [JsonConverter(typeof(ObjMoveLookAt_AsArrayConverter))]
public class ObjectMoveAndLookAtControllerComponentInfo : ComponentInfoBase
{
    // [JsonIgnore]
    [JsonProperty(PropertyName = "lp")]
    public RawVector3 LocalPosition;

    // [JsonIgnore]
    [JsonProperty(PropertyName = "uv")]
    public RawVector3 UpVector;

    // LookAtTargetNameは、LookAtTargetの名前を保持する
    // [JsonIgnore]
    [JsonProperty(PropertyName = "tn")]
    public string LookAtTargetName;

    public ObjectMoveAndLookAtControllerComponentInfo(ObjectMoveAndLookAtController objectMoveAndLookAtController) : base(ComponentType.ObjectMoveAndLookAtController)
    {
        LocalPosition = objectMoveAndLookAtController.LocalPosition;
        UpVector = objectMoveAndLookAtController.UpVector;
        LookAtTargetName = objectMoveAndLookAtController.LookAtTarget.name;
    }
}


// 配列化コンバータ
public sealed class ObjMoveLookAt_AsArrayConverter
    : JsonConverter<ObjectMoveAndLookAtControllerComponentInfo>
{
    public override void WriteJson(JsonWriter w,
        ObjectMoveAndLookAtControllerComponentInfo value,
        JsonSerializer serializer)
    {
        // [ type, name, px, py, pz ]
        w.WriteStartArray();
        w.WriteValue(value.Type);
        w.WriteValue(value.LookAtTargetName);
        JsonUtilities.WriteRoundedValue(w, value.LocalPosition.x, JsonUtilities.DefaultRoundingDecimals);
        JsonUtilities.WriteRoundedValue(w, value.LocalPosition.y, JsonUtilities.DefaultRoundingDecimals);
        JsonUtilities.WriteRoundedValue(w, value.LocalPosition.z, JsonUtilities.DefaultRoundingDecimals);
        w.WriteEndArray();
    }

    // もし読み込み不要なら CanRead=false & 例外でOK
    public override bool CanRead => false;

    public override ObjectMoveAndLookAtControllerComponentInfo ReadJson(
        JsonReader r, Type t, ObjectMoveAndLookAtControllerComponentInfo existingValue,
        bool hasExistingValue, JsonSerializer serializer)
        => throw new NotSupportedException("Deserialization not supported.");
}
