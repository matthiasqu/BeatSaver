using System.Linq;
using UnityEngine;
using Utils;

namespace Game
{
    /// <summary>
    ///     Detects collisions and trigger objects with <see cref="Obstacle" /> components.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ObstacleDetector : MonoBehaviour
    {
        /// <summary>
        ///     Invoked whenever an object with an attached obstacle component enters the collider.
        /// </summary>
        /// TODO: Rename to onObstacleDetected.
        [SerializeField] private bool useExit = true;

        [SerializeField] private UnityEventGameObject onBlockDetected = new UnityEventGameObject();

        [SerializeField] private UnityEventGameObject onObstacleStay = new UnityEventGameObject();

        private void Start()
        {
            onBlockDetected.AddListener(o =>
            {
                var listeners = "";
                if (onBlockDetected.GetPersistentEventCount() > 0)
                    listeners = Enumerable.Range(0, onBlockDetected.GetPersistentEventCount())
                        .Select(i => onBlockDetected.GetPersistentMethodName(i)).Aggregate((s1, s2) => $"{s1}, {s2}");
                else listeners = "No listeners assigned.";
                Debug.Log(
                    $@"[ObstacleDetector] Obstacle {o.name} detected by {gameObject.name}. Invoking {listeners}");
            });
        }

        /// <summary>
        ///     Detects collisions with other objects that have an obstacle component attached. Invokes
        ///     <see cref="onBlockDetected" />.
        /// </summary>
        /// <param name="other">Colliding object.</param>
        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.GetComponent<Obstacle>() != null) onBlockDetected.Invoke(other.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (useExit) return;
            if (other.GetComponent<Obstacle>() != null) onBlockDetected.Invoke(other.gameObject);
        }

        /// <summary>
        ///     Detects whether objects entering the attached collider are triggers with an obstacle component attached. Invokes
        ///     <see cref="onBlockDetected" />.
        /// </summary>
        /// <param name="other">Other triggers.</param>
        private void OnTriggerExit(Collider other)
        {
            if (!useExit) return;
            if (other.GetComponent<Obstacle>() != null) onBlockDetected.Invoke(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<Obstacle>() != null) onObstacleStay.Invoke(other.gameObject);
        }
    }
}