using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Utils.IntProcessors
{
    /// <summary>
    ///     MonoBehaviour to apply a chain of <see cref="AbstractIntProcessor" /> in the scene.
    /// </summary>
    public class IntProcessorBehaviour : SerializedMonoBehaviour
    {
        /// <summary>
        ///     Returns the result of the processing steps-
        /// </summary>
        [SerializeField] private UnityEventInt onProcessed = new UnityEventInt();

        /// <summary>
        ///     The processing steps to apply to any incoming int.
        /// </summary>
        [OdinSerialize] private IntProcessorContainer processors = new IntProcessorContainer();

        /// <summary>
        ///     Processes the incoming int by piping it through all processors present in <see cref="processors" /> and invokes
        ///     <see cref="onProcessed" /> with the result.
        /// </summary>
        /// <param name="i">The value to process.</param>
        public void Process(int i)
        {
            onProcessed.Invoke(processors.Process(i));
        }
    }
}