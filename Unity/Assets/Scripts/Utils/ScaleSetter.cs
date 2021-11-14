using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     Sets the objects local or global scale.
    /// </summary>
    public class ScaleSetter : MonoBehaviour
    {
        /// <summary>
        ///     Whether to set scale on the x-axis.
        /// </summary>
        [SerializeField] private bool setX;

        /// <summary>
        ///     Whether to set scale on the y-axis.
        /// </summary>
        [SerializeField] private bool setY;

        /// <summary>
        ///     Whether to set scale on the z-axis.
        /// </summary>
        [SerializeField] private bool setZ;

        /// <summary>
        ///     Whether to use global instead of local scale.
        /// </summary>
        [SerializeField] private bool useGlobalScale;

        /// <summary>
        ///     Sets the Transforms scale according to the settings above.
        /// </summary>
        /// <param name="scale">The scale to apply.</param>
        public void SetScale(Vector3 scale)
        {
            var t = gameObject.transform;
            var currentScale = useGlobalScale ? t.lossyScale : t.localScale;

            var newScale = new Vector3(
                setX ? scale.x : currentScale.x,
                setY ? scale.y : currentScale.y,
                setZ ? scale.z : currentScale.z);

            t.localScale = newScale;
        }
    }
}