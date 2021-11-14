using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     Destructs the GameObject its assigned to after a specific time the <see cref="Trigger" /> method is called.
    /// </summary>
    public class TimedSelfDestruct : MonoBehaviour
    {
        /// <summary>
        ///     The delay in milliseconds after which the GameObject will be destroyed.
        /// </summary>
        [Tooltip("The delay in MS.")] [SerializeField]
        private int delay;

        /// <summary>
        ///     Destroy the Game Object after <see cref="delay" /> milliseconds.
        /// </summary>
        public void Trigger()
        {
            gameObject.DestroyDelayed(delay);
        }
    }
}