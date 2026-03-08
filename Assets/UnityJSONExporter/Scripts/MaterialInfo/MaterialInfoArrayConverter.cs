using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnityJSONExporter
{
    public class MaterialInfoArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MaterialInfo) || objectType.IsSubclassOf(typeof(MaterialInfo));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartArray();

            if (value is LitMaterialInfo litMaterial)
            {
                writer.WriteValue((int)litMaterial.Type);
                writer.WriteValue(litMaterial.Color);
                serializer.Serialize(writer, litMaterial.Tiling);
                writer.WriteValue(litMaterial.Metallic);
                writer.WriteValue(litMaterial.Roughness);
                writer.WriteValue(litMaterial.EmissionColor);
                writer.WriteValue(litMaterial.ReceiveShadow);
            }
            else if (value is UnlitMaterialInfo unlitMaterial)
            {
                writer.WriteValue((int)unlitMaterial.Type);
                writer.WriteValue(unlitMaterial.Color);
            }
            else if (value is MaterialInfo baseMaterial)
            {
                writer.WriteValue((int)baseMaterial.Type);
            }

            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Deserialization is not supported for MaterialInfo");
        }

        public override bool CanRead => false;
    }
}
