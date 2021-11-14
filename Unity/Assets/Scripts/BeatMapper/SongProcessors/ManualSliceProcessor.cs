using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace BeatMapper.SongProcessors
{
    public class ManualSliceProcessor : SongProcessor
    {
        [SerializeField] private UnityEvent onNextSliceEmpty = new UnityEvent();
        [SerializeField] private UnityEvent onPreviousSliceEmpty = new UnityEvent();
        [SerializeField] private UnityEvent onFirstBookmarkReached = new UnityEvent();
        [SerializeField] private UnityEvent onLastBookmarkReached = new UnityEvent();
        [SerializeField] [ReadOnly] private float previousTime = -1;
        [SerializeField] [ReadOnly] private float currentTime = -1;
        [SerializeField] [ReadOnly] private float nextTime = -1;

        [SerializeField] [ReadOnly] private SongMapping previousMapping = new SongMapping();
        [SerializeField] [ReadOnly] private SongMapping currentMapping = new SongMapping();
        [SerializeField] [ReadOnly] private SongMapping nextMapping = new SongMapping();

        private void Awake()
        {
            InitializeListeners();
        }

        [Button]
        public void NextSlice()
        {
            InitializeAtTimestamp(nextTime);
        }

        [Button]
        public void PreviousSlice()
        {
            InitializeAtTimestamp(previousTime);
        }

        [Button]
        public void InvokeCurrentSlice()
        {
            SafeInvoke();
        }

        [Button]
        public void InitializeBookmark(string bookmark)
        {
            InitializeAtTimestamp(SongMapping.Bookmarks.First(b => b._name == bookmark)._time);
        }

        [Button]
        public void InitializePreviousBookmark()
        {
            InitializeAtTimestamp(GetPreviousBookmark(currentTime)._time);
        }

        [Button]
        public void InitializeNextBookmark()
        {
            InitializeAtTimestamp(GetNextBookmark(currentTime)._time);
        }

        [Button]
        private void InitializeListeners()
        {
            onNextSliceEmpty.AddListener(() => Debug.Log("[ManualSliceProcessor] Next slice is empty"));
            onPreviousSliceEmpty.AddListener(() => Debug.Log("[ManualSliceProcessor] Previous slice empty"));
            onFirstBookmarkReached.AddListener(() => Debug.Log("[ManualSliceProcessor] First bookmark reached."));
            onLastBookmarkReached.AddListener(() => Debug.Log("[ManualSliceProcessor] Last bookmark reached."));
            onProcessed.AddListener(mapping => Debug.Log($"[ManualSliceProcessor] Spawning {mapping}"));
        }

        public override void ProcessSong(SongMapping songMapping)
        {
            SongMapping = songMapping;
            InitializeAtTimestamp(SongMapping.FirstTimestamp);
        }

        protected override void Print()
        {
            throw new NotImplementedException();
        }

        private void SafeInvoke()
        {
            if (!currentMapping.IsEmpty()) onProcessed.Invoke(currentMapping);
        }


        private SongMapping WindForward(float time)
        {
            // find the next timestep for spawning
            var notes = SongMapping.Notes.Reverse().TakeWhile(n => n._time > time).ToArray();
            var obstacles = SongMapping.Obstacles.Reverse().TakeWhile(n => n._time > time).ToArray();

            // Abort if there are no notes or obstacles left 
            if (!(notes.Any() || obstacles.Any()))
                return new SongMapping();

            float obstacleTime, noteTime;
            noteTime = obstacleTime = int.MaxValue;

            // take the time of the last note
            if (notes.Any())
            {
                var lastNote = notes.Last();
                noteTime = lastNote._time;
            }

            // take the time of the last obstacle
            if (obstacles.Any())
            {
                var lastObstacle = obstacles.Last();
                obstacleTime = lastObstacle._time;
            }

            var earlierTime = obstacleTime < noteTime ? obstacleTime : noteTime;

            var newObstacles = SongMapping.Obstacles.Where(o => Math.Abs(o._time - earlierTime) < float.Epsilon)
                .Select(o => o.Clone() as ObstacleEvent).ToArray();
            var newNotes = SongMapping.Notes.Where(n => Math.Abs(n._time - earlierTime) < float.Epsilon)
                .Select(n => n.Clone() as NoteEvent).ToArray();

            return new SongMapping
            {
                _obstacles = newObstacles,
                _notes = newNotes
            };
        }

        private bool PreviousSliceEmpty()
        {
            return Math.Abs(SongMapping.Obstacles.First()._time - previousTime) < float.Epsilon &&
                   Math.Abs(SongMapping.Notes.First()._time - previousTime) < float.Epsilon;
        }

        private SongMapping Rewind(float time)
        {
            // find the notes before the old previous time
            var notes = SongMapping.Notes.TakeWhile(n => n._time < time).ToList();
            var obstacles = SongMapping.Obstacles.TakeWhile(o => o._time < time).ToList();

            // abort if there are none
            if (!(notes.Any() || obstacles.Any()))
                return new SongMapping();

            float obstacleTime;
            var noteTime = obstacleTime = int.MinValue;

            // take the earliest note time
            if (notes.Count > 0)
            {
                var lastNote = notes.Last();
                noteTime = lastNote._time;
            }

            // take the earliest obstacle time
            if (obstacles.Count > 0)
            {
                var lastObstacle = obstacles.Last();
                obstacleTime = lastObstacle._time;
            }

            var latestTime = obstacleTime > noteTime ? obstacleTime : noteTime;

            var newObstacles = SongMapping.Obstacles.Where(o => Math.Abs(o._time - latestTime) < float.Epsilon)
                .Select(o => o.Clone() as ObstacleEvent).ToArray();
            var newNotes = SongMapping.Notes.Where(n => Math.Abs(n._time - latestTime) < float.Epsilon)
                .Select(n => n.Clone() as NoteEvent).ToArray();

            return new SongMapping
            {
                Obstacles = newObstacles,
                Notes = newNotes
            };
        }

        [Button]
        private BookmarkEvent GetCurrentBookmark(float time)
        {
            var bookmarks = SongMapping.Bookmarks;
            for (var index = 0; index < bookmarks.Length; index++)
            {
                var bookmark = bookmarks[index];
                // time is at a bookmark location
                if (Math.Abs(bookmark._time - time) < float.Epsilon) return bookmark;

                // time is between two bookmarks
                if (bookmark._time > time) return bookmarks[index - 1];
            }

            throw new Exception(
                $"[ManualSliceProcessor] No bookmark found corresponding to current time {time}");
        }

        private BookmarkEvent GetPreviousBookmark(float time)
        {
            var current = GetCurrentBookmark(time);
            var previous = SongMapping.Bookmarks.Reverse().FirstOrDefault(b => b._time < current._time);

            if (previous != null) return previous;


            onFirstBookmarkReached.Invoke();
            return SongMapping.Bookmarks.First();
            ;
        }

        private BookmarkEvent GetNextBookmark(float time)
        {
            var current = GetCurrentBookmark(time);
            var next = SongMapping.Bookmarks.FirstOrDefault(b => b._time > current._time);
            if (next != null) return next;


            onLastBookmarkReached.Invoke();
            return SongMapping.Bookmarks.Last();
            ;
        }

        private void InitializeAtTimestamp(float time)
        {
            currentMapping = new SongMapping
            {
                Notes = SongMapping.Notes.Where(n => Math.Abs(n._time - time) < float.Epsilon).ToArray(),
                Obstacles = SongMapping.Obstacles.Where(o => Math.Abs(o._time - time) < float.Epsilon).ToArray(),
                CustomData = new CustomData
                {
                    _bookmarks = SongMapping.Bookmarks.Where(b => Math.Abs(b._time - time) < float.Epsilon).ToArray()
                }
            };

            previousMapping = Rewind(time);
            nextMapping = WindForward(time);

            currentTime = time;

            if (previousMapping.IsEmpty())
            {
                onPreviousSliceEmpty.Invoke();
                previousTime = time;
            }
            else
            {
                previousTime = previousMapping.FirstTimestamp;
            }

            if (nextMapping.IsEmpty())
            {
                onNextSliceEmpty.Invoke();
                nextTime = time;
            }
            else
            {
                nextTime = nextMapping.FirstTimestamp;
            }
        }
    }
}