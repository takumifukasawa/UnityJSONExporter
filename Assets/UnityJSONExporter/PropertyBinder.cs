using System;
using UnityEditor;
using UnityEngine;

namespace UnityJSONExporter
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PropertyBinder
    {
        public PropertyBinder(
            AnimationClip animationClip,
            EditorCurveBinding[] bindings,
            float time
        )
        {
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class AnimationTrackBinder : PropertyBinder
    {
        // ---------------------------------------------------------------------------
        // constants
        // ---------------------------------------------------------------------------

        const string PROPERTY_LOCAL_POSITION_X = "m_LocalPosition.x";
        const string PROPERTY_LOCAL_POSITION_Y = "m_LocalPosition.y";
        const string PROPERTY_LOCAL_POSITION_Z = "m_LocalPosition.z";
        const string PROPERTY_LOCAL_EULER_ANGLES_RAW_X = "localEulerAnglesRaw.x";
        const string PROPERTY_LOCAL_EULER_ANGLES_RAW_Y = "localEulerAnglesRaw.y";
        const string PROPERTY_LOCAL_EULER_ANGLES_RAW_Z = "localEulerAnglesRaw.z";
        const string PROPERTY_LOCAL_SCALE_X = "m_LocalScale.x";
        const string PROPERTY_LOCAL_SCALE_Y = "m_LocalScale.y";
        const string PROPERTY_LOCAL_SCALE_Z = "m_LocalScale.z";

        // ---------------------------------------------------------------------------
        // public
        // ---------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animationClip"></param>
        /// <param name="bindings"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public AnimationTrackBinder(
            AnimationClip animationClip,
            EditorCurveBinding[] bindings,
            float time
        ) : base(animationClip, bindings, time)
        {
            foreach (var binding in bindings)
            {
                // Debug.Log(binding.type.FullName);
                // animated transform
                if (binding.type.FullName == typeof(Transform).FullName)
                {
                    var curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                    // for debug
                    // Debug.Log(binding.propertyName);
                    var value = CurveUtilities.EvaluateCurve(time, curve);

                    // // animated transform
                    // if (binding.type.FullName == typeof(Transform).FullName)
                    // {
                    switch (binding.propertyName)
                    {
                        case PROPERTY_LOCAL_POSITION_X:
                            _hasLocalPosition = true;
                            _localPosition.x = value;
                            break;
                        case PROPERTY_LOCAL_POSITION_Y:
                            _hasLocalPosition = true;
                            _localPosition.y = value;
                            break;
                        case PROPERTY_LOCAL_POSITION_Z:
                            _hasLocalPosition = true;
                            _localPosition.z = value;
                            break;
                        case PROPERTY_LOCAL_EULER_ANGLES_RAW_X:
                            _hasLocalRotationEuler = true;
                            _localRotationEuler.x = value;
                            break;
                        case PROPERTY_LOCAL_EULER_ANGLES_RAW_Y:
                            _hasLocalRotationEuler = true;
                            _localRotationEuler.y = value;
                            break;
                        case PROPERTY_LOCAL_EULER_ANGLES_RAW_Z:
                            _hasLocalRotationEuler = true;
                            _localRotationEuler.z = value;
                            break;
                        case PROPERTY_LOCAL_SCALE_X:
                            _hasLocalScale = true;
                            _localScale.x = value;
                            break;
                        case PROPERTY_LOCAL_SCALE_Y:
                            _hasLocalScale = true;
                            _localScale.y = value;
                            break;
                        case PROPERTY_LOCAL_SCALE_Z:
                            _hasLocalScale = true;
                            _localScale.z = value;
                            break;
                        default:
                            throw new Exception($"invalid property: {binding.propertyName}");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public void AssignProperty(Transform t)
        {
            // Debug.Log("==========");
            // Debug.Log(LocalPosition);
            // Debug.Log(LocalRotationEuler);
            // Debug.Log(LocalScale);
            if (_hasLocalPosition)
            {
                t.localPosition = _localPosition;
            }

            if (_hasLocalRotationEuler)
            {
                t.localRotation = Quaternion.Euler(_localRotationEuler);
            }

            if (_hasLocalScale)
            {
                t.localScale = _localScale;
            }
        }

        // ---------------------------------------------------------------------------
        // private
        // ---------------------------------------------------------------------------

        private bool _hasLocalPosition;
        private bool _hasLocalRotationEuler;
        private bool _hasLocalScale;

        private Vector3 _localPosition = Vector3.zero;
        private Vector3 _localRotationEuler = Vector3.zero;
        private Vector3 _localScale = Vector3.one;
    }

    /// <summary>
    /// 
    /// </summary>
    public class LightControlTrackPropertyBinder : PropertyBinder
    {
        // ---------------------------------------------------------------------------
        // constants
        // ---------------------------------------------------------------------------

        private const string PROPERTY_COLOR_R = "color.r";
        private const string PROPERTY_COLOR_G = "color.g";
        private const string PROPERTY_COLOR_B = "color.b";
        private const string PROPERTY_COLOR_A = "color.a";
        private const string PROPERTY_BOUNCE_INTENSITY = "bounceIntensity";
        private const string PROPERTY_INTENSITY = "intensity";
        private const string PROPERTY_RANGE = "range";

        // ---------------------------------------------------------------------------
        // public
        // ---------------------------------------------------------------------------

        public Color Color => _color;
        public float Intensity => _intensity;
        public float BounceIntensity => _bounceIntensity;
        public float Range => _range;

        public LightControlTrackPropertyBinder(
            AnimationClip animationClip,
            EditorCurveBinding[] bindings,
            float time
        ) : base(animationClip, bindings, time)
        {
            foreach (var binding in bindings)
            {
                // for debug
                // Debug.Log($"binding type: {binding.type}, path: {binding.path}, property name: {binding.propertyName}");
                var curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                var value = CurveUtilities.EvaluateCurve(time, curve);

                switch (binding.propertyName)
                {
                    case PROPERTY_COLOR_R:
                        _hasColor = true;
                        _color.r = value;
                        break;
                    case PROPERTY_COLOR_G:
                        _hasColor = true;
                        _color.g = value;
                        break;
                    case PROPERTY_COLOR_B:
                        _hasColor = true;
                        _color.b = value;
                        break;
                    case PROPERTY_COLOR_A:
                        _hasColor = true;
                        _color.a = value;
                        break;
                    case PROPERTY_BOUNCE_INTENSITY:
                        _hasBounceIntensity = true;
                        _bounceIntensity = value;
                        break;
                    case PROPERTY_INTENSITY:
                        _hasIntensity = true;
                        _intensity = value;
                        break;
                    case PROPERTY_RANGE:
                        _hasRange = true;
                        _range = value;
                        break;
                    default:
                        throw new Exception($"invalid property: {binding.propertyName}");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="light"></param>
        public void AssignProperty(Light light)
        {
            if (_hasIntensity)
            {
                light.intensity = _intensity;
            }

            if (_hasBounceIntensity)
            {
                light.bounceIntensity = _bounceIntensity;
            }

            if (_hasRange)
            {
                light.range = _range;
            }

            if (_hasColor)
            {
                light.color = _color;
            }
        }

        // ---------------------------------------------------------------------------
        // private
        // ---------------------------------------------------------------------------

        private bool _hasIntensity;
        private bool _hasBounceIntensity;
        private bool _hasRange;
        private bool _hasColor;

        private float _intensity;
        private float _bounceIntensity;
        private float _range;
        private Color _color = new Color();
    }
}
