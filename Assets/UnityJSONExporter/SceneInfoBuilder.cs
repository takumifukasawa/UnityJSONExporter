using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityJSONExporter
{
    public static class SceneInfoBuilder
    {
        public static SceneInfo GenerateSceneInfo()
        {
            Debug.Log("[SceneInfo.GenerateSceneInfo]");

            var sceneInfo = new SceneInfo();
            sceneInfo.Name = SceneManager.GetActiveScene().name;

            var objectInfoList = new List<ObjectInfo>();

            // シーンのルートオブジェクトを取得
            var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var rootObject in rootObjects)
            {
                Debug.Log($"[SceneInfo.GenerateSceneInfo] object name: {rootObject.name}");
                var objectInfo = new ObjectInfo();
                objectInfo.Name = rootObject.name;

                var componentInfoList = ParseComponents(rootObject);
                objectInfo.Components = componentInfoList;

                objectInfoList.Add(objectInfo);

                // TODO: recursive exec

                // // シーンのルートオブジェクトの子オブジェクトを取得
                // var childObjects = rootObject.GetComponentsInChildren<Transform>();
                // foreach (var childObject in childObjects)
                // {
                //     // 子オブジェクトの名前を表示
                //     Debug.Log(childObject.name);

                //     var go = childObject.gameObject;
                //     var componentInfoList = ParseComponents(go);
                // }
            }

            var hierarchy = new Hierarchy();
            sceneInfo.Hierarchy = hierarchy;

            hierarchy.Objects = objectInfoList;

            return sceneInfo;
        }

        public static List<ComponentInfoBase> ParseComponents(GameObject go)
        {
            var componentInfoList = new List<ComponentInfoBase>();

            if (go.TryGetComponent<Light>(out Light light))
            {
                var lightComponentInfo = new LightComponentInfo(light);
                componentInfoList.Add(lightComponentInfo);
            }

            return componentInfoList;
        }
    }
}
