using System;
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

                case ConvertAxis.Default:
                    return value;

                default:
                    break;
            }

            throw new Exception("invalid params");
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
    }
}
