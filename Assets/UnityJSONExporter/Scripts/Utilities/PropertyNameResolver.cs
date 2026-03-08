using System.Collections.Generic;
using UnityEngine;

namespace UnityJSONExporter
{
    public static class PropertyNameResolver
    {
        //
        // general
        //
        
        public const string ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_X = "m_LocalPosition.x";
        public const string ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_X_SHORTEN = "lp.x";

        public const string ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Y = "m_LocalPosition.y";
        public const string ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Y_SHORTEN = "lp.y";

        public const string ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Z = "m_LocalPosition.z";
        public const string ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Z_SHORTEN = "lp.z";

        public const string ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_X = "localEulerAnglesRaw.x";
        public const string ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_X_SHORTEN = "lr.x";

        public const string ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Y = "localEulerAnglesRaw.y";
        public const string ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Y_SHORTEN = "lr.y";

        public const string ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Z = "localEulerAnglesRaw.z";
        public const string ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Z_SHORTEN = "lr.z";

        public const string ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_X = "m_LocalScale.x";
        public const string ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_X_SHORTEN = "ls.x";

        public const string ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Y = "m_LocalScale.y";
        public const string ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Y_SHORTEN = "ls.y";

        public const string ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Z = "m_LocalScale.z";
        public const string ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Z_SHORTEN = "ls.z";

        public const string LIGHT_CONTROL_CLIP_PROPERTY_COLOR_R = "color.r";
        public const string LIGHT_CONTROL_CLIP_PROPERTY_COLOR_R_SHORTEN = "c.r";

        public const string LIGHT_CONTROL_CLIP_PROPERTY_COLOR_G = "color.g";
        public const string LIGHT_CONTROL_CLIP_PROPERTY_COLOR_G_SHORTEN = "c.g";

        public const string LIGHT_CONTROL_CLIP_PROPERTY_COLOR_B = "color.b";
        public const string LIGHT_CONTROL_CLIP_PROPERTY_COLOR_B_SHORTEN = "c.b";

        public const string LIGHT_CONTROL_CLIP_PROPERTY_COLOR_A = "color.a";
        public const string LIGHT_CONTROL_CLIP_PROPERTY_COLOR_A_SHORTEN = "c.a";

        //
        // post process - ambient occlusion color
        //

        public const string AMBIENT_OCCLUSION_COLOR_R = "AmbientOcclusionColor.r";
        public const string AMBIENT_OCCLUSION_COLOR_R_SHORTEN = "ao_c.r";

        public const string AMBIENT_OCCLUSION_COLOR_G = "AmbientOcclusionColor.g";
        public const string AMBIENT_OCCLUSION_COLOR_G_SHORTEN = "ao_c.g";

        public const string AMBIENT_OCCLUSION_COLOR_B = "AmbientOcclusionColor.b";
        public const string AMBIENT_OCCLUSION_COLOR_B_SHORTEN = "ao_c.b";

        public const string AMBIENT_OCCLUSION_COLOR_A = "AmbientOcclusionColor.a";
        public const string AMBIENT_OCCLUSION_COLOR_A_SHORTEN = "ao_c.a";

        //
        // post process - fog color
        //

        public const string FOG_COLOR_R = "FogColor.r";
        public const string FOG_COLOR_R_SHORTEN = "fg_c.r";

        public const string FOG_COLOR_G = "FogColor.g";
        public const string FOG_COLOR_G_SHORTEN = "fg_c.g";

        public const string FOG_COLOR_B = "FogColor.b";
        public const string FOG_COLOR_B_SHORTEN = "fg_c.b";

        public const string FOG_COLOR_A = "FogColor.a";
        public const string FOG_COLOR_A_SHORTEN = "fg_c.a";

        //
        // post process - fog sss fog color
        //

        public const string FOG_SSS_FOG_COLOR_R = "FogSSSFogColor.r";
        public const string FOG_SSS_FOG_COLOR_R_SHORTEN = "fg_sss_fc.r";

        public const string FOG_SSS_FOG_COLOR_G = "FogSSSFogColor.g";
        public const string FOG_SSS_FOG_COLOR_G_SHORTEN = "fg_sss_fc.g";

        public const string FOG_SSS_FOG_COLOR_B = "FogSSSFogColor.b";
        public const string FOG_SSS_FOG_COLOR_B_SHORTEN = "fg_sss_fc.b";

        public const string FOG_SSS_FOG_COLOR_A = "FogSSSFogColor.a";
        public const string FOG_SSS_FOG_COLOR_A_SHORTEN = "fg_sss_fc.a";

