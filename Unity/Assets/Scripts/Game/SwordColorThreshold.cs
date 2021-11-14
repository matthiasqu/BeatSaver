using BeatMapper;
using Sirenix.OdinInspector;
using Spawning;
using UnityEngine;
using Utils;

namespace Game
{
    /// <summary>
    ///     Checks whether a colliding / triggering <see cref="Sword" /> GameObject has the same colour as this Block.
    ///     Invokes <see cref="onCorrectSword" /> if so, and <see cref="onFalseSword" /> if otherwise.
    /// </summary>
    [RequireComponent(typeof(BlockSettings))]
    public class SwordColorThreshold : MonoBehaviour
    {
        /// <summary>
        ///     The color of the sword that has hit this Block first.
        /// </summary>
        [SerializeField] [ReadOnly] private CubeColor detectedSwordColor;

        /// <summary>
        ///     Invoked when a sword with the same color has been detected.
        /// </summary>
        [SerializeField] private UnityEventGameObject onCorrectSword = new UnityEventGameObject();

        /// <summary>
        ///     Invoked when a sword with a different color than the Block's is detected.
        /// </summary>
        [SerializeField] private UnityEventGameObject onIncorrectSword = new UnityEventGameObject();

        /// <summary>
        ///     The Block settings attached to this GameObject.
        /// </summary>
        private BlockSettings _blockSettings;

        /// <summary>
        ///     The color of the sword that has hit this Block first.
        /// </summary>
        public CubeColor DetectedSwordColor => detectedSwordColor;

        public bool WasCorrect { get; private set; } = true;

        /// <summary>
        ///     Fetch the BlockSettings of this GameObject.
        /// </summary>
        private void Awake()
        {
            _blockSettings = GetComponent<BlockSettings>();
            detectedSwordColor = CubeColor.None;
            onCorrectSword.AddListener(o => WasCorrect = true);
            onIncorrectSword.AddListener(o => WasCorrect = false);
        }

        /// <summary>
        ///     Check whether the Sword component on <see cref="other" /> has the same color assigned as
        ///     <see cref="_blockSettings" />.
        /// </summary>
        /// <param name="other"></param>
        public void CheckSword(GameObject other)
        {
            var controllerColor = other.GetComponentInChildren<ControllerColor>();
//            Debug.Log(
//                $"[SwordColorThreshold]{(controllerColor == null ? "NoControllerColor" : controllerColor.Color.ToString())} found on {other.name}");
            if ((detectedSwordColor = controllerColor.Color) == _blockSettings.NoteEvent._type)
                onCorrectSword.Invoke(other);
            else
                onIncorrectSword.Invoke(other);
        }
    }
}