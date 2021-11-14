using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Utils.FloatProcessors
{
    public class FloatProcessorBehaviour : SerializedMonoBehaviour
    {
        [SerializeField] private UnityEventFloat onProcessed = new UnityEventFloat();
        [OdinSerialize] private FloatProcessorContainer processors = new FloatProcessorContainer();

        public void Process(float i)
        {
            onProcessed.Invoke(processors.Process(i));
        }
    }
}