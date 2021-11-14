using System;
using System.Linq;
using BeatMapper.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeatMapper.SongProcessors
{
    public class SongMappingSliceProcessor : SongProcessor
    {
        [SerializeField] [ReadOnly] private bool running;
        [SerializeField] [ReadOnly] private float time;


        private void Update()
        {
            if (!running) return;

            var oldTime = time;
            time += Time.deltaTime;

            var mapSlice = new SongMapping();
            mapSlice.Notes = SongMapping.Notes.GetCurrentEvents(time, oldTime).ToArray();
            mapSlice.Obstacles = SongMapping.Obstacles.GetCurrentEvents(time, oldTime).ToArray();
            mapSlice.Events = SongMapping.Events.GetCurrentEvents(time, oldTime).ToArray();

            if (mapSlice.Notes.Length == 0 && mapSlice.Events.Length == 0 && mapSlice.Obstacles.Length == 0) return;

            //Debug.Log($"[{GetType()}] Sending {mapSlice.Events.Length} events, {mapSlice.Notes.Length} note, and {mapSlice.Obstacles.Length} obstacles.");

            onProcessed.Invoke(mapSlice);
        }

        public override void ProcessSong(SongMapping songMapping)
        {
            SongMapping = songMapping;
        }

        protected override void Print()
        {
            throw new NotImplementedException();
        }

        public void StartDispatching(float time)
        {
//            Debug.Log($"[SongMappingSliceProcessor] Starting to dispatch");
            this.time = time;
            running = true;
        }
    }
}