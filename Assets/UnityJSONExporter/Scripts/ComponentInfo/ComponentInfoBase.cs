using System.Collections.Generic;
using System.Linq;
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
    [JsonConverter(typeof(ComponentInfoBaseConverter))]
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
    /// ComponentInfoBaseを配列形式でシリアライズするためのJsonConverter
    /// 配列形式: [type, {...残りのプロパティ}]
    /// </summary>
    public class ComponentInfoBaseConverter : JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            return typeof(ComponentInfoBase).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var componentInfo = (ComponentInfoBase)value;
            var objectType = value.GetType();

            // CustomJsonWriterの丸め桁数を一時的に変更
            var customWriter = writer as CustomJsonWriter;
            int originalDecimals = customWriter?.CurrentRoundingDecimals ?? JsonUtilities.DefaultRoundingDecimals;

            // PostProcessControllerComponentInfoの場合は高精度（4桁）を維持
            if (objectType == typeof(PostProcessControllerComponentInfo))
            {
                if (customWriter != null)
                {
                    customWriter.CurrentRoundingDecimals = JsonUtilities.HighPrecisionRoundingDecimals;
                }
            }

            writer.WriteStartArray();

            // index 0: type
            writer.WriteValue((int)componentInfo.Type);

            // index 1: check if this component has a "Data" field or "LightData" property
            object arrayData = null;

            // Check for "Data" field
            var dataField = objectType.GetField("Data", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (dataField != null && dataField.FieldType == typeof(object[]))
            {
                arrayData = dataField.GetValue(value);
            }

            // If no Data field, check for "LightData" property
            if (arrayData == null)
            {
                var lightDataProperty = objectType.GetProperty("LightData", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (lightDataProperty != null && lightDataProperty.PropertyType == typeof(object[]))
                {
                    arrayData = lightDataProperty.GetValue(value);
                }
            }

            if (arrayData != null)
            {
                // If Data field or LightData property exists, serialize it directly as an array
                serializer.Serialize(writer, arrayData);
            }
            else
            {
                // Otherwise, write as object with properties (original behavior)
                writer.WriteStartObject();

                // Type以外のすべてのフィールドとプロパティを取得して書き出し
                var fields = objectType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var properties = objectType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                // フィールドを処理
                foreach (var field in fields)
                {
                    // Typeフィールドはスキップ
                    if (field.Name == "Type")
                    {
                        continue;
                    }

                    // JsonPropertyアトリビュートからプロパティ名を取得
                    var jsonPropertyAttr = field.GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                        .FirstOrDefault() as JsonPropertyAttribute;

                    // プロパティ名を決定（アトリビュートがあればそれを使う、なければフィールド名をそのまま使う）
                    var propertyName = jsonPropertyAttr != null ? jsonPropertyAttr.PropertyName : field.Name;

                    // フィールド値を取得
                    var fieldValue = field.GetValue(value);

                    // フィールドを書き出し
                    writer.WritePropertyName(propertyName);
                    serializer.Serialize(writer, fieldValue);
                }

                // プロパティを処理
                foreach (var property in properties)
                {
                    // Typeプロパティはスキップ
                    if (property.Name == "Type")
                    {
                        continue;
                    }

                    // JsonPropertyアトリビュートからプロパティ名を取得
                    var jsonPropertyAttr = property.GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                        .FirstOrDefault() as JsonPropertyAttribute;

                    // プロパティ名を決定（アトリビュートがあればそれを使う、なければプロパティ名をそのまま使う）
                    var propertyName = jsonPropertyAttr != null ? jsonPropertyAttr.PropertyName : property.Name;

                    // プロパティ値を取得
                    var propertyValue = property.GetValue(value);

                    // プロパティを書き出し
                    writer.WritePropertyName(propertyName);
                    serializer.Serialize(writer, propertyValue);
                }

                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            // 元の桁数に戻す
            if (customWriter != null)
            {
                customWriter.CurrentRoundingDecimals = originalDecimals;
            }
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            // デシリアライズは未実装（必要に応じて実装）
            throw new System.NotImplementedException("ComponentInfoBase deserialization is not implemented");
        }
    }
}
