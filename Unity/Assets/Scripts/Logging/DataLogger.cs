using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using BeatMapper;
using Game;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Spawning;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Utils;

namespace Logging
{
    public class DataLogger : MonoBehaviour
    {
        [SerializeField] private DataLoggerConfiguration config;
        [SerializeField] private int blocksToLog;
        [SerializeField] private int obstaclesToLog;
        [SerializeField] private UnityEventString onServiceFound = new UnityEventString();
        [SerializeField] private UnityEvent onNoServiceFound = new UnityEvent();
        [SerializeField] private UnityEvent onDataLogged = new UnityEvent();
        [SerializeField] private UnityEventString onDataLogError = new UnityEventString();
        [SerializeField] [HideLabel] private ParticipantData participantData;

        private bool isSending = false;

        public UnityEvent<Group> onConditionChanged { get; } = new UnityEvent<Group>();
        public UnityEventInt onIdChanged { get; } = new UnityEventInt();
        public UnityEvent OnServerFound { get; } = new UnityEvent();

        public int BlocksToLog
        {
            get => blocksToLog;
            set => blocksToLog = value;
        }

        public int ObstaclesToLog
        {
            get => obstaclesToLog;
            set => obstaclesToLog = value;
        }

        private void Start()
        {
            onServiceFound.AddListener(s => StopAllCoroutines());

            participantData.Clear();
        }

        private IEnumerable<string> GetIPv4Adresses()
        {
            return Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.Where(
                    f => f.AddressFamily == AddressFamily.InterNetwork)
                .Select(a => a.ToString());
        }

        [Button]
        public void FindLocalServer()
        {
            var localIPs = GetIPv4Adresses().ToArray();

            //localIPs.ForEach(ip => Debug.Log($"[DataLogger] Checking ip from {ip}"));

            var IPs = localIPs.Select(GenerateIpRange).SelectMany(s => s).ToArray();

            // Start coroutines
            StartCoroutine(IsResponding(IPs));

            Debug.Log("[Logger] Service lookup started.");
        }

        private IEnumerable<string> GenerateIpRange(string ip)
        {
            var ipPattern = @"(\d+.){3}";
            var baseIP = Regex.Match(ip, ipPattern);
            return Enumerable.Range(1, 255).Select(i => $"{baseIP.Value}{i}");
        }

        private IEnumerator IsResponding(string[] ips)
        {
            var ipFound = false;
            var requests = ips.Select(ip =>
            {
                var uri = "http://" + ip + ":" + config.port + "/data/status";
                var request = UnityWebRequest.Get(uri);
                request.timeout = config.timeout;
//                Debug.Log($"[Logger] Scanning {uri}");

                var a = request.SendWebRequest();
                return (Ip: ip, Request: a);
            }).ToArray();

            // assign listener to the completed event, aborting other requests
            requests.ForEach(r => r.Request.completed += a =>
            {
                if (r.Request.webRequest.responseCode >= 200 && r.Request.webRequest.responseCode < 300)
                {
                    ipFound = true;
                    requests.ForEach(req =>
                    {
                        if (req != r) req.Request.webRequest.Abort();
                    });
                }
            });

            while (!ipFound) yield return null;

            var succeeded = requests.Where(r =>
                    r.Request.webRequest.responseCode >= 200 && r.Request.webRequest.responseCode < 300)
                .ForEach(r => Debug.Log($"[Logger] Found service at {r.Ip}")).ToArray();
            if (succeeded.Length > 0)
            {
                config.IP = succeeded.First().Ip;
                PlayerPrefs.SetString("IP", config.IP);
                onServiceFound.Invoke(config.IP);
            }
            else
            {
                onNoServiceFound.Invoke();
                Debug.LogWarning("[DataLogger] No service found on local network! :-(");
            }
        }

        public void AddBlock(GameObject n)
        {
            participantData.AddBlock(n);
            if (participantData.detectedBlocks.Count == BlocksToLog)
            {
                Debug.Log("[DataLogger] All blocks detected, sending data.");
                SendData();
            }
        }

