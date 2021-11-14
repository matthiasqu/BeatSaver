using System;
using System.Collections.Generic;
using BeatMapper;
using BeatMapper.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

namespace Spawning
{
    /// <summary>
    ///     implementation of <see cref="IRotationSetterStrategy" />. Instantly rotates the Transform attached to
    ///     <see cref="gameObject" />.
    /// </summary>
    [Serializable]
    [InlineProperty]
    public class InstantRotationSetter : IRotationSetterStrategy
    {
        private CutDirection _cutDirection;
        private GameObject gameObject;

        public void Start(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void SetRotation(CutDirection cutDirection)
        {
            _cutDirection = cutDirection;
        }

        public void InvokeRotation()
        {
            var rotation = _cutDirection.ToRotationVector();
//            Debug.Log($"[NoteRotationSetter] Applying rotation {rotation}");
            gameObject.transform.Rotate(Vector3.forward, rotation.z);
        }
    }


    /// <summary>
    ///     Implementation of <see cref="IRotationSetterStrategy" /> which invokes a specific animation to cause rotation using
    ///     the <see cref="director" />.
    /// </summary>
    [Serializable]
    [InlineProperty]
    public class AnimatedRotationSetter : IRotationSetterStrategy
    {
        [SerializeField] [InlineProperty] private PlayableDirector director;
        private CutDirection _cutDirection;

        private GameObject gameObject;

        /// <summary>
        ///     The playables which correspond to the rotation vectors.
        /// </summary>
        [SerializeField] [InlineProperty]
        private Dictionary<CutDirection, PlayableAsset> playables = new Dictionary<CutDirection, PlayableAsset>();

        public void Start(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void SetRotation(CutDirection cutDirection)
        {
            _cutDirection = cutDirection;
        }

        public void InvokeRotation()
        {
            if (_cutDirection == CutDirection.Down || _cutDirection == CutDirection.Any) return;
            director.Play(playables[_cutDirection]);
        }
    }
}