using BeatMapper;
using UnityEngine;

namespace Spawning
{
    /// <summary>
    ///     Interface defining a Spawn() method. Used throughout Spawner scriptable objects
    ///     (such as <see cref="ObstacleSpawner" /> and <see cref="NoteSpawner" />
    /// </summary>
    public interface ISpawner
    {
        /// <summary>
        ///     Through the Awake() call, implementations of <see cref="ISpawner" /> can gain access to the
        ///     <see cref="BeatMapperSpawner" /> GameObject this is used in.
        /// </summary>
        /// <param name="gameObject"></param>
        public void Awake(GameObject gameObject);

        /// <summary>
        ///     Spawns Objects using the Data present in <see cref="SongEvent" /> using the assigned <see cref="spawnpositions" />.
        /// </summary>
        /// <param name="songMapping">The data to process.</param>
        /// <param name="spawnpositions">The SpawnPositions to use for spawning. </param>
        public void Spawn(SongMapping songMapping, SpawnPositionsContainer spawnpositions);
    }
}