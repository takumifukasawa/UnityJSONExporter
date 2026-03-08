#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityJSONExporter
{
    public static class SerializedPropertyAttributeUtilities
    {
        static readonly BindingFlags Flags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

        // キャッシュ（Editor中はOK）
        static readonly Dictionary<(Type, string), FieldInfo> _fieldCache = new();

        /// <summary>
        /// SerializedProperty が指している最終フィールドに TAttr が付いているか
        /// </summary>
        public static bool HasAttribute<TAttr>(SerializedProperty prop, bool inherit = true) where TAttr : Attribute
            => GetFieldInfo(prop) is FieldInfo fi && fi.IsDefined(typeof(TAttr), inherit);

        /// <summary>
        /// そのフィールドから属性を全取得
        /// </summary>
        public static TAttr[] GetAttributes<TAttr>(SerializedProperty prop, bool inherit = true) where TAttr : Attribute
            => GetFieldInfo(prop) is FieldInfo fi
                ? (TAttr[])fi.GetCustomAttributes(typeof(TAttr), inherit)
                : Array.Empty<TAttr>();

        /// <summary>
        /// SerializedProperty の propertyPath を解釈して最終 FieldInfo を返す
        /// （配列 / List / ネスト対応）
        /// </summary>
        public static FieldInfo GetFieldInfo(SerializedProperty prop)
        {
            if (prop == null || prop.serializedObject == null) return null;

            var target = prop.serializedObject.targetObject;
            var rootType = target.GetType();
            var cacheKey = (rootType, prop.propertyPath);
            if (_fieldCache.TryGetValue(cacheKey, out var cached)) return cached;

            // 例: "foo.bar.Array.data[0].baz"
            var path = prop.propertyPath.Replace(".Array.data[", "["); // "foo.bar[0].baz"
            var parts = path.Split('.');

            Type curType = rootType;
            FieldInfo fi = null;

            foreach (var raw in parts)
            {
                if (raw.EndsWith("]")) // 配列 or List のインデクサ
                {
                    // セグメント例: "items[3]"
                    var bracket = raw.IndexOf('[');
                    var fieldName = raw.Substring(0, bracket);
                    fi = GetFieldInTypeHierarchy(curType, fieldName);
                    if (fi == null) return null;

                    // 次の探索は要素型で続ける
                    curType = GetElementType(fi.FieldType);
                }
                else
                {
                    fi = GetFieldInTypeHierarchy(curType, raw);
                    if (fi == null) return null;
                    curType = fi.FieldType;
                }
            }

            _fieldCache[cacheKey] = fi;
            return fi;
        }

        static FieldInfo GetFieldInTypeHierarchy(Type type, string name)
        {
            // 継承をさかのぼってフィールドを探す（privateも拾う）
            while (type != null)
            {
                var fi = type.GetField(name, Flags);
                if (fi != null) return fi;
                type = type.BaseType;
            }

            return null;
        }

        static Type GetElementType(Type t)
        {
            if (t.IsArray) return t.GetElementType();
            if (typeof(IList).IsAssignableFrom(t))
            {
                // List<T> など
                if (t.IsGenericType) return t.GetGenericArguments()[0];
                // 非ジェネリック IList は object にフォールバック
                return typeof(object);
            }

            return t; // そもそも配列/リストではない
        }
    }
}
#endif
