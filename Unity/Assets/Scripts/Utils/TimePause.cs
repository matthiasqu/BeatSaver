using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     Pauses the GameTime and resumes it when the <see cref="Trigger" /> method is called.
    /// </summary>
    public class TimePause : MonoBehaviour
    {
        /// <summary>
        ///     Triggers pausing and resuming.
        /// </summary>
        [Button]
        public void Trigger()
        {
            Time.timeScale = Time.timeScale > 0 ? 0 : 1;
        }
    }
}