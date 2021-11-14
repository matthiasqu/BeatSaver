using UnityEngine;

namespace Environment
{
    public class PlayerPositionSetter : MonoBehaviour
    {
        public void Start()
        {
            FindObjectOfType<PlayerPosition>().transform.position = transform.position;
        }
    }
}