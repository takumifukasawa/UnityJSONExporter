using System;
using UnityEngine;

namespace UnityJSONExporter
{
    public static class SceneUtilities
    {
        public static Component FindComponentInScene(Type componentType)
        {
            // var allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            var allObjects = UnityEngine.Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );
            foreach (var obj in allObjects)
            {
                var component = obj.GetComponent(componentType);
                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }
    }
}
