using System;
using System.Collections.Generic;
using BeatMapper;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using Object = UnityEngine.Object;

namespace Spawning
{
    /// <summary>
    ///     /// Concrete implementation of the <see cref="ISpawner" /> interface. This spawns only Obstacles based on
    ///     <see cref="ObstacleEvent" /> objects in the received <see cref="SongMapping" /> using the <see cref="Spawn" />
    ///     method.
    /// </summary>
    /// TODO: rename to ObstacleSpawnerStrategy
    [Serializable]
    public class ObstacleSpawner : ISpawner
    {
        /// <summary>
        ///     The prefab to spawn.
        /// </summary>
        [TabGroup("Spawner")] [SerializeField] private Obstacle prefab;

        [SerializeField] private UnityEventSongMapping onMappingSpawned = new UnityEventSongMapping();
        [SerializeField] private UnityEvent<Obstacle[]> onObstaclesSpawned = new UnityEvent<Obstacle[]>();

        private GameObject _gameObject;

        public void Awake(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        /// <summary>
        ///     Spawns obstacles inside the supplied <see cref="SongMapping" />.
        /// </summary>
        /// <param name="songMapping">The SongMapping which contains the obstacles to be spawned.</param>
        /// <param name="spawnPositions">A <see cref="SpawnPositionsContainer" /> object holding the spawn positions.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Spawn(SongMapping songMapping, SpawnPositionsContainer spawnPositions)
        {
            // Warn iof the prefab has a scale not equal to (1, 1, 1)
            if (prefab.transform.localScale != Vector3.one)
                Debug.LogWarning(
                    $"[{GetType()}] Your prefab should have a scale of {Vector3.one}, but has {prefab.transform.localScale}");

            var obstacles = songMapping.Obstacles;
            if (obstacles.Length == 0) return;

            foreach (var obstacle in obstacles)
            {
                // Obstacles of zero length are ignored.
                if (obstacle._width == 0)
                {
                    Debug.LogWarning(
                        "[ObstacleSpawner] You have a 0-length obstacle in your data - it is skipped, but something is wrong.");
                    continue;
                }

                var (scale, center) = GetTransformProperties(obstacle, spawnPositions);
                var o = Object.Instantiate(prefab, center, prefab.transform.rotation, spawnPositions.transform);
                o.GetComponent<ObstacleSettings>().ObstacleEvent = obstacle;
                //TODO: Duration is not yet converted to realtime.
                o.transform.localScale = scale;
                spawnedObjects.Add(o.gameObject);
            }
        }

        /// <summary>
        ///     Returns the scale and spawn position of a supplied obstacle
        /// </summary>
        /// <param name="obstacle">The obstacle to process.</param>
        /// <param name="spawnPositions">The <see cref="SpawnPositionsContainer" /> used for calculations.</param>
        /// <returns>Scale and positions vectors.</returns>
        private (Vector3 scale, Vector3 position) GetTransformProperties(ObstacleEvent obstacle,
            SpawnPositionsContainer spawnPositions)
        {
            // Get the layer to which the obstacle expands
            var bottomLayer = obstacle._type == ObstacleType.Full ? LineLayer.Bottom : LineLayer.Middle;
            // Get the rightmost lineIndex to which the obstacle expands
            var rightLineIndex = GetRightLineIndex(obstacle);

            // Get the transforms at the top left and bottom right boundaries of the obstacle
            var (upperLeftTrans, bottomRightTrans) =
                GetBoundaryTransforms(spawnPositions, new NotePosition(obstacle._lineIndex, LineLayer.Top),
                    new NotePosition(rightLineIndex, bottomLayer));

            // get the local scale of the SpawnPositions transform
            var localScale = upperLeftTrans.localScale;
            // expand the size of the obstacle by half the size of the SpawnPositions local scale to make it expand like the notes being spawned
            var bottomPosition = bottomRightTrans.position -
                                 new Vector3(localScale.x / 2, localScale.y / 2, -localScale.z / 2);
            var topPosition = upperLeftTrans.position +
                              new Vector3(localScale.x / 2, localScale.y / 2, localScale.z / 2);

            // Get the center between both positions
            var centerPos = GetCenter(topPosition, bottomPosition);

            // Move the position to the back so the obstacle begins on beat
            centerPos.z = topPosition.z - obstacle._duration / 2;

            // Get the scale of the obstacle
            var scale = GetObstacleScale(topPosition, bottomPosition, obstacle._duration);

            return (scale, position: centerPos);
        }

        /// <summary>
        ///     Returns the rightmost line index of the obstacle.
        /// </summary>
        /// <param name="obstacle">The obstacle of which to determine the line it expands to.</param>
        /// <returns>The line index to which the obstacle expands to.</returns>
        private static LineIndex GetRightLineIndex(ObstacleEvent obstacle)
        {
            var width = obstacle._width;
            // return the same line index if width is 1
            var rightLineInt = width - 1 + (int) obstacle._lineIndex;
            if (!Enum.IsDefined(typeof(LineIndex), rightLineInt))
                rightLineInt = (int) LineIndex.Fourth;

            return (LineIndex) rightLineInt;
        }

        /// <summary>
        ///     Returns the transforms of two <see cref="NotePosition" /> objects.
        /// </summary>
        /// <param name="positions">The <see cref="SpawnPositionsContainer" /> in which to look up the positions.</param>
        /// <param name="upper">First <see cref="NotePosition" /> to look up.</param>
        /// <param name="lower">Second <see cref="NotePosition" /> to lookup.</param>
        /// <returns>The Transforms of both supplied note positions.</returns>
        private static (Transform, Transform) GetBoundaryTransforms(SpawnPositionsContainer positions,
            NotePosition upper,
            NotePosition lower)
        {
            return (positions[upper].transform, positions[lower].transform);
        }

        /// <summary>
        ///     Gets the center between two Vector3s
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Seconds vector</param>
        /// <returns>The center position as Vector3.</returns>
        private static Vector3 GetCenter(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z) / 2;
        }

