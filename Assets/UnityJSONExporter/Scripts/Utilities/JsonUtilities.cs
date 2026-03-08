using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityJSONExporter
{
    public static class JsonUtilities
    {
        /// <summary>
        /// JSON出力時の浮動小数点数の丸め精度（小数点以下の桁数）
        /// 3桁 = 0.001まで許容
        /// -1 = 丸めを無効化
        /// </summary>
        public const int DefaultRoundingDecimals = 2;

        /// <summary>
        /// 高精度な丸め精度（PostProcess/AtmosphereSkybox用）
        /// 4桁 = 0.0001まで許容
        /// </summary>
        public const int HighPrecisionRoundingDecimals = 4;

        /// <summary>
        /// 高精度（HighPrecisionRoundingDecimals）を要求するプロパティ名のセット
        /// PostProcessとAtmosphereSkybox関連のプロパティを自動収集
        /// </summary>
        private static readonly System.Lazy<System.Collections.Generic.HashSet<string>> HighPrecisionPropertyNames =
            new System.Lazy<System.Collections.Generic.HashSet<string>>(() =>
        {
            var set = new System.Collections.Generic.HashSet<string>();

            // --- PostProcessControllerComponentInfoの定数を自動収集 ---
            var ppType = typeof(PostProcessControllerComponentInfo);
            var constantFields = ppType.GetFields(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Static
            );

            foreach (var field in constantFields)
            {
                // XXX_PROPERTY_NAME という定数フィールドから値を取得
                if (field.Name.EndsWith("_PROPERTY_NAME") && field.GetValue(null) is string propName)
                {
                    set.Add(propName);

                    // Vector3型のプロパティの場合、サブプロパティ(.x, .y, .z)も高精度対象に追加
                    // 例: "sss_js" → "sss_js.x", "sss_js.y", "sss_js.z"
                    if (field.Name.Contains("JITTER_SIZE") || field.Name.Contains("RAY_JITTER_SIZE"))
                    {
                        set.Add(propName + ".x");
                        set.Add(propName + ".y");
                        set.Add(propName + ".z");
                    }

                    // 一旦外す
                    // // Color型のプロパティの場合、サブプロパティ(.r, .g, .b, .a)も高精度対象に追加
                    // // 例: "fg_c" → "fg_c.r", "fg_c.g", "fg_c.b", "fg_c.a"
                    // if (field.Name.Contains("COLOR"))
                    // {
                    //     set.Add(propName + ".r");
                    //     set.Add(propName + ".g");
                    //     set.Add(propName + ".b");
                    //     set.Add(propName + ".a");
                    // }
                }
            }

            // --- AtmosphereSkyboxControllerのプロパティを手動追加 ---
            // JsonProperty属性で定義されているプロパティ名
            var atmosphereProps = new[]
            {
                "r_on",  // RenderMeshEnabled
                "i_b",   // BaseIntensity
                "i_s",   // SpecularIntensity
                "sp",    // SunPosition (Vector3)
                "si",    // SunIntensity
                "pr",    // PlanetRadius
                "ar",    // AtmosphereRadius
                "ca",    // CameraAltitude
                "rc",    // RayleighCoefficient (Vector3)
                "rsh",   // RayleighScaleHeight
                "mc",    // MieCoefficient
                "msh",   // MieScaleHeight
                "msd"    // MieScatteringDirection
            };

            foreach (var prop in atmosphereProps)
            {
                set.Add(prop);
            }

            // Vector3型のプロパティのサブプロパティも高精度対象に追加
            set.Add("sp.x");
            set.Add("sp.y");
            set.Add("sp.z");
            set.Add("rc.x");
            set.Add("rc.y");
            set.Add("rc.z");

            return set;
        });

        /// <summary>
        /// プロパティ名に応じた丸め桁数を取得
        /// PostProcess/AtmosphereSkybox関連は高精度（4桁）、それ以外はデフォルト（3桁）
        /// </summary>
        public static int GetRoundingDecimalsForProperty(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return DefaultRoundingDecimals;
            }

            return HighPrecisionPropertyNames.Value.Contains(propertyName)
                ? HighPrecisionRoundingDecimals
                : DefaultRoundingDecimals;
        }

        /// <summary>
        /// 指定した精度で丸めた浮動小数点数を書き出します。
        /// decimals=-1の場合は丸めを行いません。
        /// 例: value=0.22000001, decimals=4 => 0.22
        /// </summary>
        public static void WriteRoundedValue(JsonWriter writer, float value, int decimals)
        {
            // CustomJsonWriter の場合は CurrentRoundingDecimals を設定してから WriteValue を呼ぶ
            // そうすることで CustomJsonWriter が正しい精度で丸めてくれる（二重丸めを回避）
            if (writer is CustomJsonWriter customWriter)
            {
                customWriter.CurrentRoundingDecimals = decimals >= 0 ? decimals : DefaultRoundingDecimals;
                writer.WriteValue(value);
            }
            else
            {
                // 通常の JsonWriter の場合は自分で丸める
                if (decimals < 0)
                {
                    writer.WriteValue(value);
                }
                else
                {
                    float rounded = (float)System.Math.Round(value, decimals);
                    writer.WriteValue(rounded);
                }
            }
        }

        public static string ResolveJsonProperty<T>(
            T component,
            string targetField
            // bool needsMinify
        ) where T : Component
        {
            // if (!needsMinify)
            // {
            //     return targetField;
            // }

            var type = typeof(T);
            var fields = type.GetFields();
            // for debug
            LoggerProxy.Log($"[JsonUtilities.ResolveJsonProperty] ===================================");
            LoggerProxy.Log($"[JsonUtilities.ResolveJsonProperty] type: {type}, target field: {targetField}, fields: {fields.Length}");

            // 1: オブジェクト名
            // 2: フィールド
            var fieldObjectPattern = @"(^.*?)\.(x|y|z|w|r|g|b|a)$";

            // オブジェクトの場合。color, vectorなど
            var matchFieldObject = System.Text.RegularExpressions.Regex.Match(targetField, fieldObjectPattern);

            LoggerProxy.Log($"[JsonUtilities.ResolveJsonProperty] field: {targetField}, match object field: {matchFieldObject.Success}");

            // 一致するfieldを探索
            foreach (var field in fields)
            {
                // 宣言されているjsonpropertryを取得
                var jsonProperty = field
                    .GetCustomAttributes(typeof(JsonPropertyAttribute), false)
                    .Cast<JsonPropertyAttribute>()
                    .FirstOrDefault();
                var fieldName = field.Name;

                if (jsonProperty == null)
                {
                    continue;
                }

                // for debug
                // if (matchFieldObject.Success)
                // {
                //     LoggerProxy.Log($"[JsonUtilities.ResolveJsonProperty] field: {fieldName}, match object field: // {matchFieldObject.Groups[1].Value}, json property: {jsonProperty.PropertyName}");
                // }
                
                // オブジェクトの場合
                if (
                    matchFieldObject.Success
                    && matchFieldObject.Groups[1].Value == fieldName
                )
                {
                    return $"{jsonProperty.PropertyName}.{matchFieldObject.Groups[2].Value}";
                }

                // 完全一致していたらそのまま変換. float など
                if (field.Name == targetField)
                {
                    return jsonProperty.PropertyName;
                }
            }

            // fallback
            return targetField;
        }

        /// <summary>
        /// 配列の後方にある連続する0要素を削除し、各要素を指定精度で丸めます。
        /// decimals=-1の場合は丸めを行いません。
        /// 例: [1.11119,1.00009,0,0], decimals=4 => [1.1112,1.0001]
        /// 例: [1,1,0,0] => [1,1], [0,0,0,0] => [], [0,1,1,0] => [0,1,1]
        /// </summary>
        public static void WriteArrayWithoutTrailingZeros(
            JsonWriter writer,
            System.Collections.Generic.List<float> values,
            int decimals)
        {
            writer.WriteStartArray();

            if (values != null && values.Count > 0)
            {
                // 後方から0でない要素のインデックスを探す
                int lastNonZeroIndex = values.Count - 1;
                while (lastNonZeroIndex >= 0 && values[lastNonZeroIndex] == 0f)
                {
                    lastNonZeroIndex--;
                }

                // 0でない要素まで出力（全て0の場合は空配列）
                for (int i = 0; i <= lastNonZeroIndex; i++)
                {
                    WriteRoundedValue(writer, values[i], decimals);
                }
            }

            writer.WriteEndArray();
        }
    }
}
