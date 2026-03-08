using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityJSONExporter;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Animator))]
[ExecuteAlways]
public class PostProcessController : TimelineBindingObjectBase
{
    [Space(13)]
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Transform _dofTarget;

    // screen space shadow ---

    [Space(13)]
    [Header("Screen Space Shadow")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_SHADOW_ENABLED_PROPERTY_NAME)]
    public float ScreenSpaceShadowEnabled = 0f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_SHADOW_BIAS_PROPERTY_NAME)]
    public float ScreenSpaceShadowBias = 0f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_SHADOW_JITTER_SIZE_PROPERTY_NAME)]
    public Vector3 ScreenSpaceShadowJitterSize = new Vector3(0.025f, 0.025f, 0.025f);

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_SHADOW_SHARPNESS_PROPERTY_NAME)]
    public float ScreenSpaceShadowSharpness = 2f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_SHADOW_STRENGTH_PROPERTY_NAME)]
    public float ScreenSpaceShadowStrength = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_SHADOW_RATIO_PROPERTY_NAME)]
    public float ScreenSpaceShadowRatio = 0.5f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_SHADOW_RAY_STEP_MULTIPLIER_PROPERTY_NAME)]
    public float ScreenSpaceShadowRayStepMultiplier = 1f;

    // ambient occlusion ---

    [Space(13)]
    [Header("Ambient Occlusion")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.AMBIENT_OCCLUSION_ENABLED_PROPERTY_NAME)]
    public float AmbientOcclusionEnabled = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.AMBIENT_OCCLUSION_SAMPLE_LENGTH_PROPERTY_NAME)]
    public float AmbientOcclusionSampleLength = 0.121f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.AMBIENT_OCCLUSION_BIAS_PROPERTY_NAME)]
    public float AmbientOcclusionBias = 0.0001f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.AMBIENT_OCCLUSION_MIN_DISTANCE_PROPERTY_NAME)]
    public float AmbientOcclusionMinDistance = 0.006f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.AMBIENT_OCCLUSION_MAX_DISTANCE_PROPERTY_NAME)]
    public float AmbientOcclusionMaxDistance = 0.2f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.AMBIENT_OCCLUSION_COLOR_PROPERTY_NAME)]
    public Color AmbientOcclusionColor = Color.black;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.AMBIENT_OCCLUSION_POWER_PROPERTY_NAME)]
    public float AmbientOcclusionPower = 1.85f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.AMBIENT_OCCLUSION_STRENGTH_PROPERTY_NAME)]
    public float AmbientOcclusionStrength = 0.448f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.AMBIENT_OCCLUSION_BLEND_RATE_PROPERTY_NAME)]
    public float AmbientOcclusionBlendRate = 1f;

    // screen space reflection ---

    [Space(13)]
    [Header("Screen Space Reflection")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_ENABLED_PROPERTY_NAME)]
    public float ScreenSpaceReflectionEnabled = 0f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_RAY_DEPTH_BIAS_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRayDepthBias = 0.0099f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_RAY_NEAREST_DISTANCE_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRayNearestDistance = 0.13f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_RAY_MAX_DISTANCE_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRayMaxDistance = 3.25f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_RAY_THICKNESS_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRayThickness = 0.3f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_RAY_JITTER_SIZE_X_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRayJitterSizeX = 0.05f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_RAY_JITTER_SIZE_Y_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRayJitterSizeY = 0.05f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_FADE_MIN_DISTANCE_PROPERTY_NAME)]
    public float ScreenSpaceReflectionFadeMinDistance = 0f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_FADE_MAX_DISTANCE_PROPERTY_NAME)]
    public float ScreenSpaceReflectionFadeMaxDistance = 4.2f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_SCREEN_EDGE_FADE_FACTOR_MIN_X_PROPERTY_NAME)]
    public float ScreenSpaceReflectionScreenEdgeFadeFactorMinX = 0.42f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_SCREEN_EDGE_FADE_FACTOR_MAX_X_PROPERTY_NAME)]
    public float ScreenSpaceReflectionScreenEdgeFadeFactorMaxX = 0.955f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_SCREEN_EDGE_FADE_FACTOR_MIN_Y_PROPERTY_NAME)]
    public float ScreenSpaceReflectionScreenEdgeFadeFactorMinY = 0.444f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_SCREEN_EDGE_FADE_FACTOR_MAX_Y_PROPERTY_NAME)]
    public float ScreenSpaceReflectionScreenEdgeFadeFactorMaxY = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_ROUGHNESS_POWER_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRoughnessPower = 0.5f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_ADDITIONAL_RATE_PROPERTY_NAME)]
    public float ScreenSpaceReflectionAdditionalRate = 0.355f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.SCREEN_SPACE_REFLECTION_BLEND_RATE_PROPERTY_NAME)]
    public float ScreenSpaceReflectionBlendRate = 1f;

    // light shaft ---

    [Space(13)]
    [Header("Light Shaft")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.LIGHT_SHAFT_ENABLED_PROPERTY_NAME)]
    public float LightShaftEnabled = 0f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.LIGHT_SHAFT_RATIO_PROPERTY_NAME)]
    public float LightShaftRatio = 0.5f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.LIGHT_SHAFT_BLEND_RATE_PROPERTY_NAME)]
    public float LightShaftBlendRate = 0.65f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.LIGHT_SHAFT_PASS_SCALE_BASE_PROPERTY_NAME)]
    public float LightShaftPassScaleBase = 0.2f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.LIGHT_SHAFT_RAY_STEP_STRENGTH_PROPERTY_NAME)]
    public float LightShaftRayStepStrength = 0.012f;

    // volumetric light ---

    [Space(13)]
    [Header("Volumetric Light")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.VOLUMETRIC_LIGHT_ENABLED_PROPERTY_NAME)]
    public float VolumetricLightEnabled = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.VOLUMETRIC_LIGHT_RAY_STEP_PROPERTY_NAME)]
    public float VolumetricLightRayStep = 0.62f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.VOLUMETRIC_LIGHT_BLEND_RATE_PROPERTY_NAME)]
    public float VolumetricLightBlendRate = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.VOLUMETRIC_LIGHT_DENSITY_MULTIPLIER_PROPERTY_NAME)]
    public float VolumetricLightDensityMultiplier = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.VOLUMETRIC_LIGHT_RAY_JITTER_SIZE_PROPERTY_NAME)]
    public Vector3 VolumetricLightRayJitterSize = new Vector3(0.1f, 0.1f, 0.1f);

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.VOLUMETRIC_LIGHT_RATIO_PROPERTY_NAME)]
    public float VolumetricLightRatio = 0.5f;

    // fog ---

    [Space(13)]
    [Header("Fog")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.FOG_ENABLED_PROPERTY_NAME)]
    public float FogEnabled = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.FOG_COLOR_PROPERTY_NAME)]
    public Color FogColor = Color.black;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.FOG_STRENGTH_PROPERTY_NAME)]
    public float FogStrength = 0.01f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.FOG_DENSITY_PROPERTY_NAME)]
    public float FogDensity = 0.001f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.FOG_DENSITY_ATTENUATION_PROPERTY_NAME)]
    public float FogDensityAttenuation = 0.001f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.FOG_END_HEIGHT_PROPERTY_NAME)]
    public float FogEndHeight = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.FOG_DISTANCE_START_PROPERTY_NAME)]
    public float FogDistanceStart = 1000f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.FOG_DISTANCE_END_PROPERTY_NAME)]
    public float FogDistanceEnd = 1000f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.FOG_DISTANCE_POWER_PROPERTY_NAME)]
    public float FogDistancePower = 0.29f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.FOG_SSS_RATE_PROPERTY_NAME)]
    public float FogSSSRate = 0f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.FOG_SSS_FOG_COLOR_PROPERTY_NAME)]
    public Color FogSSSFogColor = Color.white;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.FOG_BLEND_RATE_PROPERTY_NAME)]
    public float FogBlendRate = 1f;

    // dof ---

    [Space(13)]
    [Header("Depth of Field")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.DEPTH_OF_FIELD_FOCUS_ENABLED_PROPERTY_ENABLED)]
    public float DepthOfFieldFocusEnabled = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.DEPTH_OF_FIELD_FOCUS_DISTANCE_PROPERTY_NAME)]
    public float DepthOfFieldFocusDistance;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.DEPTH_OF_FIELD_FOCUS_RANGE_PROPERTY_NAME)]
    public float DepthOfFieldFocusRange;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.DEPTH_OF_FIELD_BOKEH_RADIUS_PROPERTY_NAME)]
    public float DepthOfFieldBokehRadius;


    // bloom ---

    [Space(13)]
    [Header("Bloom")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.BLOOM_ENABLED_PROPERTY_NAME)]
    public float BloomEnabled = 1f;

    [Range(0, 10)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.BLOOM_AMOUNT_PROPERTY_NAME)]
    public float BloomAmount;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.BLOOM_TONE_PROPERTY_NAME)]
    public float BloomTone;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.BLOOM_THRESHOLD_PROPERTY_NAME)]
    public float BloomThreshold;

    // streak ---

    [Space(13)]
    [Header("Streak")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.STREAK_ENABLED_PROPERTY_NAME)]
    public float StreakEnabled = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.STREAK_THRESHOLD_PROPERTY_NAME)]
    public float StreakThreshold = 0.9f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.STREAK_STRETCH_PROPERTY_NAME)]
    public float StreakStretch = 0.5f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.STREAK_COLOR_PROPERTY_NAME)]
    public Color StreakColor = Color.white;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.STREAK_INTENSITY_PROPERTY_NAME)]
    public float StreakIntensity = 0.03f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.STREAK_VERTICAL_SCALE_PROPERTY_NAME)]
    public float StreakVerticalScale = 1.12f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.STREAK_HORIZONTAL_SCALE_PROPERTY_NAME)]
    public float StreakHorizontalScale = 1.9f;

    // vignette ---

    [Space(13)]
    [Header("Vignette")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.VIGNETTE_ENABLED_PROPERTY_NAME)]
    public float VignetteEnabled = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.VIGNETTE_RADIUS_FROM_PROPERTY_NAME)]
    public float VignetteRadiusFrom = 1.77f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.VIGNETTE_RADIUS_TO_PROPERTY_NAME)]
    public float VignetteRadiusTo = 5f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.VIGNETTE_POWER_PROPERTY_NAME)]
    public float VignettePower = 1.345f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.VIGNETTE_BLEND_RATE_PROPERTY_NAME)]
    public float VignetteBlendRate = 0.73f;


    // motion blur ---

    [Space(13)]
    [Header("Motion Blur")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.MOTION_BLUR_ENABLED_PROPERTY_NAME)]
    public float MotionBlurEnabled = 0f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.MOTION_BLUR_INTENSITY_PROPERTY_NAME)]
    public float MotionBlurIntensity = 0.7f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.MOTION_BLUR_BLEND_RATE_PROPERTY_NAME)]
    public float MotionBlurBlendRate = 1f;


    // chromatic aberration ---

    [Space(13)]
    [Header("Chromatic Aberration")]
    [Range(0, 1)]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.CHROMATIC_ABERRATION_ENABLED_PROPERTY_NAME)]
    public float ChromaticAberrationEnabled = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.CHROMATIC_ABERRATION_SCALE_PROPERTY_NAME)]
    public float ChromaticAberrationScale = 0.1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.CHROMATIC_ABERRATION_POWER_PROPERTY_NAME)]
    public float ChromaticAberrationPower = 1f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.CHROMATIC_ABERRATION_BLEND_RATE_PROPERTY_NAME)]
    public float ChromaticAberrationBlendRate = 1f;

    // glitch ---

    [Space(13)]
    [Header("Glitch")]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.GLITCH_ENABLED_PROPERTY_NAME)]
    public float GlitchEnabled = 0f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.GLITCH_BLEND_RATE_PROPERTY_NAME)]
    public float GlitchBlendRate = 0f;

    // black curtain ---

    [Space(13)]
    [Header("Color Curtain")]
    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.COLOR_CURTAIN_ENABLED_PROPERTY_NAME)]
    public float ColorCurtainEnabled = 0f;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.COLOR_CURTAIN_COLOR_PROPERTY_NAME)]
    public Color ColorCurtainColor = Color.black;

    [JsonProperty(PropertyName = PostProcessControllerComponentInfo.COLOR_CURTAIN_BLEND_RATE_PROPERTY_NAME)]
    public float ColorCurtainBlendRate = 0f;


    public override string ResolvePropertyName(string propertyName)
    {
        return JsonUtilities.ResolveJsonProperty(this, propertyName);
    }
}
