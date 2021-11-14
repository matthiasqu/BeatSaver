using UnityEngine;

namespace Utils
{
    public class PositionSetter : MonoBehaviour
    {
        /// <summary>
        ///     Whether to negate the incoming values
        /// </summary>
        [SerializeField] private bool inverse;

        /// <summary>
        ///     Should the x-position be set?
        /// </summary>
        [SerializeField] private bool setX;

        /// <summary>
        ///     Should the y-position be set?
        /// </summary>
        [SerializeField] private bool setY;

        /// <summary>
        ///     Should the z-position be set?
        /// </summary>
        [SerializeField] private bool setZ;

        private Rigidbody _rb;
        private bool _useRb;

        /// <summary>
        ///     Checks whether to set the position using of an attached Rigidbody component or the Transform's position.
        /// </summary>
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            if (_rb != null) _useRb = true;
        }

        /// <summary>
        ///     Uses the incoming Vector3 to set the position according to the setX, setY, and setZ settings above.
        /// </summary>
        /// <param name="vector"></param>
        public void SetPosition(Vector3 vector)
        {
            var t = transform;
            var position = _useRb ? _rb.position : t.position;

            vector = vector * (inverse ? -1 : 1);

            var newPosition = new Vector3(setX ? vector.x : position.x, setY ? vector.y : position.y,
                setZ ? vector.z : position.z);

            if (_useRb) _rb.position = newPosition;
            else t.position = newPosition;

            //Debug.Log($"[PositionSetter] {name} position set to {newPosition}, input {vector}");
        }
    }
}