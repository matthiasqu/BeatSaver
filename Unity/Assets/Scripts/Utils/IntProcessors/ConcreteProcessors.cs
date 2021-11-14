using System;
using UnityEngine;

namespace Utils.IntProcessors
{
    /// <summary>
    ///     Processor providing a multiplicator step in a chain of int value processors.
    /// </summary>
    [Serializable]
    public class IntMultiplicator : AbstractIntProcessor
    {
        protected override int ProcessData(int data)
        {
            return Variable * data;
        }
    }

    /// <summary>
    ///     Processor providing a substractor step in a chain of int value processors.
    /// </summary>
    [Serializable]
    public class IntSubstractor : AbstractIntProcessor
    {
        protected override int ProcessData(int data)
        {
            return data - Variable;
        }
    }

    /// <summary>
    ///     Processor providing a addition step in a chain of int value processors.
    /// </summary>
    [Serializable]
    public class IntAdder : AbstractIntProcessor
    {
        protected override int ProcessData(int data)
        {
            return data + Variable;
        }
    }

    /// <summary>
    ///     Processor providing a division step in a chain of int value processors.
    /// </summary>
    [Serializable]
    public class IntDivider : AbstractIntProcessor
    {
        protected override int ProcessData(int data)
        {
            if (data == 0) Debug.LogError("[IntDivider] Attempting to divide by 0! Aborting.");
            return data / Variable;
        }
    }

    /// <summary>
    ///     Lets through only values greater than defined in <see cref="Variable" />. Otherwise passes variable to the next
    ///     step.
    /// </summary>
    [Serializable]
    public class IntThreshold : AbstractIntProcessor
    {
        protected override int ProcessData(int data)
        {
            return data > Variable ? data : Variable;
        }
    }

    /// <summary>
    ///     Passes 1 if the supplied value is greater than <see cref="Variable" />. Otherwise passes 0 to the next step.
    /// </summary>
    [Serializable]
    public class IntGate : AbstractIntProcessor
    {
        protected override int ProcessData(int data)
        {
            return data >= Variable ? 1 : 0;
        }
    }

    /// <summary>
    ///     Clamps values between <see cref="bottom" /> and <see cref="top" />, i.e. values exceeding <see cref="top" /> are
    ///     set to <see cref="top" />, values less than <see cref="bottom" /> are set to <see cref="bottom" />.
    /// </summary>
    [Serializable]
    public class IntClamp : AbstractIntProcessor
    {
        [SerializeField] private int bottom;
        [SerializeField] private int top;

        protected override int ProcessData(int data)
        {
            if (data > top) return top;
            if (data < bottom) return bottom;
            return data;
        }
    }

    /// <summary>
    ///     Does not affect data but adds an event to get the current state of the processing pipeline.
    /// </summary>
    [Serializable]
    public class IntReader : AbstractIntProcessor
    {
        protected override int ProcessData(int data)
        {
            return data;
        }
    }

    /// <summary>
    ///     Passes on <see cref="whenMet" /> whenever the given condition in <see cref="condition" /> evaluates true.
    ///     Otherwise it passes on the incoming value.
    /// </summary>
    [Serializable]
    public class IntConditional : AbstractIntProcessor
    {
        public int whenMet;
        [SerializeField] private IIntCondition condition;

        protected override int ProcessData(int data)
        {
            return condition.Evaluate(data, Variable) ? whenMet : data;
        }
    }
}