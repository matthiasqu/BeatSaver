using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     Moves the Transform forward with a specific speed each frame. Uses the Rigidbody if present.
    /// </summary>
    public class TransformMover : MonoBehaviour
    {
        /// <summary>
        ///     How many units should the Transform/Rigidbody move per second?
        /// </summary>
        [SerializeField] private float speed = .1f;

        private Rigidbody _rb;
        private Transform _trans;
        private bool _useRb;

        /// <summary>
        ///     The Objects speed in units/second.
        /// </summary>
        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        /// <summary>
        ///     Decides whether to use a Rigidbody or not.
        /// </summary>
        private void Start()
        {
            _trans = transform;
            _rb = GetComponent<Rigidbody>();
            _useRb = _rb != null;
            if (_useRb)
                if (!_rb.isKinematic)
                    Debug.LogWarning(
                        $"[TransformMover]{gameObject.name} has non kinematic rigidbody but is being moved by script.");
        }

        /// <summary>
        ///     Moves the Transform's position each frame.
        /// </summary>
        private void Update()
        {
            if (_useRb) return;
            var posDelta = Vector3.forward * (Speed * Time.deltaTime);
            _trans.position += posDelta;
        }

        /// <summary>
        ///     moves the Rigidbody's position each frame.
        /// </summary>
        private void FixedUpdate()
        {
            if (!_useRb) return;
            var posDelta = Vector3.forward * (Speed * Time.fixedDeltaTime);
            _rb.position += posDelta;
        }
    }
}