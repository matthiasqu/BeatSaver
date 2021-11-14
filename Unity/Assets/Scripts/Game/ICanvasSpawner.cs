using UnityEngine;

namespace Game
{
    /// <summary>
    ///     Interface defining Method used for implementations of canvas spawning strategies. To be used in
    ///     <see cref="CanvasSpawner" />.
    /// </summary>
    public interface ICanvasSpawner
    {
        /// <summary>
        ///     Pass a reference to the <see cref="CanvasSpawner" /> GameObject, the prefab to use and the offset in z-direction.
        /// </summary>
        /// <param name="gameObject">The <see cref="CanvasSpawner" /> object.</param>
        /// <param name="prefab">The Canvas prefab.</param>
        /// <param name="offset">The offset in z-direction where the canvas will be spawned.</param>
        void Awake(GameObject gameObject, GameObject prefab, float offset);

        /// <summary>
        ///     Invokes instantiation.
        /// </summary>
        /// <param name="scoreGo"></param>
        void SpawnCanvas(GameObject scoreGo);
    }
}