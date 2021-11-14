using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Game
{
    public class GameSettingsBridge : MonoBehaviour
    {
        /// <summary>
        ///     Invokes events from the <see cref="GameSettings" /> into the scene and assigns listeners.
        /// </summary>
        private void Start()
        {
            onNoteDistanceLoaded.Invoke(new Vector3(0, 0, gameSettings.NoteDistance));
            onNotePlayerOffsetLoaded.Invoke(new Vector3(0, 0, gameSettings.NotePlayerOffset));
            onNoteSpeedLoaded.Invoke(gameSettings.NoteSpeed);
            onBeatsPerMinuteLoaded.Invoke(gameSettings.BeatsPerminute);
            onNoteAnticipationTimeLoaded.Invoke(gameSettings.NoteAnticipationTime);
            onGamePrewarmTimeLoaded.Invoke(-gameSettings.GamePrewarmTime);
            onTicksPerBarLoaded.Invoke(gameSettings.TicksPerBar);
            onNoteScaleChanged.Invoke(gameSettings.NoteScale);

            gameSettings.NoteSpeedChanged.AddListener(onNoteSpeedLoaded.Invoke);
            gameSettings.BeatsPerMinuteChanged.AddListener(onBeatsPerMinuteLoaded.Invoke);
            gameSettings.GamePrewarmTimeChanged.AddListener(onGamePrewarmTimeLoaded.Invoke);
            gameSettings.NotePlayerOffsetChanged.AddListener(onNotePlayerOffsetLoaded.Invoke);
            gameSettings.NoteDistanceChanged.AddListener(onNoteDistanceLoaded.Invoke);
            gameSettings.TicksPerBarChanged.AddListener(onTicksPerBarLoaded.Invoke);
            gameSettings.NoteScaleChanged.AddListener(onNoteScaleChanged.Invoke);
            gameSettings.NoteAnticipationTimeChanged.AddListener(onNoteAnticipationTimeLoaded.Invoke);
            if(OVRManager.display != null) OVRManager.display.displayFrequency = 90;
        }

        [Button]
        private void AssignListeners()
        {
            if (Application.isPlaying) return;
            Start();
        }

        #region Events

        /// <summary>
        ///     The GameSettings object to track.
        /// </summary>
        [SerializeField] private GameSettings gameSettings;

        /// <summary>
        ///     The distance between the Player and the SpawnPositions
        /// </summary>
        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventVector3 onNoteDistanceLoaded = new UnityEventVector3();

        /// <summary>
        ///     The Distance between the Player and the place where notes are on their beat.
        /// </summary>
        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventVector3 onNotePlayerOffsetLoaded = new UnityEventVector3();

        /// <summary>
        ///     The speed with which Blocks and Obstacles move.
        /// </summary>
        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventFloat onNoteSpeedLoaded = new UnityEventFloat();

        /// <summary>
        ///     The BPM of the current song.
        /// </summary>
        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventFloat onBeatsPerMinuteLoaded = new UnityEventFloat();

        /// <summary>
        ///     The ticks (quarters) that make up a bar. 4 for a 4/4 metrum.
        /// </summary>
        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventInt onTicksPerBarLoaded = new UnityEventInt();

        /// <summary>
        ///     The time between spawning of Blocks and arrival at the NotePlayerOFfset position.
        /// </summary>
        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventFloat onNoteAnticipationTimeLoaded = new UnityEventFloat();

        /// <summary>
        ///     The time before the spawning begins.
        /// </summary>
        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventFloat onGamePrewarmTimeLoaded = new UnityEventFloat();

        /// <summary>
        ///     The scale of the notes. TODO: Deprecated, use spawngrid settings instead.
        /// </summary>
        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventVector3 onNoteScaleChanged = new UnityEventVector3();

        #endregion Events
    }
}