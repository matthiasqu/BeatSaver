using System.Collections.Generic;
using System.Linq;
using BeatMapper;
using Sirenix.OdinInspector;
using Spawning;
using UnityEngine;
using Utils;

namespace Game
{
    /// <summary>
    ///     Aggregates the score of all Blocks spawned at a time. Receives <see cref="SongMapping" /> objects from the
    ///     <see cref="NoteSpawner" /> of the scene and waits for their arrival. Averages the score of all Block objects
    ///     spawned and invokes <see cref="onScoreCalculated" />. Each single score is also supplied through the
    ///     <see cref="onScoreDetected" /> event.
    /// </summary>
    public class ScoreAggregator : MonoBehaviour
    {
        /// <summary>
        ///     The number of blocks to detect for the next average score.
        /// </summary>
        [SerializeField] [ReadOnly] private int blocksToDetect;

        /// <summary>
        ///     The number of blocks that have already been detected for the current score.
        /// </summary>
        [SerializeField] [ReadOnly] private int blocksDetected;

        /// <summary>
        ///     Supplies the average score of the last blocks.
        /// </summary>
        [SerializeField] private UnityEventInt onScoreCalculated = new UnityEventInt();


        /// <summary>
        ///     Supplies the single score of the last block that was detected.
        /// </summary>
        [SerializeField] private UnityEventInt onScoreDetected = new UnityEventInt();


        /// <summary>
        ///     Stores the last spawned blocks in a queue.
        /// </summary>
        private readonly Queue<SongMapping> mappings = new Queue<SongMapping>();

        /// <summary>
        ///     Stores the scores of the detected blocks.
        /// </summary>
        private readonly List<ScoreCalculator> scores = new List<ScoreCalculator>();

        /// <summary>
        ///     Supplies the average score of the last blocks.
        /// </summary>
        public UnityEventInt ONScoreCalculated => onScoreCalculated;

        /// <summary>
        ///     Supplies the single score of the last block that was detected.
        /// </summary>
        public UnityEventInt ONScoreDetected => onScoreDetected;

        /// <summary>
        ///     Adds a supplied mapping to the queue.
        /// </summary>
        /// <param name="mapping"></param>
        public void AddSongmapping(SongMapping mapping)
        {
            mappings.Enqueue(mapping);
        }

        /// <summary>
        ///     Adds the score of a single block object to <see cref="scores" /> and dequeues the next entry in
        ///     <see cref="mappings" />
        ///     if none is currently active.
        /// </summary>
        /// <param name="o"></param>
        public void AddToScore(GameObject o)
        {
            // get the Block's score
            var score = o.GetComponentInChildren<ScoreCalculator>();
            scores.Add(score);
            onScoreDetected.Invoke(score.Score);

            // check whether this belongs to a new song mapping
            if (blocksToDetect == 0)
            {
                var nextMapping = mappings.Dequeue();

                // in case there is no note but an obstacle or so in the next mapping
                while (nextMapping.Notes.Length < 1) nextMapping = mappings.Dequeue();

                blocksToDetect = nextMapping.Notes.Length;
//                Debug.Log($"[ScoreAggregator] Next mapping has {nextMapping.Notes.Length} blocks in it.");
            }

            // increase the number of detected Blocks for the current mapping
            blocksDetected++;
//            Debug.Log($"[ScoreAggregator] Increasing count to {blocksDetected}");

            // invoke onScoreCalculated if all blocks for the current mapping have been detected
            if (blocksDetected >= blocksToDetect)
            {
                var maxScore = scores.Select(a => a.Score).Max();
                var minScore = scores.Select(a => a.Score).Min();
                var weightedMax = maxScore - (maxScore - minScore);
                onScoreCalculated.Invoke(weightedMax);
//                Debug.Log(
//                    $"[ScoreAggregator] Mean score is {maxScore / blocksToDetect} ({blocksToDetect} Blocks).");

                // begin a new mapping.
                blocksDetected = blocksToDetect = 0;
                scores.Clear();
            }
        }
    }
}