        //
        // post process - streak color
        //

        public const string STREAK_COLOR_R = "StreakColor.r";
        public const string STREAK_COLOR_R_SHORTEN = "sk_c.r";

        public const string STREAK_COLOR_G = "StreakColor.g";
        public const string STREAK_COLOR_G_SHORTEN = "sk_c.g";

        public const string STREAK_COLOR_B = "StreakColor.b";
        public const string STREAK_COLOR_B_SHORTEN = "sk_c.b";

        public const string STREAK_COLOR_A = "StreakColor.a";
        public const string STREAK_COLOR_A_SHORTEN = "sk_c.a";

        //
        // post process - color curtain color
        //

        public const string COLOR_CURTAIN_COLOR_R = "ColorCurtainColor.r";
        public const string COLOR_CURTAIN_COLOR_R_SHORTEN = "cc_c.r";

        public const string COLOR_CURTAIN_COLOR_G = "ColorCurtainColor.g";
        public const string COLOR_CURTAIN_COLOR_G_SHORTEN = "cc_c.g";

        public const string COLOR_CURTAIN_COLOR_B = "ColorCurtainColor.b";
        public const string COLOR_CURTAIN_COLOR_B_SHORTEN = "cc_c.b";

        public const string COLOR_CURTAIN_COLOR_A = "ColorCurtainColor.a";
        public const string COLOR_CURTAIN_COLOR_A_SHORTEN = "cc_c.a";

        //
        // material
        //
        
        public const string MATERIAL_PROPERTY_BASE_COLOR_R = "material._BaseColor.r";
        public const string MATERIAL_PROPERTY_BASE_COLOR_R_SHORTEN = "m.bc.r";

        public const string MATERIAL_PROPERTY_BASE_COLOR_G = "material._BaseColor.g";
        public const string MATERIAL_PROPERTY_BASE_COLOR_G_SHORTEN = "m.bc.g";

        public const string MATERIAL_PROPERTY_BASE_COLOR_B = "material._BaseColor.b";
        public const string MATERIAL_PROPERTY_BASE_COLOR_B_SHORTEN = "m.bc.b";

        public const string MATERIAL_PROPERTY_BASE_COLOR_A = "material._BaseColor.a";
        public const string MATERIAL_PROPERTY_BASE_COLOR_A_SHORTEN = "m.bc.a";

        //
        // light
        //
        
        public const string LIGHT_CONTROL_CLIP_PROPERTY_BOUNCE_INTENSITY = "bounceIntensity";
        public const string LIGHT_CONTROL_CLIP_PROPERTY_BOUNCE_INTENSITY_SHORTEN = "bi";

        public const string LIGHT_CONTROL_CLIP_PROPERTY_INTENSITY = "intensity";
        public const string LIGHT_CONTROL_CLIP_PROPERTY_INTENSITY_SHORTEN = "i";

        public const string LIGHT_CONTROL_CLIP_PROPERTY_RANGE = "range";
        public const string LIGHT_CONTROL_CLIP_PROPERTY_RANGE_SHORTEN = "r";

        //
        // camera
        //
        
        public const string CAMERA_FIELD_OF_VIEW = "field of view";
        public const string CAMERA_FIELD_OF_VIEW_SHORTEN = "fov";
        
        //
        // create pair
        //
        
