using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Utils.FloatProcessors;

namespace Utils
{
    [CreateAssetMenu(menuName = "_custom/Float Variable", fileName = "FloatVariable", order = 0)]
    public class FloatVariable : SerializedScriptableObject
    {
        /// <summary>
        ///     The object's value.
        /// </summary>
        [SerializeField] private float value;

        /// <summary>
        ///     An event holding the new <see cref="value" /> of the object, fired whenever it is changed.
        /// </summary>
        public UnityEventFloat onValueChanged = new UnityEventFloat();

        /// <summary>
        ///     Concrete floatProcessors which may alter any given float before setting <see cref="value" />.
        /// </summary>
        [OdinSerialize] private FloatProcessorContainer processors = new FloatProcessorContainer();

        /// <summary>
        ///     Public access to the objects <see cref="value" />.
        /// </summary>
        public float Value
        {
            // returns the value.
            get => value;
            // applies all processing steps to the incoming value and sets the object's value to it's result.
            private set
            {
                if (Math.Abs(value - this.value) < float.Epsilon) return;
                this.value = value;
                onValueChanged.Invoke(this.value);
            }
        }

        /// <summary>
        ///     Resets the objects float value to the value given in <see cref="f" />. This bypasses all processing that may
        ///     otherwise happen.
        /// </summary>
        /// <param name="f">The value to reset to, bypassing all processing steps.</param>
        public void Reset(float f)
        {
            value = f;
        }

        /// <summary>
        ///     Adds a specific number to add to <see cref="Value" />. Note that any value passed may be altered by any processing
        ///     steps defined in <see cref="processors" />.
        /// </summary>
        /// <param name="value">The value to add.</param>
        public void Add(float value)
        {
            Value += processors.Process(value);
        }

        /// <summary>
        ///     Adds a specific number to substract from<see cref="Value" />. Note that any value passed may be altered by any
        ///     processing steps defined in <see cref="processors" />.
        /// </summary>
        /// <param name="value">The value to add.</param>
        public void Substract(float value)
        {
            Value -= processors.Process(value);
            ;
        }
    }
}