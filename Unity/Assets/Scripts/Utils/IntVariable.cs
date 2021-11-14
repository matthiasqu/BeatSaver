using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     A scriptable object storing a single integer value.
    /// </summary>
    [CreateAssetMenu(fileName = "IntVariable", menuName = "_custom/Int Variable", order = 0)]
    public class IntVariable : SerializedScriptableObject
    {
        [TitleGroup("Actual Value", GroupName = "Actual")] [SerializeField] [PropertyOrder(0)]
        private int value;

        /// <summary>
        ///     An event holding the new <see cref="value" /> of the object, fired whenever it is changed.
        /// </summary>
        [InfoBox("Called after all processing has been applied, yields the final value stored in this IntVariable")]
        [TabGroup("Actual Value", GroupName = "Actual")]
        [PropertyOrder(0)]
        public UnityEventInt onValueChanged = new UnityEventInt();


        /// <summary>
        ///     An event holding the difference between the old <see cref="value" /> and the new one. Fired whenever
        ///     <see cref="value" /> changes..
        /// </summary>
        [InfoBox("Called after all processing has been applied, yields the final value stored in this IntVariable.")]
        [TabGroup("Post Processing", GroupName = "Postprocessing")]
        [PropertyOrder(20)]
        public UnityEventInt onChangeDifference = new UnityEventInt();

        public int Value
        {
            // returns the value.
            get => value;
            // applies all processing steps to the incoming value and sets the object's value to it's result.
            protected set
            {
                if (value == this.value) return;
                var oldValue = Value;
                this.value = value;
                //Debug.Log($"[{name}] Set to {value}");
                onValueChanged.Invoke(this.value);
                onChangeDifference.Invoke(value - oldValue);
            }
        }

        /// <summary>
        ///     Sets <see cref="value" /> to the supplied value, bypassing only the preprocessing steps. Note that any value passed
        ///     may be altered by any processing
        ///     steps defined in <see cref="preProcessors" /> and the result may be altered by any <see cref="postProcessors" />
        ///     before invocation of <see cref="onValueChanged" />.
        /// </summary>
        /// <param name="i"></param>
        public void Set(int value)
        {
            Value = value;
        }
    }
}