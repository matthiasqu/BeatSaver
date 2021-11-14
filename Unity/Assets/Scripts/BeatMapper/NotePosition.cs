using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeatMapper
{
    /// <summary>
    ///     Class representing a notes position on the grid.
    /// </summary>
    [Serializable]
    [InlineProperty]
    public struct NotePosition
    {
        /// <summary>
        ///     The line from left to right
        /// </summary>
        [SerializeField] [InlineProperty] [EnumPaging]
        private LineIndex lineIndex;

        /// <summary>
        ///     The spawn layer from top to bottom
        /// </summary>
        [SerializeField] [InlineProperty] [EnumPaging]
        private LineLayer lineLayer;

        public NotePosition(LineIndex lineIndex, LineLayer lineLayer)
        {
            this.lineIndex = lineIndex;
            this.lineLayer = lineLayer;
        }

        public LineLayer LineLayer => lineLayer;

        public LineIndex LineIndex => lineIndex;

        private bool Equals(NotePosition other)
        {
            return lineIndex == other.lineIndex && lineLayer == other.lineLayer;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((NotePosition) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) lineIndex * 397) ^ (int) lineLayer;
            }
        }
    }
}