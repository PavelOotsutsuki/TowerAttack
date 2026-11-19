using Tools;
using UnityEngine;

namespace Sounds
{
    public abstract class SoundConfig : ScriptableObject, IVolume
    {
        [field: SerializeField, Range(0, 1)] private float _percent;

        public float Percent => _percent;

        public void SetVolumePercent(float percent)
        {
            _percent = percent;
        }
    }
}