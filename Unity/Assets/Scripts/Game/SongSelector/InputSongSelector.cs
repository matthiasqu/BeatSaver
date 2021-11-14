using System;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Game.SongSelector
{
    /// <summary>
    ///     Selects the next song based on an average score calculated by an instance of <see cref="ScoreAggregator" />.
    ///     Does not wait for ticks and assumes that the <see cref="ScoreAggregator.OnScoreCalculated" /> event is fired on
    ///     beat.
    ///     TODO: Rename to AverageScoreSongSelectorStrategy
    /// </summary>
    [Serializable]
    public class InputSongSelector : ISongSelector
    {
        /// <summary>
        ///     The <see cref="ScoreAggregator" /> instance which supplies the OnScoreCalculated event.
        /// </summary>
        [SerializeField] private ScoreAggregator scoreAggregator;

        /// <summary>
        ///     The threshold to determine which score triggers the next, and which score triggers the previous level.
        /// </summary>
        [SerializeField] private IntVariable threshold;

        /// <summary>
        ///     Invoked whenever the the next level should be palyed.
        /// </summary>
        [SerializeField] private readonly UnityEvent onNextLevel = new UnityEvent();

        /// <summary>
        ///     Invoked whenever the previous level should be played.
        /// </summary>
        [SerializeField] private readonly UnityEvent onPreviousLevel = new UnityEvent();

        /// <summary>
        ///     A reference to the GameObject to which a <see cref="SongSelector" /> component is attached.
        /// </summary>
        private GameObject _go;


        public int SelectSong()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Receives a reference to the scene and adds a listener to the OnScoreCalculated event on
        ///     <see cref="scoreAggregator" />.
        /// </summary>
        /// <param name="go">Reference to the object which uses this strategy.</param>
        public void Start(GameObject go)
        {
            _go = go;
            scoreAggregator.ONScoreCalculated.AddListener(i =>
            {
                Debug.Log($"[InputSongSelector] Score {i} received");
                if (i >= threshold.Value)
                {
                    Debug.Log("[InputSongSelector] Next event is onNextLevel");
                    onNextLevel.Invoke();
                }
                else
                {
                    Debug.Log("[InputSongSelector] Next event is onPreviousLevel");
                    onPreviousLevel.Invoke();
                }
            });
        }

        public void ReceiveTick(int tick)
        {
        }
    }
}