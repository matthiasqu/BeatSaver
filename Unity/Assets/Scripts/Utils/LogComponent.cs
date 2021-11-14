using System.Globalization;
using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     Logs a supplied value to the console.
    /// </summary>
    public class LogComponent : MonoBehaviour
    {
        /// <summary>
        ///     Log the supplied value to the console.
        /// </summary>
        /// <param name="value">The value to log as its string representation</param>
        public void Log(int value)
        {
            _Log(value.ToString());
        }

        /// <summary>
        ///     Log the supplied value to the console.
        /// </summary>
        /// <param name="value">The value to log as its string representation</param>
        public void Log(float value)
        {
            _Log(value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Log the supplied value to the console.
        /// </summary>
        /// <param name="value">The value to log as its string representation</param>
        public void Log(string value)
        {
            _Log(value);
        }

        private void _Log(string s)
        {
            Debug.Log($"[LogComponent on {gameObject.name}] {s}");
        }
    }
}