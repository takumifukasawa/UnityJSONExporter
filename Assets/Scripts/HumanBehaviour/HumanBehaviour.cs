// using UnityEngine;
// using UnityEngine.Playables;
// 
// [System.Serializable]
// public class HumanBehaviour : PlayableBehaviour
// {
//     // HumanControllerと同じにしておく
//     // property name を string で指定する際に分かりやすくするため
// 
//     // public Vector3 LocalPosition;
//     // public ExposedReference<Transform> LookAtTarget;
// 
//     public Vector3 LeftShoulderRotation;
// 
//     public override void ProcessFrame(Playable playable, FrameData info, object playerData)
//     {
//         var humanController = playerData as HumanController;
// 
//         var director = playable.GetGraph().GetResolver() as PlayableDirector;
//         // var lookAtTarget = LookAtTarget.Resolve(director);
// 
//         // for debug
//         // Debug.Log($"[HumanBehaviour.ProcessFrame] humanController: {humanController}, LeftShoulderRotation: {LeftShoulderRotation}");
//         
//         humanController.SetLeftShoulderRotation(LeftShoulderRotation);
//     }
// }
// 
