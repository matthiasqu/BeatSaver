using Game.SongSelector;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Game
{
    /// <summary>
    ///     Wrapper for implementations of <see cref="ISongSelector" /> responsible for changing between versions of a
    ///     specific song.
    /// </summary>
    /// TODO: rename to SongSelector
    public class SongManipulator : SerializedMonoBehaviour
    {
        /// <summary>
        ///     Actual behaviour determining which song version to pick.
        /// </summary>
        [OdinSerialize] private ISongSelector songSelector;

        /// <summary>
        ///     Supply a reference to the scene via this GameObject.
        /// </summary>
        private void Start()
        {
            songSelector.Start(gameObject);
        }

        /// <summary>
        ///     Forwarding ticks received from a <see cref="Clock" /> component to the <see cref="songSelector" />.
        /// </summary>
        /// <param name="tick">Tick from a <see cref="Clock" /> component.</param>
        public void ReceiveTick(int tick)
        {
            songSelector.ReceiveTick(tick);
        }
    }
}