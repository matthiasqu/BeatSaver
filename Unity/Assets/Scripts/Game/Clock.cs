using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Game
{
    /// <summary>
    ///     A basic musical clock providing ticks for up to 16th notes based on the supplied BPM.
    /// </summary>
    public class Clock : MonoBehaviour
    {
        /// <summary>
        ///     The tempo of the song.
        /// </summary>
        [SerializeField] [ReadOnly] private float bpm;

        /// <summary>
        ///     The length of a bar in seconds.
        /// </summary>
        [SerializeField] [ReadOnly] private float barLength;

        /// <summary>
        ///     The current time of the clock.
        /// </summary>
        [SerializeField] [ReadOnly] private float currentTime;

        /// <summary>
        ///     Fired on each tick, providing an int which represents the note size (i.e. 16 for a sixteenth tick, 8 for an
        ///     eigths).
        /// </summary>
        [SerializeField] private UnityEventInt onTick = new UnityEventInt();

        /// <summary>
        ///     How many bars bars have passed since <see cref="Run" /> has been called.
        /// </summary>
        [SerializeField] [ReadOnly] private int bar;

        /// <summary>
        ///     Number of half notes played in the current bar.
        /// </summary>
        [SerializeField] [ReadOnly] private int half;

        /// <summary>
        ///     Number of quarter notes played in the current bar.
        /// </summary>
        [SerializeField] [ReadOnly] private int quarter;

        /// <summary>
        ///     Number of eight notes played in the current bar.
        /// </summary>
        [SerializeField] [ReadOnly] private int eight;

        /// <summary>
        ///     Number of sixteenth notes played in the current bar.
        /// </summary>
        [SerializeField] [ReadOnly] private int sixteenth;

        /// <summary>
        ///     Ticks that happened in the last Update() cycle (i.e. a 16th, an 8th, etc.)
        /// </summary>
        private readonly List<int> _ticks = new List<int>();

        private bool _initialized;
        private bool _isRunning;
        private int _summedSixteenths;

        /// <summary>
        ///     The length of a full bar in seconds.
        /// </summary>
        public float BarLength => 60 / bpm * 4;

        /// <summary>
        ///     The length of a 2nd note in seconds.
        /// </summary>
        public float HalfNoteLength => BarLength / 2;

        /// <summary>
        ///     The length of a 4th in seconds.
        /// </summary>
        public float QuarterNoteLength => HalfNoteLength / 2;

        /// <summary>
        ///     The length of an 8th in seconds.
        /// </summary>
        public float EightNotelength => QuarterNoteLength / 2;

        /// <summary>
        ///     The length of a 16th in seconds.
        /// </summary>
        public float SixteenthNoteLength => EightNotelength / 2;

        private void Awake()
        {
            Bar = Half = Quarter = Eight = Sixteenth = 0;
            barLength = BarLength;
            Reset();
        }

        public void Reset()
        {
            _isRunning = _initialized = false;
        }

        /// <summary>
        ///     Checks whether a new 16th tick has occured and updates <see cref="Eight" />, <see cref="Quarter" />,
        ///     <see cref="Half" />, and <see cref="Bar" />.
        ///     Invokes the <see cref="onTick" /> event with the greates tick present in <see cref="_ticks" />.
        /// </summary>
        private void Update()
        {
            if (!_isRunning) return;

            ///Send first tick on startup.
            if (!_initialized)
            {
                Bar = Half = Quarter = Eight = Sixteenth = _summedSixteenths = 1;
                onTick.Invoke(1);
                _initialized = true;
            }
            else
            {
                // update current and old time
                var oldTime = currentTime;
                currentTime += Time.deltaTime;

                //calculate how many sixteenth have fitted in the time apssed until the last frame and the current
                var currentSixteenths = currentTime / SixteenthNoteLength;
                var oldSixteenths = oldTime / SixteenthNoteLength;
                // if more sixteenth fit the current time than the last frame'S time, update _summedSixteenth
                if (Math.Floor(currentSixteenths) > Math.Floor(oldSixteenths))
                {
                    _ticks.Clear();

                    Sixteenth++;
                    _summedSixteenths++;
                    // Update each notes count if the addition of a sixteenth also reaches a greater note length
                    if (_summedSixteenths % 2 == 1) Eight++;
                    if (_summedSixteenths % 4 == 1) Quarter++;
                    if (_summedSixteenths % 8 == 1) Half++;
                    if (_summedSixteenths % 16 == 1) Bar++;

                    // Send the greatest note reached with this tick via the event.
                    onTick.Invoke(_ticks.Aggregate((i, j) => i < j ? i : j));
                }
            }
        }

        /// <summary>
        ///     Starts running the clock.
        /// </summary>
        [Button]
        public void Run()
        {
            _isRunning = true;
        }

        #region properties

        /// <summary>
        ///     Bars passed since starting.
        /// </summary>
        public int Bar
        {
            get => bar;
            set
            {
                bar = value;
                _ticks.Add(1);
            }
        }

        /// <summary>
        ///     Halfs passed since starting.
        /// </summary>
        public int Half
        {
            get => half;
            set
            {
                if (value > 2)
                    half = 1;
                else half = value;
                _ticks.Add(2);
            }
        }

        /// <summary>
        ///     Quarters passed since starting.
        /// </summary>
        public int Quarter
        {
            get => quarter;
            set
            {
                if (value > 4)
                    quarter = 1;
                else quarter = value;
                _ticks.Add(4);
            }
        }

        /// <summary>
        ///     Eights passed since starting.
        /// </summary>
        public int Eight
        {
            get => eight;
            set
            {
                if (value > 8)
                    eight = 1;
                else eight = value;
                _ticks.Add(8);
            }
        }

        /// <summary>
        ///     Sixteenth passed since starting.
        /// </summary>
        public int Sixteenth
        {
            get => sixteenth;
            set
            {
                if (value > 16)
                    sixteenth = 1;
                else sixteenth = value;
                _ticks.Add(16);
            }
        }

        /// <summary>
        ///     The tempo of the song.
        /// </summary>
        public float Bpm
        {
            get => bpm;
            set => bpm = value;
        }

        #endregion
    }
}