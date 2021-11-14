using System.Collections.Generic;
using BeatMapper;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Spawning
{
    /// <summary>
    ///     A single container for spawn positions in the <see cref="SpawnGrid" />.
    ///     SpawnPositions can be accessed just like in a dictionary or array using [] notation.
    /// </summary>
    public class SpawnPositionsContainer : SerializedMonoBehaviour
    {
        /// <summary>
        ///     The <see cref="SpawnPosition" /> objects tracked by this container.
        /// </summary>
        [OdinSerialize] [HideLabel]
        private Dictionary<NotePosition, SpawnPosition> spawnPositions = new Dictionary<NotePosition, SpawnPosition>();

        /// <summary>
        ///     Gets the SpawnPosition object corresponding to the <see cref="NotePosition" /> supplied.
        /// </summary>
        /// <param name="notePosition">The <see cref="NotePosition" /> to look up in <see cref="spawnPositions" /></param>
        public SpawnPosition this[NotePosition notePosition] => spawnPositions[notePosition];
    }
}