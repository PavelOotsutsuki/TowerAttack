using Tools;

namespace Cards
{
    public class CardSoundVolume : IVolume
    {
        private readonly float _maxVolume = 1f;

        private float _percent;

        public CardSoundVolume()
        {
            _percent = 1f;
        }

        public float Percent => _percent;
        public float Volume => _maxVolume * _percent;

        public void SetVolumePercent(float percent)
        {
            _percent = percent;
        }
    }
}