using Game.SongSelector;

namespace Game
{
    /// <summary>
    ///     Provides tatic methods that can be used throughout the Game namespace.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        ///     Determines whether or not a supplied tick is relevant when listening for a specific note length.
        /// </summary>
        /// <param name="tick">Note length as int</param>
        /// <param name="note">The kind of note to listen for</param>
        /// <returns>Whether a tick of length <see cref="tick" /> also marks a note of type <see cref="note" /></returns>
        public static bool TickIsRelevant(int tick, Note note)
        {
            return tick <= (int) note;
        }
    }
}