using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Game
{
    [CreateAssetMenu(menuName = "_custom/GameSettings", fileName = "Game Settings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private bool inverseDirections;

        private void OnValidate()
        {
            //TODO: Add Application.isRunning check
            noteAnticipationTime = NoteAnticipationTime.ToString();

            NotePlayerOffsetChanged.Invoke(new Vector3(0, 0, NotePlayerOffset));
            BeatsPerMinuteChanged.Invoke(BeatsPerminute);
            NoteDistanceChanged.Invoke(new Vector3(0, 0, NoteDistance));
            NoteScaleChanged.Invoke(NoteScale);
            TicksPerBarChanged.Invoke(TicksPerBar);
        }

        /// <summary>
        ///     Invokes NoteDistanceChanged with the supplied Vector3's z coordinate + <see cref="NoteDistance" /> value.
        /// </summary>
        /// <param name="f"></param>
        public void AdjustSpawnDistance(Vector3 f)
        {
            // Debug.Log($"Adjusting SpawnDistance to {NoteDistance + (inverseDirections? -f.z:f.z)} on z (add {f}");
            NoteDistanceChanged.Invoke(new Vector3(0, 0, NoteDistance + (inverseDirections ? -f.z : f.z)));
        }

        /// <summary>
        ///     Invokes <see cref="NotePlayerOffsetChanged" /> with the supplied Vector3's z-coordinate + the
        ///     <see cref="NotePlayerOffset" /> value.
        /// </summary>
        /// <param name="f"></param>
        public void AdjustPlayerOffset(Vector3 f)
        {
            //Debug.Log($"Adjusting NotePlayerOffset to {NotePlayerOffset + (inverseDirections? -f.z:f.z)} on z (add {f}");
            NotePlayerOffsetChanged.Invoke(new Vector3(0, 0, NotePlayerOffset + (inverseDirections ? -f.z : f.z)));
        }

        private void DetermineNoteSpeed()
        {
        }

        #region Member

        [SerializeField] private float beatsPerminute = 120;
        [SerializeField] private float noteSpeed = 1;
        [SerializeField] private float noteDistance = 3;
        [SerializeField] private float notePlayerOffset = .1f;
        [ReadOnly] [SerializeField] private string noteAnticipationTime;
        [SerializeField] private float gamePrewarmTime = 5f;
        [SerializeField] private int ticksPerBar;
        [SerializeField] [ReadOnly] private Vector3 noteScale;

        #endregion

        #region Events

        [FoldoutGroup("Events", false)] public UnityEventFloat BeatsPerMinuteChanged = new UnityEventFloat();

        [FoldoutGroup("Events", false)] public UnityEventFloat NoteSpeedChanged = new UnityEventFloat();

        [FoldoutGroup("Events", false)] public UnityEventVector3 NoteDistanceChanged = new UnityEventVector3();

        [FoldoutGroup("Events", false)] public UnityEventVector3 NotePlayerOffsetChanged = new UnityEventVector3();

        [FoldoutGroup("Events", false)] public UnityEventFloat GamePrewarmTimeChanged = new UnityEventFloat();

        [FoldoutGroup("Events", false)] public UnityEventVector3 NoteScaleChanged = new UnityEventVector3();

        [FoldoutGroup("Events", false)] public UnityEventInt TicksPerBarChanged = new UnityEventInt();

        [FoldoutGroup("Events", false)] public UnityEventFloat NoteAnticipationTimeChanged = new UnityEventFloat();

        #endregion Events

        #region Properties

        /// <summary>
        ///     The offset between the player and where the notes are aiming. This changes where the notes are on a specific beat.
        /// </summary>
        /// TODO: Remove, and use in-scene objects instead.
        public float NotePlayerOffset
        {
            get => notePlayerOffset;
            set
            {
                if (Math.Abs(value - notePlayerOffset) < float.Epsilon) return;

                notePlayerOffset = value;
                NotePlayerOffsetChanged.Invoke(new Vector3(0, 0,
                    inverseDirections ? -NotePlayerOffset : +NotePlayerOffset));
            }
        }

        /// <summary>
        ///     The time until the game starts playing. This is used to have notes be generated before the song starts.
        /// </summary>
        public float GamePrewarmTime
        {
            get => gamePrewarmTime + NoteAnticipationTime;
            set
            {
                if (Math.Abs(value - gamePrewarmTime) < float.Epsilon) return;

                gamePrewarmTime = value;
                GamePrewarmTimeChanged.Invoke(GamePrewarmTime);
            }
        }

        /// <summary>
        ///     Beats per minute of the track being played
        /// </summary>
        public float BeatsPerminute
        {
            get => beatsPerminute;
            set
            {
                if (Math.Abs(value - beatsPerminute) < float.Epsilon) return;

                beatsPerminute = value;
                BeatsPerMinuteChanged.Invoke(BeatsPerminute);
            }
        }

        /// <summary>
        ///     The movement speed of notes
        /// </summary>
        public float NoteSpeed
        {
            get => noteSpeed;
            set
            {
                if (Math.Abs(value - noteSpeed) < float.Epsilon) return;

                noteSpeed = value;
                NoteSpeedChanged.Invoke((inverseDirections ? -1 : 1) * NoteSpeed);
                NoteAnticipationTimeChanged.Invoke(NoteAnticipationTime);
            }
        }

        /// <summary>
        ///     The spawn distance of the notes
        /// </summary>
        public float NoteDistance
        {
            //get => noteDistance - NoteScale.z/2;
            get => noteDistance;
            set
            {
                if (Math.Abs(value - noteDistance) < float.Epsilon) return;

                noteDistance = value;
                NoteDistanceChanged.Invoke(new Vector3(0, 0, inverseDirections ? -NoteDistance : NoteDistance));
                NoteAnticipationTimeChanged.Invoke(NoteAnticipationTime);
            }
        }

        /// <summary>
        ///     TODO: Deprecated. Use the spawngrid instead.
        ///     The scale of the Notes when spawned.
        /// </summary>
        public Vector3 NoteScale
        {
            get => noteScale;
            set
            {
                if (value == noteScale) return;
                noteScale = value;
                NoteScaleChanged.Invoke(value);
                NoteAnticipationTimeChanged.Invoke(NoteAnticipationTime);
            }
        }

        /// <summary>
        ///     The time before notes reach the player
        /// </summary>
        public float NoteAnticipationTime
        {
            get
            {
                if (noteDistance == 0 || noteSpeed == 0)
                    throw new Exception(
                        $"Game Settings invalid, Note Speed: {noteSpeed}, Note Distance: {noteDistance}");
                return (NoteDistance - NotePlayerOffset) / NoteSpeed;
            }
        }

        /// <summary>
        ///     How many Ticks make up one bar. The Ticks should be sent from Ableton to Unity and can be used to
        ///     visualize song progression.
        /// </summary>
        public int TicksPerBar
        {
            get => ticksPerBar;
            set
            {
                if (value == ticksPerBar) return;
                ticksPerBar = value;
                TicksPerBarChanged.Invoke(TicksPerBar);
            }
        }

        #endregion
    }
}