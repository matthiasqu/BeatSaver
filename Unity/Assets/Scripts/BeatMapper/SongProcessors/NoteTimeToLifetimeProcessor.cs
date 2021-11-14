using System;
using Sirenix.Utilities;

namespace BeatMapper.SongProcessors
{
    public class NoteTimeToLifetimeProcessor : SongProcessor
    {
        public float NoteAnticipationTime { get; set; }

        public override void ProcessSong(SongMapping songMapping)
        {
            SongMapping = songMapping;
            songMapping.Notes.ForEach(n => n._time = NoteAnticipationTime);
            onProcessed.Invoke(songMapping);
        }

        protected override void Print()
        {
            throw new NotImplementedException();
        }
    }
}