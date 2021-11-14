using UnityEngine;

namespace Game
{
    /// <summary>
    ///     Used for triggering impulses on one controller.
    /// </summary>
    public class HapticFeedbackTrigger : MonoBehaviour
    {
        /// <summary>
        ///     Invokes haptic feedback using the <see cref="Vibrator" /> attached to <see cref="other" />.
        /// </summary>
        /// <param name="other">A GameObject which functions as a controller</param>
        public void Trigger(GameObject other)
        {
            var vibrator = other.GetComponentInChildren<Vibrator>();
            if (vibrator != null) vibrator.Vibrate();
            else
                Debug.LogError($"[HapticFeedbackTrigger] No Vibrator component found on {other.name} or its children!");
        }
    }
}