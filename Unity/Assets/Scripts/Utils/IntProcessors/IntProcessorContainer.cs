using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Utils.IntProcessors
{
    /// <summary>
    ///     Container class which capsulates the functionality for piping an int through multiple steps of processing.
    /// </summary>
    [Serializable]
    [InlineProperty]
    [HideLabel]
    public class IntProcessorContainer
    {
        /// <summary>
        ///     List of processors through which a supplied int is piped.
        /// </summary>
        [OdinSerialize] [InlineProperty]
        private List<AbstractIntProcessor> processors = new List<AbstractIntProcessor>();

        /// <summary>
        ///     Processes an int by passing it to the first processor in <see cref="processors" /> and then passing the results of
        ///     all following processors subsequently to the next.
        /// </summary>
        /// <param name="i">The int value to process.</param>
        /// <returns>The result of all modifications made by the chain of processors.</returns>
        public int Process(int i)
        {
            if (processors == null) return i;
            if (processors.Count == 0) return i;

            return processors.Aggregate(i, (current, t) => t.Process(current));
        }
    }
}