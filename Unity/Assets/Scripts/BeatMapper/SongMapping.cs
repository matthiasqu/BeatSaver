using System;
using System.Linq;
using BeatMapper.Utils;
using UnityEngine.Events;
using Utils;

namespace BeatMapper
{
    /// <summary>
    ///     Implementation of the BeatSaber map schema as can be found here
    ///     <seealso>
    ///         <cref>https://www.notion.so/matthiasqu/Masterarbeit-d9df3d7ca6904e1ead4141888d1105de#270a97122da946a3b9f967090b5164ae</cref>
    ///     </seealso>
    /// </summary>
    [Serializable]
    public class SongMapping : ICloneable
    {
        /// <summary>
        ///     Copy the object.
        /// </summary>
        /// <returns>A deep copy of the current object.</returns>
        public object Clone()
        {
            return new SongMapping
            {
                _customData = _customData.Clone() as CustomData,
                _events = Events.DeepCopy() as SongEvent[],
                _notes = Notes.DeepCopy() as NoteEvent[],
                _obstacles = Obstacles.DeepCopy() as ObstacleEvent[],
                _version = Version?.Clone() as string,
            };
        }

        public override string ToString()
        {
            var none = "none";
            return
                $"\n SongEvents: {(_events.Length > 0 ? _events.Select(b => b.ToString()).Aggregate((a, b) => $"{a}, {b}") : none)}" +
                $"\n Notes: {(_notes.Length > 0 ? _notes.Select(b => b.ToString()).Aggregate((a, b) => $"{a}, {b}") : none)}" +
                $"\n Obstacles: {(_obstacles.Length > 0 ? _obstacles.Select(b => b.ToString()).Aggregate((a, b) => $"{a}, {b}") : none)}" +
                $"\n {CustomData}";
        }

        /// <summary>
        ///     Whether there are no events present in this SongMapping
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            var counts = new[] {CustomData.Bookmarks.Length, Events.Length, Notes.Length, Obstacles.Length, CustomData.BpmChanges.Length};
            return counts.All(c => c == 0);
        }

        #region properties

        public float FirstTimestamp
        {
            get
            {
                var bookmark = CustomData.Bookmarks.Select(b => b._time).DefaultIfEmpty(float.MaxValue).OrderBy(b => b).First();
                var note = Notes.Select(n => n._time).DefaultIfEmpty(float.MaxValue).OrderBy(n => n).First();
                var obstacle = Obstacles.Select(o => o._time).DefaultIfEmpty(float.MaxValue).OrderBy(o => o).First();

                return new[] {bookmark, note, obstacle}.OrderBy(f => f).First();
            }
        }

        public float LastTimestamp
        {
            get
            {
                var bookmark = CustomData.Bookmarks.Select(b => b._time).DefaultIfEmpty(float.MinValue).OrderBy(b => b).First();
                var note = Notes.Select(n => n._time).DefaultIfEmpty(float.MinValue).OrderBy(n => n).First();
                var obstacle = Obstacles.Select(o => o._time).DefaultIfEmpty(float.MinValue).OrderBy(o => o).First();

                return new[] {bookmark, note, obstacle}.OrderBy(f => f).First();
            }
        }

        /// <summary>
        ///     The Version of the used BeatSaber file format.
        /// </summary>
        public string Version
        {
            get => _version;
            set => _version = value;
        }
        
        /// <summary>
        ///     Array of Events during the song.
        /// </summary>
        public SongEvent[] Events
        {
            get => _events;
            set => _events = value;
        }

        /// <summary>
        ///     Array of Notes during the song.
        /// </summary>
        public NoteEvent[] Notes
        {
            get => _notes;
            set => _notes = value;
        }

        /// <summary>
        ///     Array of Obstacles during the song
        /// </summary>
        public ObstacleEvent[] Obstacles
        {
            get => _obstacles;
            set => _obstacles = value;
        }

        public CustomData CustomData
        {
            get => _customData;
            set => _customData = value;
        }

        public BookmarkEvent[] Bookmarks => _customData.Bookmarks;
        public BPMChangeEvent[] BpmChanges => CustomData.BpmChanges;
        
        #endregion properties

        #region backingfields

        public string _version = "";


