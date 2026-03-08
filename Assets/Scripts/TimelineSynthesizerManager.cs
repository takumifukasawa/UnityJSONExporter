using System;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityJSONExporter;

namespace TimelineSynthesizer
{
    /// <summary>
    /// 
    /// </summary>
    public class TimelineSynthesizerSeekData
    {
        public string Type = "seekTimeline";
        public float CurrentTime;

        public TimelineSynthesizerSeekData(float time)
        {
            CurrentTime = time;
        }
    }

    public class TimelineSynthesizerPlayData
    {
        public string Type = "playTimeline";
        public float CurrentTime;

        public TimelineSynthesizerPlayData(float time)
        {
            CurrentTime = time;
        }
    }

    public class TimelineSynthesizerStopData
    {
        public string Type = "stopTimeline";
    }

    public class TimelineSynthesizerSceneViewEnabledData
    {
        public string Type = "setSceneViewEnabled";
        public bool Enabled;

        public TimelineSynthesizerSceneViewEnabledData(bool enabled)
        {
            Enabled = enabled;
        }
    }

    public class TimelineSynthesizerSceneViewData
    {
        public string Type = "setSceneViewData";
        public RawVector3 CameraPosition;
        public RawVector4 CameraRotation;
        public float CameraFov;
        public float CameraNear;
        public float CameraFar;
    }
    
    public class TimelineSynthesizerBeginPlayerData
    {
        public string Type = "beginPlayer";
    }

    public class TimelineSynthesizerReloadData
    {
        public string Type = "reload";
    }

    /// <summary>
    /// 
    /// </summary>
    [ExecuteAlways]
    public class TimelineSynthesizerManager : MonoBehaviour
    {
        // ----------------------------------------------------------------------------
        // members, fields
        // ----------------------------------------------------------------------------

        // serialize --------------------------------------------

        [SerializeField]
        private WebSocketConnector _webSocketConnector;

        [SerializeField]
        private PlayableDirector _playableDirector;

        [SerializeField]
        private Camera _mainCamera;

        // [SerializeField]
        // private SceneViewActivitySensor _sceneViewActivitySensor;

        // [Space(13)]
        // [SerializeField]
        // private float _sendInterval = 1f;

        [Space(13)]
        [SerializeField]
        private float _seekIntervalFps = 5f;

        [SerializeField]
        private float _sceneViewDataIntervalFps = 5f;

        [SerializeField]
        private bool _showLog = false;

        [SerializeField]
        private bool _sceneViewEnabled = false;

        [SerializeField]
        private bool _syncSceneViewToMainCamera = false;

        // public --------------------------------------------

        public bool IsPlaying => _isPlaying;

        // private --------------------------------------------

        private bool _isPlaying = false;
        private DateTime _prevSeekTime;
        private DateTime _prevSceneViewDataTime;

        // ----------------------------------------------------------------------------
        // methods
        // ----------------------------------------------------------------------------

        // engine --------------------------------------------

        private void OnValidate()
        {
            // TODO: 間引きたい
            SendSceneViewEnabled();
        }

        void Update()
        {
#if UNITY_EDITOR
            SendSeek();

            if (_syncSceneViewToMainCamera && _mainCamera != null)
            {
                SyncSceneViewToMainCamera();
            }

            // // TODO: 間引きたい
            // SendSceneViewEnabled();
            // if (_sceneViewEnabled)
            // {
            //     SendSceneViewData();
            // }
#endif
        }

        // private --------------------------------------------

        async void SendSeek()
        {
            if (_playableDirector == null || _webSocketConnector == null)
            {
                return;
            }

            if (!_webSocketConnector.CanSend)
            {
                return;
            }

            // guard by interval
            if ((DateTime.Now - _prevSeekTime).TotalSeconds < Mathf.Max(1f / Mathf.Max(0.1f, _seekIntervalFps), 1f / 60f))
            {
                return;
            }

            if (_isPlaying)
            {
                return;
            }

            // LoggerProxy.Log($"diff: {(DateTime.Now - _prevSeekTime).TotalSeconds}");
            _prevSeekTime = DateTime.Now;

            var data = new TimelineSynthesizerSeekData((float)_playableDirector.time);

            var jsonContent = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ContractResolver = new PropertyNameSwitchResolver(false),
                Formatting = Formatting.None
            });

            if (_showLog)
            {
                LoggerProxy.Log("[WebSocketConnector.SendSeek] sending...");
                LoggerProxy.Log($"[WebSocketConnector.SendSeek] send json: {jsonContent}");
            }

