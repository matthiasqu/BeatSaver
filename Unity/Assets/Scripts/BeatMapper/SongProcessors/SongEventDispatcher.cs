using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Utils;

namespace BeatMapper.SongProcessors
{
    /// <summary>
    ///     Component providing functionality to read BeatSaber beat maps and send it using the <see cref="OnSongLoaded" />
    ///     event.
    /// </summary>
    public class SongEventDispatcher : MonoBehaviour
    {
        /// <summary>
        ///     The song data. Mediocre map Assistant 2 supplies .dat files, which have to be renamed to .json to use them
        /// </summary>
        [Tooltip(".json file containing a BeatSaber map - rename .dat files from MMA2 to use them.")] [SerializeField]
        private TextAsset songData;

        /// <summary>
        ///     Event fired as soon as the song has been loaded.
        /// </summary>
        [Tooltip("Provides the loaded SongMapping.")] [SerializeField]
        private UnityEventSongMapping OnSongLoaded = new UnityEventSongMapping();

        public UnityEventInt OnNumberBlocksLoaded = new UnityEventInt();
        public UnityEventInt OnNumberObstaclesLoaded = new UnityEventInt();

        private void Start()
        {
            ReadSongData();
        }

        [Button]
        private void PrintNotes()
        {
            var song = GetSongMapping(songData);
            Debug.Log($"[SongEventDispatcher] The song contains {song.Notes.Length} notes");
            song.Notes
                .ForEach(n => Debug.Log($"[SongEventDispatcher] Note time: {n._time}" +
                                        $"\nline index: {n._lineIndex}" +
                                        $"\nline layer: {n._lineLayer}" +
                                        $"\ntype: {n._type}" +
                                        $"\ncut direction: {n._cutDirection}"));
        }

        /// <summary>
        ///     Reads the song data from the file defined in <see cref="songData" /> and invokes <see cref="OnSongLoaded" />.
        /// </summary>
        [Button]
        private void ReadSongData()
        {
            var mapping = GetSongMapping(songData);
            OnSongLoaded.Invoke(mapping);
            OnNumberBlocksLoaded.Invoke(mapping.Notes.Length);
            OnNumberObstaclesLoaded.Invoke(mapping.Obstacles.Length);
        }

        /// <summary>
        ///     Reads the song mapping.
        /// </summary>
        /// <param name="song">The <see cref="TextAsset" /> to read from</param>
        /// <returns>A <see cref="SongMapping" />.</returns>
        private SongMapping GetSongMapping(TextAsset song)
        {
            return JsonUtility.FromJson<SongMapping>(song.text);
        }
    }
}