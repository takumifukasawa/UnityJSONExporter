using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Custom;

[Serializable]
public class TestCustomBehaviour : PlayableBehaviour
{
    public Color CustomPropertyColor;
    public float CustomPropertyFloat;
    public Vector3 CustomPropertyVector3;
}
