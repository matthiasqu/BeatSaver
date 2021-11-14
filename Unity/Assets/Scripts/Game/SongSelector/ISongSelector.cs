using UnityEngine;

namespace Game.SongSelector
{
    public interface ISongSelector
    {
        public int SelectSong();
        void Start(GameObject go);
        void ReceiveTick(int tick);
    }
}