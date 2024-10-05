using System.Collections.Generic;
using Newtonsoft.Json;

namespace UnityJSONExporter
{
    public enum ClipInfoType
    {
        None, // 0
        AnimationClip, // 1
        LightControlClip, // 2
        ActivationControlClip, // 3
        SignalEmitter // 4
    }
    
    public class ClipKeyframe
    {
        [JsonProperty(PropertyName = "t")]
        public float Time;

        [JsonProperty(PropertyName = "v")]
        public float Value;

        [JsonProperty(PropertyName = "i")]
        public float InTangent;

        [JsonProperty(PropertyName = "o")]
        public float OutTangent;
    }

    public class ClipBinding
    {
        [JsonProperty(PropertyName = "n")]
        public string PropertyName;

        [JsonProperty(PropertyName = "k")]
        public List<ClipKeyframe> Keyframes = new List<ClipKeyframe>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class ClipInfoBase
    {
        [JsonProperty(PropertyName = "t")]
        public ClipInfoType Type;

        [JsonProperty(PropertyName = "s")]
        public float Start;

        [JsonProperty(PropertyName = "d")]
        public float Duration;

        public ClipInfoBase(ClipInfoType type, float s, float d)
        {
            Type = type;
            Start = s;
            Duration = d;
        }
    }
}