        public void AddVelocityLeft(float value)
        {
            participantData.AddVelocity(Hand.left, value);
        }

        public void AddVelocityRight(float value)
        {
            participantData.AddVelocity(Hand.right, value);
        }

        [Button]
        private void PrintSerialized()
        {
            Debug.Log(JsonUtility.ToJson(participantData));
        }

        [Button]
        public void SendData()
        {
            StartCoroutine(Send());
        }

        public IEnumerator Send()
        {
            
            // write to file
            var serializedData = JsonUtility.ToJson(participantData);
            SaveToFile(serializedData);
            
            
            var ip = PlayerPrefs.GetString("IP");
            var URI = $"http://{ip}:{config.port}/{config.path}";

            if (!Uri.IsWellFormedUriString(URI, UriKind.RelativeOrAbsolute))
            {
                Debug.LogWarning($"[DataLogger] Invalid Uri format, saving to local storage only");
                onDataLogError.Invoke($"\nDaten lokal gespeichert!\n{URI}");
            }
            else
            {
                Debug.Log($"[DataLogger] Sending to URI {URI}");

                participantData.id = PlayerPrefs.GetInt("ID");
                participantData.Condition = (Group) PlayerPrefs.GetInt("Condition");
                var bytePayload = new UTF8Encoding().GetBytes(serializedData);

                using UnityWebRequest request = new UnityWebRequest(URI, "POST");

                request.uploadHandler = new UploadHandlerRaw(bytePayload);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.timeout = 5;
                request.SetRequestHeader("Content-Type", "application/json");
                var r = request.SendWebRequest();
                yield return r;

                var res = r.webRequest.result;
                if (res == UnityWebRequest.Result.Success) onDataLogged.Invoke();
                else if (res == UnityWebRequest.Result.ConnectionError ||
                         res == UnityWebRequest.Result.ProtocolError ||
                         res == UnityWebRequest.Result.DataProcessingError)
                    onDataLogError.Invoke($"\nDaten lokal gespeichert!\n{res}");
                else onDataLogError.Invoke($"Saving in progress");
            }
            yield return null;
        }

        [Button]
        private async void SaveToFile(string json)
        {
            var culture = CultureInfo.CreateSpecificCulture("de-DE");
            var path = Path.Combine(new[]
            {
                Application.persistentDataPath,
                "Sessions", DateTime.Now.ToString("d", culture)
            });
            var filename = $"{participantData.id} T{DateTime.Now.Hour}{DateTime.Now.Minute}.json";
            var file = Path.Combine(path, filename);

            Debug.Log($"[DataLogger] Logging participant data to file {file}");

            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                using var writer = File.CreateText(file);
                writer.Write(json);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    [Serializable]
    public class LogBlock
    {
        public string color;
        public string expectedCutDirection;
        public string detectedCutDirection;
        public int lineIndex;
        public int rowIndex;
        public bool hit;
        public float velocity;
        public float time;

        public LogBlock(GameObject blockObject)
        {
            var noteEvent = blockObject.GetComponentInChildren<BlockSettings>().NoteEvent;
            var scoreCalculator = blockObject.GetComponentInChildren<ScoreCalculator>();
            var velocityBuffer = blockObject.GetComponentInChildren<VelocityBuffer>();
            var swordColorThreshold = blockObject.GetComponentInChildren<SwordColorThreshold>();

            color = noteEvent._type.ToString();
            hit = swordColorThreshold.DetectedSwordColor != CubeColor.None;
            time = Time.timeSinceLevelLoad;
            velocity = velocityBuffer.MaxVelocity;
            lineIndex = (int) noteEvent._lineIndex;
            rowIndex = (int) noteEvent._lineLayer;
            expectedCutDirection = noteEvent._cutDirection.ToString();
            detectedCutDirection = scoreCalculator.DetectedCutDirection.ToString();
        }
    }
}