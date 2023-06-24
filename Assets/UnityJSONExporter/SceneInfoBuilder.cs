using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace UnityJSONExporter
{
    public static class SceneInfoBuilder
    {
        // ---------------------------------------------------------------------------------------------
        // public
        // ---------------------------------------------------------------------------------------------
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SceneInfo GenerateSceneInfo()
        {
            Debug.Log("[SceneInfo.GenerateSceneInfo]");

            var sceneInfo = new SceneInfo();
            sceneInfo.Name = SceneManager.GetActiveScene().name;

            var objectInfoList = RecursiveBuildGameObjectInfo();

            sceneInfo.Objects = objectInfoList;

            return sceneInfo;
        }
        
        // ---------------------------------------------------------------------------------------------
        // private
        // ---------------------------------------------------------------------------------------------

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
                // for debug
                // Debug.Log($"[SceneInfo.GenerateSceneInfo] object name: {rootObject.name}");
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
        static List<ComponentInfoBase> ParseComponents(GameObject go)
        {
            var componentInfoList = new List<ComponentInfoBase>();

            if (go.TryGetComponent<Light>(out Light light))
            {
                var lightComponentInfo = new LightComponentInfo(light);
                componentInfoList.Add(lightComponentInfo);
            }

            if (go.TryGetComponent<PlayableDirector>(out PlayableDirector playableDirector))
            {
                var playableDirectorComponentInfo = new PlayableDirectorComponentInfo(playableDirector);
                componentInfoList.Add(playableDirectorComponentInfo);
            }

            return componentInfoList;
        }
    }
}
