using System;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Game
{
    /// <summary>
    ///     Concrete implementation of the <see cref="ICanvasSpawner" />. Spawn canvases for each block displaying its score.
    /// </summary>
    /// TODO: Rename to ScoreCanvasSpawnerStrategy
    [Serializable]
    public class ScoreCanvasSpawner : ICanvasSpawner
    {
        /// <summary>
        ///     Whether to spawn or not.
        /// </summary>
        [SerializeField] private bool enabled = true;

        /// <summary>
        ///     The text to display whenever a score of 0 is detected.
        /// </summary>
        [SerializeField] private string onScoreZeroText = "missed!";

        /// <summary>
        ///     A reference to the <see cref="CanvasSpawner" /> GameObject this is used in.
        /// </summary>
        private GameObject _gameObject;

        /// <summary>
        ///     The Canvas prefab to spawn.
        /// </summary>
        private GameObject _prefab;

        /// <summary>
        ///     The offset on the z-axis to spawn the Canvas from.
        /// </summary>
        private float _zOffset;

        /// <summary>
        ///     Offset in 3d space.
        /// </summary>
        private Vector3 Offset => new Vector3(0, 0, _zOffset);

        /// <summary>
        ///     Receive references to the scene.
        /// </summary>
        /// <param name="gameObject">The GameObject the <see cref="CanvasSpawner" /> component is attached to.</param>
        /// <param name="prefab">The ScoreCanvas prefab to spawn.</param>
        /// <param name="zOffset">The offset to spawn in z-direction.</param>
        public void Awake(GameObject gameObject, GameObject prefab, float zOffset)
        {
            _gameObject = gameObject;
            _prefab = prefab;
            _zOffset = zOffset;
        }

        /// <summary>
        ///     Spawns a ScoreCanvas displaying the score stored in the <see cref="scoreGO" />.
        ///     If Score is 0, spawns a "missed!" canvas.
        /// </summary>
        /// <param name="scoreGO">A GameObject with a <see cref="ScoreCalculator" /> component.</param>
        public void SpawnCanvas(GameObject scoreGO)
        {
            if (!enabled) return;
            var score = scoreGO.GetComponentInChildren<ScoreCalculator>();
            var o = Object.Instantiate(_prefab, scoreGO.transform.position - Offset, _prefab.transform.rotation);
            var setter = o.GetComponentInChildren<TextSetter>();
            setter.SetText(score.Score == 0 ? onScoreZeroText : score.Score.ToString());
        }
    }
}