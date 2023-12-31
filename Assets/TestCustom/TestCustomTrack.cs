using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;
using Custom;

[TrackColor(0.9811321f, 0.4118904f, 0.9574461f)]
[TrackClipType(typeof(TestCustomClip))]
[TrackBindingType(typeof(CustomScript))]
public class TestCustomTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<TestCustomMixerBehaviour>.Create (graph, inputCount);
    }

    // Please note this assumes only one component of type CustomScript on the same gameobject.
    public override void GatherProperties (PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        CustomScript trackBinding = director.GetGenericBinding(this) as CustomScript;
        if (trackBinding == null)
            return;

        // These field names are procedurally generated estimations based on the associated property names.
        // If any of the names are incorrect you will get a DrivenPropertyManager error saying it has failed to register the name.
        // In this case you will need to find the correct backing field name.
        // The suggested way of finding the field name is to:
        // 1. Make sure your scene is serialized to text.
        // 2. Search the text for the track binding component type.
        // 3. Look through the field names until you see one that looks correct.
        driver.AddFromName<CustomScript>(trackBinding.gameObject, "CustomPropertyColor");
        driver.AddFromName<CustomScript>(trackBinding.gameObject, "CustomPropertyFloat");
        driver.AddFromName<CustomScript>(trackBinding.gameObject, "CustomPropertyVector3");
#endif
        base.GatherProperties (director, driver);
    }
}
