// using UnityEngine;
// using UnityEngine.Playables;
// using UnityEngine.Timeline;

// [TrackColor(0.2f, 0.4f, 0.6f)]
// [TrackClipType(typeof(HumanClip))]
// [TrackBindingType(typeof(HumanController))]
// public class HumanTrack : TrackAsset
// {
//     public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
//     {
//         return ScriptPlayable<HumanMixerBehaviour>.Create(graph, inputCount);
//     }
// 
//     public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
//     {
// #if UNITY_EDITOR
//         HumanController trackBinding = director.GetGenericBinding(this) as HumanController;
//         if (trackBinding == null)
//             return;
//         driver.AddFromName<HumanController>(trackBinding.gameObject, HumanController.LeftShoulderRotationPropertyName);
// #endif
//         base.GatherProperties(director, driver);
//     }
// }
