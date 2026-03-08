using System;
using UnityEditor;
using UnityEngine;

namespace UnityJSONExporter
{



    public static class TransformConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public enum TransformType
        {
            Position,
            Rotation,
            Scale,
        }

        /// <summary>
        /// 
        /// </summary>
        public enum AxisDirection
        {
            X,
            Y,
            Z
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="convertAxis"></param>
        /// <param name="transformType"></param>
        /// <param name="axisDirection"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static float ConvertValue(
            ConvertAxis convertAxis,
            TransformType transformType,
            AxisDirection axisDirection,
            float value
        )
        {
            switch (convertAxis)
            {
                case ConvertAxis.RightHand:
                    if (transformType == TransformType.Position)
                    {
                        if (axisDirection == AxisDirection.Z)
                        {
                            return -value;
                        }
                    }

                    // TODO: 本当はc#側でxyを反転させて渡したいが、なぜかうまくいかないのでここだけフロント側で反転
                    // if (transformType == TransformType.Rotation)
                    // {
                    //     if (
                    //         axisDirection == AxisDirection.X ||
                    //         axisDirection == AxisDirection.Y
                    //     )
                    //     {
                    //         return -value;
                    //     }
                    // }

                    return value;

                // case ConvertAxis.Default:
                //     return value;

                default:
                    break;
            }

            throw new Exception($"[TransformConverter.ConvertAxis] invalid params: convertAxis={convertAxis}, transformType={transformType}, axisDirection={axisDirection}, value={value}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Vector3Info ConvertPosition(ConvertAxis axis, Vector3 p)
        {
            return new Vector3Info(
                ConvertValue(axis, TransformType.Position, AxisDirection.X, p.x),
                ConvertValue(axis, TransformType.Position, AxisDirection.Y, p.y),
                ConvertValue(axis, TransformType.Position, AxisDirection.Z, p.z)
            );
        }

        // /// <summary>
        // /// 
        // /// </summary>
        // /// <param name="axis"></param>
        // /// <param name="p"></param>
        // /// <returns></returns>
        // public static Vector3Info ConvertRotationEuler(ConvertAxis axis, Vector3 eulerAngles)
        // {
        //     return new Vector3Info(
        //         ConvertValue(axis, TransformType.Rotation, AxisDirection.X, eulerAngles.x),
        //         ConvertValue(axis, TransformType.Rotation, AxisDirection.Y, eulerAngles.y),
        //         ConvertValue(axis, TransformType.Rotation, AxisDirection.Z, eulerAngles.z)
        //     );
        // }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static Vector4Info ConvertRotationQuaternion(ConvertAxis axis, Quaternion quaternion)
        {
            switch (axis)
            {
                case ConvertAxis.RightHand:
                    return new Vector4Info(quaternion.x, quaternion.y, -quaternion.z, -quaternion.w);
                // return new Vector4Info(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
                default:
                    return new Vector4Info(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
            }
        }

        public static float ConvertRotationQuaternionX(ConvertAxis axis, float x)
        {
            switch (axis)
            {
                case ConvertAxis.RightHand:
                    return x;
                default:
                    return x;
            }
        }

        public static float ConvertRotationQuaternionY(ConvertAxis axis, float y)
        {
            switch (axis)
            {
                case ConvertAxis.RightHand:
                    return y;
                default:
                    return y;
            }
        }

        public static float ConvertRotationQuaternionZ(ConvertAxis axis, float z)
        {
            switch (axis)
            {
                case ConvertAxis.RightHand:
                    return -z;
                default:
                    return z;
            }
        }

        public static float ConvertRotationQuaternionW(ConvertAxis axis, float w)
        {
            switch (axis)
            {
                case ConvertAxis.RightHand:
                    return -w;
                default:
                    return w;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Vector3Info ConvertScale(ConvertAxis axis, Vector3 scale)
        {
            return new Vector3Info(
                ConvertValue(axis, TransformType.Scale, AxisDirection.X, scale.x),
                ConvertValue(axis, TransformType.Scale, AxisDirection.Y, scale.y),
                ConvertValue(axis, TransformType.Scale, AxisDirection.Z, scale.z)
            );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool IsTransformProperty(string propertyName)
        {
            return
                propertyName == PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_X ||
                propertyName == PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Y ||
                propertyName == PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Z ||
                propertyName == PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_X ||
                propertyName == PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Y ||
                propertyName == PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Z ||
                propertyName == PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_X ||
                propertyName == PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Y ||
                propertyName == PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="keyValue"></param>
        /// <param name="convertAxis"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static float ConvertTransformCurveValue(EditorCurveBinding binding, float keyValue, ConvertAxis convertAxis)
        {
            // for debug
            // Debug.Log($"[PlayableDirectorComponentInfo.ConvertTransformCurveValue] binding propertyName: {binding.propertyName}, keyValue: {keyValue}");

            switch (binding.propertyName)
            {
                case PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_X:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Position, TransformConverter.AxisDirection.X, keyValue);
                case PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Y:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Position, TransformConverter.AxisDirection.Y, keyValue);
                case PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_POSITION_Z:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Position, TransformConverter.AxisDirection.Z, keyValue);
                case PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_X:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Rotation, TransformConverter.AxisDirection.X, keyValue);
                case PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Y:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Rotation, TransformConverter.AxisDirection.Y, keyValue);
                case PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_EULER_ANGLES_RAW_Z:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Rotation, TransformConverter.AxisDirection.Z, keyValue);
                case PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_X:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Scale, TransformConverter.AxisDirection.X, keyValue);
                case PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Y:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Scale, TransformConverter.AxisDirection.Y, keyValue);
                case PropertyNameResolver.ANIMATION_CLIP_PROPERTY_LOCAL_SCALE_Z:
                    return TransformConverter.ConvertValue(convertAxis, TransformConverter.TransformType.Scale, TransformConverter.AxisDirection.Z, keyValue);
                default:
                    throw new Exception($"[PlayableDirectorComponentInfo.ConvertTransformCurveValue] invalid property: {binding.propertyName}");
            }
        }
    }
}
