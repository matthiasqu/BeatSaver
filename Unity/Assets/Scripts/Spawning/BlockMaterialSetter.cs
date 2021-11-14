using BeatMapper;
using UnityEngine;

namespace Spawning
{
    /// <summary>
    ///     Sets the materials on a Block prefab according to the <see cref="CubeColor" /> supplied.
    /// </summary>
    public class BlockMaterialSetter : MonoBehaviour
    {
        /// <summary>
        ///     A material for use when a <see cref="CubeColor.Red" /> is supplied.
        /// </summary>
        [SerializeField] private Material redMaterial;

        /// <summary>
        ///     A material for use when a <see cref="CubeColor.Blue" /> is supplied.
        /// </summary>
        [SerializeField] private Material blueMaterial;

        /// <summary>
        ///     The mesh renderer to assign the materials to.
        /// </summary>
        [SerializeField] private new MeshRenderer renderer;

        /// <summary>
        ///     The materials on the <see cref="renderer" />.
        /// </summary>
        private Material[] _meshMaterials;

        /// <summary>
        ///     Try to get the renderer from the GameObject if its not set explicitly in the editor.
        /// </summary>
        private void Awake()
        {
            renderer ??= GetComponent<MeshRenderer>();
        }

        /// <summary>
        ///     Applies the desired material to the first two material slots (body and edges).
        /// </summary>
        /// <param name="color">The color defining which material to use</param>
        public void ApplyMaterial(CubeColor color)
        {
            _meshMaterials ??= renderer.materials;
            var c = color == CubeColor.Red
                ? redMaterial
                : blueMaterial;

            // When applying multiple materials, the whole array of materials has to be replaced.
            _meshMaterials[0] = _meshMaterials[1] = c;
            renderer.materials = _meshMaterials;
        }
    }
}