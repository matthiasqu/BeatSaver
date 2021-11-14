using UnityEngine;
using Utils;

namespace Game
{
    /// <summary>
    ///     Detects Collisions and Triggers with GameObjects with attached <see cref="Block" /> components and fires an event
    ///     supplying the GameObject corresponding GameObject.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class BlockDetector : MonoBehaviour
    {
        /// <summary>
        ///     Supplies GameObjects with attached <see cref="Block" /> components whenever a Trigger or Collision is detected.
        /// </summary>
        [SerializeField] private UnityEventGameObject onBlockDetected = new UnityEventGameObject();

        /// <summary>
        ///     Invokes <see cref="onBlockDetected" /> if the collision has an attached <see cref="Block" /> component.
        /// </summary>
        /// <param name="other">The object with which the collision occured.</param>
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<Block>() != null) onBlockDetected.Invoke(other.gameObject);
        }

        /// <summary>
        ///     Invokes <see cref="onBlockDetected" /> if <see cref="other" /> has a <see cref="Block" /> component attached.
        /// </summary>
        /// <param name="other">The trigger of the other GameObject</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Block>() != null) onBlockDetected.Invoke(other.gameObject);
        }
    }
}