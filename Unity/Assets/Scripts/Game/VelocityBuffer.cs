using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Game
{
    public class VelocityBuffer : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private float _maxVelocity;
        private VelocityTracker _tracker;

        public float MaxVelocity
        {
            get => _maxVelocity;
            private set
            {
                if (value > _maxVelocity)
                    _maxVelocity = value;
            }
        }

        public void SetTrackedController(GameObject controller)
        {
            _tracker = controller.GetComponentInChildren<VelocityTracker>();
            _maxVelocity = _tracker.Velocity;
            _tracker
                .ONVelocityChanged.AddListener(UpdateMaxVelocity);
        }

        private void UpdateMaxVelocity(float f)
        {
            MaxVelocity = f;
        }

        public void RemoveTrackedController()
        {
            _tracker.ONVelocityChanged.RemoveListener(UpdateMaxVelocity);
        }
    }
}