using System;
using UnityEngine;

namespace Utils
{
    public class IntVariableResetter : MonoBehaviour
    {
        //TODO: Use IntVariable
        [SerializeField] private SuperchargedIntVariable superchargedIntVariable;
        [SerializeField] private int resetValue;

        //TODO: make sure there is only one instance in the scene with the same IntVariable assigned
        private void Start()
        {
            if (superchargedIntVariable == null)
                throw new Exception($"[IntVariableResetter on {gameObject.name}] IntVariable not assigned!");
            superchargedIntVariable.Reset(resetValue);
            Debug.Log($"[IntVariableResetter] {superchargedIntVariable.name} reset to {resetValue}.");
        }
    }
}