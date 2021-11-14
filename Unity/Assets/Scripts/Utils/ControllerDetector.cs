using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    [RequireComponent(typeof(Collider))]
    public class ControllerDetector : MonoBehaviour
    {
        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventGameObject controllerEnter = new UnityEventGameObject();

        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventGameObject controllerExit = new UnityEventGameObject();

        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventGameObject controllerStay = new UnityEventGameObject();

        [SerializeField] private GameObject _lastSword;

        private Collider _collider;

        public GameObject LastSword
        {
            get => _lastSword;
            private set => _lastSword = value;
        }
        private bool _swordEntered;
        
        public UnityEventGameObject TriggerEnter => controllerEnter;
        public UnityEventGameObject TriggerExit => controllerExit;
        public UnityEventGameObject TriggerStay => controllerStay;

        private void Start()
        {
            if (_collider == null) _collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (ShouldTrackOther(other.gameObject))
            {
                _swordEntered = true;
                FireEvent(controllerEnter, other.gameObject);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject == LastSword)
            {
                _swordEntered = false;
                FireEvent(controllerExit, other.gameObject);
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject == LastSword) controllerStay.Invoke(LastSword);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (ShouldTrackOther(other.gameObject))
            {
                _swordEntered = true;
                FireEvent(controllerEnter, other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == LastSword)
            {
                _swordEntered = false;
                FireEvent(controllerExit, other.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject == LastSword) controllerStay.Invoke(other.gameObject);
        }

        private void FireEvent(UnityEventGameObject e, GameObject controller)
        {
            LastSword = controller.gameObject;
            e.Invoke(controller.gameObject);
        }

        private bool ShouldTrackOther(GameObject other)
        {
            return other.GetComponentInChildren<Sword>() != null && !_swordEntered;
        }


        [Button]
        [ButtonGroup]
        private void InvokeEnter()
        {
            TriggerEnter.Invoke(null);
        }

        [Button]
        [ButtonGroup]
        private void InvokeExit()
        {
            TriggerExit.Invoke(null);
        }
    }

    [Serializable]
    public class UnityEventGameObject : UnityEvent<GameObject>
    {
    }
}