using System.Linq;
using BeatMapper.Utils;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace BeatMapper.SongProcessors
{
    /// <summary>
    ///     Component that delays all incoming SongMappings by a certain amount of time in MS and invokes
    ///     <see cref="SongProcessor.onProcessed" /> with a deep copy of the song mapping after applying the delay.
    /// </summary>
    public class SongEventDelayProcessor : SongProcessor
    {
        /// <summary>
        ///     The delay to apply to all _time fields in the <see cref="SongMapping" />.
        /// </summary>
        [Tooltip(
            "The delay in MS to apply. This should not be set directly but rather be set by the Game Settings Bridge on song load.")]
        [SerializeField]
        [ReadOnly]
        private float delay;

        [SerializeField] private bool inverted;

        public float Delay
        {
            get => inverted ? -delay : delay;
            set
            {
                delay = value;
                if (SongMapping == null) return;

                Debug.Log($"[EventDelayProcessor] Triggering processing due to delay change to {value}.");
                ProcessSong(SongMapping);
            }
        }

        public override void ProcessSong(SongMapping songMapping)
        {
            SongMapping = songMapping;
            var delayed = SongMapping.ApplyDelay(Delay);
            Debug.Log($"[SongEventDelayProcessor] Delayed SongMapping: {delayed}");
            onProcessed.Invoke(SongMapping.ApplyDelay(Delay));
        }

        /// <summary>
        ///     Prints out the original and processed time values of all notes saved in <see cref="SongMapping" />.
        /// </summary>
        protected override void Print()
        {
            SongMapping.Notes.Zip(SongMapping.ApplyDelay(Delay).Notes, (first, second) => (first, second)).ForEach(
                pair =>
                    Debug.Log($"[SongEventDelay] Original _time: {pair.first._time}" +
                              $"\nProcessed _time: {pair.second._time}"));
        }
    }
}