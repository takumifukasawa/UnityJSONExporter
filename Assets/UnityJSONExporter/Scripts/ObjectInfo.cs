using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityJSONExporter
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [JsonConverter(typeof(ObjectInfoConverter))]
    public class ObjectInfo
    {
        // [JsonIgnore]
        // public GameObject InternalGameObject;
        [JsonProperty(PropertyName = "n")]
        public string Name;

        [JsonProperty(PropertyName = "t")]
        // public TransformInfo Transform;
        // [
        //      [lp.x, lp.y, lp.z],
        //      [lr.x, lr.y, lr.z, lr.w],
        //      [ls.x, ls.y, ls.z]
        // ]
        public List<List<float>> Transform;

        [JsonProperty(PropertyName = "co")]
        public List<ComponentInfoBase> Components = null;

        [JsonProperty(PropertyName = "ch")]
        public List<ObjectInfo> Children = null;

        public ObjectInfo(GameObject obj, ConvertAxis axis)
        {
            // // NOTE: spotlightの場合はぐるっと一周させた状態をつくるテスト
            // var needsFacingAwayY = false;
            // if (obj.TryGetComponent(out Light light) && light.type == LightType.Spot)
            // {
            //     // needsFacingAwayY = true;
            // }

            Name = obj.name;

            // ExportTransformコンポーネントの有無をチェック
            bool hasExportTransform = obj.GetComponent<ExportTransform>() != null;

            if (hasExportTransform)
            {
                // ExportTransformがある場合: Transform情報を出力
                var localPosition = obj.transform.localPosition;

                // // NOTE: spotlightの場合はぐるっと一周させた状態をつくるテスト
                // var localRotation = needsFacingAwayY
                //     ? Quaternion.AngleAxis(180f, Vector3.up) * obj.transform.localRotation
                //     : obj.transform.localRotation;
                var localRotation = obj.transform.localRotation;

                var localScale = obj.transform.localScale;

                // for debug
                // Debug.Log("object info name: " + Name);

                // Transform = new TransformInfo()
                // {
                //     LocalPosition = TransformConverter.ConvertPosition(axis, localPosition),
                //     // LocalRotation = TransformConverter.ConvertRotationEuler(axis, localRotation.eulerAngles),
                //     LocalRotation = TransformConverter.ConvertRotationQuaternion(axis, localRotation),
                //     LocalScale = TransformConverter.ConvertScale(axis, localScale)
                // };

                // デフォルト値判定（Convert前）
                bool isDefaultPosition = localPosition == Vector3.zero;
                bool isDefaultRotation = localRotation == Quaternion.identity;
                bool isDefaultScale = localScale == Vector3.one;

                // Convert実行
                var lp = TransformConverter.ConvertPosition(axis, localPosition);
                var lr = TransformConverter.ConvertRotationQuaternion(axis, localRotation);
                var ls = TransformConverter.ConvertScale(axis, localScale);

                // デフォルト値の場合は空配列、それ以外は値を格納
                Transform = new List<List<float>>()
                {
                    isDefaultPosition ? new List<float>() : new List<float>() { lp.X, lp.Y, lp.Z },
                    isDefaultRotation ? new List<float>() : new List<float>() { lr.X, lr.Y, lr.Z, lr.W },
                    isDefaultScale ? new List<float>() : new List<float>() { ls.X, ls.Y, ls.Z }
                };
            }
            else
            {
                // ExportTransformがない場合: 空配列を設定（ファイルサイズ削減）
                Transform = new List<List<float>>();
            }
        }

        public void AddChild(ObjectInfo child)
        {
            if (Children == null)
            {
                Children = new List<ObjectInfo>();
            }

            Children.Add(child);
        }

        public void AddComponent(ComponentInfoBase component)
        {
            if (Components == null)
            {
                Components = new List<ComponentInfoBase>();
            }

            Components.Add(component);
        }

        public void AddComponentRange(List<ComponentInfoBase> components)
        {
            if (Components == null)
            {
                Components = new List<ComponentInfoBase>();
            }

            Components.AddRange(components);
        }
    }
    
    
    /// <summary>
    /// ObjectInfoを配列形式でシリアライズするためのJsonConverter
    /// 配列形式: [name, transform, components, children]
    /// 末尾のnullは省略される
    /// </summary>
    public class ObjectInfoConverter : JsonConverter<ObjectInfo>
    {
        public override void WriteJson(JsonWriter writer, ObjectInfo value, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            // index 0: name
            writer.WriteValue(value.Name);

            // index 1: transform
            serializer.Serialize(writer, value.Transform);

            // index 2: components (nullの場合は空配列)
            serializer.Serialize(writer, value.Components ?? new List<ComponentInfoBase>());

            // index 3: children (nullなら省略)
            if (value.Children != null)
            {
                serializer.Serialize(writer, value.Children);
            }

            writer.WriteEndArray();
        }

        public override ObjectInfo ReadJson(JsonReader reader, Type objectType, ObjectInfo existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            // デシリアライズは未実装（必要に応じて実装）
            throw new NotImplementedException("ObjectInfo deserialization is not implemented");
        }
    }
}
