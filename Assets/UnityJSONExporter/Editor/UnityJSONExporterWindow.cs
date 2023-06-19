using System;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace UnityJSONExporter
{
    public class UnityJSONExporterWindow : EditorWindow
    {
        private bool _dryRun = false;
        private bool _prettyFormat = false;
        private string _fileName = "file_name";
        private string _exportDirectoryPath = "";

        // private bool _groupEnabled;
        // private bool _myBool = true;
        // private float _myFloat = 1.23f;

        /// <summary>
        /// メニューアイテムを追加してウィンドウを開く
        /// </summary>
        [MenuItem("Window/Unity JSON Exporter Window")]
        public static void ShowWindow()
        {
            // 既存のウィンドウを表示する、または新しいウィンドウを作成する
            GetWindow<UnityJSONExporterWindow>("Unity JSON Exporter Window");
        }

        /// <summary>
        /// ここに GUI コントロールを描画するコードを書く
        /// </summary>
        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            _dryRun = EditorGUILayout.Toggle("Dry Run", _dryRun);
            _prettyFormat = EditorGUILayout.Toggle("Pretty Format", _prettyFormat);

            _fileName = EditorGUILayout.TextField("File Name", _fileName);

            EditorGUI.BeginDisabledGroup(true);
            _exportDirectoryPath = EditorGUILayout.TextField("Export Path", _exportDirectoryPath);
            EditorGUI.EndDisabledGroup();

            // _groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", _groupEnabled);
            // _myBool = EditorGUILayout.Toggle("Toggle", _myBool);
            // _myFloat = EditorGUILayout.Slider("Slider", _myFloat, -3, 3);
            // EditorGUILayout.EndToggleGroup();

            GUILayout.Space(13);

            if (GUILayout.Button("Open Folder"))
            {
                var selectedPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                Debug.Log($"[UnityJSONExporterWindow] selected path name: {selectedPath}");
                Debug.Log($"[UnityJSONExporterWindow] click cancel button: {string.IsNullOrEmpty(selectedPath)}");
                _exportDirectoryPath = selectedPath;
            }

            if (GUILayout.Button("Process"))
            {
                // var projectPath = System.IO.Path.GetDirectoryName(Application.dataPath);
                // Debug.Log($"[UnityJSONExporterWindow] persistent data path: {Application.persistentDataPath}");
                // Debug.Log($"[UnityJSONExporterWindow] data path: {Application.dataPath}");
                // Debug.Log($"[UnityJSONExporterWindow] project path: {projectPath}");

                // var fileName = "test";
                // var filePath = $"{fileName}.txt";
                // var writeFilePath = $"{projectPath}/../{filePath}";

                var filePath = $"{_fileName}.json";
                var writeFilePath = Path.Combine(_exportDirectoryPath, filePath);

                Debug.Log($"[UnityJSONExporterWindow] write file path: {writeFilePath}");

                var sceneInfo = SceneInfoBuilder.GenerateSceneInfo();

                // var jsonContent = JsonUtility.ToJson(sceneInfo);
                var jsonContent = JsonConvert.SerializeObject(sceneInfo, _prettyFormat ? Formatting.Indented : Formatting.None);

                Debug.Log($"[UnityJSONExporterWindow] json content: {jsonContent}");

                if (_dryRun)
                {
                    return;
                }

                if (File.Exists(writeFilePath))
                {
                    File.Delete(writeFilePath);
                }

                File.WriteAllText(writeFilePath, jsonContent);
            }
        }
    }
}
