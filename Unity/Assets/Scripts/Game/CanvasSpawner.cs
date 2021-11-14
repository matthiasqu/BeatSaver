using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    /// <summary>
    ///     Wrapper for a variety of <see cref="ICanvasSpawner" /> implementations.
    /// </summary>
    public class CanvasSpawner : SerializedMonoBehaviour
    {
        /// <summary>
        ///     The Canvas prefab to use
        /// </summary>
        /// TODO: mark ScoreCanvas prefab with ScoreCanvas.cs component.
        [SerializeField] private GameObject prefab;

        /// <summary>
        ///     The offset on the z-axis to apply upon spawning the canvas.
        /// </summary>
        [SerializeField] private float zOffset = 1;

        /// <summary>
        ///     The spawn strategy to use on this object.
        /// </summary>
        /// TODO: rename to spawner
        [SerializeField] private ICanvasSpawner _spawner;

        /// <summary>
        ///     Supplies the <see cref="_spawner" />  with a reference to the current GameObject, the prefab, and the z-offset
        /// </summary>
        private void Awake()
        {
            _spawner.Awake(gameObject, prefab, zOffset);
        }

        /// <summary>
        ///     Delegates spawning to the <see cref="_spawner" /> and passes a reference to the GameObject containing a
        ///     <see cref="ScoreCalculator" />
        /// </summary>
        /// <param name="scoreGo">A GameObject with an attached <see cref="ScoreCalculator" /> component.</param>
        public void SpawnCanvas(GameObject scoreGo)
        {
            _spawner.SpawnCanvas(scoreGo);
        }
    }
}