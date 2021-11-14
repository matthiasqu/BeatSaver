using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    /// <summary>
    ///     Triggers haptic feedback on the controller set in <see cref="controller" />.
    ///     TODO: find better name
    /// </summary>
    public class Vibrator : MonoBehaviour
    {
        /// <summary>
        ///     The controller on which to provide haptic feedback.
        /// </summary>
        [SerializeField] private OVRInput.Controller controller;

        /// <summary>
        ///     The feedback duration in ms.
        /// </summary>
        [SerializeField] [Range(1, 300)] private int feedbackDuration = 50;

        /// <summary>
        ///     The amplitude of the vibration.
        /// </summary>
        [SerializeField] [Range(0f, 1f)] private float amplitude = .5f;

        /// <summary>
        ///     The frequency of the vibration.
        /// </summary>
        [SerializeField] [Range(0f, 1f)] private float frequency = .5f;

        /// <summary>
        ///     A <see cref="TimeSpan" /> object corresponding to the time set in <see cref="feedbackDuration" />
        /// </summary>
        private TimeSpan FeedbackDuration => new TimeSpan(0, 0, 0, 0, feedbackDuration);

        /// <summary>
        ///     The current VibrationStatus of the controller.
        /// </summary>
        private VibrationStatus VibrationStatus { get; set; } = VibrationStatus.Still;

        /// <summary>
        ///     Set the vibration if its not yet vibrating.
        /// </summary>
        public async void Vibrate()
        {
            if (VibrationStatus == VibrationStatus.Vibrating) return;

            // set vibration ON
            VibrationStatus = VibrationStatus.Vibrating;
            OVRInput.SetControllerVibration(frequency, amplitude, controller);

            // wait 
            await Task.Delay(FeedbackDuration);

            // set vibration OFF
            OVRInput.SetControllerVibration(0, 0, controller);
            VibrationStatus = VibrationStatus.Still;
        }
    }

    internal enum VibrationStatus
    {
        Still,
        Vibrating
    }
}