        /// <summary>
        ///     Returns a scale corresponding to the distances between two Vector3s
        /// </summary>
        /// <param name="topBounds">First Vector3.</param>
        /// <param name="bottomBounds">Second Vector3.</param>
        /// <param name="duration">The obstacle's duration.</param>
        /// <returns></returns>
        private static Vector3 GetObstacleScale(Vector3 topBounds, Vector3 bottomBounds, float duration)
        {
            return new Vector3(topBounds.x - bottomBounds.x,
                topBounds.y - bottomBounds.y, duration);
        }

        #region Debug

        [SerializeField] [TabGroup("Debug")] private SpawnPositionsContainer spawnPositionsContainer;
        [SerializeField] [TabGroup("Debug")] private byte width;
        [SerializeField] [TabGroup("Debug")] private LineIndex lineIndex;
        [SerializeField] [TabGroup("Debug")] private ObstacleType type;
        [SerializeField] [TabGroup("Debug")] private float length;
        [SerializeField] [TabGroup("Debug")] private List<GameObject> spawnedObjects = new List<GameObject>();

        [Button]
        [TabGroup("Debug")]
        private void SpawnObstacle()
        {
            if (spawnPositionsContainer == null)
            {
                Debug.LogWarning("[ObstacleSpawner] No SpawnPositionContainer is not assigned, looking in scene.");
                spawnPositionsContainer = Object.FindObjectOfType<SpawnPositionsContainer>();
                if (spawnPositionsContainer == null)
                    Debug.LogError(
                        "[ObstacleSpawner] No suitable object found - is a SpawnPositionContainer in the scene? ");
            }

            var songMapping = new SongMapping
            {
                _obstacles = new[]
                {
                    new ObstacleEvent
                    {
                        _duration = length,
                        _time = 0f,
                        _type = type,
                        _width = width,
                        _lineIndex = lineIndex
                    }
                }
            };

            Spawn(songMapping, spawnPositionsContainer);
        }

        [Button]
        [TabGroup("Debug")]
        private void DeleteSpawnedObjects()
        {
            spawnedObjects.ForEach(Object.DestroyImmediate);
        }

        #endregion Debug
    }
}