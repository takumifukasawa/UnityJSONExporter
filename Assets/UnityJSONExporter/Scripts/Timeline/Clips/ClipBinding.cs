using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityJSONExporter
{
    [JsonConverter(typeof(ClipBindingAsArrayConverter))]
    public class ClipBinding
    {
        [JsonIgnore]
        // [JsonProperty(PropertyName = "n")]
        public string PropertyName;

        // for obj
        // [JsonProperty(PropertyName = "k")]
        // public List<ClipKeyframe> Keyframes = new List<ClipKeyframe>();

        // for arr
        [JsonIgnore]
        // [JsonProperty(PropertyName = "k")]
        public List<List<float>> Keyframes = new List<List<float>>();

        public void AddKeyframe(float t, float v, float it, float ot)
        {
            // Keyframes.Add(new ClipKeyframe(t, v, it, ot));
            Keyframes.Add(new List<float> { t, v, it, ot });
        }
    }

    public sealed class ClipBindingAsArrayConverter : JsonConverter<ClipBinding>
    {
        public override void WriteJson(JsonWriter w, ClipBinding value, JsonSerializer serializer)
        {
            // [ name, keyframes ]
            w.WriteStartArray();
            w.WriteValue(value.PropertyName);

            var kf = value.Keyframes ?? new List<List<float>>();
            // 全てのキーフレーム要素が0かチェック（WriteArrayWithoutTrailingZerosで空配列になるケース）
            bool allEmpty = kf.Count > 0 && kf.All(row => row.All(v => v == 0f));

            // PropertyNameに応じた丸め桁数を取得
            int decimals = JsonUtilities.GetRoundingDecimalsForProperty(value.PropertyName);

            // 空でない場合のみkeyframes配列を出力
            if (!allEmpty)
            {
                w.WriteStartArray(); // keyframes
                foreach (var row in kf)
                {
                    // PropertyNameに応じた桁数を使用（PostProcess/AtmosphereSkybox関連は高精度、それ以外はデフォルト）
                    JsonUtilities.WriteArrayWithoutTrailingZeros(w, row, decimals);
                }
                w.WriteEndArray();
            }

            w.WriteEndArray();
        }

        public override bool CanRead => false;

        public override ClipBinding ReadJson(JsonReader reader, Type objectType, ClipBinding existingValue, bool hasExistingValue, JsonSerializer serializer)
            => throw new NotSupportedException("Deserialization is not supported for ClipBindingAsArrayConverter.");
    }
}
