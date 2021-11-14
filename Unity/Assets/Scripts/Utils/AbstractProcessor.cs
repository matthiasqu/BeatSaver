using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    /// <summary>
    ///     Base class which provides functionality needed for linking multiple processing steps.
    ///     It was decided against the use of the strategy pattern here since unity can not serialize typed members.
    /// </summary>
    /// <typeparam name="T">The data type which will be processed by linked Processors.</typeparam>
    [Serializable]
    [InlineProperty]
    [HideLabel]
    public abstract class AbstractProcessor<T>
    {
        [SerializeField] private UnityEvent<T> onProcessed = new UnityEvent<T>();
        private T _data;

        /// <summary>
        ///     Processes the provided <see cref="data" /> and invokes the <see cref="onProcessed" /> event.
        /// </summary>
        /// <param name="data">The data to process.</param>
        /// <returns>The processed data also provided in the <see cref="onProcessed" /> event.</returns>
        public T Process(T data)
        {
            _data = data;
            var processedData = ProcessData(data);
            onProcessed.Invoke(processedData);
            return processedData;
        }

        /// <summary>
        ///     Concrete data processing steps.
        /// </summary>
        /// <param name="data">The data to process.</param>
        /// <returns>The processed data.</returns>
        protected abstract T ProcessData(T data);
    }
}