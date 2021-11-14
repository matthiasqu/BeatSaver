using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    /// <summary>
    ///     Fires an event after Start() has been called, but with a delay of <see cref="delay" /> in ms.
    /// </summary>
    public class DelayedEventSender : MonoBehaviour
    {
        [SerializeField] private int delay;
        [SerializeField] private bool autoStart = true;
        [SerializeField] private bool triggerOnEnable;
        [SerializeField] private UnityEvent onDelayPassed = new UnityEvent();

        private async void Start()
        {
            if (!autoStart) return;
            await Delay();
        }

        private async void OnEnable()
        {
            if (triggerOnEnable) await Delay();
        }

        [Button]
        public async void Invoke()
        {
            await Delay();
        }

        private async Task Delay()
        {
            await Task.Delay(delay);
            onDelayPassed.Invoke();
        }
    }
}