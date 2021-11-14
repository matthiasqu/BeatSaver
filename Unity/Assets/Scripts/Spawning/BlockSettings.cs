using BeatMapper;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spawning
{
    /// <summary>
    ///     Represents the Note's configuration and passes the value on to other components
    /// </summary>
    public class BlockSettings : MonoBehaviour
    {
        /// <summary>
        ///     The assigned NoteEvent that spawned this instance.
        /// </summary>
        [SerializeField] [InlineProperty] [HideLabel]
        private NoteEvent noteEvent;


        /// <summary>
        ///     Passes the current settings onto other objects, such as the <see cref="BlockRotationSetter" />
        ///     and <see cref="BlockMaterialSetter" />.
        /// </summary>
        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventNoteEvent onNoteReceived = new UnityEventNoteEvent();

        /// <summary>
        ///     Passes the cut direction for this block onto other components, such as the <see cref="BlockRotationSetter" />.
        /// </summary>
        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventCutDirection onCutDirectionReceived = new UnityEventCutDirection();

        /// <summary>
        ///     Passes the color of the Block onto other components, such as the <see cref="BlockMaterialSetter" />.
        /// </summary>
        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventCubeColor onBlockTypeReceived = new UnityEventCubeColor();


        /// <summary>
        ///     The <see cref="NoteEvent" /> which caused this Note to spawn. Invokes <see cref="onNoteReceived" />,
        ///     <see cref="onBlockTypeReceived" />, and <see cref="onCutDirectionReceived" />.
        /// </summary>
        public NoteEvent NoteEvent
        {
            get => noteEvent;
            set
            {
                noteEvent = value;
                onNoteReceived.Invoke(value);
                onCutDirectionReceived.Invoke(noteEvent._cutDirection);
                onBlockTypeReceived.Invoke(noteEvent._type);
            }
        }

        private void Start()
        {
            if (NoteEvent is null) onBlockTypeReceived.Invoke(NoteEvent._type);
        }

        private void OnValidate()
        {
            if (!Application.isPlaying) return;
            NoteEvent = noteEvent;
        }
    }
}