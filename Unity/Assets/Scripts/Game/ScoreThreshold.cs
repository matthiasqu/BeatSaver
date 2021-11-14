using UnityEngine;
using Utils;

namespace Game
{
    //TODO: remove
    public class ScoreThreshold : MonoBehaviour
    {
        [SerializeField] private IntVariable upperThreshold;

        [SerializeField] private IntVariable lowerThreshold;
        //[SerializeField] private SuperchargedIntVariable scoreVariable;

        [SerializeField] public UnityEventInt onUpper = new UnityEventInt();
        [SerializeField] public UnityEventInt onLower = new UnityEventInt();
        [SerializeField] public UnityEventInt onCorridor = new UnityEventInt();

        private void Start()
        {
            //if (scoreVariable == null || upperThreshold == null || lowerThreshold == null)
            //    Debug.LogError("[IntThreshold] One or more IntVariables were not assigned");
            //scoreVariable.onRawDataReceived.AddListener(Process);
        }

        public void Process(int hitStrength)
        {
//            Debug.Log($"{hitStrength}");
            if (hitStrength >= upperThreshold.Value) onUpper.Invoke(hitStrength);
            else if (hitStrength <= lowerThreshold.Value) onLower.Invoke(hitStrength);
            else onCorridor.Invoke(hitStrength);
        }
    }
}