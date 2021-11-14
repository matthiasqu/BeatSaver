using UnityEngine;

namespace Utils
{
    public class ImageRotator : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float speed;
        private Quaternion originalRotation;

        private void Awake()
        {
            originalRotation = rectTransform.rotation;
        }

        private void Update()
        {
            var deltaRotation = Time.deltaTime * speed;
            rectTransform.Rotate(Vector3.forward, deltaRotation);
        }

        private void OnDisable()
        {
            rectTransform.rotation = originalRotation;
        }
    }
}