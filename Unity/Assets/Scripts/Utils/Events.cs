using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    [Serializable]
    public class UnityEventInt : UnityEvent<int>
    {
    }

    [Serializable]
    public class UnityEventFloat : UnityEvent<float>
    {
    }

    [Serializable]
    public class UnityEventString : UnityEvent<string>
    {
    }

    [Serializable]
    public class UnityEventVector3 : UnityEvent<Vector3>
    {
    }
}