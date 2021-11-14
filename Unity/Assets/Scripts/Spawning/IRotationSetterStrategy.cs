using BeatMapper;
using UnityEngine;

namespace Spawning
{
    /// <summary>
    ///     Interface defining possible strategies on how to set the rotation of objects. Implementations should be used in
    ///     <see cref="BlockRotationSetter" /> components.
    /// </summary>
    public interface IRotationSetterStrategy
    {
        /// <summary>
        ///     Implementations must receive a reference to the GameObject on which the <see cref="BlockRotationSetter" /> is
        ///     present to gain access to the scene.
        /// </summary>
        /// <param name="gameObject"></param>
        public abstract void Start(GameObject gameObject);

        /// <summary>
        ///     Implementations can possibly gain access to the Update() calls of the <see cref="BlockRotationSetter" /> through
        ///     this.
        /// </summary>
        public abstract void Update();

        /// <summary>
        ///     Sets the rotation value, but does not finally apply it.
        /// </summary>
        /// <param name="cutDirection"></param>
        public abstract void SetRotation(CutDirection cutDirection);

        /// <summary>
        ///     Applies the rotation value set through <see cref="SetRotation" />.
        /// </summary>
        public abstract void InvokeRotation();
    }
}