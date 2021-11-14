using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Game
{
    /// <summary>
    ///     Detects Collisions and trigger objects with attached <see cref="Sliceable" /> component.
    /// </summary>
    /// TODO: remove
    public class SliceableDetector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Sliceable>() != null) onSliceableEnter.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Sliceable>() != null) onSliceableExit.Invoke(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<Sliceable>() != null) onSliceableStay.Invoke(other.gameObject);
        }

        #region events

        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventGameObject onSliceableEnter = new UnityEventGameObject();

        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventGameObject onSliceableStay = new UnityEventGameObject();

        [FoldoutGroup("Events", false)] [SerializeField]
        private UnityEventGameObject onSliceableExit = new UnityEventGameObject();

        #endregion
    }
}