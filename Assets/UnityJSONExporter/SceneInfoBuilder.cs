using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace UnityJSONExporter
{
    /// <summary>
    /// 
    /// </summary>
    public class SceneInfoBuilder
    {
        public const string EXCLUDE_TAG = "ExcludeExport";

        // ---------------------------------------------------------------------------------------------
        // public
        // ---------------------------------------------------------------------------------------------

        public SceneInfoBuilder(ConvertAxis exportAxis)
        {
            _convertAxis = exportAxis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SceneInfo GenerateSceneInfo()
        {
            // for debug
            // Debug.Log("[SceneInfo.GenerateSceneInfo]");

            var sceneInfo = new SceneInfo();
            sceneInfo.Name = SceneManager.GetActiveScene().name;

            var objectInfoList = RecursiveBuildGameObjectInfo();

            sceneInfo.Objects = objectInfoList;

            return sceneInfo;
        }

        // ---------------------------------------------------------------------------------------------
        // private
        // ---------------------------------------------------------------------------------------------

        private ConvertAxis _convertAxis;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<ObjectInfo> RecursiveBuildGameObjectInfo()
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
                    if (!child.gameObject.tag.Equals(EXCLUDE_TAG))
                    {
                        InternalRecursiveBuildGameObjectInfo(child.gameObject, ref objectInfo);
                    }
                }

                objectInfoList.Add(objectInfo);
            }

            return objectInfoList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        void InternalRecursiveBuildGameObjectInfo(GameObject go, ref ObjectInfo parent)
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
        ObjectInfo GenerateObjectInfo(GameObject go)
        {
            var objectInfo = new ObjectInfo(go, _convertAxis);
            objectInfo.Components = ParseComponents(go);
            return objectInfo;
        }

        /// <summary>
        /// parseしたいcomponentを列挙
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        List<ComponentInfoBase> ParseComponents(GameObject go)
        {
            var componentInfoList = new List<ComponentInfoBase>();

            //
            // camera
            //
            if (go.TryGetComponent(out Camera camera))
            {
                var cameraComponentInfo = new CameraComponentInfo(camera);
                componentInfoList.Add(cameraComponentInfo);
            }

            //
            // light
            //
            if (go.TryGetComponent(out Light light))
            {
                var lightComponentInfo = new LightComponentInfo(light);
                componentInfoList.Add(lightComponentInfo);
            }

            //
            // mesh renderer
            //
            if (go.TryGetComponent(out MeshRenderer meshRenderer))
            {
                var meshRendererComponentInfo = new MeshRendererComponentInfo(meshRenderer);
                componentInfoList.Add(meshRendererComponentInfo);
            }

            //
            // mesh filter
            //
            if (go.TryGetComponent(out MeshFilter meshFilter))
            {
                var meshFilterComponentInfo = new MeshFilterComponentInfo(meshFilter);
                componentInfoList.Add(meshFilterComponentInfo);
            }

            //
            // playable director
            //
            if (go.TryGetComponent(out PlayableDirector playableDirector))
            {
                var playableDirectorComponentInfo = new PlayableDirectorComponentInfo(playableDirector, _convertAxis);
                componentInfoList.Add(playableDirectorComponentInfo);
            }

            return componentInfoList;
        }
    }
}