            _webSocketConnector.TrySendText(jsonContent);
        }

        public async void SendPlay()
        {
            if (_isPlaying)
            {
                return;
            }

            _isPlaying = true;

            var data = new TimelineSynthesizerPlayData((float)_playableDirector.time);

            var jsonContent = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ContractResolver = new PropertyNameSwitchResolver(false),
                Formatting = Formatting.None
            });

            if (_showLog)
            {
                LoggerProxy.Log("[WebSocketConnector.SendPlay] sending...");
                LoggerProxy.Log($"[WebSocketConnector.SendPlay] send json: {jsonContent}");
            }

            _webSocketConnector.TrySendText(jsonContent);
        }

        public async void SendStop()
        {
            if (!_isPlaying)
            {
                return;
            }

            _isPlaying = false;

            var data = new TimelineSynthesizerStopData();

            var jsonContent = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ContractResolver = new PropertyNameSwitchResolver(false),
                Formatting = Formatting.None
            });


            if (_showLog)
            {
                LoggerProxy.Log("[WebSocketConnector.SendStop] sending...");
                LoggerProxy.Log($"[WebSocketConnector.SendStop] send json: {jsonContent}");
            }

            _webSocketConnector.TrySendText(jsonContent);
        }

        public void SendSceneViewEnabled()
        {
            if (_webSocketConnector == null)
            {
                return;
            }

            var data = new TimelineSynthesizerSceneViewEnabledData(_sceneViewEnabled);

            var jsonContent = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ContractResolver = new PropertyNameSwitchResolver(false),
                Formatting = Formatting.None
            });

            if (_showLog)
            {
                LoggerProxy.Log("[WebSocketConnector.SendSceneViewEnabled] sending...");
                LoggerProxy.Log($"[WebSocketConnector.SendSceneViewEnabled] send json: {jsonContent}");
            }

            _webSocketConnector.TrySendText(jsonContent);
        }

        public void SendSceneViewData()
        {
            // guard by interval
            if ((DateTime.Now - _prevSceneViewDataTime).TotalSeconds < Mathf.Max(1f / Mathf.Max(0.1f, _sceneViewDataIntervalFps), 1f / 60f))
            {
                return;
            }

            _prevSceneViewDataTime = DateTime.Now;


            var sceneCamera = SceneView.lastActiveSceneView.camera;
            var data = new TimelineSynthesizerSceneViewData()
            {
                CameraPosition = RawVector3.fromVector3Info(TransformConverter.ConvertPosition(ConvertAxis.RightHand, sceneCamera.transform.position)),
                CameraRotation = RawVector4.fromVector4Info(TransformConverter.ConvertRotationQuaternion(ConvertAxis.RightHand, sceneCamera.transform.rotation)),
                CameraFov = sceneCamera.fieldOfView,
                CameraNear = sceneCamera.nearClipPlane,
                CameraFar = sceneCamera.farClipPlane
            };

            SendSceneViewEnabled();

            var jsonContent = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ContractResolver = new PropertyNameSwitchResolver(false),
                Formatting = Formatting.None
            });

            if (_showLog)
            {
                LoggerProxy.Log("[WebSocketConnector.SendSceneViewData] sending...");
                LoggerProxy.Log($"[WebSocketConnector.SendSceneViewData] send json: {jsonContent}");
            }

            _webSocketConnector.TrySendText(jsonContent);
        }

        public void SendBeginPlayer()
        {
            if (_webSocketConnector == null)
            {
                return;
            }

            var data = new TimelineSynthesizerBeginPlayerData();

            var jsonContent = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ContractResolver = new PropertyNameSwitchResolver(false),
                Formatting = Formatting.None
            });

            if (_showLog)
            {
                LoggerProxy.Log("[WebSocketConnector.SendBeginPlayer] sending...");
                LoggerProxy.Log($"[WebSocketConnector.SendBeginPlayer] send json: {jsonContent}");
            }

            _webSocketConnector.TrySendText(jsonContent);
        }
        
        public void SendReload()
        {
            if (_webSocketConnector == null)
            {
                return;
            }

            var data = new TimelineSynthesizerReloadData();

            var jsonContent = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ContractResolver = new PropertyNameSwitchResolver(false),
                Formatting = Formatting.None
            });

            if (_showLog)
            {
                LoggerProxy.Log("[WebSocketConnector.SendReload] sending...");
                LoggerProxy.Log($"[WebSocketConnector.SendReload] send json: {jsonContent}");
            }

            _webSocketConnector.TrySendText(jsonContent);
        }

        private void SyncSceneViewToMainCamera()
        {
#if UNITY_EDITOR
            var sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null || _mainCamera == null)
            {
                return;
            }

            var mainCameraTransform = _mainCamera.transform;

            // Position & Rotation
            sceneView.LookAt(mainCameraTransform.position + mainCameraTransform.forward, mainCameraTransform.rotation);

            // FOV
            sceneView.cameraSettings.fieldOfView = _mainCamera.fieldOfView;

            // Near/Far Clip Plane
            sceneView.cameraSettings.nearClip = _mainCamera.nearClipPlane;
            sceneView.cameraSettings.farClip = _mainCamera.farClipPlane;

            // Scene Viewを再描画
            sceneView.Repaint();

            // Scene ViewデータをWebSocketで送信
            if (_sceneViewEnabled)
            {
                SendSceneViewData();
            }
#endif
        }
    }
}
