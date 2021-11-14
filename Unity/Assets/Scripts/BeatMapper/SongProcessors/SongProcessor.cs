using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace BeatMapper.SongProcessors
{
    /// <summary>
    ///     Base class from which all components processing <see cref="SongMapping" /> data should inherit from.
    /// </summary>
    public abstract class SongProcessor : SerializedMonoBehaviour
    {
        /// <summary>
        ///     Sends the processed SongMapping to all listeners.
        ///     Should be Invoked when ProcessSong() has finished.
        /// </summary>
        [SerializeField] protected UnityEvent<SongMapping> onProcessed = new UnityEvent<SongMapping>();

        // A deep copy of the last SongMapping before processing it
        protected SongMapping SongMapping { get; set; }

        /// <summary>
        ///     Processes the incoming SongMapping and invokes OnProcessed() with the processed SongMapping.
        /// </summary>
        /// <param name="songMapping"></param>
        public abstract void ProcessSong(SongMapping songMapping);

        /// <summary>
        ///     Print the original and processed data.
        /// </summary>
        [Button]
        protected abstract void Print();
    }
}