        public SongEvent[] _events = new SongEvent[0]; //General events 
        public NoteEvent[] _notes = new NoteEvent[0]; // single note events
        public ObstacleEvent[] _obstacles = new ObstacleEvent[0]; // obstacle events
        public CustomData _customData = new CustomData();

        #endregion backingfields
    }

    /// <summary>
    ///     Unity event carrying a <see cref="Songmapping" /> object.
    /// </summary>
    [Serializable]
    public class UnityEventSongMapping : UnityEvent<SongMapping>
    {
    }

    /// <summary>
    ///     Base class for all Events of a BeatSaber map that are bound to a timecode.
    /// </summary>
    public abstract class TimedSongEvent : ICloneable
    {
        /// <summary>
        ///     The events time, either in Ticks or MS.
        /// </summary>
        public float _time;

        public abstract object Clone();
    }

    /// <summary>
    ///     Song Events represent Lasers etc. not used in this project.
    /// </summary>
    [Serializable]
    public class SongEvent : TimedSongEvent
    {
        public byte _type;
        public byte _value;

        public override object Clone()
        {
            return new SongEvent
            {
                _time = _time,
                _type = _type,
                _value = _value
            };
        }

        public override string ToString()
        {
            return $"(time: {_time}, type: {_type}, value: {_value})";
        }
    }


    /// <summary>
    ///     Notes in a BeatSaber map.
    /// </summary>
    [Serializable]
    public class NoteEvent : TimedSongEvent
    {
        /// <summary>
        ///     The line on which the note appears. From left (0) to right (3).
        /// </summary>
        public LineIndex _lineIndex = LineIndex.First;

        /// <summary>
        ///     The Line Layer (vertical) on which the note appears. From top (0) to bottom (2).
        /// </summary>
        public LineLayer _lineLayer = LineLayer.Bottom;

        /// <summary>
        ///     The note's color. <see cref="CubeColor" />
        /// </summary>
        public CubeColor _type = CubeColor.Blue;

        /// <summary>
        ///     The cut direction of a note <see cref="CutDirection" />.
        ///     Counted from the top (0) to bottom (1) to left (3) to right (4). The same scheme applies for diagonal cuts.
        /// </summary>
        public CutDirection _cutDirection = CutDirection.Down;

        /// <summary>
        ///     Clones the object.
        /// </summary>
        /// <returns>A deep copy of the Note.</returns>
        public override object Clone()
        {
            return new NoteEvent
            {
                _time = _time,
                _type = _type,
                _cutDirection = _cutDirection,
                _lineIndex = _lineIndex,
                _lineLayer = _lineLayer
            };
        }

        public override string ToString()
        {
            return
                $"(time: {_time}, index: {_lineIndex}, layer: {_lineLayer}, color: {_type}, direction: {_cutDirection})";
        }
    }

    [Serializable]
    public class UnityEventNoteEvent : UnityEvent<NoteEvent>
    {
    }

    /// <summary>
    ///     Obstacles present in the BeatSaber map. Not used throughout the project.
    /// </summary>
    [Serializable]
    public class ObstacleEvent : TimedSongEvent
    {
        /// <summary>
        ///     Length of the Obstacles (in beats)
        /// </summary>
        public float _duration;

        /// <summary>
        ///     The line the where the left side of the obstacle is located
        /// </summary>
        public LineIndex _lineIndex;

        /// <summary>
        ///     The obstacle type, either a full heigt wall or crouch wall.
        /// </summary>
        public ObstacleType _type;

        /// <summary>
        ///     The width of the obstacle in lines starting from <see cref="_lineIndex" />
        /// </summary>
        public byte _width;

        /// <summary>
        ///     Clones the object.
        /// </summary>
        /// <returns>A deep copy of the Obstacle.</returns>
        public override object Clone()
        {
            return new ObstacleEvent
            {
                _duration = _duration,
                _time = _time,
                _type = _type,
                _width = _width,
                _lineIndex = _lineIndex
            };
        }

        public override string ToString()
        {
            return $"(time: {_time}, duration: {_duration}, index: {_lineIndex}, type: {_type}, width: {_width})";
        }
    }


    [Serializable]
    public class CustomData : ICloneable
    {
        public float _time;
        public BookmarkEvent[] _bookmarks = new BookmarkEvent[0];
        public BPMChangeEvent[] _BPMChanges = new BPMChangeEvent[0];

        public float Time
        {
            get => _time;
            set => _time = value;
        }

        public BookmarkEvent[] Bookmarks
        {
            get => _bookmarks;
            set => _bookmarks = value;
        }

        public BPMChangeEvent[] BpmChanges
        {
            get => _BPMChanges;
            set => _BPMChanges = value;
        }

        public override string ToString()
        {
            return
                $"\n Custom Data:" +
                $"\n\t Time: {_time}" +
                $"\n\t Bookmarks: {(_bookmarks.Length > 0 ? _bookmarks.Select(b => b.ToString()).Aggregate((a, b) => $"{a}, {b}") : "none")}" +
                $"\n\t BPM Changes: {(_BPMChanges.Length > 0 ? _BPMChanges.Select(b => b.ToString()).Aggregate((a, b) => $"{a}, {b}") : "none")}";

        }

        public object Clone()
        {
            return new CustomData()
            {
                _time = _time,
                _bookmarks = _bookmarks.DeepCopy() as BookmarkEvent[],
                _BPMChanges = _BPMChanges.DeepCopy() as BPMChangeEvent[]
            };
        }
    }
    /// <summary>
    ///     Bookmarks present in the BeatSaber map, e.g. Refrain, Chorus, Bridge etc.
    /// </summary>
    [Serializable]
    public class BookmarkEvent : TimedSongEvent
    {
        /// <summary>
        ///     The Bookmarks descriptive name.
        /// </summary>
        public string _name = "";
        
        /// <summary>
        ///     Clones the bookmark object.
        /// </summary>
        /// <returns>A deep copy.</returns>
        public override object Clone()
        {
            return new BookmarkEvent
            {
                _name = _name.Clone() as string,
                _time = _time
            };
        }

        public override string ToString()
        {
            return $"(time: {_time}, name: {_name})";
        }
    }

    /// <summary>
    ///     Changes of the songs Beats per Minute in the BeatSaber map. Not used in this project.
    /// </summary>
    [Serializable]
    public class BPMChangeEvent : TimedSongEvent
    {
        /// <summary>
        ///     The BPM to change to
        /// </summary>
        public float _BPM;

        /// <summary>
        ///     Number of beats a bar has from this point on
        /// </summary>
        public int _beatsPerBar;

        /// <summary>
        ///     Don't know what this is exactly, presumably for breaks or such?
        /// </summary>
        public int _metronomeOffset;

        public override object Clone()
        {
            return new BPMChangeEvent
            {
                _time = _time,
                _metronomeOffset = _metronomeOffset,
                _BPM = _BPM,
                _beatsPerBar = _beatsPerBar
            };
        }

        public override string ToString()
        {
            return $"(time: {_time}, tempo: {_BPM}, metrum: {_beatsPerBar}, offset: {_metronomeOffset})";
        }
    }

    #region enums

    /// <summary>
    ///     The cut direction  of blocks in beatSaber maps created wit Mediocre Map Assistant 2
    /// </summary>
    public enum CutDirection
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
        UpLeft = 4,
        UpRight = 5,
        DownLeft = 6,
        DownRight = 7,
        Any = 8,
        None = 99
    }

    [Serializable]
    public class UnityEventCutDirection : UnityEvent<CutDirection>
    {
    }

    /// <summary>
    ///     The cube colors in beatSaber maps created with Mediocre Map Assistant 2
    /// </summary>
    public enum CubeColor
    {
        Red = 0,
        Blue = 1,
        None = 99
    }

    [Serializable]
    public class UnityEventCubeColor : UnityEvent<CubeColor>
    {
    }

    /// <summary>
    ///     The block layers in BeatSaber maps created with Mediocre Map Assistant 2
    /// </summary>
    public enum LineLayer
    {
        Bottom = 0,
        Middle = 1,
        Top = 2
    }


    /// <summary>
    ///     The line index from left to right in BeatSaber maps created with Mediocre Map Assistant 2
    /// </summary>
    public enum LineIndex
    {
        First = 0,
        Second = 1,
        Third = 2,
        Fourth = 3
    }

    /// <summary>
    ///     The obstacle types defining how high an obstacle wall is (either full height or crouching).
    /// </summary>
    public enum ObstacleType
    {
        Full = 0,
        Crouch = 1
    }

    #endregion enums
}