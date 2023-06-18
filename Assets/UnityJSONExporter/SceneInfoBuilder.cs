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

            // var objectInfoList = new List<ObjectInfo>();

            var objectInfoList = RecursiveBuildGameObjectInfo();
            // // シーンのルートオブジェクトを取得
            // var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            // foreach (var rootObject in rootObjects)
            // {
            //     Debug.Log($"[SceneInfo.GenerateSceneInfo] object name: {rootObject.name}");
            //     var objectInfo = new ObjectInfo(rootObject);
            //     objectInfo.Name = rootObject.name;

            //     var componentInfoList = ParseComponents(rootObject);
            //     objectInfo.Components = componentInfoList;

            //     objectInfoList.Add(objectInfo);

            //     // TODO: recursive exec

            //     // // シーンのルートオブジェクトの子オブジェクトを取得
            //     // var childObjects = rootObject.GetComponentsInChildren<Transform>();
            //     // foreach (var childObject in childObjects)
            //     // {
            //     //     // 子オブジェクトの名前を表示
            //     //     Debug.Log(childObject.name);

            //     //     var go = childObject.gameObject;
            //     //     var componentInfoList = ParseComponents(go);
            //     // }
            // }

            // var hierarchy = new Hierarchy();
            // sceneInfo.Hierarchy = hierarchy;
            // hierarchy.Objects = objectInfoList;

            sceneInfo.Objects = objectInfoList;

            return sceneInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static List<ObjectInfo> RecursiveBuildGameObjectInfo()
        {
            var objectInfoList = new List<ObjectInfo>();

            // シーンのルートオブジェクトを取得
            var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var rootObject in rootObjects)
            {
                Debug.Log($"[SceneInfo.GenerateSceneInfo] object name: {rootObject.name}");
                var objectInfo = GenerateObjectInfo(rootObject);
                foreach (Transform child in rootObject.transform)
                {
                    InternalRecursiveBuildGameObjectInfo(child.gameObject, ref objectInfo);
                }

                // InternalRecursiveBuildGameObjectInfo(rootObject, ref objectInfo);

                objectInfoList.Add(objectInfo);
            }

            return objectInfoList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        static ObjectInfo GenerateObjectInfo(GameObject go)
        {
            var objectInfo = new ObjectInfo(go);
            objectInfo.Components = ParseComponents(go);
            return objectInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        static void InternalRecursiveBuildGameObjectInfo(GameObject go, ref ObjectInfo parent)
        {
            var objectInfo = GenerateObjectInfo(go);
            parent.AddChild(objectInfo);

            foreach (Transform child in go.transform)
            {
                InternalRecursiveBuildGameObjectInfo(child.gameObject, ref objectInfo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
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
