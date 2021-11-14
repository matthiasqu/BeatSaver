using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Random = System.Random;

namespace Environment
{
    public class CarPort : SerializedMonoBehaviour
    {
        [SerializeField] private int maxCars = 20;

        [SerializeField] [ReadOnly] private int spawnedCars;
        private readonly Random _rnd = new Random(789023451);
        [OdinSerialize] [ReadOnly] private Queue<Car> _cars = new Queue<Car>();

        [OdinSerialize] private Dictionary<Car, int> weights;

        public void AddCar(Car car)
        {
            car.enabled = false;
            _cars.Enqueue(car);
        }

        /// <summary>
        ///     Tries to return a new or pooled car instance if possible. This is dependent on the
        ///     <see cref="maxCars" /> and <see cref="spawnedCars" /> values, and whether any Car instances have been
        ///     pooled in <see cref="_cars" />.
        /// </summary>
        /// <param name="trans">The transform to spawn the car under.</param>
        /// <param name="car">The car if any can be supplied.</param>
        /// <returns>Whether a car is available.</returns>
        public bool TryGetCar(Transform trans, out Car car)
        {
            car = null;
            if (_cars.Count <= 0 && spawnedCars >= maxCars) return false;

            car = NextCar(trans);
            return true;
        }

        private Car NextCar(Transform parent)
        {
            if (_cars.Count <= 0) return GetRandomCar(parent);

            var car = _cars.Dequeue();
            car.transform.SetParent(parent);
            car.enabled = true;
            return car;
        }

        private Car GetRandomCar(Transform parent)
        {
            spawnedCars++;
            var summedWeights = weights.Select(a => a.Value).Aggregate((a, b) => a + b);
            var random = _rnd.Next(0, summedWeights);

            foreach (var pair in weights)
            {
                if (pair.Value > random) return Instantiate(pair.Key, parent);
                random -= pair.Value;
            }

            throw new Exception("No Car prefab chosen based on supplied weights.");
        }
    }
}