using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using static Game.Utils;

namespace Game.SongSelector
{
    /// <summary>
    ///     TODO: Replace this with a InputSongSelector like structure using ScoreAggregator.
    /// </summary>
    [Serializable]
    [InlineProperty]
    [HideLabel]
    public class ScoreSongSelector : ISongSelector
    {
        [SerializeField] private SuperchargedIntVariable scoreVariable;
        [SerializeField] private SuperchargedIntVariable multiplierVariable;
        [SerializeField] private int scoreThreshold;
        [SerializeField] [ReadOnly] private int scoreCopy;

        [SerializeField] private Note changeAt = Note.Full;
        [SerializeField] private int countTillChange = 1;

        [SerializeField] private UnityEvent OnThresholdReached = new UnityEvent();
        [SerializeField] private UnityEvent OnNoteMissed = new UnityEvent();
        private int _currentCount;
        private UnityEvent _nextEvent;
        private int _scoreProgress;


        private GameObject go;

        public int ScoreCopy
        {
            get => scoreCopy;
            set
            {
                if (scoreCopy == value) return;

                //Debug.Log("[ScoreSongSelector] Score updated.");

                ScoreProgress += value - scoreCopy;
                scoreCopy = value;
            }
        }

        public int ScoreProgress
        {
            get => _scoreProgress;
            private set
            {
                if (_scoreProgress == value) return;
                _scoreProgress = value;
                if (_scoreProgress > ScoreThreshold)
                {
                    //Debug.Log("[ScoreSongSelector] Reaching next level");
                    _scoreProgress = 0;
                    _nextEvent = OnThresholdReached;
                }
            }
        }


        public int ScoreThreshold => scoreThreshold;

        public int SelectSong()
        {
            return +1;
        }

        public void Start(GameObject go)
        {
            this.go = go;
            scoreVariable.onValueChanged.AddListener(i => ScoreCopy = i);
            multiplierVariable.onValueChanged.AddListener(i =>
            {
                if (i == 1) _nextEvent = OnNoteMissed;
            });
        }

        public void ReceiveTick(int tick)
        {
            if (TickIsRelevant(tick, changeAt) && _nextEvent != null)
            {
                _currentCount++;
                if (_currentCount < countTillChange) return;
                _nextEvent.Invoke();
                _nextEvent = null;
                _currentCount = 0;
            }
        }
    }

    public enum Note
    {
        Full = 1,
        Half = 2,
        Quarter = 4,
        Eights = 8,
        Sixteenth = 16
    }
}