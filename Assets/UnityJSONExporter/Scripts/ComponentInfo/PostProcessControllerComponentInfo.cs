using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class PostProcessControllerComponentInfo : ComponentInfoBase
{
    // screen space shadow ---
    // bias: number;
    // jitterSize: Vector3;
    // sharpness: number;
    // strength: number;
    // ratio: number;
    // rayStepMultiplier: number;

    public const string SCREEN_SPACE_SHADOW_ENABLED_PROPERTY_NAME = "sss_on";
    public const string SCREEN_SPACE_SHADOW_BIAS_PROPERTY_NAME = "sss_b";
    public const string SCREEN_SPACE_SHADOW_JITTER_SIZE_PROPERTY_NAME = "sss_js";
    public const string SCREEN_SPACE_SHADOW_SHARPNESS_PROPERTY_NAME = "sss_sh";
    public const string SCREEN_SPACE_SHADOW_STRENGTH_PROPERTY_NAME = "sss_s";
    public const string SCREEN_SPACE_SHADOW_RATIO_PROPERTY_NAME = "sss_r";
    public const string SCREEN_SPACE_SHADOW_RAY_STEP_MULTIPLIER_PROPERTY_NAME = "sss_rsm";

    [JsonProperty(PropertyName = SCREEN_SPACE_SHADOW_ENABLED_PROPERTY_NAME)]
    public int ScreenSpaceShadowEnabled;

    [JsonProperty(PropertyName = SCREEN_SPACE_SHADOW_BIAS_PROPERTY_NAME)]
    public float ScreenSpaceShadowBias;

    [JsonProperty(PropertyName = SCREEN_SPACE_SHADOW_JITTER_SIZE_PROPERTY_NAME)]
    public RawVector3 ScreenSpaceShadowJitterSize;

    [JsonProperty(PropertyName = SCREEN_SPACE_SHADOW_SHARPNESS_PROPERTY_NAME)]
    public float ScreenSpaceShadowSharpness;

    [JsonProperty(PropertyName = SCREEN_SPACE_SHADOW_STRENGTH_PROPERTY_NAME)]
    public float ScreenSpaceShadowStrength;

    [JsonProperty(PropertyName = SCREEN_SPACE_SHADOW_RATIO_PROPERTY_NAME)]
    public float ScreenSpaceShadowRatio;

    [JsonProperty(PropertyName = SCREEN_SPACE_SHADOW_RAY_STEP_MULTIPLIER_PROPERTY_NAME)]
    public float ScreenSpaceShadowRayStepMultiplier;

    // ambient occlusion ---
    // occlusionSampleLength: number;
    // occlusionBias: number;
    // occlusionMinDistance: number;
    // occlusionMaxDistance: number;
    // occlusionColor: Color;
    // occlusionPower: number;
    // occlusionStrength: number;
    // blendRate: number;
    // samplingTexture: Texture; -- not use

    public const string AMBIENT_OCCLUSION_ENABLED_PROPERTY_NAME = "ao_on";
    public const string AMBIENT_OCCLUSION_SAMPLE_LENGTH_PROPERTY_NAME = "ao_sl";
    public const string AMBIENT_OCCLUSION_BIAS_PROPERTY_NAME = "ao_b";
    public const string AMBIENT_OCCLUSION_MIN_DISTANCE_PROPERTY_NAME = "ao_mid";
    public const string AMBIENT_OCCLUSION_MAX_DISTANCE_PROPERTY_NAME = "ao_mad";
    public const string AMBIENT_OCCLUSION_COLOR_PROPERTY_NAME = "ao_c";
    public const string AMBIENT_OCCLUSION_POWER_PROPERTY_NAME = "ao_p";
    public const string AMBIENT_OCCLUSION_STRENGTH_PROPERTY_NAME = "ao_s";
    public const string AMBIENT_OCCLUSION_BLEND_RATE_PROPERTY_NAME = "ao_br";

    [JsonProperty(PropertyName = AMBIENT_OCCLUSION_ENABLED_PROPERTY_NAME)]
    public int AmbientOcclusionEnabled;

    [JsonProperty(PropertyName = AMBIENT_OCCLUSION_SAMPLE_LENGTH_PROPERTY_NAME)]
    public float AmbientOcclusionSampleLength;

    [JsonProperty(PropertyName = AMBIENT_OCCLUSION_BIAS_PROPERTY_NAME)]
    public float AmbientOcclusionBias;

    [JsonProperty(PropertyName = AMBIENT_OCCLUSION_MIN_DISTANCE_PROPERTY_NAME)]
    public float AmbientOcclusionMinDistance;

    [JsonProperty(PropertyName = AMBIENT_OCCLUSION_MAX_DISTANCE_PROPERTY_NAME)]
    public float AmbientOcclusionMaxDistance;

    [JsonProperty(PropertyName = AMBIENT_OCCLUSION_COLOR_PROPERTY_NAME)]
    public string AmbientOcclusionColor;

    [JsonProperty(PropertyName = AMBIENT_OCCLUSION_POWER_PROPERTY_NAME)]
    public float AmbientOcclusionPower;

    [JsonProperty(PropertyName = AMBIENT_OCCLUSION_STRENGTH_PROPERTY_NAME)]
    public float AmbientOcclusionStrength;

    [JsonProperty(PropertyName = AMBIENT_OCCLUSION_BLEND_RATE_PROPERTY_NAME)]
    public float AmbientOcclusionBlendRate;

    // screen space reflection ---
    // rayDepthBias: number;
    // rayNearestDistance: number;
    // rayMaxDistance: number;
    // reflectionRayThickness: number;
    // reflectionRayJitterSizeX: number;
    // reflectionRayJitterSizeY: number;
    // reflectionFadeMinDistance: number;
    // reflectionFadeMaxDistance: number;
    // reflectionScreenEdgeFadeFactorMinX: number;
    // reflectionScreenEdgeFadeFactorMaxX: number;
    // reflectionScreenEdgeFadeFactorMinY: number;
    // reflectionScreenEdgeFadeFactorMaxY: number;
    // reflectionRoughnessPower: number;
    // reflectionAdditionalRate: number;
    // blendRate: number;

    public const string SCREEN_SPACE_REFLECTION_ENABLED_PROPERTY_NAME = "ssr_on";
    public const string SCREEN_SPACE_REFLECTION_RAY_DEPTH_BIAS_PROPERTY_NAME = "ssr_rdb";
    public const string SCREEN_SPACE_REFLECTION_RAY_NEAREST_DISTANCE_PROPERTY_NAME = "ssr_rnd";
    public const string SCREEN_SPACE_REFLECTION_RAY_MAX_DISTANCE_PROPERTY_NAME = "ssr_rmd";
    public const string SCREEN_SPACE_REFLECTION_RAY_THICKNESS_PROPERTY_NAME = "ssr_rt";
    public const string SCREEN_SPACE_REFLECTION_RAY_JITTER_SIZE_X_PROPERTY_NAME = "ssr_rjx";
    public const string SCREEN_SPACE_REFLECTION_RAY_JITTER_SIZE_Y_PROPERTY_NAME = "ssr_rjy";
    public const string SCREEN_SPACE_REFLECTION_FADE_MIN_DISTANCE_PROPERTY_NAME = "ssr_fmd";
    public const string SCREEN_SPACE_REFLECTION_FADE_MAX_DISTANCE_PROPERTY_NAME = "ssr_fxd";
    public const string SCREEN_SPACE_REFLECTION_SCREEN_EDGE_FADE_FACTOR_MIN_X_PROPERTY_NAME = "ssr_seffmx";
    public const string SCREEN_SPACE_REFLECTION_SCREEN_EDGE_FADE_FACTOR_MAX_X_PROPERTY_NAME = "ssr_seffMx";
    public const string SCREEN_SPACE_REFLECTION_SCREEN_EDGE_FADE_FACTOR_MIN_Y_PROPERTY_NAME = "ssr_seffmy";
    public const string SCREEN_SPACE_REFLECTION_SCREEN_EDGE_FADE_FACTOR_MAX_Y_PROPERTY_NAME = "ssr_seffMy";
    public const string SCREEN_SPACE_REFLECTION_ROUGHNESS_POWER_PROPERTY_NAME = "ssr_rp";
    public const string SCREEN_SPACE_REFLECTION_ADDITIONAL_RATE_PROPERTY_NAME = "ssr_ar";
    public const string SCREEN_SPACE_REFLECTION_BLEND_RATE_PROPERTY_NAME = "ssr_br";

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_ENABLED_PROPERTY_NAME)]
    public int ScreenSpaceReflectionEnabled;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_RAY_DEPTH_BIAS_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRayDepthBias;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_RAY_NEAREST_DISTANCE_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRayNearestDistance;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_RAY_MAX_DISTANCE_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRayMaxDistance;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_RAY_THICKNESS_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRayThickness;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_RAY_JITTER_SIZE_X_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRayJitterSizeX;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_RAY_JITTER_SIZE_Y_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRayJitterSizeY;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_FADE_MIN_DISTANCE_PROPERTY_NAME)]
    public float ScreenSpaceReflectionFadeMinDistance;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_FADE_MAX_DISTANCE_PROPERTY_NAME)]
    public float ScreenSpaceReflectionFadeMaxDistance;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_SCREEN_EDGE_FADE_FACTOR_MIN_X_PROPERTY_NAME)]
    public float ScreenSpaceReflectionScreenEdgeFadeFactorMinX;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_SCREEN_EDGE_FADE_FACTOR_MAX_X_PROPERTY_NAME)]
    public float ScreenSpaceReflectionScreenEdgeFadeFactorMaxX;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_SCREEN_EDGE_FADE_FACTOR_MIN_Y_PROPERTY_NAME)]
    public float ScreenSpaceReflectionScreenEdgeFadeFactorMinY;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_SCREEN_EDGE_FADE_FACTOR_MAX_Y_PROPERTY_NAME)]
    public float ScreenSpaceReflectionScreenEdgeFadeFactorMaxY;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_ROUGHNESS_POWER_PROPERTY_NAME)]
    public float ScreenSpaceReflectionRoughnessPower;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_ADDITIONAL_RATE_PROPERTY_NAME)]
    public float ScreenSpaceReflectionAdditionalRate;

    [JsonProperty(PropertyName = SCREEN_SPACE_REFLECTION_BLEND_RATE_PROPERTY_NAME)]
    public float ScreenSpaceReflectionBlendRate;

    // light shaft ---
    // ratio: number;
    // blendRate: number;
    // passScaleBase: number;
    // rayStepStrength: number;

    public const string LIGHT_SHAFT_ENABLED_PROPERTY_NAME = "ls_on";
    public const string LIGHT_SHAFT_RATIO_PROPERTY_NAME = "ls_r";
    public const string LIGHT_SHAFT_BLEND_RATE_PROPERTY_NAME = "ls_br";
    public const string LIGHT_SHAFT_PASS_SCALE_BASE_PROPERTY_NAME = "ls_psb";
    public const string LIGHT_SHAFT_RAY_STEP_STRENGTH_PROPERTY_NAME = "ls_rss";

    [JsonProperty(PropertyName = LIGHT_SHAFT_ENABLED_PROPERTY_NAME)]
    public int LightShaftEnabled;

    [JsonProperty(PropertyName = LIGHT_SHAFT_RATIO_PROPERTY_NAME)]
    public float LightShaftRatio;

    [JsonProperty(PropertyName = LIGHT_SHAFT_BLEND_RATE_PROPERTY_NAME)]
    public float LightShaftBlendRate;

    [JsonProperty(PropertyName = LIGHT_SHAFT_PASS_SCALE_BASE_PROPERTY_NAME)]
    public float LightShaftPassScaleBase;

    [JsonProperty(PropertyName = LIGHT_SHAFT_RAY_STEP_STRENGTH_PROPERTY_NAME)]
    public float LightShaftRayStepStrength;

    // volumetric light ---
    // rayStep: number;
    // blendRate: number;
    // densityMultiplier: number;
    // rayJitterSize: Vector3;
    // ratio: number;

    public const string VOLUMETRIC_LIGHT_ENABLED_PROPERTY_NAME = "vl_on";
    public const string VOLUMETRIC_LIGHT_RAY_STEP_PROPERTY_NAME = "vl_rs";
    public const string VOLUMETRIC_LIGHT_BLEND_RATE_PROPERTY_NAME = "vl_br";
    public const string VOLUMETRIC_LIGHT_DENSITY_MULTIPLIER_PROPERTY_NAME = "vl_dm";
    public const string VOLUMETRIC_LIGHT_RAY_JITTER_SIZE_PROPERTY_NAME = "vl_rjs";
    public const string VOLUMETRIC_LIGHT_RATIO_PROPERTY_NAME = "vl_r";

    [JsonProperty(PropertyName = VOLUMETRIC_LIGHT_ENABLED_PROPERTY_NAME)]
    public int VolumetricLightEnabled;

    [JsonProperty(PropertyName = VOLUMETRIC_LIGHT_RAY_STEP_PROPERTY_NAME)]
    public float VolumetricLightRayStep;

    [JsonProperty(PropertyName = VOLUMETRIC_LIGHT_BLEND_RATE_PROPERTY_NAME)]
    public float VolumetricLightBlendRate;

    [JsonProperty(PropertyName = VOLUMETRIC_LIGHT_DENSITY_MULTIPLIER_PROPERTY_NAME)]
    public float VolumetricLightDensityMultiplier;

    [JsonProperty(PropertyName = VOLUMETRIC_LIGHT_RAY_JITTER_SIZE_PROPERTY_NAME)]
    public RawVector3 VolumetricLightRayJitterSize;

    [JsonProperty(PropertyName = VOLUMETRIC_LIGHT_RATIO_PROPERTY_NAME)]
    public float VolumetricLightRatio;

    // fog --- 

    public const string FOG_ENABLED_PROPERTY_NAME = "fg_on";
    public const string FOG_COLOR_PROPERTY_NAME = "fg_c";
    public const string FOG_STRENGTH_PROPERTY_NAME = "fg_s";
    public const string FOG_DENSITY_PROPERTY_NAME = "fg_d";
    public const string FOG_DENSITY_ATTENUATION_PROPERTY_NAME = "fg_da";
    public const string FOG_END_HEIGHT_PROPERTY_NAME = "fg_eh";
    public const string FOG_DISTANCE_START_PROPERTY_NAME = "fg_ds";
    public const string FOG_DISTANCE_POWER_PROPERTY_NAME = "fg_dp";
    public const string FOG_DISTANCE_END_PROPERTY_NAME = "fg_de";
    public const string FOG_SSS_RATE_PROPERTY_NAME = "fg_sss_r";
    public const string FOG_SSS_FOG_COLOR_PROPERTY_NAME = "fg_sss_fc";
    public const string FOG_BLEND_RATE_PROPERTY_NAME = "fg_br";

    [JsonProperty(PropertyName = FOG_ENABLED_PROPERTY_NAME)]
    public int FogEnabled;

    [JsonProperty(PropertyName = FOG_COLOR_PROPERTY_NAME)]
    public string FogColor;

    [JsonProperty(PropertyName = FOG_STRENGTH_PROPERTY_NAME)]
    public float FogStrength;

    [JsonProperty(PropertyName = FOG_DENSITY_PROPERTY_NAME)]
    public float FogDensity;

    [JsonProperty(PropertyName = FOG_DENSITY_ATTENUATION_PROPERTY_NAME)]
    public float FogDensityAttenuation;

    [JsonProperty(PropertyName = FOG_END_HEIGHT_PROPERTY_NAME)]
    public float FogEndHeight;

    [JsonProperty(PropertyName = FOG_DISTANCE_START_PROPERTY_NAME)]
    public float FogDistanceStart;

    [JsonProperty(PropertyName = FOG_DISTANCE_END_PROPERTY_NAME)]
    public float FogDistanceEnd;

    [JsonProperty(PropertyName = FOG_DISTANCE_POWER_PROPERTY_NAME)]
    public float FogDistancePower;

    [JsonProperty(PropertyName = FOG_SSS_RATE_PROPERTY_NAME)]
    public float FogSSSRate;

    [JsonProperty(PropertyName = FOG_SSS_FOG_COLOR_PROPERTY_NAME)]
    public string FogSSSFogColor;

    [JsonProperty(PropertyName = FOG_BLEND_RATE_PROPERTY_NAME)]
    public float FogBlendRate;

    // dof ---

    public const string DEPTH_OF_FIELD_FOCUS_ENABLED_PROPERTY_ENABLED = "dof_on";
    public const string DEPTH_OF_FIELD_FOCUS_DISTANCE_PROPERTY_NAME = "dof_fd";
    public const string DEPTH_OF_FIELD_FOCUS_RANGE_PROPERTY_NAME = "dof_fr";
    public const string DEPTH_OF_FIELD_BOKEH_RADIUS_PROPERTY_NAME = "dof_br";

    [JsonProperty(PropertyName = DEPTH_OF_FIELD_FOCUS_ENABLED_PROPERTY_ENABLED)]
    public int DepthOfFieldFocusEnabled;

    [JsonProperty(PropertyName = DEPTH_OF_FIELD_FOCUS_DISTANCE_PROPERTY_NAME)]
    public float DepthOfFieldFocusDistance;

    [JsonProperty(PropertyName = DEPTH_OF_FIELD_FOCUS_RANGE_PROPERTY_NAME)]
    public float DepthOfFieldFocusRange;

    [JsonProperty(PropertyName = DEPTH_OF_FIELD_BOKEH_RADIUS_PROPERTY_NAME)]
    public float DepthOfFieldBokehRadius;

    // bloom --- 

    public const string BLOOM_ENABLED_PROPERTY_NAME = "bl_on";
    public const string BLOOM_AMOUNT_PROPERTY_NAME = "bl_a";
    public const string BLOOM_TONE_PROPERTY_NAME = "bl_to";
    public const string BLOOM_THRESHOLD_PROPERTY_NAME = "bl_th";

    [JsonProperty(PropertyName = BLOOM_ENABLED_PROPERTY_NAME)]
    public int BloomEnabled;

    [JsonProperty(PropertyName = BLOOM_AMOUNT_PROPERTY_NAME)]
    public float BloomAmount;

    [JsonProperty(PropertyName = BLOOM_TONE_PROPERTY_NAME)]
    public float BloomTone;

    [JsonProperty(PropertyName = BLOOM_THRESHOLD_PROPERTY_NAME)]
    public float BloomThreshold;

    // streak --- 
    // enabled
    // threshold
    // stretch
    // color
    // intensity
    // verticalScale
    // horizontalScale

    public const string STREAK_ENABLED_PROPERTY_NAME = "sk_on";
    public const string STREAK_THRESHOLD_PROPERTY_NAME = "sk_th";
    public const string STREAK_STRETCH_PROPERTY_NAME = "sk_st";
    public const string STREAK_COLOR_PROPERTY_NAME = "sk_c";
    public const string STREAK_INTENSITY_PROPERTY_NAME = "sk_i";
    public const string STREAK_VERTICAL_SCALE_PROPERTY_NAME = "sk_vs";
    public const string STREAK_HORIZONTAL_SCALE_PROPERTY_NAME = "sk_hs";

    [JsonProperty(PropertyName = STREAK_ENABLED_PROPERTY_NAME)]
    public int StreakEnabled;

    [JsonProperty(PropertyName = STREAK_THRESHOLD_PROPERTY_NAME)]
    public float StreakThreshold;

    [JsonProperty(PropertyName = STREAK_STRETCH_PROPERTY_NAME)]
    public float StreakStretch;

    [JsonProperty(PropertyName = STREAK_COLOR_PROPERTY_NAME)]
    public string StreakColor;

    [JsonProperty(PropertyName = STREAK_INTENSITY_PROPERTY_NAME)]
    public float StreakIntensity;

    [JsonProperty(PropertyName = STREAK_VERTICAL_SCALE_PROPERTY_NAME)]
    public float StreakVerticalScale;

    [JsonProperty(PropertyName = STREAK_HORIZONTAL_SCALE_PROPERTY_NAME)]
    public float StreakHorizontalScale;

    // vignette ---

    public const string VIGNETTE_ENABLED_PROPERTY_NAME = "vg_on";
    public const string VIGNETTE_RADIUS_FROM_PROPERTY_NAME = "vg_rf";
    public const string VIGNETTE_RADIUS_TO_PROPERTY_NAME = "vg_rt";
    public const string VIGNETTE_POWER_PROPERTY_NAME = "vg_p";
    public const string VIGNETTE_BLEND_RATE_PROPERTY_NAME = "vg_br";

    [JsonProperty(PropertyName = VIGNETTE_ENABLED_PROPERTY_NAME)]
    public int VignetteEnabled;

    [JsonProperty(PropertyName = VIGNETTE_RADIUS_FROM_PROPERTY_NAME)]
    public float VignetteRadiusFrom = 1.77f;

    [JsonProperty(PropertyName = VIGNETTE_RADIUS_TO_PROPERTY_NAME)]
    public float VignetteRadiusTo = 4.5f;

    [JsonProperty(PropertyName = VIGNETTE_POWER_PROPERTY_NAME)]
    public float VignettePower = 1.345f;

    [JsonProperty(PropertyName = VIGNETTE_BLEND_RATE_PROPERTY_NAME)]
    public float VignetteBlendRate = 0.73f;

    // motion blur ---

    public const string MOTION_BLUR_ENABLED_PROPERTY_NAME = "mb_on";
    public const string MOTION_BLUR_INTENSITY_PROPERTY_NAME = "mb_i";
    public const string MOTION_BLUR_BLEND_RATE_PROPERTY_NAME = "mb_br";

    [JsonProperty(PropertyName = MOTION_BLUR_ENABLED_PROPERTY_NAME)]
    public int MotionBlurEnabled;

    [JsonProperty(PropertyName = MOTION_BLUR_INTENSITY_PROPERTY_NAME)]
    public float MotionBlurIntensity;

    [JsonProperty(PropertyName = MOTION_BLUR_BLEND_RATE_PROPERTY_NAME)]
    public float MotionBlurBlendRate;

    // chromatic aberration --- 
    // scale
    // power
    // blendRate

    public const string CHROMATIC_ABERRATION_ENABLED_PROPERTY_NAME = "ca_on";
    public const string CHROMATIC_ABERRATION_SCALE_PROPERTY_NAME = "ca_s";
    public const string CHROMATIC_ABERRATION_POWER_PROPERTY_NAME = "ca_p";
    public const string CHROMATIC_ABERRATION_BLEND_RATE_PROPERTY_NAME = "ca_br";

    [JsonProperty(PropertyName = CHROMATIC_ABERRATION_ENABLED_PROPERTY_NAME)]
    public int ChromaticAberrationEnabled;

    [JsonProperty(PropertyName = CHROMATIC_ABERRATION_SCALE_PROPERTY_NAME)]
    public float ChromaticAberrationScale;

    [JsonProperty(PropertyName = CHROMATIC_ABERRATION_POWER_PROPERTY_NAME)]
    public float ChromaticAberrationPower;

    [JsonProperty(PropertyName = CHROMATIC_ABERRATION_BLEND_RATE_PROPERTY_NAME)]
    public float ChromaticAberrationBlendRate;

    // glitch ---
    // blendRate

    public const string GLITCH_ENABLED_PROPERTY_NAME = "gl_on";
    public const string GLITCH_BLEND_RATE_PROPERTY_NAME = "gl_br";

    [JsonProperty(PropertyName = GLITCH_ENABLED_PROPERTY_NAME)]
    public int GlitchEnabled;

    [JsonProperty(PropertyName = GLITCH_BLEND_RATE_PROPERTY_NAME)]
    public float GlitchBlendRate;
    
    // color curtain ---
    // blendRate
    
    public const string COLOR_CURTAIN_ENABLED_PROPERTY_NAME = "cc_on";
    public const string COLOR_CURTAIN_COLOR_PROPERTY_NAME = "cc_c";
    public const string COLOR_CURTAIN_BLEND_RATE_PROPERTY_NAME = "cc_br";
    
    [JsonProperty(PropertyName = COLOR_CURTAIN_ENABLED_PROPERTY_NAME)]
    public int ColorCurtainEnabled;
    
    [JsonProperty(PropertyName = COLOR_CURTAIN_COLOR_PROPERTY_NAME)]
    public string ColorCurtainColor;
    
    [JsonProperty(PropertyName = COLOR_CURTAIN_BLEND_RATE_PROPERTY_NAME)]
    public float ColorCurtainBlendRate;


    public PostProcessControllerComponentInfo(PostProcessController postProcessController) : base(ComponentType.PostProcess)
    {
        // screen space shadow ---

        ScreenSpaceShadowEnabled = IsEnabled(postProcessController.ScreenSpaceShadowEnabled);
        ScreenSpaceShadowBias = postProcessController.ScreenSpaceShadowBias;
        ScreenSpaceShadowJitterSize = RawVector3.fromVector3(postProcessController.ScreenSpaceShadowJitterSize);
        ScreenSpaceShadowSharpness = postProcessController.ScreenSpaceShadowSharpness;
        ScreenSpaceShadowStrength = postProcessController.ScreenSpaceShadowStrength;
        ScreenSpaceShadowRatio = postProcessController.ScreenSpaceShadowRatio;
        ScreenSpaceShadowRayStepMultiplier = postProcessController.ScreenSpaceShadowRayStepMultiplier;

        // ambient occlusion ---

        AmbientOcclusionEnabled = IsEnabled(postProcessController.AmbientOcclusionEnabled);
        AmbientOcclusionSampleLength = postProcessController.AmbientOcclusionSampleLength;
        AmbientOcclusionBias = postProcessController.AmbientOcclusionBias;
        AmbientOcclusionMinDistance = postProcessController.AmbientOcclusionMinDistance;
        AmbientOcclusionMaxDistance = postProcessController.AmbientOcclusionMaxDistance;
        AmbientOcclusionColor = ColorUtilities.ConvertColorToHexString(postProcessController.AmbientOcclusionColor);
        AmbientOcclusionPower = postProcessController.AmbientOcclusionPower;
        AmbientOcclusionStrength = postProcessController.AmbientOcclusionStrength;
        AmbientOcclusionBlendRate = postProcessController.AmbientOcclusionBlendRate;

        // screen space reflection ---

        ScreenSpaceReflectionEnabled = IsEnabled(postProcessController.ScreenSpaceReflectionEnabled);
        ScreenSpaceReflectionRayDepthBias = postProcessController.ScreenSpaceReflectionRayDepthBias;
        ScreenSpaceReflectionRayNearestDistance = postProcessController.ScreenSpaceReflectionRayNearestDistance;
        ScreenSpaceReflectionRayMaxDistance = postProcessController.ScreenSpaceReflectionRayMaxDistance;
        ScreenSpaceReflectionRayThickness = postProcessController.ScreenSpaceReflectionRayThickness;
        ScreenSpaceReflectionRayJitterSizeX = postProcessController.ScreenSpaceReflectionRayJitterSizeX;
        ScreenSpaceReflectionRayJitterSizeY = postProcessController.ScreenSpaceReflectionRayJitterSizeY;
        ScreenSpaceReflectionFadeMinDistance = postProcessController.ScreenSpaceReflectionFadeMinDistance;
        ScreenSpaceReflectionFadeMaxDistance = postProcessController.ScreenSpaceReflectionFadeMaxDistance;
        ScreenSpaceReflectionScreenEdgeFadeFactorMinX = postProcessController.ScreenSpaceReflectionScreenEdgeFadeFactorMinX;
        ScreenSpaceReflectionScreenEdgeFadeFactorMaxX = postProcessController.ScreenSpaceReflectionScreenEdgeFadeFactorMaxX;
        ScreenSpaceReflectionScreenEdgeFadeFactorMinY = postProcessController.ScreenSpaceReflectionScreenEdgeFadeFactorMinY;
        ScreenSpaceReflectionScreenEdgeFadeFactorMaxY = postProcessController.ScreenSpaceReflectionScreenEdgeFadeFactorMaxY;
        ScreenSpaceReflectionRoughnessPower = postProcessController.ScreenSpaceReflectionRoughnessPower;
        ScreenSpaceReflectionAdditionalRate = postProcessController.ScreenSpaceReflectionAdditionalRate;
        ScreenSpaceReflectionBlendRate = postProcessController.ScreenSpaceReflectionBlendRate;

        // light shaft ---

        LightShaftEnabled = IsEnabled(postProcessController.LightShaftEnabled);
        LightShaftRatio = postProcessController.LightShaftRatio;
        LightShaftBlendRate = postProcessController.LightShaftBlendRate;
        LightShaftPassScaleBase = postProcessController.LightShaftPassScaleBase;
        LightShaftRayStepStrength = postProcessController.LightShaftRayStepStrength;

        // volumetric light ---

        VolumetricLightEnabled = IsEnabled(postProcessController.VolumetricLightEnabled);
        VolumetricLightRayStep = postProcessController.VolumetricLightRayStep;
        VolumetricLightBlendRate = postProcessController.VolumetricLightBlendRate;
        VolumetricLightDensityMultiplier = postProcessController.VolumetricLightDensityMultiplier;
        VolumetricLightRayJitterSize = RawVector3.fromVector3(postProcessController.VolumetricLightRayJitterSize);
        VolumetricLightRatio = postProcessController.VolumetricLightRatio;

        // fog ---

        FogEnabled = IsEnabled(postProcessController.FogEnabled);
        FogColor = ColorUtilities.ConvertColorToHexString(postProcessController.FogColor);
        FogStrength = postProcessController.FogStrength;
        FogDensity = postProcessController.FogDensity;
        FogDensityAttenuation = postProcessController.FogDensityAttenuation;
        FogEndHeight = postProcessController.FogEndHeight;
        FogDistanceStart = postProcessController.FogDistanceStart;
        FogDistanceEnd = postProcessController.FogDistanceEnd;
        FogDistancePower = postProcessController.FogDistancePower;
        FogSSSRate = postProcessController.FogSSSRate;
        FogSSSFogColor = ColorUtilities.ConvertColorToHexString(postProcessController.FogSSSFogColor);
        FogBlendRate = postProcessController.FogBlendRate;

        // dof ---

        DepthOfFieldFocusEnabled = IsEnabled(postProcessController.DepthOfFieldFocusEnabled);
        DepthOfFieldFocusDistance = postProcessController.DepthOfFieldFocusDistance;
        DepthOfFieldFocusRange = postProcessController.DepthOfFieldFocusRange;
        DepthOfFieldBokehRadius = postProcessController.DepthOfFieldBokehRadius;

        // bloom --- 

        BloomEnabled = IsEnabled(postProcessController.BloomEnabled);
        BloomAmount = postProcessController.BloomAmount;
        BloomTone = postProcessController.BloomTone;
        BloomThreshold = postProcessController.BloomThreshold;

        // streak ---

        StreakEnabled = IsEnabled(postProcessController.StreakEnabled);
        StreakThreshold = postProcessController.StreakThreshold;
        StreakStretch = postProcessController.StreakStretch;
        StreakColor = ColorUtilities.ConvertColorToHexString(postProcessController.StreakColor);
        StreakIntensity = postProcessController.StreakIntensity;
        StreakVerticalScale = postProcessController.StreakVerticalScale;
        StreakHorizontalScale = postProcessController.StreakHorizontalScale;

        // vignette ---

        VignetteEnabled = IsEnabled(postProcessController.VignetteEnabled);
        VignetteRadiusFrom = postProcessController.VignetteRadiusFrom;
        VignetteRadiusTo = postProcessController.VignetteRadiusTo;
        VignettePower = postProcessController.VignettePower;
        VignetteBlendRate = postProcessController.VignetteBlendRate;

        // motion blur ---

        MotionBlurEnabled = IsEnabled(postProcessController.MotionBlurEnabled);
        MotionBlurIntensity = postProcessController.MotionBlurIntensity;
        MotionBlurBlendRate = postProcessController.MotionBlurBlendRate;

        // chromatic aberration ---

        ChromaticAberrationEnabled = IsEnabled(postProcessController.ChromaticAberrationEnabled);
        ChromaticAberrationScale = postProcessController.ChromaticAberrationScale;
        ChromaticAberrationPower = postProcessController.ChromaticAberrationPower;
        ChromaticAberrationBlendRate = postProcessController.ChromaticAberrationBlendRate;

        // glitch ---

        GlitchEnabled = IsEnabled(postProcessController.GlitchEnabled);
        GlitchBlendRate = postProcessController.GlitchBlendRate;
        
        // color curtain ---
        
        ColorCurtainEnabled = IsEnabled(postProcessController.ColorCurtainEnabled);
        ColorCurtainColor = ColorUtilities.ConvertColorToHexString(postProcessController.ColorCurtainColor);
        ColorCurtainBlendRate = postProcessController.ColorCurtainBlendRate;
    }

    public int IsEnabled(float value)
    {
        return value > 0.5f ? 1 : 0;
    }
}
