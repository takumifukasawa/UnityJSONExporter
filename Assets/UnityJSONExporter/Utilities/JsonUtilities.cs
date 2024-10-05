using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityJSONExporter
{
    public static class JsonUtilities
    {
        public static string ResolveJsonProperty<T>(
            T component,
            string targetProperty,
            bool needsMinify
        ) where T : Component
        {
            if (!needsMinify)
            {
                return targetProperty;
            }

            var type = typeof(T);
            var fields = type.GetFields();
            // for debug
            // Debug.Log($"[PlayableDirectorComponentInfo.ResolveJsonProperty] type: {type}, type2: {type2}, properties count: {properties.Length}");
            foreach (var field in fields)
            {
                var jsonProperty = field
                    .GetCustomAttributes(typeof(JsonPropertyAttribute), false)
                    .Cast<JsonPropertyAttribute>()
                    .FirstOrDefault();
                // for debug
                // Debug.Log($"[PlayableDirectorComponentInfo.ResolveJsonProperty] property name: {property.Name}, target property: {targetProperty}, jsonProperty: {jsonProperty}, jsonPropertyName: {jsonProperty.PropertyName}");
                if (field.Name == targetProperty && needsMinify)
                {
                    return jsonProperty.PropertyName;
                }
            }

            // fallback
            return targetProperty;
        }
    }
}
