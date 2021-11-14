using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Game.SongSelector
{
    /// <summary>
    ///     Selects the next song based on whether a streak is broken or fullfilled. Uses the Multiplier variable to determine
    ///     when
    ///     to change to the next streak (i.e. when it increases) or when to change to the previous one (i.e. when the
    ///     multiplier is reset).
    /// </summary>
    /// TODO: Fill in documentation completely
    [Serializable]
    [InlineProperty]
    [HideLabel]
    public class StreakSongSelector : ISongSelector
    {
        [SerializeField] private SuperchargedIntVariable currentStreakVariable;
        [SerializeField] private SuperchargedIntVariable missedNotesVariable;
        [SerializeField] private int streakThreshold;
        [SerializeField] private int missedNotesThreshold;

        [SerializeField] private Note changeAt = Note.Full;
        [SerializeField] private int ticksToChange = 1;

        [SerializeField] private UnityEvent OnThresholdReached = new UnityEvent();
        [SerializeField] private UnityEvent OnNoteMissed = new UnityEvent();
        private int _missedNotesProgress;
        private UnityEvent _nextEvent;
        private int _streakProgress;
        private int _tickCount;


        private GameObject go;

        private string log = "[ScoreSongSelector]";
        public int StreakThreshold => streakThreshold;

        public int StreakProgress
        {
            get => _streakProgress;
            private set
            {
                if (_streakProgress == value) return;

//                Log($"{log} Streak Progress = {value}");
                _streakProgress = value;
                if (value > 0) MissedNotesProgress = 0;

                if (_streakProgress > StreakThreshold)
                {
                    //                   Log($"{log} Streak Threshold Reached.");
                    _streakProgress = 0;
                    _nextEvent = OnThresholdReached;
                }
            }
        }

        public int MissedNotesProgress
        {
            get => _missedNotesProgress;
            private set
            {
                if (_missedNotesProgress == value) return;

//                Log($"{log} missed notes = {value}");
                _missedNotesProgress = value;
                if (value > 0) StreakProgress = 0;

                if (_missedNotesProgress > missedNotesThreshold)
                {
//                    Log($"{log} missed notes threshold reached");
                    _missedNotesProgress = 0;
                    _nextEvent = OnNoteMissed;
                }
            }
        }

        public int SelectSong()
        {
            return 1;
        }

        public void Start(GameObject go)
        {
            this.go = go;
            currentStreakVariable.onChangeDifference.AddListener(i =>
            {
                if (i == 1) StreakProgress += i;
            });
            currentStreakVariable.onValueChanged.AddListener(i =>
            {
                if (i == 0) StreakProgress = 0;
            });
            missedNotesVariable.onValueChanged.AddListener(i =>
            {
                if (i == 0) MissedNotesProgress = 0;
            });
            missedNotesVariable.onChangeDifference.AddListener(i =>
            {
                if (i == 1) MissedNotesProgress++;
            });
        }

        public void ReceiveTick(int tick)
        {
            if (Utils.TickIsRelevant(tick, changeAt) && _nextEvent != null)
            {
                _tickCount++;
                if (_tickCount < ticksToChange) return;
                _nextEvent.Invoke();
                _nextEvent = null;
                _tickCount = 0;
            }
        }
    }
}