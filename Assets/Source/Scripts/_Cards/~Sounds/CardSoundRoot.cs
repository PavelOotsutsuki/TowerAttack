using Tools;
using UnityEngine;

namespace Cards.Sounds
{
    public class CardSoundRoot : IVolume
    {
        private readonly IVolume _soundConfig;
        private readonly float _maxVolume = 1f;

        public CardSoundRoot(IVolume soundConfig)
        {
            _soundConfig = soundConfig;
        }

        public float Percent => _soundConfig.Percent;

        private float Volume => _maxVolume * _soundConfig.Percent;

        public void Play(AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero, Volume);
        }

        public void SetVolumePercent(float percent)
        {
            _soundConfig.SetVolumePercent(percent);
        }
    }
}