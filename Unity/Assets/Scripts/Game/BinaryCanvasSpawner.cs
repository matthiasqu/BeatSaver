using System;
using TMPro;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Game
{
    /// <summary>
    ///     Implementation of <see cref="ICanvasSpawner" />. Spawns canvases which are colored according to a score.
    ///     If it passes the value set in <see cref="upperThreshold" />, <see cref="passedText" /> will be used and coloured in
    ///     <see cref="passedColor" />.
    ///     Otherwise <see cref="belowText" /> wilöl be used and coloured in <see cref="belowColor" />.
    /// </summary>
    [Serializable]
    public class BinaryCanvasSpawner : ICanvasSpawner
    {
        /// <summary>
        ///     The threshold to use for deciding whether a score is sufficient to be high enough.
        /// </summary>
        [SerializeField] private IntVariable upperThreshold;

        /// <summary>
        ///     The text used to describe passed scores.
        /// </summary>
        [SerializeField] private string passedText;

        /// <summary>
        ///     The text used to describe scores that did not pass the threshold.
        /// </summary>
        [SerializeField] private string belowText;

        /// <summary>
        ///     The color used to colour <see cref="passedText" /> canvases.
        /// </summary>
        [SerializeField] private Color passedColor = Color.green;

        /// <summary>
        ///     The colour used to colour <see cref="belowText" /> canvases.
        /// </summary>
        [SerializeField] private Color belowColor = Color.red;

        private GameObject _gameObject;

        /// <summary>
        ///     The ScoreCanvas prefab to instantiate
        /// </summary>
        /// TODO: mark ScoreCanvas prefab with ScoreCanvas.cs component.
        private GameObject _prefab;

        /// <summary>
        ///     The offset on the z-axis with which the canvas will be spawned.
        /// </summary>
        private float _zOffset;

        private Vector3 Offset => new Vector3(0, 0, _zOffset);

        public void Awake(GameObject gameObject, GameObject prefab, float zOffset)
        {
            _gameObject = gameObject;
            _prefab = prefab;
            _zOffset = zOffset;
        }

        /// <summary>
        ///     Spawns a cavas depending on the score supplied through the <see cref="scoreGo" />.
        ///     If the score passes the value in <see cref="upperThreshold" />, <see cref="passedText" /> in
        ///     <see cref="passedColor" /> is used. otherwise, <see cref="belowText" /> in <see cref="belowColor" /> is used.
        /// </summary>
        /// <param name="scoreGo"></param>
        public void SpawnCanvas(GameObject scoreGo)
        {
            // instantiate
            var score = scoreGo.GetComponentInChildren<ScoreCalculator>();
            var canvas = Object.Instantiate(_prefab, scoreGo.transform.position - Offset, _prefab.transform.rotation);
            var setter = canvas.GetComponentInChildren<TextSetter>();

            // set the text
            if (score.Score >= upperThreshold.Value) setter.SetText(passedText);
            else setter.SetText(belowText);

            // set the color
            var tmpText = canvas.GetComponentInChildren<TMP_Text>();
            tmpText.color = score.Score >= upperThreshold.Value ? passedColor : belowColor;
        }
    }
}