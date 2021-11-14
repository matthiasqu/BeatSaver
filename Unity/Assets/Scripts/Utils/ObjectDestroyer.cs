using UnityEngine;

namespace Utils
{
    /// <summary>
    ///     Destroys an object upon invocation.
    /// </summary>
    public class ObjectDestroyer : MonoBehaviour
    {
        /// <summary>
        ///     Destroy the supplied GameObject.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject" /> to Destroy</param>
        public void DestroyObject(GameObject gameObject)
        {
            Destroy(gameObject);
        }
    }
}