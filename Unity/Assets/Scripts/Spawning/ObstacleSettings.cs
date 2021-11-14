using BeatMapper;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spawning
{
    /// <summary>
    ///     A wrapper for a single <see cref="BeatMapper.ObstacleEvent" />.
    /// </summary>
    public class ObstacleSettings : MonoBehaviour
    {
        [SerializeField] [ReadOnly] [HideLabel] [InlineProperty]
        private ObstacleEvent obstacleEvent;

        /// <summary>
        ///     The <see cref="BeatMapper.ObstacleEvent" /> contained in this component.
        /// </summary>
        public ObstacleEvent ObstacleEvent
        {
            get => obstacleEvent;
            set => obstacleEvent = value;
        }
    }
}