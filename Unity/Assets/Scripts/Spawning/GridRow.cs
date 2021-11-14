using System;
using BeatMapper;
using UnityEngine;

namespace Spawning
{
    /// <summary>
    ///     Stores the LineLayer (i.e. row number) for the current row of <see cref="SpawnPosition" /> objects. To be used
    ///     on direct children of <see cref="SpawnGrid" />.
    /// </summary>
    [Serializable]
    public class GridRow : MonoBehaviour
    {
        [SerializeField] private LineLayer layer;

        /// <summary>
        ///     The <see cref="LineLayer" /> of this row.
        /// </summary>
        public LineLayer Layer => layer;
    }
}