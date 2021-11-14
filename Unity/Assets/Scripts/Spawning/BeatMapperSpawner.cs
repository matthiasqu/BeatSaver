using BeatMapper;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Spawning
{
    /// <summary>
    ///     A wrapper for an implementation of <see cref="ISpawner" /> which can process <see cref="SongMapping" />
    ///     objects and spawns objects according to the mapping.
    /// </summary>
    public class BeatMapperSpawner : SerializedMonoBehaviour
    {
        /// <summary>
        ///     The spawning strategy used by this spawner
        /// </summary>
        [OdinSerialize] private ISpawner spawner;

        /// <summary>
        ///     The spawn positions to use for this spawner instance.
        /// </summary>
        [OdinSerialize] private SpawnPositionsContainer spawnPositions;

        /// <summary>
        ///     Stores the last spawned SongMapping.
        /// </summary>
        private SongMapping SongMapping { get; set; }


        /// <summary>
        ///     Forwards a reference to the this GameObject to <see cref="spawner" />.
        /// </summary>
        private void Awake()
        {
            spawner.Awake(gameObject);
        }

        /// <summary>
        ///     Takes the supplied SongMapping and instantiates alls events present. The SongMapping should be a slice
        ///     of the events which should be processed at the current time.
        /// </summary>
        /// <param name="songMapping">A clice of a song mapping to process.</param>
        public void ProcessEvents(SongMapping songMapping)
        {
            SongMapping = songMapping;
            spawner.Spawn(SongMapping, spawnPositions);
        }
    }
}