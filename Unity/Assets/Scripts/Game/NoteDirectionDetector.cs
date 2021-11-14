using System;
using BeatMapper;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Game
{
    /// <summary>
    ///     Component used for detecting a cut direction. This can be used to store a direction before it is actually accepted,
    ///     e.g. when detecting cuts with multiple colliders etc.
    /// </summary>
    // TODO: Find a better name
    public class NoteDirectionDetector : MonoBehaviour
    {
        /// <summary>
        ///     Currently detected cut direction.
        /// </summary>
        [TabGroup("General")] [SerializeField] private CutDirection cutDirection = CutDirection.None;

        /// <summary>
        ///     Event invoked whenever a cut direction is detected or changed.
        /// </summary>
        [TabGroup("General")] [SerializeField] private UnityEventCutDirection onCut = new UnityEventCutDirection();

        /// <summary>
        ///     Event invoked whenever the cutting object is not cutting anymore (e.g. when the cutting gameobject has left the
        ///     collider).
        /// </summary>
        [TabGroup("Additional")] [SerializeField]
        private UnityEventCutDirection onLeft = new UnityEventCutDirection();

        /// <summary>
        ///     Event supplying the value for the cutting object's position on the y-axis relative to the current object's centert.
        /// </summary>
        [TabGroup("Additional")] [SerializeField]
        private UnityEventFloat onUpwards = new UnityEventFloat();

        /// <summary>
        ///     Event supplying th evalue for the cutting object'S position on the x-axis relative to the current objetc's center.
        /// </summary>
        [TabGroup("Additional")] [SerializeField]
        private UnityEventFloat onRightwards = new UnityEventFloat();

        /// <summary>
        ///     The cut direction currently observed by this component.
        /// </summary>
        public CutDirection CutDirection
        {
            get => cutDirection;
            set
            {
                if (value == cutDirection) return;

                cutDirection = value;
                //Debug.Log($"[NoteDirectionDetector] New direction is {value}");
            }
        }

        /// <summary>
        ///     Determines the component's cut direction based on the position of <see cref="other" />.
        /// </summary>
        /// <param name="other">
        ///     The cutting game object. Needs to have a <see cref="FramedPositionBuffer" /> on any of its
        ///     children.
        /// </param>
        public void Invoke(GameObject other)
        {
//            Debug.Log($"[NoteDirectionConverter] Other is {other.gameObject.name}");
            var framedPos = other.GetComponentInChildren<FramedPositionBuffer>();
            if (framedPos == null)
                Debug.LogError($"[NoteDirectionDetector] No FramedPositionBuffer component on {other.name} found!");
            var farthestPoint = framedPos.FarthestPoint;
            var normalized = (farthestPoint - transform.position).normalized;
            var up = Vector3.Dot(normalized, gameObject.transform.up);
            var right = Vector3.Dot(normalized, gameObject.transform.right);
            var detectedDirection = CutDirection.None;

            if (up > 0)
            {
                if (Math.Abs(right) > Math.Abs(up))
                    detectedDirection = right > 0 ? CutDirection.Right : CutDirection.Left;
                else
                    detectedDirection = CutDirection.Down;
            }
            else if (up < 0)
            {
                if (Math.Abs(right) > Math.Abs(up))
                    detectedDirection = right > 0 ? CutDirection.Right : CutDirection.Left;
                else
                    detectedDirection = CutDirection.Up;
            }

            onUpwards.Invoke(up);
            onRightwards.Invoke(right);
            CutDirection = detectedDirection;
            onCut.Invoke(CutDirection);
        }

        /// <summary>
        ///     Invokes the <see cref="onLeft" /> event.
        /// </summary>
        /// TODO: Remove this and fire
        /// <see cref="onCut" />
        /// with CutDirection.None
        public void Devoke()
        {
            onLeft.Invoke(CutDirection.None);
        }

        [TabGroup("General")]
        [Button]
        private void InvokeEnter()
        {
            Invoke(new GameObject());
        }

        [TabGroup("General")]
        [Button]
        private void InvokeExit()
        {
            Devoke();
        }
    }
}