#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEngine;

namespace UnityJSONExporter
{
    public static class FieldAttributeLookup
    {
        static readonly BindingFlags Flags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

        // Objectから
        public static bool HasFieldAttribute<TAttr>(UnityEngine.Object obj, string fieldName, bool inherit = true, bool searchBacking = true)
            where TAttr : Attribute
            => HasFieldAttribute<TAttr>(obj.GetType(), fieldName, inherit, searchBacking);

        // Typeから
        public static bool HasFieldAttribute<TAttr>(Type type, string fieldName, bool inherit = true, bool searchBacking = true)
            where TAttr : Attribute
        {
            // 1) そのままのフィールド名
            var fi = GetFieldInHierarchy(type, fieldName);
            if (fi != null) return fi.IsDefined(typeof(TAttr), inherit);

            if (!searchBacking) return false;

            // 2) 自動プロパティの backing field（[field: ...] を拾いたい場合）
            //    プロパティ自体に付いた属性は pi.GetCustomAttributes() で取れるが、
            //    [field:] は backing field に付くためこちらをチェック
            var pi = GetPropertyInHierarchy(type, fieldName);
            if (pi != null)
            {
                var backingName = $"<{pi.Name}>k__BackingField";
                var bfi = GetFieldInHierarchy(type, backingName);
                if (bfi != null) return bfi.IsDefined(typeof(TAttr), inherit);
            }

            return false;
        }

        static FieldInfo GetFieldInHierarchy(Type type, string name)
        {
            while (type != null)
            {
                var fi = type.GetField(name, Flags);
                if (fi != null) return fi;
                type = type.BaseType;
            }

            return null;
        }

        static PropertyInfo GetPropertyInHierarchy(Type type, string name)
        {
            while (type != null)
            {
                var pi = type.GetProperty(name, Flags);
                if (pi != null) return pi;
                type = type.BaseType;
            }

            return null;
        }
    }
}
#endif
