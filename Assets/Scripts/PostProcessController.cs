using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Animator))]
[ExecuteAlways]
public class PostProcessController : MonoBehaviour
{
    [SerializeField, Range(0, 10)]
    [JsonProperty(PropertyName = "bli")]
    public float bloomIntensity;

    [SerializeField, Range(0, 10)]
    [JsonProperty(PropertyName = "dofd")]
    public float depthOfFieldFocusDistance;

    [Space(13)]
    [SerializeField]
    private VolumeProfile volumeProfile;

    [SerializeField]
    private Bloom bloomComponent;

    [SerializeField]
    private DepthOfField depthOfFieldComponent;

    [Space(13)]
    private float bloomIntensityCache;

    private float depthOfFieldFocusDistanceCache;

    // /// <summary>
    // /// 
    // /// </summary>
    // void OnValidate()
    // {
    //     if (volumeProfile == null)
    //     {
    //         return;
    //     }

    //     // 不必要にループが回るので効率は良くないがエディター側のみなのでよしとする
    //     foreach (var item in volumeProfile.components)
    //     {
    //         if (bloomComponent == null && item is Bloom bloom)
    //         {
    //             bloomComponent = bloom;
    //         }

    //         if (depthOfFieldComponent == null && item is DepthOfField dof)
    //         {
    //             depthOfFieldComponent = dof;
    //         }
    //     }
    // }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        Debug.Log("============");
        Debug.Log(volumeProfile);
        Debug.Log(bloomComponent);
        Debug.Log(depthOfFieldComponent);
        //
        // update bloom
        //

        if (bloomComponent != null)
        {
            if (bloomComponent.intensity.value != bloomIntensityCache)
            {
                bloomIntensityCache = bloomComponent.intensity.value;
            }
            else if (bloomIntensity != bloomIntensityCache)
            {
                bloomIntensityCache = bloomIntensity;
            }

            bloomIntensity = bloomIntensityCache;
            bloomComponent.intensity.value = bloomIntensityCache;
        }

        //
        // update dof
        //

        if (depthOfFieldComponent != null)
        {
            if (depthOfFieldComponent.focusDistance.value != depthOfFieldFocusDistanceCache)
            {
                depthOfFieldFocusDistanceCache = depthOfFieldComponent.focusDistance.value;
            }
            else if (depthOfFieldFocusDistance != depthOfFieldFocusDistanceCache)
            {
                depthOfFieldFocusDistanceCache = depthOfFieldFocusDistance;
            }

            depthOfFieldFocusDistance = depthOfFieldFocusDistanceCache;
            depthOfFieldComponent.focusDistance.value = depthOfFieldFocusDistanceCache;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CacheComponents()
    {
        volumeProfile = null;
        bloomComponent = null;
        depthOfFieldComponent = null;

        volumeProfile = GetComponent<Volume>()?.profile;

        if (volumeProfile != null)
        {
            // 不必要にループが回るので効率は良くないがエディター側のみなのでよしとする
            foreach (var item in volumeProfile.components)
            {
                if (bloomComponent == null && item is Bloom bloom)
                {
                    bloomComponent = bloom;
                }

                if (depthOfFieldComponent == null && item is DepthOfField dof)
                {
                    depthOfFieldComponent = dof;
                }
            }
        }
    }
}
