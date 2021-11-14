using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     Reports the local scale of the attached transform via an unityEventVector3
    /// </summary>
    public class LocalScaleReporter : MonoBehaviour
    {
        //TODO: rename to onStart
        /// <summary>
        ///     Called once on Start(), reports the current local scale
        /// </summary>
        [SerializeField] private UnityEventVector3 onInitialReport = new UnityEventVector3();

        /// <summary>
        ///     Reports the current localScale whenever it is changed (only when the application is playing).
        /// </summary>
        [SerializeField] private UnityEventVector3 onScaleChanged = new UnityEventVector3();

        private Vector3 _bufferedScale = Vector3.zero;

        private void Start()
        {
            onInitialReport.Invoke(transform.localScale);
        }

        /// <summary>
        ///     Invokes <see cref="onScaleChanged" /> whenever it is changed during gameplay.
        /// </summary>
        private void OnValidate()
        {
            if (!Application.isPlaying) return;
            if (_bufferedScale == transform.localScale) return;
            _bufferedScale = transform.localScale;
            onScaleChanged.Invoke(_bufferedScale);
        }
    }
}