using BeatMapper;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Spawning;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    /// <summary>
    ///     Class marking objects as sliceable and applying slice behaviour to it.
    /// </summary>
    public class Sliceable : MonoBehaviour
    {
        /// <summary>
        ///     MeshRenderers to deactivate upon invocation.
        /// </summary>
        [SerializeField] private MeshRenderer meshRenderer;

        /// <summary>
        ///     Colliders to deactivate upon invocation.
        /// </summary>
        [SerializeField] private BoxCollider[] colliders;

        /// <summary>
        ///     A prefab to spawn which resembles the slicing animation.
        /// </summary>
        [SerializeField] private GameObject slicedCubePrefab;

        public UnityEvent onSliced = new UnityEvent();

        /// <summary>
        ///     Invokes the slicing behaviour, deactivating the <see cref="meshRenderer" /> and colliders in
        ///     <see cref="colliders" />.
        ///     Spawns a new <see cref="slicedCubePrefab" /> ath the transforms current position.
        /// </summary>
        [Button]
        public void Slice()
        {
            meshRenderer.enabled = false;
            colliders?.ForEach(c => c.enabled = false);

            // create new instance
            var t = transform;
            var spawnPos = gameObject.GetComponentInParent<SpawnPosition>(true);
            var sliced = Instantiate(slicedCubePrefab, t.position, Quaternion.identity,
                spawnPos.transform);

            var noteEvent = GetComponent<BlockSettings>().NoteEvent.Clone() as NoteEvent;
            // apply directions
            noteEvent._cutDirection = GetComponent<CutDirectionConverter>().DetectedCutDirection;
            sliced.GetComponent<BlockSettings>().NoteEvent = noteEvent;
            sliced.GetComponent<RandomForceCut>().Force = GetComponent<VelocityBuffer>().MaxVelocity;

            onSliced.Invoke();
        }
    }
}