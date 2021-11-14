using UnityEngine;

namespace BeatMapper.Utils
{
    public class MaterialSetter : MonoBehaviour
    {
        [SerializeField] private Material blueMaterial;
        [SerializeField] private Material redMaterial;

        public void SetMaterial(CubeColor color)
        {
            //Debug.Log($"[MaterialSetter] Setting material color to {color} on {gameObject.name}");
            GetComponent<MeshRenderer>().material = color == CubeColor.Red ? redMaterial : blueMaterial;
        }
    }
}