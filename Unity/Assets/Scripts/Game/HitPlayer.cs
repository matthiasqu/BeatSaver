using BeatMapper;
using Spawning;
using UnityEngine;

//TODO: rename
namespace Game
{
    /// <summary>
    ///     Plays a hit or miss sound on a Block's audio source.
    /// </summary>
    //TODO: Rename this to sound player o sth.
    public class HitPlayer : MonoBehaviour
    {
        /// <summary>
        ///     Sound top play when a block has been hit.
        /// </summary>
        [SerializeField] private AudioClip onHit;

        /// <summary>
        ///     Sound to play when a Block has been missed.
        /// </summary>
        [SerializeField] private AudioClip onMissed;

        private bool _lastBlockHit = true;

        /// <summary>
        ///     Plays a sound depending on the <see cref="block" /> object's AudioSource if it was cut, depending on the score.
        /// </summary>
        /// <param name="block"></param>
        public void PlaySound(GameObject block)
        {
            var blockSettings = block.GetComponentInChildren<BlockSettings>();
            var audioSource = block.GetComponent<AudioSource>();
            var cutState = block.GetComponentInChildren<CutState>();
            var cutDirectionConverter = block.GetComponentInChildren<CutDirectionConverter>();
            var swordColor = block.GetComponentInChildren<SwordColorThreshold>();
            var sliceable = block.GetComponentInChildren<Sliceable>();

            var correctlyHit = swordColor.WasCorrect &&
                               (cutDirectionConverter.DetectedCutDirection == blockSettings.NoteEvent._cutDirection ||
                                blockSettings.NoteEvent._cutDirection == CutDirection.Any);

            if (correctlyHit || _lastBlockHit && cutState.CutDirection == CutDirection.None)
                audioSource.PlayOneShot(onHit);
            else if (cutState.CutDirection != CutDirection.None) audioSource.PlayOneShot(onMissed);

            if (cutState.CutDirection == CutDirection.None)
            {
                sliceable.onSliced.AddListener(() =>
                {
                    correctlyHit = swordColor.WasCorrect &&
                                   (cutDirectionConverter.DetectedCutDirection ==
                                    blockSettings.NoteEvent._cutDirection ||
                                    blockSettings.NoteEvent._cutDirection == CutDirection.Any);
                    _lastBlockHit = correctlyHit;
                });
                if (!_lastBlockHit)
                    sliceable.onSliced.AddListener(() => audioSource.PlayOneShot(correctlyHit ? onHit : onMissed));
                _lastBlockHit = false;
            }
        }
    }
}