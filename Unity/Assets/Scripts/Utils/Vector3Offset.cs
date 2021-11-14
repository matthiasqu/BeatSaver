using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     Adds the supplied value in <see cref="offset" /> to any incoming Vewctor3 and returns the result using the
    ///     <see cref="onOffsetApplied" /> event.
    /// </summary>
    public class Vector3Offset : MonoBehaviour
    {
        /// <summary>
        ///     The offset to add to any incoming Vector3.
        /// </summary>
        [SerializeField] private Vector3 offset;

        /// <summary>
        ///     The resulting Vector3 after adding <see cref="offset" /> to any incoming value.
        /// </summary>
        [SerializeField] private UnityEventVector3 onOffsetApplied = new UnityEventVector3();

        /// <summary>
        ///     Adds <see cref="offset" /> to <see cref="v" />.
        /// </summary>
        /// <param name="v">The sum of <see cref="v" /> and <see cref="offset" /></param>
        public void ApplyOffset(Vector3 v)
        {
            onOffsetApplied.Invoke(v + offset);
        }
    }
}