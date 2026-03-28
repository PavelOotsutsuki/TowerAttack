using Tools;
using UnityEngine;

namespace Cards.Sounds
{
    public class CardSoundRoot : IVolume
    {
        private readonly float _maxVolume = 1f;

        private float _percent;

        public CardSoundRoot()
        {
            _percent = 1f;
        }

        public float Percent => _percent;

        private float Volume => _maxVolume * _percent;

        public void Play(AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero, Volume);
        }

        public void SetVolumePercent(float percent)
        {
            _percent = percent;
        }
    }
}