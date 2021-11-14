using System;
using BeatMapper;
using Sirenix.OdinInspector;
using Spawning;
using UnityEngine;
using Utils;

// TODO: This does not belong in the Spawner namespace
namespace Game
{
    /// <summary>
    ///     Wrapper component for implementations of <see cref="IScoreCalculationStrategy" />.
    /// </summary>
    [RequireComponent(typeof(BlockSettings))]
    public class ScoreCalculator : SerializedMonoBehaviour
    {
        /// <summary>
        ///     The cut direction detected on the Block object this component is attached to.
        /// </summary>
        [SerializeField] [ReadOnly] private CutDirection detectedCutDirection = CutDirection.None;


        /// <summary>
        ///     Invoked when the Score for this Block is calculated.
        /// </summary>
        public UnityEventInt OnScoreCalculated = new UnityEventInt();

        /// <summary>
        ///     The strategy to use for calculating the score for this Block.
        /// </summary>
        [SerializeField] private IScoreCalculationStrategy strategy;


        /// <summary>
        ///     The calculated Score of this Block.
        /// </summary>
        public int Score => strategy.Score();

        /// <summary>
        ///     The CutDirection detected on this Block.
        /// </summary>
        public CutDirection DetectedCutDirection
        {
            get => detectedCutDirection;
            set
            {
                if (detectedCutDirection != CutDirection.None) return;
                detectedCutDirection = value;
                strategy.SetCutDirection(detectedCutDirection);
            }
        }

        private void Awake()
        {
            strategy.Awake(this);
        }

        private void Start()
        {
            strategy.Start();
        }

        private void Update()
        {
            strategy.Update();
        }

        public void UpdateScore()
        {
            strategy.Score();
        }
    }

    /// <summary>
    ///     Interface for score calculation strategies.
    /// </summary>
    internal interface IScoreCalculationStrategy
    {
        /// <summary>
        ///     Should forward update() calls made to the GameObject on which the strategy is used to the strategy.
        /// </summary>
        public void Update();

        /// <summary>
        ///     Should be called when the Start() method on the GameObject has been called.
        /// </summary>
        public void Start();

        /// <summary>
        ///     Forward the containing <see cref="ScoreCalculator" /> to the implementation.
        /// </summary>
        /// <param name="behaviour"></param>
        public void Awake(ScoreCalculator behaviour);

        public void SetCutDirection(CutDirection direction);

        /// <summary>
        ///     Returns the Score calculated by the strategy. Used like a property which are not supported in Interfaces.
        /// </summary>
        /// <returns></returns>
        public int Score();
    }

    /// <summary>
    ///     calculates the score based on how close to the Blocks lifetime it was hit.
    /// </summary>
    [Serializable]
    internal class TimedScoreCalculationStrategy : IScoreCalculationStrategy
    {
        [SerializeField] [ReadOnly] private int _score;
        [SerializeField] [ReadOnly] private float _lifetime;
        [SerializeField] [ReadOnly] private bool _cut;
        [SerializeField] [ReadOnly] private CutDirection _cutDirection;
        [SerializeField] private BlockSettings blockSettings;
        [SerializeField] private SwordColorThreshold swordColorThreshold;
        private ScoreCalculator _behaviour;

        public void SetCutDirection(CutDirection direction)
        {
            _cutDirection = direction;
        }

        public int Score()
        {
            return CalculateScore();
        }

        public void Start()
        {
            swordColorThreshold ??= _behaviour.GetComponent<SwordColorThreshold>();
            blockSettings ??= _behaviour.GetComponent<BlockSettings>();
        }

        public void Update()
        {
            if (!_cut) _lifetime += Time.deltaTime;
        }

        public void Awake(ScoreCalculator behaviour)
        {
            _behaviour = behaviour;
        }

        private int CalculateScore()
        {
            _cut = true;
            _score = 0;

            if (swordColorThreshold.DetectedSwordColor != blockSettings.NoteEvent._type)
            {
                _score = 0;
            }
            else
            {
                var expected = blockSettings.NoteEvent._cutDirection;
                if (_cutDirection == expected)
                    _score = (int) (115f -
                                    Math.Floor((blockSettings.NoteEvent._time - _lifetime) * 10)
                        ); //TODO: make sure this never exceeds 115
            }

            _behaviour.OnScoreCalculated.Invoke(_score);
            return _score;
        }
    }

    /// <summary>
    ///     Calculates the score based on the velocity of the controller with which the Block was hit.
    /// </summary>
    [Serializable]
    internal class VelocityScoreCalculationStrategy : IScoreCalculationStrategy
    {
        [SerializeField] private VelocityBuffer velocity;
        [SerializeField] private BlockSettings blockSettings;
        [SerializeField] private SwordColorThreshold swordColorThreshold;
        [SerializeField] private int scoreDirection = 800;
        [SerializeField] [Range(1, 500)] private int strengthFactor = 200;
        [SerializeField] [ReadOnly] private int score;
        [SerializeField] [ReadOnly] private CutDirection cutDirection;
        [SerializeField] private bool ignoreDirection;
        private ScoreCalculator _behaviour;

        public void Update()
        {
        }

        public void Start()
        {
            //TODO: move this to awake
            swordColorThreshold ??= _behaviour.GetComponent<SwordColorThreshold>();
            blockSettings ??= _behaviour.GetComponent<BlockSettings>();
        }

        public void Awake(ScoreCalculator behaviour)
        {
            _behaviour = behaviour;
        }

        public void SetCutDirection(CutDirection direction)
        {
            cutDirection = direction;
        }

        public int Score()
        {
            return CalculateScore();
        }

        private int CalculateScore()
        {
            if (swordColorThreshold.DetectedSwordColor != blockSettings.NoteEvent._type) return 0;

            var rightDir = blockSettings.NoteEvent._cutDirection == cutDirection || ignoreDirection;

            var hitScore = (int) Math.Ceiling(velocity.MaxVelocity * strengthFactor);

            if (!rightDir)
            {
                score = hitScore > scoreDirection ? scoreDirection : hitScore;
            }
            else score = hitScore;
            Debug.Log($"[ScoreCalculator] Score is {score}");
            _behaviour.OnScoreCalculated.Invoke(score);
            return score;
        }
    }
}