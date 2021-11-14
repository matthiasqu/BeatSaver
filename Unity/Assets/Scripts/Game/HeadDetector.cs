using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Game
{
    /// <summary>
    ///     Detects collisions with the player's head (i.e. the camera) and invokes events.
    /// </summary>
    public class HeadDetector : MonoBehaviour
    {
        public bool HeadDetected
        {
            get => _headDetected;
            private set => _headDetected = value;
        }

        private void Awake()
        {
            onHeadEnter.AddListener(go => Debug.Log($"[HeadDetector] {gameObject.name} detected PlayerHead on {go.name}"));
            onHeadEnter.AddListener( go => { HeadDetected = true; });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerHead>()) onHeadEnter.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerHead>()) onHeadLeft.Invoke(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<PlayerHead>()) onHeadStay.Invoke(other.gameObject);
        }

        #region events

        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventGameObject onHeadEnter = new UnityEventGameObject();

        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventGameObject onHeadStay = new UnityEventGameObject();

        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventGameObject onHeadLeft = new UnityEventGameObject();

        [SerializeField, ReadOnly] private bool _headDetected;

        #endregion
    }
}