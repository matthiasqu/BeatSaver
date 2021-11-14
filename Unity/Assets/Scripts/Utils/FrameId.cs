using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class FrameId : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private int id;

        public int Id => id;

        // Start is called before the first frame update
        private void Awake()
        {
            id = Time.frameCount;
        }
    }
}