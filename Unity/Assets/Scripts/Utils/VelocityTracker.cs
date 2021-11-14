using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class VelocityTracker : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private float velocity;

        [SerializeField] private UnityEventFloat onVelocityChanged = new UnityEventFloat();

        private Vector3 _lastPosition;

        public UnityEventFloat ONVelocityChanged => onVelocityChanged;

        public float Velocity
        {
            get => velocity;
            private set
            {
                velocity = value;
                onVelocityChanged.Invoke(velocity);
            }
        }

        private void Start()
        {
            _lastPosition = transform.position;
        }

        private void FixedUpdate()
        {
            var position = transform.position;
            var distance = Vector3.Distance(_lastPosition, position);
            Velocity = distance / Time.fixedDeltaTime;
            _lastPosition = position;
        }
    }
}