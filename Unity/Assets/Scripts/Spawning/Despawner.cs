using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Spawning
{
    /// <summary>
    ///     Destroys all Sliceables that enter the trigger.
    /// </summary>
    /// TODO: Rename to SliceableDespawner
    /// TODO: Use a combination of BlockDetector and ObjectDestroyer instead.
    public class Despawner : MonoBehaviour
    {
        [SerializeField] private UnityEvent onScliceableDetected = new UnityEvent();

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Block>() != null)
            {
                Destroy(other.gameObject);
                onScliceableDetected.Invoke();
            }
        }
    }
}