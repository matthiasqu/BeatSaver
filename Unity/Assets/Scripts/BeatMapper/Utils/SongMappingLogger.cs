using UnityEngine;

namespace BeatMapper.Utils
{
    public class SongMappingLogger : MonoBehaviour
    {
        public void log(SongMapping mapping)
        {
            Debug.Log(mapping.ToString());
        }
    }
}