using UnityEngine;

namespace Logging
{
    [CreateAssetMenu(order = 0, fileName = "Logger Configuration", menuName = "_custom/LoggerConfig")]
    public class DataLoggerConfiguration : ScriptableObject
    {
        [SerializeField] public string IP;
        [SerializeField] public string path;
        [SerializeField] public int port;

        [SerializeField] [Tooltip("In seconds!")]
        public int timeout = 10;
    }
}