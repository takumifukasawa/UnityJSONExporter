using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace UnityJSONExporter
{
        public class PropertyNameSwitchResolver : DefaultContractResolver
    {
        private bool _minifyNameEnabled;

        public PropertyNameSwitchResolver(bool minifyNameEnabled)
        {
            _minifyNameEnabled = minifyNameEnabled;
            NamingStrategy = new CamelCaseNamingStrategy();
            // {
            //     // OverrideSpecifiedNames = overrideSpecifiedNames
            //     OverrideSpecifiedNames = true
            // };
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var originalPropertyName = member.Name;
            var propertyName = property.PropertyName;
            var jsonProperty = member.GetCustomAttributes<JsonPropertyAttribute>();
           
            // for debug
            // Debug.Log($"property name: ${property.PropertyName}, minify name enabled: {_minifyNameEnabled}, original property name: {originalPropertyName[0]}");

            if (jsonProperty != null && _minifyNameEnabled)
            {
                property.PropertyName = propertyName;
            }
            else
            {
                property.PropertyName = Char.ToLowerInvariant(originalPropertyName[0]) + originalPropertyName.Substring(1);
            }

            // for debug
            // Debug.Log($"[PropertyNameSwitchResolver] old name: {originalPropertyName} -> new name: {property.PropertyName}");

            return property;
        }
    }
}
