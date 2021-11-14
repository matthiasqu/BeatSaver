using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using Utils;

namespace Game
{
    /// <summary>
    ///     A countdown component which counts time from a negative timestamp to 0.
    ///     TODO: Replace by <see cref="Timer" />.
    /// </summary>
    public class PrewarmCountdown : MonoBehaviour
    {
        /// <summary>
        ///     Called when the countdown starts. This should invoke Dispatching of song events thourgh e.g.
        ///     the SongMappingSliceProcessor in the BeatMapper namespace.
        /// </summary>
        [Tooltip("Called on the beginning of the countdown. Should invoke dispatching of song events.")]
        public UnityEventFloat onCountdownStart = new UnityEventFloat();

        /// <summary>
        ///     Invoked as soon as the countdown has passed 0.
        ///     This should be used to invoke all functionality necessary to invoke the start of the song playback.
        /// </summary>
        [Tooltip("Invoked as soon as the countdpwn reaches 0. " +
                 "Can be used to exectute events when the song should start playback.")]
        public UnityEvent onCountdownEnded = new UnityEvent();

        /// <summary>
        ///     The prewarm time is calculated in <see cref="GameSettings" /> and set by <see cref="GameSettingsBridge" />.
        ///     The time includes the notes anticipation time and time before the
        /// </summary>
        [Tooltip("The time it takes until the song starts. " +
                 "Set by GameSettingsBridge as this determines when the first notes reach the player.")]
        [SerializeField]
        [ReadOnly]
        private float gamePrewarmTime;

        /// <summary>
        ///     The current time displayed for debugging purposes.
        /// </summary>
        [Tooltip("The current time of the countdown for debugging purposes.")] [SerializeField] [ReadOnly]
        private float _countdownTime;

        [SerializeField] private bool autoStart;

        private bool _started;

        /// <summary>
        ///     The time until the song itself starts. Necessary for notes to be spawned before the song hits.
        ///     This should not be set directly but is calculated by in the GameSettings prefab and set on Level Load.
        ///     This value needs to be negative.
        /// </summary>
        public float GamePrewarmTime
        {
            get => gamePrewarmTime;
            set
            {
                Assert.IsTrue(value < 0);
                gamePrewarmTime = value;
                _started = false;
                StartCountdown();
            }
        }

        private void Start()
        {
            if (autoStart) StartCountdown();
        }

        /// <summary>
        ///     Counts the time up. invokes <see cref="onCountdownEnded" /> when 0 is passed and resets
        ///     <see cref="_started" /> to false.
        /// </summary>
        private void Update()
        {
            if (!_started) return;

            _countdownTime += Time.deltaTime;
            if (_countdownTime < 0) return;

            onCountdownEnded.Invoke();
            _started = false;
        }

        /// <summary>
        ///     Starts counting up from the <see cref="GamePrewarmTime" /> to 0.
        ///     Invokes <see cref="onCountdownStart" />.
        /// </summary>
        [Button]
        public void StartCountdown()
        {
            if (_started)
            {
                Debug.Log("[PrewarmCountdown] Start has already been called. Not starting again.");
                return;
            }

            Debug.Log($"[PrewarmCountdown] Starting countdown with {GamePrewarmTime}");
            onCountdownStart.Invoke(GamePrewarmTime);
            _countdownTime = gamePrewarmTime;
            _started = true;
        }
    }
}