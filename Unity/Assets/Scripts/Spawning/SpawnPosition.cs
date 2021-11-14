using UnityEngine;

namespace Spawning
{
    /// <summary>
    ///     Tag class for a SpawnPosition in a <see cref="SpawnGrid" /> or <see cref="SpawnPositionsContainer" />
    /// </summary>
    public class SpawnPosition : MonoBehaviour
    {
        /// <summary>
        ///     The color of the Gizmo drawn by <see cref="OnDrawGizmos" />.
        /// </summary>
        [SerializeField] private Color color;

        /// <summary>
        ///     The color of the Gizmo drawn by <see cref="OnDrawGizmos" />.
        /// </summary>
        public Color Color
        {
            get => color;
            set => color = value;
        }

        /// <summary>
        ///     Draws a box in the scene which displays the final spawn size of Block prefabs.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color;
            var trans = transform;
            Gizmos.DrawCube(trans.position, trans.localScale);
        }
    }
}