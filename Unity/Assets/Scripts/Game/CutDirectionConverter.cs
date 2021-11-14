using BeatMapper;
using BeatMapper.Utils;
using Sirenix.OdinInspector;
using Spawning;
using UnityEngine;

namespace Game
{
    /// <summary>
    ///     Converts a detected cut direction by a Note's child objects to an actual world space kind of cut direction
    ///     which can then be compared to the intended cut direction present in the note's settings.
    /// </summary>
    [RequireComponent(typeof(BlockSettings))]
    public class CutDirectionConverter : MonoBehaviour
    {
        /// <summary>
        ///     The converted cut direction for debugging purposes.
        /// </summary>
        [SerializeField] [ReadOnly] private CutDirection convertedDetectedCutDirection = CutDirection.None;

        /// <summary>
        ///     Sends the converted direction to all listeners.
        /// </summary>
        [SerializeField] private UnityEventCutDirection onDirectionConverted = new UnityEventCutDirection();

        /// <summary>
        ///     The <see cref="CutDirection" /> after conversion.
        /// </summary>
        public CutDirection DetectedCutDirection
        {
            get => convertedDetectedCutDirection;
            set
            {
                convertedDetectedCutDirection = DetermineActualCutDirection(value);
                onDirectionConverted.Invoke(convertedDetectedCutDirection);
            }
        }

        /// <summary>
        ///     Converts the detected cut direction set through <see cref="DetectedCutDirection" /> to a world-space kind of cut
        ///     direction.
        /// </summary>
        /// <param name="detectedDirection"></param>
        /// <returns>World-space cut direction</returns>
        private CutDirection DetermineActualCutDirection(CutDirection detectedDirection)
        {
            var blockSettings = GetComponent<BlockSettings>();
            var noteDir = blockSettings.NoteEvent._cutDirection;
            return detectedDirection.ToWorldCutDirection(noteDir);
        }
    }
}