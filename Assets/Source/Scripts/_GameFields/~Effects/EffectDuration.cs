namespace GameFields.Effects
{
    public class EffectDuration
    {
        private int _duration;

        public EffectDuration()
        {
            _duration = -2;
        }

        internal void SetDuration(int duration)
        {
            //Debug.Log($"_duration = {_duration};;; duration = {duration}");

            if (_duration < duration)
                _duration = duration;
        }

        public void Discard()
        {
            _duration = 0;
        }

        public void Decrease()
        {
            _duration--;
        }

        public bool CanDiscard => _duration <= 0;
        public int Duration => _duration;
    }
}