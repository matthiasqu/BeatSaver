using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using Random = System.Random;

namespace Environment
{
    public class TrafficSpawner : SerializedMonoBehaviour
    {
        [SerializeField] private PlayableAsset innerPlayable;
        [SerializeField] private PlayableAsset outerPlayable;
        [SerializeField] private string trackName;
        [SerializeField] private int minSpawnInterval;
        [SerializeField] private int maxSpawnInterval;
        [SerializeField] private int seed;
        [SerializeField] private CarPort carPort;

        private float _lastSpawnTime;
        private float _nextSpawnTime;
        private Random _rnd;

        private void Start()
        {
            _rnd = new Random(seed);
        }

        private void Update()
        {
            if (Time.timeSinceLevelLoad - _lastSpawnTime > _nextSpawnTime / 1000f) Spawn();
        }

        private void Spawn()
        {
            _lastSpawnTime = Time.timeSinceLevelLoad;
            _nextSpawnTime = _rnd.Next(minSpawnInterval, maxSpawnInterval);

            if (!carPort.TryGetCar(transform, out var car)) return;

            var playableDirector = car.GetComponent<PlayableDirector>();
            var animator = car.GetComponent<Animator>();
            var asset = _rnd.NextDouble() > .5d ? innerPlayable : outerPlayable;
            playableDirector.playableAsset = asset;
            var output = playableDirector.playableAsset.outputs.First(o => o.streamName == trackName);
            playableDirector.SetGenericBinding(output.sourceObject, animator);

            playableDirector.Play();
        }
    }
}