using UnityEngine.Timeline;

namespace UnityJSONExporter
{
    public class ActivationControlClipInfo : ClipInfoBase
    {
        public ActivationControlClipInfo(string name, float s, float d, TimelineClip.ClipExtrapolation clipExtrapolation) : base(name, ClipInfoType.ActivationControlClip, s, d, clipExtrapolation)
        {
        }
    }
}
