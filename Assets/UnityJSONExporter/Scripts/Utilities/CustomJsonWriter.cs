using System.IO;
using Newtonsoft.Json;

namespace UnityJSONExporter
{
    public class CustomJsonWriter : JsonTextWriter
    {
        /// <summary>
        /// 現在の丸め桁数（動的に変更可能）
        /// デフォルトはJsonUtilities.DefaultRoundingDecimals（2桁）
        /// </summary>
        public int CurrentRoundingDecimals { get; set; } = JsonUtilities.DefaultRoundingDecimals;

        public CustomJsonWriter(TextWriter textWriter) : base(textWriter)
        {
        }

        public override void WriteValue(float value)
        {
            // CurrentRoundingDecimalsを使用して丸める
            float rounded = (float)System.Math.Round(value, CurrentRoundingDecimals);

            // 丸めた結果が整数なら整数として出力
            if (rounded == (int)rounded)
            {
                base.WriteValue((int)rounded);
            }
            else
            {
                base.WriteValue(rounded);
            }
        }

        public override void WriteValue(double value)
        {
            // CurrentRoundingDecimalsを使用して丸める
            double rounded = System.Math.Round(value, CurrentRoundingDecimals);

            // 丸めた結果が整数なら整数として出力
            if (rounded == (int)rounded)
            {
                base.WriteValue((int)rounded);
            }
            else
            {
                base.WriteValue(rounded);
            }
        }
    }
}
