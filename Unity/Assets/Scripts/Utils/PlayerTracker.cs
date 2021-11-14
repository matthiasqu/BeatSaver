using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    //TODO: Rename to CameraTracker
    public class PlayerTracker : MonoBehaviour
    {
        //TODO: Remove Readonly tag
        [SerializeField] [ReadOnly] private Camera mainCamera;
        //TODO: Introduce fields to choose which axis to update.

        private void Start()
        {
            if (mainCamera == null)
            {
                Debug.LogWarning($"[PlayerTracker] No camera assigned, assigning Camera.main on {Camera.main.name}");
                mainCamera = Camera.main;
            }
        }

        /// <summary>
        ///     Updates the transform orientation to look at the assigned camera Object in <see cref="mainCamera" />.
        /// </summary>
        private void Update()
        {
            var position = mainCamera.transform.position;
            transform.LookAt(new Vector3(position.x, transform.position.y,
                position.z));
        }
    }
}