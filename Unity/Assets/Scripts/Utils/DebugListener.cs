using System;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class DebugListener : MonoBehaviour
    {
        [SerializeField] private TextSetter textSetter;
        [SerializeField] private bool alwayShowErrors = true;
        [SerializeField] private bool useFilter;

        [SerializeField] [HideIf("@!useFilter")]
        private string filter;

        [SerializeField] [ReadOnly] [HideIf("@!useFilter")]
        private string filterPattern;

        // Start is called before the first frame update
        private void Start()
        {
            filterPattern = Regex.Escape("[") + filter + Regex.Escape("]");
            if (textSetter == null) textSetter = GetComponentInChildren<TextSetter>();
            Application.logMessageReceivedThreaded += PrintLogMessage;
        }
        
        private void PrintLogMessage(string condition, string stacktrace, LogType type) {
            if (Regex.IsMatch(condition, filterPattern) || alwayShowErrors &
                (type == LogType.Error || type == LogType.Exception))
                textSetter.SetText($"{condition}\n");
        }

        private void OnDestroy() => Application.logMessageReceivedThreaded -= PrintLogMessage;
    }
}