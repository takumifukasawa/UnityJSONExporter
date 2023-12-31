using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Custom;

public class TestCustomMixerBehaviour : PlayableBehaviour
{
    Color m_DefaultCustomPropertyColor;
    float m_DefaultCustomPropertyFloat;
    Vector3 m_DefaultCustomPropertyVector3;

    Color m_AssignedCustomPropertyColor;
    float m_AssignedCustomPropertyFloat;
    Vector3 m_AssignedCustomPropertyVector3;

    CustomScript m_TrackBinding;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_TrackBinding = playerData as CustomScript;

        if (m_TrackBinding == null)
            return;

        if (m_TrackBinding.CustomPropertyColor != m_AssignedCustomPropertyColor)
            m_DefaultCustomPropertyColor = m_TrackBinding.CustomPropertyColor;
        if (!Mathf.Approximately(m_TrackBinding.CustomPropertyFloat, m_AssignedCustomPropertyFloat))
            m_DefaultCustomPropertyFloat = m_TrackBinding.CustomPropertyFloat;
        if (m_TrackBinding.CustomPropertyVector3 != m_AssignedCustomPropertyVector3)
            m_DefaultCustomPropertyVector3 = m_TrackBinding.CustomPropertyVector3;

        int inputCount = playable.GetInputCount ();

        Color blendedCustomPropertyColor = Color.clear;
        float blendedCustomPropertyFloat = 0f;
        Vector3 blendedCustomPropertyVector3 = Vector3.zero;
        float totalWeight = 0f;
        float greatestWeight = 0f;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<TestCustomBehaviour> inputPlayable = (ScriptPlayable<TestCustomBehaviour>)playable.GetInput(i);
            TestCustomBehaviour input = inputPlayable.GetBehaviour ();
            
            blendedCustomPropertyColor += input.CustomPropertyColor * inputWeight;
            blendedCustomPropertyFloat += input.CustomPropertyFloat * inputWeight;
            blendedCustomPropertyVector3 += input.CustomPropertyVector3 * inputWeight;
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                greatestWeight = inputWeight;
            }
        }

        m_AssignedCustomPropertyColor = blendedCustomPropertyColor + m_DefaultCustomPropertyColor * (1f - totalWeight);
        m_TrackBinding.CustomPropertyColor = m_AssignedCustomPropertyColor;
        m_AssignedCustomPropertyFloat = blendedCustomPropertyFloat + m_DefaultCustomPropertyFloat * (1f - totalWeight);
        m_TrackBinding.CustomPropertyFloat = m_AssignedCustomPropertyFloat;
        m_AssignedCustomPropertyVector3 = blendedCustomPropertyVector3 + m_DefaultCustomPropertyVector3 * (1f - totalWeight);
        m_TrackBinding.CustomPropertyVector3 = m_AssignedCustomPropertyVector3;
    }
}
