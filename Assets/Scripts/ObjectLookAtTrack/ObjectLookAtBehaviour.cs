using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectLookAtBehaviour : PlayableBehaviour
{
    public Transform Target; // カメラがLookAtする対象
    public Vector3 UpVector;

    public ObjectLookAtBehaviour()
    {
        UpVector = new Vector3(0, 1, 0);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var lookAtObject = playerData as Transform;
        if (Target != null)
        {
            lookAtObject.LookAt(Target, UpVector);
        }
    }
}
