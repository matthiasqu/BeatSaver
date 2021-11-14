using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    //TODO: Is this still used?
    public class FilterEffect : MonoBehaviour
    {
        [SerializeField] private AudioLowPassFilter filter;

        [SerializeField] private float minFrequency = 500;
        [SerializeField] private float maxFrequency = 22000;
        [SerializeField] private float reductionTime = .1f;
        [SerializeField] private float increaseTime = .1f;


        // Start is called before the first frame update
        private void Start()
        {
            if (filter == null) Debug.LogError($"[FilterEffect] Filter variable not set on {gameObject.name}!");
        }

        [Button]
        public void Trigger()
        {
            StartCoroutine(Effect());
        }

        private IEnumerator Effect()
        {
            var difference = 22000 - 500;
            while (filter.cutoffFrequency > minFrequency)
            {
                filter.cutoffFrequency -= difference / reductionTime * Time.deltaTime;
                yield return null;
            }

            while (filter.cutoffFrequency < maxFrequency)
            {
                filter.cutoffFrequency += difference / increaseTime * Time.deltaTime;
                yield return null;
            }
        }
    }
}