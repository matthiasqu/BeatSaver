using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

//TODO: make this a scriptable object and use async methods instead
namespace Game
{
    public class FilterSetter : MonoBehaviour
    {
        [TabGroup("Filter Settings")] [SerializeField]
        private AudioMixer mixer;

        [TabGroup("Filter Settings")] [SerializeField]
        private string valueToSet;

        [TabGroup("Filter Settings")] [SerializeField]
        private float increaseSpeed = 20000;

        [TabGroup("Filter Settings")] [SerializeField]
        private float decreaseSpeed = 10000;

        [TabGroup("Filter Settings")] [SerializeField]
        private float minFrequency = 20000;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private bool reducing;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private bool increasing;

        public void ReduceFilterFrequency()
        {
            StopCoroutine(IncreaseFrequency());

            StartCoroutine(DecreaseFrequency());
        }

        public void IncraseFilterFrequency()
        {
            StopCoroutine(DecreaseFrequency());
            StartCoroutine(IncreaseFrequency());
        }

        private IEnumerator IncreaseFrequency()
        {
            increasing = true;
            reducing = false;
            float value = 0;
            mixer.GetFloat(valueToSet, out value);
            while (!reducing && increasing && Math.Abs(value - 22000) > float.Epsilon)
            {
                value += increaseSpeed * Time.deltaTime;
                mixer.SetFloat(valueToSet, value > 22000 ? 22000 : value);
                yield return null;
            }

            increasing = false;
        }

        private IEnumerator DecreaseFrequency()
        {
            increasing = false;
            reducing = true;
            float value = 22000;
            mixer.GetFloat(valueToSet, out value);
            while (reducing && value > minFrequency)
            {
                value -= increaseSpeed * Time.deltaTime;
                mixer.SetFloat(valueToSet, value >= minFrequency ? value : minFrequency);
                yield return null;
            }

            reducing = false;
        }

        [TabGroup("Debug")]
        [Button]
        private void StartReducing()
        {
            ReduceFilterFrequency();
        }

        [TabGroup("Debug")]
        [Button]
        private void StartIncreasing()
        {
            IncraseFilterFrequency();
        }
    }
}