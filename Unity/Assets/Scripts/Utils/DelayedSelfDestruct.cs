using UnityEngine;

namespace Utils
{
    public class DelayedSelfDestruct : MonoBehaviour
    {
        [SerializeField] private int delay = 500;

        private void Start()
        {
            gameObject.DestroyDelayed(delay);
        }
    }
}