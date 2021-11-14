using BeatMapper;
using Sirenix.OdinInspector;
using Spawning;
using UnityEngine;

namespace Game
{
    /// <summary>
    ///     Wraps the currently detected raw <see cref="CutDirection" />.
    /// </summary>
    [RequireComponent(typeof(BlockSettings))]
    public class CutState : MonoBehaviour
    {
        /// <summary>
        ///     The raw, detected <see cref="CutDirection" />.
        /// </summary>
        [SerializeField] [ReadOnly] private CutDirection detectedCutDirection = CutDirection.None;

        [SerializeField] [ReadOnly] private bool accepted;

        /// <summary>
        ///     Called each time the current note instance's CutDirection is updated, i.e. whenever the note is cut.
        /// </summary>
        [SerializeField] private UnityEventCutDirection onCutDirectionUpdated = new UnityEventCutDirection();

        [SerializeField] private UnityEventCutDirection onCutDirectionAccepted = new UnityEventCutDirection();

        /// <summary>
        ///     The currently detected, raw <see cref="CutDirection" />. Invokes <see cref="onCutDirectionUpdated" />.
        /// </summary>
        public CutDirection CutDirection
        {
            get => detectedCutDirection;
            set
            {
                // return if the CutDirection has been accepted already
                if (accepted) return;
                if (detectedCutDirection != CutDirection.None && value != CutDirection.None)
                    //Debug.Log($"[CutState] not updating, new value is {value}, old value is {detectedCutDirection}");
                    return;
                detectedCutDirection = value;
                onCutDirectionUpdated.Invoke(detectedCutDirection);
            }
        }

        /// <summary>
        ///     Accepts the current <see cref="CutDirection" /> as final and invokes <see cref="onCutDirectionAccepted" />
        /// </summary>
        [Button]
        public void AcceptCut()
        {
            if (accepted) return;
            accepted = true;
            //Debug.Log($@"[CutState] Accepting cut: {accepted}");
            onCutDirectionAccepted.Invoke(detectedCutDirection);
        }

        /// <summary>
        ///     Invokes <see cref="onCutDirectionAccepted" /> with the correct cut direction.
        /// </summary>
        [Button]
        public void InvokeCorrectCut()
        {
            if (accepted) return;
            detectedCutDirection = GetComponent<BlockSettings>().NoteEvent._cutDirection;
            AcceptCut();
        }
    }
}