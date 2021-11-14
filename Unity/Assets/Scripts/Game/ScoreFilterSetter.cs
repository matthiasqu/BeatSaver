using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    /// <summary>
    ///     Sets the Frequency of an AudioMixer's Filter effect according to an incoming score.
    /// </summary>
    public class ScoreFilterSetter : MonoBehaviour
    {
        /// <summary>
        ///     The Mixer on which the Filter effect lies.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private AudioMixer mixer;

        /// <summary>
        ///     The string referencing the exposed parameter in the mixer.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private string toSet;


        /// <summary>
        ///     The minimum frequency of the filter.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private int minFreq = 200;


        /// <summary>
        ///     The maximum frequency of the filter.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private int maxFreq = 22000;


        /// <summary>
        ///     The speed with which to move the filter frequency.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private float speed;

        /// <summary>
        ///     A factor with which scores are multiplied.
        /// </summary>
        [TabGroup("Filter Settings")] [SerializeField]
        private int factor;

        /// <summary>
        ///     Whether the script is decreasing the filter frequency atm.
        /// </summary>
        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private bool decreasing;

        /// <summary>
        ///     Whether the script is increasing the filter frequency atm.
        /// </summary>
        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private bool increasing;

        /// <summary>
        ///     The current filter frequency.
        /// </summary>
        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private float currentFrequency;


        [SerializeField] private bool _enabled = true;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                Reset();
            }
        }

        public void Reset()
        {
            StartCoroutine(IncreaseFrequency(maxFreq));
        }

        /// <summary>
        ///     Starts a coroutine that moves the filter frequency to the one according to <see cref="avgScore" />.
        /// </summary>
        /// <param name="avgScore">Average score of the last Blocks played.</param>
        public void SetFilter(int avgScore)
        {
            if (!_enabled) return;
            var gotten = mixer.GetFloat(toSet, out currentFrequency);
            if (!gotten) Debug.LogError($"[ScoreFilterSetter] could not get {toSet}");
            var next = factor * avgScore;


            StopAllCoroutines();
            if (next < currentFrequency) StartCoroutine(DecreaseFrequency(next));
            else if (next > currentFrequency) StartCoroutine(IncreaseFrequency(next));
        }

        /// <summary>
        ///     Increases the FilterFrequency over a timespan of <see cref="speed" /> Hz per second.
        /// </summary>
        /// <param name="higherFreq">The frequency to arrive at.</param>
        /// <returns></returns>
        private IEnumerator IncreaseFrequency(float higherFreq)
        {
            if (higherFreq > maxFreq) higherFreq = maxFreq;
            increasing = true;
            decreasing = false;

            while (!decreasing && increasing && currentFrequency < higherFreq)
            {
                var deltaFreq = speed * Time.deltaTime;
                currentFrequency += deltaFreq;
                currentFrequency = currentFrequency > higherFreq ? higherFreq : currentFrequency;
                mixer.SetFloat(toSet, currentFrequency);
                mixer.GetFloat(toSet, out var actual);

                yield return null;
            }

            increasing = false;
        }

        /// <summary>
        ///     Decreases the FilterFrequency over a timespan of <see cref="speed" /> Hz per second.
        /// </summary>
        /// <param name="lowerFreq">The frequency to arrive at.</param>
        /// <returns></returns>
        private IEnumerator DecreaseFrequency(float lowerFreq)
        {
            if (lowerFreq < minFreq) lowerFreq = minFreq;

            increasing = false;
            decreasing = true;
            while (decreasing && !increasing && currentFrequency > lowerFreq)
            {
                var deltaFreq = speed * Time.deltaTime;
                currentFrequency -= deltaFreq;
                currentFrequency = currentFrequency < lowerFreq ? lowerFreq : currentFrequency;
                mixer.SetFloat(toSet, currentFrequency);
                mixer.GetFloat(toSet, out var actual);
                yield return null;
            }

            decreasing = false;
        }
    }
}