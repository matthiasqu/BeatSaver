using BeatMapper;
using UnityEngine;

namespace Game
{
    /// <summary>
    ///     Wraps a <see cref="CubeColor" /> for usage on Controllers/Swords.
    /// </summary>
    public class ControllerColor : MonoBehaviour
    {
        /// <summary>
        ///     The Color to assign to the Controller.
        /// </summary>
        [SerializeField] private CubeColor color;

        /// <summary>
        ///     Invoked whenever <see cref="color" /> changes.
        /// </summary>
        [SerializeField] public UnityEventCubeColor onColorSet = new UnityEventCubeColor();

        /// <summary>
        ///     Supplies the <see cref="CubeColor" /> whenever it ios changed on the GameObject.
        /// </summary>
        public UnityEventCubeColor OnColorSet => onColorSet;

        /// <summary>
        ///     The <see cref="CubeColor" /> assigned to this object.
        /// </summary>
        public CubeColor Color
        {
            get => color;
            set
            {
                color = value;
                onColorSet.Invoke(value);
            }
        }


        private void Start()
        {
            OnColorSet.Invoke(Color);
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;
            OnColorSet.Invoke(Color);
        }
    }
}