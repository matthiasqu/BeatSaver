using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    /// <summary>
    ///     Sets a filter frequency on an AudioMixer in steps, i.e. lowers of increases it in fixed step sizes.
    /// </summary>
    public class SteppedFilterSetter : MonoBehaviour
    {
        /// <summary>
        ///     The minimum frequency to reach when reducing the filter frequency.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private float minFrequency;

        /// <summary>
        ///     The maximum frequency to reach when increasing the filter frequency.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private float maxFrequency;

        /// <summary>
        ///     The step size in Hz to use when increasing or decreasing the filter frequency.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private int stepSize;

        /// <summary>
        ///     The speed in Hz/s to use when applying a decrease.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private float decreaseSpeed;

        /// <summary>
        ///     The speed in Hz/s when applying an increase.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private float increaseSpeed;

        /// <summary>
        ///     The mixer on which the filter effect lies.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private AudioMixer mixer;

        /// <summary>
        ///     The parameter exposed by the AudioMixer referencing the filter effect.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private string toSet;

        /// <summary>
        ///     The current frequency of the filter.
        /// </summary>
        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private float currentFrequency;

        /// <summary>
        ///     Whether the script is running a reduction.
        /// </summary>
        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private bool reducing;

        /// <summary>
        ///     Whether the script is running an increase.
        /// </summary>
        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private bool increasing;

        /// <summary>
        ///     The step size in Hz to use when increasing or decreasing the filter frequency.
        /// </summary>
        public int StepSize
        {
            get => stepSize;
            set => stepSize = value;
        }


        /// <summary>
        ///     Set the filter frequency to <see cref="minFrequency" />.
        /// </summary>
        private void Start()
        {
            if (mixer == null) Debug.LogError("[SteppedFilterSetter] No audio mixer assigned!");
            mixer.SetFloat(toSet, minFrequency);
        }

        /// <summary>
        ///     Update the current frequency and start a coroutine that decreases in <see cref="stepSize" /> from there.
        /// </summary>
        public void DecreaseFilter()
        {
            StopAllCoroutines();

            var current = currentFrequency = GetCurrentValue();
            var aim = current - stepSize;

            // clamp the value
            aim = aim < minFrequency ? minFrequency : aim;

            StartCoroutine(DecreaseFrequency(current, aim));
        }

        /// <summary>
        ///     Update the current frequency and start a coroutine that increases in <see cref="stepSize" /> from there.
        /// </summary>
        public void IncreaseFilter()
        {
            StopAllCoroutines();

            var current = currentFrequency = GetCurrentValue();
            var aim = current + stepSize;
            aim = aim > maxFrequency ? maxFrequency : aim;

            StartCoroutine(IncreaseFrequency(current, aim));
        }

        /// <summary>
        ///     Increase the filter frequency per frame.
        /// </summary>
        /// <param name="from">The start frequency.</param>
        /// <param name="to">The end frequency.</param>
        /// <returns></returns>
        private IEnumerator IncreaseFrequency(float from, float to)
        {
            increasing = true;
            reducing = false;

            while (!reducing && increasing)
            {
                from += increaseSpeed * Time.deltaTime;
                currentFrequency = from = from <= to ? from : to;
                mixer.SetFloat(toSet, from);

                yield return null;
            }

            increasing = false;
        }

        /// <summary>
        ///     Decrease the filter frequency per frame.
        /// </summary>
        /// <param name="from">The start frequency.</param>
        /// <param name="to">The end frequency.</param>
        /// <returns></returns>
        private IEnumerator DecreaseFrequency(float from, float to)
        {
            increasing = false;
            reducing = true;
            while (reducing && !increasing)
            {
                from -= decreaseSpeed * Time.deltaTime;
                currentFrequency = from = from >= to ? from : to;

                mixer.SetFloat(toSet, from);
                yield return null;
            }

            reducing = false;
        }

        /// <summary>
        ///     Returns the current filter's value.
        /// </summary>
        /// <returns></returns>
        private float GetCurrentValue()
        {
            mixer.GetFloat(toSet, out var value);
            return value;
        }

        [TabGroup("Debug")]
        [Button]
        private void StepDown()
        {
            DecreaseFilter();
        }

        [TabGroup("Debug")]
        [Button]
        private void StepUp()
        {
            IncreaseFilter();
        }
    }
}