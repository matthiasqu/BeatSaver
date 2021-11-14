using UnityEngine;
using Utils;

namespace Game
{
    /// <summary>
    ///     Filter component which forwards scores detected based on a threshold defined in <see cref="threshold" />.
    ///     Score above will be reported using the <see cref="onPastThreshold" /> event, scores below using the
    ///     <see cref="onBelowThreshold" /> event.
    /// </summary>
    public class ScoreFilter : MonoBehaviour
    {
        /// <summary>
        ///     The threshold to use for categorizing scores.
        /// </summary>
        [SerializeField] private int threshold = 1;

        /// <summary>
        ///     Event supplying score below the <see cref="threshold" />.
        /// </summary>
        [SerializeField] private UnityEventGameObject onBelowThreshold = new UnityEventGameObject();

        /// <summary>
        ///     Events supplying scores above or equal to the <see cref="threshold" />.
        /// </summary>
        [SerializeField] private UnityEventGameObject onPastThreshold = new UnityEventGameObject();

        /// <summary>
        ///     Filters Gameobjects with attached  <see cref="ScoreCalculator" /> components according to <see cref="threshold" />
        ///     and invokes <see cref="onPastThreshold" /> and <see cref="onBelowThreshold" />.
        /// </summary>
        /// <param name="o">A GameObject with attached <see cref="ScoreCalculator" /> component.</param>
        public void Filter(GameObject o)
        {
            var score = o.GetComponentInChildren<ScoreCalculator>();
            if (score == null) Debug.LogError($"[ScoreFilter] No Score Calculator found on {o.name}");

            if (score.Score >= threshold) onBelowThreshold.Invoke(score.gameObject);
            else onPastThreshold.Invoke(score.gameObject);
        }
    }
}