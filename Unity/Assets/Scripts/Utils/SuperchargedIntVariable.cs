using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Utils.IntProcessors;

namespace Utils
{
    [CreateAssetMenu(menuName = "_custom/Supercharged Int Variable", fileName = "SuperIntVariable", order = 0)]
    public class SuperchargedIntVariable : IntVariable
    {
        /// <summary>
        ///     Public access to the objects <see cref="value" />.
        /// </summary>
        /// <summary>
        ///     Resets the objects int value to the value given in <see cref="i" />. This bypasses all processing that may
        ///     otherwise happen.
        /// </summary>
        /// <param name="i">The value to reset to, bypassing all processing steps.</param>
        public void Reset(int i)
        {
            onRawDataReceived.Invoke(i);
            Value = i;
        }

        /// <summary>
        ///     Sets <see cref="value" /> to the supplied value, bypassing only the preprocessing steps. Note that any value passed
        ///     may be altered by any processing
        ///     steps defined in <see cref="preProcessors" /> and the result may be altered by any <see cref="postProcessors" />
        ///     before invocation of <see cref="IntVariable.onValueChanged" />.
        /// </summary>
        /// <param name="i"></param>
        public new void Set(int value)
        {
            onRawDataReceived.Invoke(value);

            var preProcessed = preProcessors.Process(value);
            onBeforePostProcessing.Invoke(preProcessed);

            Value = postProcessors.Process(preProcessed);
        }

        /// <summary>
        ///     Adds a specific number to add to <see cref="IntVariable.Value" />. Note that any value passed may be altered by any
        ///     processing
        ///     steps defined in <see cref="preProcessors" /> and the result may be altered by any <see cref="postProcessors" />
        ///     before invocation of <see cref="IntVariable.onValueChanged" />.
        /// </summary>
        /// <param name="value">The value to add.</param>
        public void Add(int value)
        {
            onRawDataReceived.Invoke(value);

            var preProcessed = preProcessors.Process(value);
            var appliedValue = Value + preProcessed;
            onBeforePostProcessing.Invoke(appliedValue);

            Value = postProcessors.Process(appliedValue);
        }

        /// <summary>
        ///     Adds a specific number to substract from<see cref="IntVariable.Value" />. Note that any value passed may be altered
        ///     by any
        ///     processing steps defined in <see cref="preProcessors" /> and the result may be altered by any
        ///     <see cref="postProcessors" />
        ///     before invocation of <see cref="IntVariable.onValueChanged" />.
        /// </summary>
        /// <param name="value">The value to add.</param>
        public void Substract(int value)
        {
            onRawDataReceived.Invoke(value);

            var preProcessed = preProcessors.Process(value);
            var appliedValue = Value - preProcessed;
            onBeforePostProcessing.Invoke(appliedValue);

            Value = postProcessors.Process(appliedValue);
        }

        /// <summary>
        ///     Multiplies the current <see cref="IntVariable.Value" /> by the provided number. Note that any value passed may be
        ///     altered by
        ///     any
        ///     processing steps defined in <see cref="preProcessors" /> and the result may be altered by any
        ///     <see cref="postProcessors" />
        ///     before invocation of <see cref="IntVariable.onValueChanged" />.
        /// </summary>
        /// <param name="value">The value to multiply by.</param>
        public void Multiply(int value)
        {
            onRawDataReceived.Invoke(value);

            var preProcessed = preProcessors.Process(value);
            var appliedValue = Value * preProcessed;
            onBeforePostProcessing.Invoke(appliedValue);

            Value = postProcessors.Process(appliedValue);
        }

        /// <summary>
        ///     divides the current <see cref="IntVariable.Value" /> by the provided number. Note that any value passed may be
        ///     altered by any
        ///     processing steps defined in <see cref="preProcessors" /> and the result may be altered by any
        ///     <see cref="postProcessors" />
        ///     before invocation of <see cref="IntVariable.onValueChanged" />.
        /// </summary>
        /// <param name="value">The value to divide by.</param>
        public void Divide(int value)
        {
            onRawDataReceived.Invoke(value);

            var preProcessed = preProcessors.Process(value);
            var appliedValue = Value / preProcessed;
            onBeforePostProcessing.Invoke(appliedValue);

            Value = postProcessors.Process(appliedValue);
        }

        #region events

        /// <summary>
        ///     Event fired whenever raw data is passed to the object, before any processing has occured.
        /// </summary>
        [InfoBox("Called before anything happens, yields the raw value passed to this IntVariable.")]
        [TabGroup("Pre Processing")]
        [PropertyOrder(10)]
        public UnityEventInt onRawDataReceived = new UnityEventInt();

        /// <summary>
        ///     Event fired whenever a value has passed preprocessing steps and the called function has been applied (e.g. the
        ///     pre-processed value has been added to <see cref="Value" />, before post-processing.
        /// </summary>
        [InfoBox("Called after pre-processing and methods have been applied, but before post-processing.")]
        [TabGroup("Actual Value", GroupName = "Actual")]
        public UnityEventInt onBeforePostProcessing = new UnityEventInt();

        /// <summary>
        ///     Event fired whenever <see cref="Reset" /> has been called.
        /// </summary>
        [InfoBox("Called whenever the Reset() method is called.")] [TabGroup("Actual Value", GroupName = "Actual")]
        public UnityEventInt onReset = new UnityEventInt();

        #endregion


        #region processors

        /// <summary>
        ///     Concrete IntProcessors which may alter any given integer before using it in any methods.<see cref="value" />.
        /// </summary>
        [InfoBox(
            "Processing steps applied before anything is done to the variable (like calling the Add() method of the intVariable).")]
        [TabGroup("Pre Processing", GroupName = "Preprocessing")]
        [OdinSerialize]
        private IntProcessorContainer preProcessors = new IntProcessorContainer();

        /// <summary>
        ///     Concrete IntProcessors which may alter the result of any
        /// </summary>
        [InfoBox(
            "Processing steps applied after all pre-processing steps have finished and any called methods have been applied (e.g. adding)")]
        [TabGroup("Post Processing", GroupName = "Postprocessing")]
        [OdinSerialize]
        private IntProcessorContainer postProcessors = new IntProcessorContainer();

        #endregion

        #region debug

        /// <summary>
        ///     Value for the below functions.
        /// </summary>
        [TabGroup("Debug", Order = 999)] [PropertyOrder(999)] [SerializeField]
        private int debugValue;

        /// <summary>
        ///     calls the <see cref="Add" /> method, hence invokes processing <see cref="debugValue" />.
        /// </summary>
        [TabGroup("Debug", Order = 999)]
        [Button]
        private void Increment()
        {
            Add(debugValue);
        }

        /// <summary>
        ///     Calls the <see cref="Substract" /> method, hence invokes processing <see cref="debugValue" />.
        /// </summary>
        [TabGroup("Debug", Order = 999)]
        [Button]
        private void Decrement()
        {
            Substract(debugValue);
        }

        #endregion
    }
}