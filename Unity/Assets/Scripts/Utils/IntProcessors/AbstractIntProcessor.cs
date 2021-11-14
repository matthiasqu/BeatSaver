using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils.IntProcessors
{
    /// <summary>
    ///     Base class for all processors that deal with int variables.
    /// </summary>
    [Serializable]
    public abstract class AbstractIntProcessor : AbstractProcessor<int>
    {
        /// <summary>
        ///     Whether or not to use a <see cref="SuperchargedIntVariable" /> scriptable object for determining the value of the
        ///     processing
        ///     step.
        /// </summary>
        [SerializeField] private bool useIntVariable;

        /// <summary>
        ///     The value upon which the processing step relies when <see cref="useIntVariable" /> is false.
        /// </summary>
        [SerializeField] [ShowIf("@!useIntVariable")]
        private int value;

        /// <summary>
        ///     The value upon which the processing step relies when <see cref="useIntVariable" /> is true.
        /// </summary>
        [SerializeField] [ShowIf("@useIntVariable")]
        private IntVariable valueVariable;

        /// <summary>
        ///     The actual value that is being used by concrete implementations of this abstract superclass.
        /// </summary>
        protected int Variable => useIntVariable ? valueVariable.Value : value;

        /// <summary>
        ///     Processing of data is forwarded to concrete implementations.
        /// </summary>
        /// <param name="data">The int value to process.</param>
        /// <returns>The processed int value.</returns>
        protected abstract override int ProcessData(int data);
    }
}