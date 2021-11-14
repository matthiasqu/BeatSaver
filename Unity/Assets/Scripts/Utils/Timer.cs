using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    /// <summary>
    ///     A simple timer which triggers an event when the timer reaches the specified threshold.
    /// </summary>
    public class Timer : MonoBehaviour
    {
        [SerializeField] private float threshold;
        [SerializeField] [ReadOnly] private float _time;
        [SerializeField] private UnityEvent onThresholdReached = new UnityEvent();
        private bool _running;

        private void Update()
        {
            if (!_running) return;
            _time += Time.deltaTime;
            if (_time >= threshold)
            {
                onThresholdReached.Invoke();
                StopTimer();
            }
        }

        public void StartTimer()
        {
            _running = true;
        }

        public void StopTimer()
        {
            _running = false;
            _time = 0;
        }
    }
}