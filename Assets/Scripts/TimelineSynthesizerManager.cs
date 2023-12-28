using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Playables;
using UnityJSONExporter;

namespace TimelineSynthesizer
{
    /// <summary>
    /// 
    /// </summary>
    public class TimelineSynthesizerData
    {
        public string Type = "seekTimeline";

        // [JsonProperty(PropertyName = "currentTime")]
        public float CurrentTime;
    }

    /// <summary>
    /// 
    /// </summary>
    [ExecuteInEditMode]
    public class TimelineSynthesizerManager : MonoBehaviour
    {
        // ----------------------------------------------------------------------------
        // serialize
        // ----------------------------------------------------------------------------

        [SerializeField]
        private WebSocketConnector _webSocketConnector;

        [SerializeField]
        private PlayableDirector _playableDirector;

        [Space(13)]
        [SerializeField]
        private float _sendInterval = 1f;

        [SerializeField]
        private bool _showLog = false;

        // ----------------------------------------------------------------------------
        // unity engine
        // ----------------------------------------------------------------------------

        void Update()
        {
#if UNITY_EDITOR
            SendCurrentTime();
#endif
        }

        // ----------------------------------------------------------------------------
        // private
        // ----------------------------------------------------------------------------

        private DateTime _prevDateTimeNow;

        /// <summary>
        /// 
        /// </summary>
        async void SendCurrentTime()
        {
            if (_playableDirector == null || _webSocketConnector == null)
            {
                return;
            }

            if (_prevDateTimeNow == null)
            {
                _prevDateTimeNow = DateTime.Now;
            }

            if (!_webSocketConnector.CanSend)
            {
                return;
            }

            // guard by interval
            if ((DateTime.Now - _prevDateTimeNow).TotalSeconds < Mathf.Max(_sendInterval, 1f / 60f))
            {
                return;
            }

            // Debug.Log($"diff: {(DateTime.Now - _prevDateTimeNow).TotalSeconds}");
            _prevDateTimeNow = DateTime.Now;

            var data = new TimelineSynthesizerData();
            data.CurrentTime = (float)_playableDirector.time;

            var jsonContent = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                ContractResolver = new PropertyNameSwitchResolver(false),
                Formatting = Formatting.None
            });

            if (_showLog)
            {
                Debug.Log("[WebSocketConnector.SendCurrentTime] sending...");
                Debug.Log($"[WebSocketConnector.SendCurrentTime] send json: {jsonContent}");
            }

            _webSocketConnector.TrySendText(jsonContent);
        }
    }
}