        private static Dictionary<string, string> _propertyPairs = new Dictionary<string, string>
        {
            {
                ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_X,
                ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_X_SHORTEN
            },
            {
                ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Y,
                ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Y_SHORTEN
            },
            {
                ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Z,
                ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Z_SHORTEN
            },
            {
                ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_X,
                ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_X_SHORTEN
            },
            {
                ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Y,
                ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Y_SHORTEN
            },
            {
                ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Z,
                ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Z_SHORTEN
            },
            {
                ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_X,
                ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_X_SHORTEN
            },
            {
                ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Y,
                ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Y_SHORTEN
            },
            {
                ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Z,
                ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Z_SHORTEN
            },
            {
                LIGHT_CONTROL_CLIP_PROPERTY_COLOR_R,
                LIGHT_CONTROL_CLIP_PROPERTY_COLOR_R_SHORTEN
            },
            {
                LIGHT_CONTROL_CLIP_PROPERTY_COLOR_G,
                LIGHT_CONTROL_CLIP_PROPERTY_COLOR_G_SHORTEN
            },
            {
                LIGHT_CONTROL_CLIP_PROPERTY_COLOR_B,
                LIGHT_CONTROL_CLIP_PROPERTY_COLOR_B_SHORTEN
            },
            {
                LIGHT_CONTROL_CLIP_PROPERTY_COLOR_A,
                LIGHT_CONTROL_CLIP_PROPERTY_COLOR_A_SHORTEN
            },
            {
                AMBIENT_OCCLUSION_COLOR_R,
                AMBIENT_OCCLUSION_COLOR_R_SHORTEN
            },
            {
                AMBIENT_OCCLUSION_COLOR_G,
                AMBIENT_OCCLUSION_COLOR_G_SHORTEN
            },
            {
                AMBIENT_OCCLUSION_COLOR_B,
                AMBIENT_OCCLUSION_COLOR_B_SHORTEN
            },
            {
                AMBIENT_OCCLUSION_COLOR_A,
                AMBIENT_OCCLUSION_COLOR_A_SHORTEN
            },
            {
                FOG_COLOR_R,
                FOG_COLOR_R_SHORTEN
            },
            {
                FOG_COLOR_G,
                FOG_COLOR_G_SHORTEN
            },
            {
                FOG_COLOR_B,
                FOG_COLOR_B_SHORTEN
            },
            {
                FOG_COLOR_A,
                FOG_COLOR_A_SHORTEN
            },
            {
                FOG_SSS_FOG_COLOR_R,
                FOG_SSS_FOG_COLOR_R_SHORTEN
            },
            {
                FOG_SSS_FOG_COLOR_G,
                FOG_SSS_FOG_COLOR_G_SHORTEN
            },
            {
                FOG_SSS_FOG_COLOR_B,
                FOG_SSS_FOG_COLOR_B_SHORTEN
            },
            {
                FOG_SSS_FOG_COLOR_A,
                FOG_SSS_FOG_COLOR_A_SHORTEN
            },
            {
                STREAK_COLOR_R,
                STREAK_COLOR_R_SHORTEN
            },
            {
                STREAK_COLOR_G,
                STREAK_COLOR_G_SHORTEN
            },
            {
                STREAK_COLOR_B,
                STREAK_COLOR_B_SHORTEN
            },
            {
                STREAK_COLOR_A,
                STREAK_COLOR_A_SHORTEN
            },
            {
                COLOR_CURTAIN_COLOR_R,
                COLOR_CURTAIN_COLOR_R_SHORTEN
            },
            {
                COLOR_CURTAIN_COLOR_G,
                COLOR_CURTAIN_COLOR_G_SHORTEN
            },
            {
                COLOR_CURTAIN_COLOR_B,
                COLOR_CURTAIN_COLOR_B_SHORTEN
            },
            {
                COLOR_CURTAIN_COLOR_A,
                COLOR_CURTAIN_COLOR_A_SHORTEN
            },
            {
                MATERIAL_PROPERTY_BASE_COLOR_R,
                MATERIAL_PROPERTY_BASE_COLOR_R_SHORTEN
            },
            {
                MATERIAL_PROPERTY_BASE_COLOR_G,
                MATERIAL_PROPERTY_BASE_COLOR_G_SHORTEN
            },
            {
                MATERIAL_PROPERTY_BASE_COLOR_B,
                MATERIAL_PROPERTY_BASE_COLOR_B_SHORTEN
            },
            {
                MATERIAL_PROPERTY_BASE_COLOR_A,
                MATERIAL_PROPERTY_BASE_COLOR_A_SHORTEN
            },
            {
                LIGHT_CONTROL_CLIP_PROPERTY_BOUNCE_INTENSITY,
                LIGHT_CONTROL_CLIP_PROPERTY_BOUNCE_INTENSITY_SHORTEN
            },
            {
                LIGHT_CONTROL_CLIP_PROPERTY_INTENSITY,
                LIGHT_CONTROL_CLIP_PROPERTY_INTENSITY_SHORTEN
            },
            {
                LIGHT_CONTROL_CLIP_PROPERTY_RANGE,
                LIGHT_CONTROL_CLIP_PROPERTY_RANGE_SHORTEN
            },
            {
                CAMERA_FIELD_OF_VIEW,
                CAMERA_FIELD_OF_VIEW_SHORTEN
            }
        };

        public static string ResolveUnityBuiltinPropertyName(string propertyName)
        {
            if (_propertyPairs.ContainsKey(propertyName))
            {
                return _propertyPairs[propertyName];
            }

            LoggerProxy.LogWarning($"[PropertyNameResolver.ResolveUnityBuiltinPropertyName] invalid property name: {propertyName}");


            return propertyName;
        }
    }
}
