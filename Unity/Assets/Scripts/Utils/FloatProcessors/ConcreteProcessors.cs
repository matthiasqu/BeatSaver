using System;

namespace Utils.FloatProcessors
{
    [Serializable]
    public class FloatAdder : AbstractFloatProcessor
    {
        protected override float ProcessData(float data)
        {
            return Variable + data;
        }
    }

    [Serializable]
    public class FloatSubstractor : AbstractFloatProcessor
    {
        protected override float ProcessData(float data)
        {
            return data - Variable;
        }
    }

    [Serializable]
    public class FloatMultiplier : AbstractFloatProcessor
    {
        protected override float ProcessData(float data)
        {
            return Variable * data;
        }
    }
}