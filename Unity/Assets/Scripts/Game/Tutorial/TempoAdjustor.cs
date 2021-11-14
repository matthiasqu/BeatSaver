using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Game.Tutorial
{
    public class TempoAdjustor : MonoBehaviour
    {
        [TabGroup("General")] [SerializeField] private float timespan = .5f;
        [TabGroup("General")] [SerializeField] private float slowFactor = 100;
        [TabGroup("General")] [SerializeField] private TransformMover blockMoverPrefab;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private List<TransformMover> spawnedMovers;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private float originalTempo;

        [TabGroup("General")] [SerializeField] private UnityEvent onPauseSpawning = new UnityEvent();
        [TabGroup("General")] [SerializeField] private UnityEvent onResumeSpawning = new UnityEvent();

        private bool performing;

        private void Start()
        {
            originalTempo = blockMoverPrefab.Speed;
        }

        [TabGroup("Debug")]
        [Button]
        public void SlowDown()
        {
            StartCoroutine(DecreaseTimeScale());
        }

        [TabGroup("Debug")]
        [Button]
        public void SpeedUp()
        {
            StartCoroutine(IncreaseTimeScale());
        }

        public void AddBlock(Block[] go)
        {
            spawnedMovers.AddRange(go.Select(b => b.GetComponent<TransformMover>()));
        }

        public void RemoveBlock(GameObject go)
        {
            spawnedMovers.Remove(go.GetComponent<TransformMover>());
        }

        public void AddObstacle(Obstacle[] go)
        {
            spawnedMovers.AddRange(go.Select(b => b.GetComponent<TransformMover>()));
        }

        public void RemoveObstacle(GameObject go)
        {
            spawnedMovers.Remove(go.GetComponent<TransformMover>());
        }


        private IEnumerator DecreaseTimeScale()
        {
            if (performing) yield break;
            performing = true;
            while (Time.timeScale > 1 / slowFactor)
            {
                yield return null;
                
                var timeDelta = 1 / timespan * Time.deltaTime;
                float newTimescale;
                if (Time.timeScale - timeDelta < 1 / slowFactor)
                    newTimescale = 1 / slowFactor;
                else
                    newTimescale = Time.timeScale - timeDelta;

                Time.timeScale = newTimescale;
            }

            Debug.Log($"[BlocktempoAdjustor] Timescale set to {Time.timeScale}.");
            performing = false;
        }

        private IEnumerator IncreaseTimeScale()
        {
            if (performing) yield break;
            performing = true;
            while (Time.timeScale < 1 )
            {
                yield return null;
                float newTimescale;
                var timeDelta = 1 / timespan * Time.deltaTime;
                if (Time.timeScale - timeDelta > 1) 
                    newTimescale = 1;
                else 
                    newTimescale = Time.timeScale + timeDelta;

                Time.timeScale = newTimescale;
            }

            Debug.Log($"[BlocktempoAdjustor] Timescale set to {Time.timeScale}.");
            performing = false;
        }

        private IEnumerator IncreaseTempoMovement()
        {
            if (performing) yield break;
            performing = true;
            float tempo = 0;
            while (tempo < originalTempo)
            {
                yield return null;
                var tempoDelta = originalTempo / timespan * Time.deltaTime;
                if (tempo + tempoDelta > originalTempo) tempo = originalTempo;
                else tempo += tempoDelta;
                UpdateSpeeds(tempo);
            }

            Debug.Log("[BlocktempoAdjustor] Speedup Finished.");
            onResumeSpawning.Invoke();
            performing = false;
        }

        private IEnumerator DecreaseTempoMovement()
        {
            if (performing) yield break;
            performing = true;
            var tempo = originalTempo;
            while (tempo > 0)
            {
                yield return null;
                var tempoDelta = originalTempo / timespan * Time.deltaTime;
                if (tempo - tempoDelta < 0) tempo = 0;
                else tempo -= tempoDelta;
                UpdateSpeeds(tempo);
            }

            Debug.Log("[BlocktempoAdjustor] Slowdown Finished.");
            onPauseSpawning.Invoke();
            performing = false;
        }

        private void UpdateSpeeds(float speed)
        {
            spawnedMovers.ForEach(b => b.Speed = speed);
        }
    }
}