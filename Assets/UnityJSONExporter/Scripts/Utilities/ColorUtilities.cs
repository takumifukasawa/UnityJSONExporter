using UnityEngine;

namespace UnityJSONExporter
{
    /// <summary>
    /// 
    /// </summary>
    public static class ColorUtilities
    {
        public static string ConvertColorToHexString(Color color)
        {
            var r = Mathf.RoundToInt(color.r * 255);
            var g = Mathf.RoundToInt(color.g * 255);
            var b = Mathf.RoundToInt(color.b * 255);
            var a = Mathf.RoundToInt(color.a * 255);

            // for debug
            // LoggerProxy.Log(r.ToString("X2"));
            // LoggerProxy.Log(g.ToString("X2"));
            // LoggerProxy.Log(b.ToString("X2"));
            // LoggerProxy.Log(a.ToString("X2"));

            var hexColor = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
            return hexColor;
        }

        public static string ConvertEmissionToHexString(Color hdr)
        {
            // 強さ（peak）
            var intensity = Mathf.Max(hdr.r, Mathf.Max(hdr.g, hdr.b));

            // 色を0..1に正規化（強さで割る）
            Color color01 = (intensity > 0f) ? hdr / intensity : Color.black;

            // sRGB で出力（Web/UI向け）
            color01 = color01.gamma;

            // 0..255 へ
            var r = Mathf.RoundToInt(color01.r * 255);
            var g = Mathf.RoundToInt(color01.g * 255);
            var b = Mathf.RoundToInt(color01.b * 255);
            var i = Mathf.RoundToInt(intensity * 25.5f); // 強さを0..10に圧縮

            var hexColor = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, i);
            return hexColor;
        }
    }
}