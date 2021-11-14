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
    ///     Concrete implementation of the <see cref="ISpawner" /> interface. This spawns only Blocks based on
    ///     <see cref="NoteEvent" /> objects in the received <see cref="SongMapping" /> using the <see cref="Spawn" /> method.
    /// </summary>
    /// TODO: rename to BlockSpawnerStrategy
    [Serializable]
    [HideLabel]
    public class NoteSpawner : ISpawner
    {
        /// <summary>
        ///     The prefab to use for spawning.
        /// </summary>
        [SerializeField] private Block arrowedPrefab;

        [SerializeField] private Block omniPrefab;

        [SerializeField] [ReadOnly] private int spawnedNotes;

        /// <summary>
        ///     invoked whenever the <see cref="NoteEvent" /> objects in a <see cref="SongMapping" /> have been spawned.
        /// </summary>
        [SerializeField] private UnityEventSongMapping onMappingSpawned = new UnityEventSongMapping();

        [SerializeField] private UnityEvent<Block[]> onBlocksSpawned = new UnityEvent<Block[]>();

        private GameObject gameObject;


        public void Awake(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        /// <summary>
        ///     Spawns all notes at the corresponding spawn positions and applies the <see cref="BlockSettings" />. Invokes the
        ///     <see cref="onMappingSpawned" /> event.
        /// </summary>
        /// <param name="songMapping">SongMapping from which to extract note events</param>
        /// <param name="spawnPositions">A scene object of type <see cref="SpawnPositionsContainer" /></param>
        public void Spawn(SongMapping songMapping, SpawnPositionsContainer spawnPositions)
        {
            var spawnedNotes = new List<Block>();
            foreach (var note in songMapping.Notes)
            {
                // select Prefab
                var prefab = arrowedPrefab;
                if (note._cutDirection == CutDirection.Any) prefab = omniPrefab;

                // Instantiate
                var spawnPosition = spawnPositions[new NotePosition(note._lineIndex, note._lineLayer)];
                var transform = spawnPosition.transform;
                var cube = Object.Instantiate(prefab, transform.position, arrowedPrefab.transform.rotation,
                    transform);
                spawnedNotes.Add(cube);

                // Assign note properties to prefab
                var settings = cube.GetComponentInChildren<BlockSettings>();
                settings.NoteEvent = note;

                // Sets the object back by half its scale.
                cube.transform.position -= new Vector3(0, 0, cube.transform.localScale.z / 2);

                this.spawnedNotes++;
            }

            onBlocksSpawned.Invoke(spawnedNotes.ToArray());
            onMappingSpawned.Invoke(songMapping);
        }
    }
}