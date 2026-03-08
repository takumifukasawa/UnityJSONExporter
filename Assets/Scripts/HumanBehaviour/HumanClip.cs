// using UnityEngine;
// using UnityEngine.Playables;
// using UnityEngine.Timeline;
// 
// [System.Serializable]
// public class HumanClip : PlayableAsset, ITimelineClipAsset
// {
//     public HumanBehaviour template = new HumanBehaviour();
// 
//     public ClipCaps clipCaps
//     {
//         get { return ClipCaps.Blending; }
//     }
// 
//     // public ExposedReference<Transform> Target;
// 
//     public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
//     {
//         var playable = ScriptPlayable<HumanBehaviour>.Create(graph, template);
//         return playable;
//     }
// }
// 
