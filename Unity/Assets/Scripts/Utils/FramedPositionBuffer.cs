using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class FramedPositionBuffer : MonoBehaviour
    {
        [SerializeField] private int bufferCapacity = 10;
        [SerializeField] private Transform transformToTrack;
        [SerializeField] [ReadOnly] private FixedQueue<Vector3> _positions;
        public Vector3 FarthestPoint => _positions.OrderByDistance(transform.position).Last();
        public Vector3 CurrentPoint => transformToTrack.position;
        public Vector3 Velocity { get; private set; }

        private void Awake()
        {
            _positions = new FixedQueue<Vector3>(bufferCapacity);
        }

        private void FixedUpdate()
        {
            var position = transformToTrack.position;
            _positions.Enqueue(position);
        }
    }

    [Serializable]
    public class FixedQueue<T>
    {
        [SerializeField] private Queue<T> _queue = new Queue<T>();

        public FixedQueue(int size)
        {
            Size = size;
        }

        public int Size { get; private set; }

        public void Enqueue(T element)
        {
            _queue.Enqueue(element);
            while (_queue.Count > Size) _queue.Dequeue();
        }

        public IEnumerable<T> ToArray()
        {
            return _queue.ToArray();
        }
    }
}