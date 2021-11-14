using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Game.Tutorial
{
    public class TutorialSection : MonoBehaviour
    {
        [SerializeField] private UnityEvent onContinue = new UnityEvent();
        public UnityEvent OnContinue => onContinue;

        public bool ShouldReleaseTutobjects { get; set; } = false;

        private void Awake() => onContinue.AddListener(() => Debug.Log("[TutorialSection] OnContinue called!"));

        [Button]
        public void Continue() => onContinue.Invoke();
    }
}