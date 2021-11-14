using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     Reports the position of the GameObject this instance is attached to using a UnityEventVector3.
    /// </summary>
    public class PositionReporter : MonoBehaviour
    {
        /// <summary>
        ///     Reports the new position of the Transform/Rigidbody.
        /// </summary>
        [SerializeField] private UnityEventVector3 onPositionChanged = new UnityEventVector3();

        /// <summary>
        ///     The last position reported by this component.
        /// </summary>
        [SerializeField] [ReadOnly] private Vector3 _lastPosition = Vector3.zero;

        /// <summary>
        ///     The attached Rigidbody if any.
        /// </summary>
        [SerializeField] [ReadOnly] private Rigidbody _rb;

        private bool _useRb;

        /// <summary>
        ///     Checks whether the GameObject has a Rigidbody and sets <see cref="_useRb" /> accordingly.
        /// </summary>
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            if (_rb != null) _useRb = true;
        }

        /// <summary>
        ///     Invokes <see cref="onPositionChanged" /> each frame if the position of the Transform has changed significantly.
        /// </summary>
        private void Update()
        {
            if (_useRb) return;
            var currentPosition = transform.position;

            if (!_lastPosition.Approximates(currentPosition) && !currentPosition.Approximates(Vector3.zero))
                //Debug.Log($"[PositionReporter] Reporting {currentPosition} on {name}");
                onPositionChanged.Invoke(currentPosition);
            _lastPosition = currentPosition;
        }

        /// <summary>
        ///     Invokes <see cref="onPositionChanged" /> each frame if the position of the Rigidbody has changed significantly.
        /// </summary>
        private void FixedUpdate()
        {
            if (!_useRb) return;
            var currentPosition = _rb.position;

            if (!_lastPosition.Approximates(currentPosition) && !currentPosition.Approximates(Vector3.zero))
                //Debug.Log($"[PositionReporter] Reporting {currentPosition} on {name}");
                onPositionChanged.Invoke(currentPosition);
            _lastPosition = currentPosition;
        }
    }
}