// using UnityEngine;
// using UnityEngine.Playables;
// using UnityJSONExporter;
// 
// public class HumanMixerBehaviour : PlayableBehaviour
// {
//     // Vector3 m_DefaultLocalPosition;
//     // Transform m_DefaultLookAtTarget;
// 
//     // Vector3 m_AssignedLocalPosition;
//     // Transform m_AssignedLookAtTarget;
// 
//     private Vector3 m_DefaultLeftShoulderRotation;
// 
//     Vector3 m_AssignedLeftShoulderRotation;
// 
//     HumanController m_TrackBinding;
//     bool m_FirstFrameHappened;
// 
//     public override void ProcessFrame(Playable playable, FrameData info, object playerData)
//     {
//         var director = playable.GetGraph().GetResolver() as PlayableDirector;
// 
//         m_TrackBinding = playerData as HumanController;
// 
//         if (m_TrackBinding == null)
//             return;
// 
//         if (!m_FirstFrameHappened)
//         {
//             // m_DefaultLocalPosition = RawVector3.toVector3(m_TrackBinding.LocalPosition);
//             // m_DefaultLookAtTarget = m_TrackBinding.LookAtTarget;
//             m_DefaultLeftShoulderRotation = RawVector3.toVector3(m_TrackBinding.LeftShoulderRotation);
//             m_FirstFrameHappened = true;
//         }
// 
//         int inputCount = playable.GetInputCount();
// 
//         Vector3 blendedLeftShoulderRotation = Vector3.zero;
// 
//         float totalWeight = 0f;
//         float greatestWeight = 0f;
//         int currentInputs = 0;
// 
//         for (int i = 0; i < inputCount; i++)
//         {
//             float inputWeight = playable.GetInputWeight(i);
//             ScriptPlayable<HumanBehaviour> inputPlayable = (ScriptPlayable<HumanBehaviour>)playable.GetInput(i);
//             HumanBehaviour input = inputPlayable.GetBehaviour();
// 
//             blendedLeftShoulderRotation += input.LeftShoulderRotation * inputWeight;
// 
//             totalWeight += inputWeight;
// 
//             if (inputWeight > greatestWeight)
//             {
//                 greatestWeight = inputWeight;
//             }
// 
//             if (!Mathf.Approximately(inputWeight, 0f))
//                 currentInputs++;
//         }
// 
//         m_AssignedLeftShoulderRotation = blendedLeftShoulderRotation + m_DefaultLeftShoulderRotation * (1f - totalWeight);
//         // m_TrackBinding.LocalPosition = RawVector3.fromVector3(m_AssignedLocalPosition);
//         m_TrackBinding.LeftShoulderRotation = RawVector3.fromVector3(m_AssignedLeftShoulderRotation);
// 
//         // base.ProcessFrame(playable, info, playerData);
//     }
// }
// 
