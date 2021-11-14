using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class RandomForceCut : MonoBehaviour
    {
        [SerializeField] private Rigidbody rbLeft;
        [SerializeField] private Rigidbody rbRight;
        [SerializeField] private bool ignoreForce = true;
        [SerializeField] private float velocityFactor = .1f;

        [SerializeField] private int minTorque;
        [SerializeField] private int maxTorque;
        [SerializeField] private int minForceSide;
        [SerializeField] private int maxForceSide;

        [SerializeField] private int minForceUp;
        [SerializeField] private int maxForceUp;

        private Random _rnd;

        public float Force { get; set; } = 1;

        private void Start()
        {
            Hit();
        }

        [Button]
        private void Hit()
        {
            _rnd = new Random(minTorque * maxTorque * minForceUp + maxForceUp +
                              minForceSide * maxForceSide * Time.frameCount);
            ApplyRandom(rbLeft, Vector3.left);
            ApplyRandom(rbRight, Vector3.right);
        }

        private void ApplyRandom(Rigidbody rb, Vector3 dir)
        {
            if (ignoreForce) Force = 1;
            var angleX = _rnd.Next(minTorque, maxTorque);
            var angleY = _rnd.Next(minTorque, maxTorque);
            var angleZ = _rnd.Next(minTorque, maxTorque);

            var forceUp = _rnd.Next(minForceUp, maxForceUp);
            var forceSide = _rnd.Next(minForceSide, maxForceSide);

            rb.AddForce(dir * forceSide * Force * velocityFactor);
            rb.AddForce(Vector3.up * forceUp * Force * velocityFactor);
            rb.AddTorque(new Vector3(angleX, angleY, angleZ) * Force * velocityFactor);
            rb.useGravity = true;
        }
    }
}