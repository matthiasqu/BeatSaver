using UnityEngine;
using UnityEngine.Events;

namespace Game.Tutorial
{
    public class TickListener : MonoBehaviour
    {
        [SerializeField] private int maxTickSize;
        [SerializeField] private UnityEvent relevantTickReceived = new UnityEvent();

        public void ReceiveTick(int tick)
        {
            if (tick <= maxTickSize) relevantTickReceived.Invoke();
        }
    }
}