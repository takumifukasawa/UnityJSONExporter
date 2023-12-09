using System;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace UnityJSONExporter
{
    public class UnityJSONExporterWindow : EditorWindow
    {
        // ---------------------------------------------------------------------------------------------
        // serialize
        // ---------------------------------------------------------------------------------------------

        [SerializeField]
        private WebSocketConnector _webSocketConnector;

        [SerializeField]
        private bool _dryRun = false;

        [SerializeField]
        private bool _prettyFormat = false;

        [SerializeField]
        private bool _minifyPropertyName = false;

        [SerializeField]
        private ConvertAxis _convertAxis = ConvertAxis.Default;

        [SerializeField]
        private string _fileName = "file_name";

        [SerializeField]
        private string _exportDirectoryPath = "";

        [SerializeField]
        private bool _autoRun = false;

        [SerializeField]
        private float _autoRunInterval = 1f;

        [SerializeField]
        private bool _showExportLog = false;

        // ---------------------------------------------------------------------------------------------
        // unity engine
        // ---------------------------------------------------------------------------------------------

        /// <summary>
        /// ここに GUI コントロールを描画するコードを書く
        /// </summary>
        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            _webSocketConnector = (WebSocketConnector)EditorGUILayout.ObjectField("WebSocket Connector", _webSocketConnector, typeof(WebSocketConnector), true);
            _showExportLog = EditorGUILayout.Toggle("Show Export Log", _showExportLog);
            _dryRun = EditorGUILayout.Toggle("Dry Run", _dryRun);
            _prettyFormat = EditorGUILayout.Toggle("Pretty Format", _prettyFormat);
            _minifyPropertyName = EditorGUILayout.Toggle("Minify Property Name", _minifyPropertyName);
            _convertAxis = (ConvertAxis)EditorGUILayout.EnumPopup("Convert Axis", _convertAxis);

            GUILayout.Space(13);

            _autoRun = EditorGUILayout.Toggle("Auto Run", _autoRun);
            _autoRunInterval = EditorGUILayout.FloatField("Auto Run Interval", _autoRunInterval);

            GUILayout.Space(13);

            _fileName = EditorGUILayout.TextField("File Name", _fileName);

            EditorGUI.BeginDisabledGroup(true);
            _exportDirectoryPath = EditorGUILayout.TextField("Export Path", _exportDirectoryPath);
            EditorGUI.EndDisabledGroup();

            // _groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", _groupEnabled);
            // _myBool = EditorGUILayout.Toggle("Toggle", _myBool);
            // _myFloat = EditorGUILayout.Slider("Slider", _myFloat, -3, 3);
            // EditorGUILayout.EndToggleGroup();
            
            GUILayout.Space(13);

            if (GUILayout.Button("Connect WebSocket"))
            {
                _webSocketConnector.Connect();
            }

            if (GUILayout.Button("Close WebSocket"))
            {
                _webSocketConnector.Close();
            }

            GUILayout.Space(13);

            if (GUILayout.Button("Open Folder"))
            {
                var selectedPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                // for debug
                // Debug.Log($"[UnityJSONExporterWindow] selected path name: {selectedPath}");
                // Debug.Log($"[UnityJSONExporterWindow] click cancel button: {string.IsNullOrEmpty(selectedPath)}");
                _exportDirectoryPath = selectedPath;
            }

            if (GUILayout.Button("Export"))
            {
                Export();
            }
        }

        /// <summary>
        /// メニューアイテムを追加してウィンドウを開く
        /// </summary>
        [MenuItem("Window/Unity JSON Exporter Window")]
        public static void ShowWindow()
        {
            // 既存のウィンドウを表示する、または新しいウィンドウを作成する
            GetWindow<UnityJSONExporterWindow>("Unity JSON Exporter Window");
        }

        private string SAVE_KEY = "UnityJSONExporterWindowSaveData";

        private void OnEnable()
        {
            EditorApplication.update += OnUpdateEditor;
            // var defaultSaveData = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            // {
            //     ContractResolver = new PropertyNameSwitchResolver(false),
            //     Formatting = Formatting.None
            // });
            // var saveDataString = EditorPrefs.GetString(SAVE_KEY, defaultSaveData);
            // var saveData = JsonConvert.DeserializeObject<UnityJSONExporterWindow>(saveDataString);
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnUpdateEditor;
            // var saveData = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            // {
            //     ContractResolver = new PropertyNameSwitchResolver(false),
            //     Formatting = Formatting.None
            // });
            // EditorPrefs.SetString(SAVE_KEY, saveData);
        }

        void OnUpdateEditor()
        {
            if (!_autoRun)
            {
                return;
            }

            if (_prevAutoExportTime == null)
            {
                _prevAutoExportTime = DateTime.Now;
            }

            if ((DateTime.Now - _prevAutoExportTime).TotalSeconds < _autoRunInterval)
            {
                return;
            }

            _prevAutoExportTime = DateTime.Now;

            Export();
        }

        private DateTime _prevAutoExportTime;

        // ---------------------------------------------------------------------------------------------
        // private
        // ---------------------------------------------------------------------------------------------


        /// <summary>
        /// 
        /// </summary>
        async void Export()
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

            // for debug
            // Debug.Log($"[UnityJSONExporterWindow] write file path: {writeFilePath}");

            var sceneInfo = (new SceneInfoBuilder(_convertAxis)).GenerateSceneInfo();

            // var jsonContent = JsonUtility.ToJson(sceneInfo);
            // var jsonContent = JsonConvert.SerializeObject(sceneInfo, _prettyFormat ? Formatting.Indented : Formatting.None);
            var jsonContent = JsonConvert.SerializeObject(sceneInfo, new JsonSerializerSettings()
            {
                ContractResolver = new PropertyNameSwitchResolver(_minifyPropertyName),
                Formatting = _prettyFormat ? Formatting.Indented : Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // vector3のnormalizedの無限参照対策
            });

            if (_dryRun)
            {
                Debug.Log($"[UnityJSONExporterWindow] dry json content: {jsonContent}");
                return;
            }

            if (File.Exists(writeFilePath))
            {
                File.Delete(writeFilePath);
            }

            File.WriteAllText(writeFilePath, jsonContent);

            if (_webSocketConnector)
            {
                var exportSceneMessageContent = JsonConvert.SerializeObject(new ExportSceneMessage(), new JsonSerializerSettings()
                {
                    ContractResolver = new PropertyNameSwitchResolver(false),
                    Formatting = Formatting.None
                });
                _webSocketConnector.TrySendText(exportSceneMessageContent);
            }

            if (_showExportLog)
            {
                Debug.Log($"[UnityJSONExporterWindow] complete write: {writeFilePath}");
            }
        }

        // private bool _groupEnabled;
        // private bool _myBool = true;
        // private float _myFloat = 1.23f;

    /// <summary>
    /// 
    /// </summary>
    private class ExportSceneMessage
    {
        public string Type = "exportScene";
    }
    }
}
