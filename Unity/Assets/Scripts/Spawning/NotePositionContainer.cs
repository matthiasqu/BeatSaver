using System;
using BeatMapper;
using UnityEngine;

namespace Spawning
{
    /// <summary>
    ///     Tags the GameObject to be a specific position in a <see cref="SpawnGrid" />.
    /// </summary>
    [Serializable]
    public class NotePositionContainer : MonoBehaviour
    {
        /// <summary>
        ///     The position of this object in the grid.
        /// </summary>
        [SerializeField] private NotePosition noteposition;

        public NotePosition NotePosition => noteposition;
    }
}