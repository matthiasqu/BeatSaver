using System.Linq;
using BeatMapper.Utils;
using Sirenix.Utilities;
using Unity.Collections;
using UnityEngine;

namespace BeatMapper.SongProcessors
{
    /// <summary>
    ///     Component which converts the time in <see cref="SongMapping" /> to MS. A new copy of the supplied SongMapping
    ///     is sent using the <see cref="SongProcessor.onProcessed" /> event.
    /// </summary>
    public class TicksToSecondsProcessor : SongProcessor
    {
        [Tooltip("Should not be set directly but rather be assigned from GameSettingsBridge.cs on song load.")]
        [SerializeField]
        [ReadOnly]
        private float beatsPerminute = 120f;

        public float BeatsPerminute
        {
            get => beatsPerminute;
            set
            {
                beatsPerminute = value;
                Debug.Log($"[TicksToTimeProcessor] Triggering processing due to bpm change to {value}.");
                if (SongMapping != null) ProcessSong(SongMapping);
            }
        }

        public override void ProcessSong(SongMapping songMapping)
        {
            SongMapping = songMapping;
            var converted = SongMapping.ToTimedSongMapping(beatsPerminute);
            Debug.Log($"Timed song mapping: {converted}");
            onProcessed.Invoke(converted);
        }

        /// <summary>
        ///     Prints the original and processed _time values.
        /// </summary>
        protected override void Print()
        {
            SongMapping.Notes
                .Zip(SongMapping.ToTimedSongMapping(beatsPerminute).Notes, (first, second) => (first, second))
                .ForEach(pair => Debug.Log($"[TicksToTimeProcessor] Original time: {pair.first._time}" +
                                           $"\nProcessed time: {pair.second._time}"));
        }
    }
}