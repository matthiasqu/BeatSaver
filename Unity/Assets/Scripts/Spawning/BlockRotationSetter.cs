using BeatMapper;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Spawning
{
    public class BlockRotationSetter : SerializedMonoBehaviour
    {
        /// <summary>
        ///     The rotation strategy to use when rotating Blocks.
        /// </summary>
        [OdinSerialize] [InlineProperty] [HideLabel]
        private IRotationSetterStrategy strategy;

        /// <summary>
        ///     Forwards a reference to the current GameObject to <see cref="strategy" />.
        /// </summary>
        private void Awake()
        {
            strategy.Start(gameObject);
        }

        /// <summary>
        ///     Applies the correct rotation for the given cut direction.
        /// </summary>
        /// <param name="cutDirection"></param>
        public void ApplyRotation(CutDirection cutDirection)
        {
            strategy.SetRotation(cutDirection);
            //InvokeRotation();
        }

        /// <summary>
        ///     Invokes setting the rotation for this Block using the strategy defined in <see cref="strategy" />.
        /// </summary>
        public void InvokeRotation()
        {
            strategy.InvokeRotation();
        }
    }
}