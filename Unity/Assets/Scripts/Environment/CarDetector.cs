using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class CarDetector : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Car> onCarDetected = new UnityEvent<Car>();

        private void OnTriggerEnter(Collider other)
        {
            var car = other.GetComponent<Car>();
            if (car != null) onCarDetected.Invoke(car);
        }
    }
}