using UnityEngine;
using UnityEngine.Playables;
using UnityJSONExporter;

public class ObjectMoveAndLookAtMixerBehaviour : PlayableBehaviour
{
    Vector3 m_DefaultLocalPosition;
    Vector3 m_DefaultUpVector;
    Transform m_DefaultLookAtTarget;

    Vector3 m_AssignedLocalPosition;
    Vector3 m_AssignedUpVector;
    Transform m_AssignedLookAtTarget;

    ObjectMoveAndLookAtController m_TrackBinding;
    bool m_FirstFrameHappened;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var director = playable.GetGraph().GetResolver() as PlayableDirector;

        m_TrackBinding = playerData as ObjectMoveAndLookAtController;

        if (m_TrackBinding == null)
            return;

        if (!m_FirstFrameHappened)
        {
            m_DefaultLocalPosition = RawVector3.toVector3(m_TrackBinding.LocalPosition);
            m_DefaultUpVector = RawVector3.toVector3(m_TrackBinding.UpVector);
            m_DefaultLookAtTarget = m_TrackBinding.LookAtTarget;
            m_FirstFrameHappened = true;
        }

        int inputCount = playable.GetInputCount();

        Vector3 blendedLocalPosition = Vector3.zero;
        Vector3 blendedUpVector = Vector3.zero;
        Transform specifiedLookAtTarget = m_DefaultLookAtTarget;

        float totalWeight = 0f;
        float greatestWeight = 0f;
        int currentInputs = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<ObjectMoveAndLookAtBehaviour> inputPlayable = (ScriptPlayable<ObjectMoveAndLookAtBehaviour>)playable.GetInput(i);
            ObjectMoveAndLookAtBehaviour input = inputPlayable.GetBehaviour();

            blendedLocalPosition += input.LocalPosition * inputWeight;
            blendedUpVector += input.UpVector * inputWeight;
            specifiedLookAtTarget = input.LookAtTarget.Resolve(director);

            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                greatestWeight = inputWeight;
            }

            if (!Mathf.Approximately(inputWeight, 0f))
                currentInputs++;
        }

        m_AssignedLocalPosition = blendedLocalPosition + m_DefaultLocalPosition * (1f - totalWeight);
        m_TrackBinding.LocalPosition = RawVector3.fromVector3(m_AssignedLocalPosition);

        m_AssignedUpVector = blendedUpVector + m_DefaultUpVector * (1f - totalWeight);
        m_TrackBinding.UpVector = RawVector3.fromVector3(m_AssignedUpVector);

        m_AssignedLookAtTarget = specifiedLookAtTarget;
        m_TrackBinding.LookAtTarget = m_AssignedLookAtTarget;

        // base.ProcessFrame(playable, info, playerData);
    }
}
