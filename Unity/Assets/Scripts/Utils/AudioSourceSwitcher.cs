using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class AudioSourceSwitcher : SerializedMonoBehaviour
    {
        [OdinSerialize] [EnumToggleButtons] private SongVersion intitalSongVersion;

        [OdinSerialize] private UnityEvent onHigherSong = new UnityEvent();
        [OdinSerialize] private UnityEvent onLowerSong = new UnityEvent();

        [OdinSerialize] [InlineProperty] private Dictionary<SongVersion, AudioSource> songVersions =
            new Dictionary<SongVersion, AudioSource>();

        public SongVersion SongVersion
        {
            get => intitalSongVersion;
            set
            {
                if (value == intitalSongVersion) return;
                intitalSongVersion = value;
                SwitchSong(value);
            }
        }

        private void Start()
        {
            SwitchSong(SongVersion);
        }

        public void LowerSong()
        {
            if (IsLegalValue((int) SongVersion - 1))
            {
                onLowerSong.Invoke();
                SongVersion -= 1;
            }
        }

        public void HigherSong()
        {
            if (IsLegalValue((int) SongVersion + 1))
            {
                onHigherSong.Invoke();
                SongVersion += 1;
            }
        }

        private void SwitchSong(SongVersion songVersion)
        {
            Debug.Log($"[AudioSourceSwitcher] Switching to {songVersion}");
            songVersions.Where(s => s.Key != songVersion).Select(s => s.Value).ForEach(a => a.mute = true);
            songVersions[songVersion].mute = false;
        }

        private bool IsLegalValue(int i)
        {
            if (!Enum.IsDefined(typeof(SongVersion), i))
                //Debug.LogWarning($"[{GetType()}] Passing illegal enum value {i} to SongVersion.");
                return false;

            return true;
        }
    }

    public enum SongVersion
    {
        First,
        Fourth
    }
}