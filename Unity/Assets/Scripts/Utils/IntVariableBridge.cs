using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     Pulls functionality of an <see cref="IntVariable" /> instance into the current scene.
    /// </summary>
    public class IntVariableBridge : MonoBehaviour
    {
        //TODO: use IntVariable
        [SerializeField] private SuperchargedIntVariable superchargedIntVariable;
        [SerializeField] private UnityEventInt onValueChanged = new UnityEventInt();

        //TODO: make sure there is only one instance of this behvaiour in the scene referencing the same IntVariable
        private void Start()
        {
            superchargedIntVariable.onValueChanged.AddListener(onValueChanged.Invoke);
            onValueChanged.Invoke(superchargedIntVariable.Value);
        }
    }
}