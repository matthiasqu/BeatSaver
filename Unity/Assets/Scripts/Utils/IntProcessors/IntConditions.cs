using System;
using UnityEngine;

namespace Utils.IntProcessors
{
    public interface IIntCondition
    {
        public bool Evaluate(int a, int b);
    }

    /// <summary>
    ///     Evaluates <see cref="condition" /> and calls <see cref="onConditionMet" /> whenever it  evaluates to true.
    ///     It always returns false, so that <see cref="IntConditional" /> passes on the original data unchanged.
    /// </summary>
    [Serializable]
    public class ConditionalReader : IIntCondition
    {
        [SerializeField] private UnityEventInt onConditionMet = new UnityEventInt();
        [SerializeField] private IIntCondition condition;

        /// <summary>
        ///     Does not evaluate anything but returns the incoming value as it is in the <see cref="onConditionMet" /> event.
        /// </summary>
        /// <param name="a">Incoming value.</param>
        /// <param name="b">any value (not used)</param>
        /// <returns>False, always.</returns>
        public bool Evaluate(int a, int b)
        {
            if (condition.Evaluate(a, b))
                //Debug.Log($"[Conditional Reader] {a} and {b} = {condition.Evaluate(a, b)}");
                onConditionMet.Invoke(a);

            return false;
        }
    }

    /// <summary>
    ///     Evaluates whether a is greater than b.
    /// </summary>
    [Serializable]
    public class GreaterThanCondition : IIntCondition
    {
        /// <summary>
        ///     Returns whether <see cref="a" /> is greater than <see cref="b" />
        /// </summary>
        /// <param name="a">Incoming value</param>
        /// <param name="b">The value to compare the incoming value to</param>
        /// <returns>True if a > b</returns>
        public bool Evaluate(int a, int b)
        {
            return a > b;
        }
    }

    /// <summary>
    ///     Evaluates whether a is smaller than b.
    /// </summary>
    [Serializable]
    public class SmallerThanCondition : IIntCondition
    {
        /// <summary>
        ///     Returns whether <see cref="a" /> is smaller than <see cref="b" />
        /// </summary>
        /// <param name="a">Incoming value</param>
        /// <param name="b">The value to compare the incoming value to</param>
        /// <returns>True if a < b</returns>
        public bool Evaluate(int a, int b)
        {
            return a < b;
        }
    }

    /// <summary>
    ///     Evaluates whether a is equal to b.
    /// </summary>
    [Serializable]
    public class EqualCondition : IIntCondition
    {
        /// <summary>
        ///     Returns whether <see cref="a" /> is equal to <see cref="b" />
        /// </summary>
        /// <param name="a">Incoming value</param>
        /// <param name="b">Value to compare to</param>
        /// <returns>True if a == b</returns>
        public bool Evaluate(int a, int b)
        {
            return a == b;
        }
    }

    /// <summary>
    ///     Evaluates whether a is equal or greater than b.
    /// </summary>
    [Serializable]
    public class GreaterOrEqualConditon : IIntCondition
    {
        /// <summary>
        ///     Returns whether <see cref="a" /> is equal to or greater than <see cref="b" />
        /// </summary>
        /// <param name="a">Incoming value</param>
        /// <param name="b">Value to compare to</param>
        /// <returns>True if a >= b</returns>
        public bool Evaluate(int a, int b)
        {
            return a >= b;
        }
    }

    /// <summary>
    ///     Evaluates whether a is equal or smaller than b.
    /// </summary>
    [Serializable]
    public class SmallerOrEqualCondition : IIntCondition
    {
        /// <summary>
        ///     Returns whether <see cref="a" /> is equal to or smaller than <see cref="b" />
        /// </summary>
        /// <param name="a">Incoming value</param>
        /// <param name="b">Value to compare to</param>
        /// <returns>
        ///     True if a <= b</returns>
        public bool Evaluate(int a, int b)
        {
            return a <= b;
        }
    }
}