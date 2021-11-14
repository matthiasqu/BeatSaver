using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils.FloatProcessors
{
    /// <summary>
    ///     Base class for all processors that deal with int variables.
    /// </summary>
    [Serializable]
    public abstract class AbstractFloatProcessor : AbstractProcessor<float>
    {
        /// <summary>
        ///     Whether or not to use a <see cref="SuperchargedIntVariable" /> scriptable object for determining the value of the
        ///     processing
        ///     step.
        /// </summary>
        [SerializeField] private bool useFloatVariable;

        /// <summary>
        ///     The value upon which the processing step relies when <see cref="useIntVariable" /> is false.
        /// </summary>
        [SerializeField] [ShowIf("@!useFloatVariable")]
        private float value;

        /// <summary>
        ///     The value upon which the processing step relies when <see cref="useIntVariable" /> is true.
        /// </summary>
        [SerializeField] [ShowIf("@useFloatVariable")]
        private FloatVariable valueVariable;

        /// <summary>
        ///     The actual value that is being used by concrete implementations of this abstract superclass.
        /// </summary>
        protected float Variable => useFloatVariable ? valueVariable.Value : value;

        /// <summary>
        ///     Processing of data is forwarded to concrete implementations.
        /// </summary>
        /// <param name="data">The int value to process.</param>
        /// <returns>The processed int value.</returns>
        protected abstract override float ProcessData(float data);
    }
}