using System;
using System.Linq;
using BeatMapper.Utils;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace BeatMapper.SongProcessors
{
    public class SongObstacleDurationProcessor : SongProcessor
    {
        [SerializeField] [ReadOnly] private float movementSpeed;
        [SerializeField] [ReadOnly] private float bpm;

        /// <summary>
        ///     The song's beats per minute to be set by the <see cref="GameSettingsBridge" />.
        /// </summary>
        public float BPM
        {
            get => bpm;
            set
            {
                if (Math.Abs(value - bpm) < float.Epsilon) return;
                bpm = value;

                Debug.Log($"[SongObstacleDurationProcessor] Processing due to BPM change to {value}");
                ProcessSong(SongMapping);
            }
        }

        /// <summary>
        ///     The blocks/obstacles movement speed to be set by the <see cref="GameSettingsBridge" />.
        /// </summary>
        public float MovementSpeed
        {
            get => movementSpeed;
            set
            {
                if (Math.Abs(value - movementSpeed) < float.Epsilon) return;
                movementSpeed = value;
                if (SongMapping == null) return;

                Debug.Log($"[{GetType()}] Triggering processing gue to movement speed change to {value}");
                ProcessSong(SongMapping);
            }
        }

        /// <summary>
        ///     Converts the supplied song mappings ObstaclesEvents duration to space units.
        /// </summary>
        /// <param name="songMapping"></param>
        public override void ProcessSong(SongMapping songMapping)
        {
            if (songMapping == null) return;
            if (songMapping.Obstacles.Length == 0) return;

            SongMapping = songMapping;
            onProcessed.Invoke(songMapping.ToLengthenedMapping(movementSpeed, bpm));
        }

        protected override void Print()
        {
            if (SongMapping == null) Debug.LogError($"[{GetType()}] SongMapping is Null!");
            SongMapping.Obstacles
                .Zip(SongMapping.ToLengthenedMapping(movementSpeed, bpm).Obstacles, (first, second) => (first, second))
                .ForEach(pair => Debug.Log($"[{GetType()}] Original duration: {pair.first._duration}s" +
                                           $"\nProcessed duration: {pair.second._duration} units"));
        }
    }
}