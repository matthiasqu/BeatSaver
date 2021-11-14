using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Utils.FloatProcessors
{
    /// <summary>
    ///     Container class which capsulates the functionality for piping a float through multiple steps of processing.
    /// </summary>
    [Serializable]
    [InlineProperty]
    [HideLabel]
    public class FloatProcessorContainer
    {
        /// <summary>
        ///     List of processors through which a supplied float is piped.
        /// </summary>
        [OdinSerialize] [InlineProperty]
        private List<AbstractFloatProcessor> processors = new List<AbstractFloatProcessor>();

        /// <summary>
        ///     Processes an float by passing it to the first processor in <see cref="processors" /> and then passing the results
        ///     of
        ///     all following processors subsequently to the next.
        /// </summary>
        /// <param name="f">The float value to process.</param>
        /// <returns>The result of all modifications made by the chain of processors.</returns>
        public float Process(float f)
        {
            if (processors == null) return f;
            if (processors.Count == 0) return f;

            return processors.Aggregate(f, (current, t) => t.Process(current));
        }
    }
}