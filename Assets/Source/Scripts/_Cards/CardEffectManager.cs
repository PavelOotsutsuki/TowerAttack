namespace Cards
{
    public class CardEffectManager
    {
        private readonly IEffectFactory _effectFactory;
        private readonly CardEffectConfig _cardEffectConfig;

        private int _effectCounter;
        private Effect _effect;

        //public bool IsPlayingEffect => _effect is null ? false : _effect.IsComplete;

        public CardEffectManager(CardEffectConfig cardEffectConfig, IEffectFactory effectFactory)
        {
            _cardEffectConfig = cardEffectConfig;
            _effectFactory = effectFactory;
        }

        public void Play()
        {
            _effect = _effectFactory.Create(_cardEffectConfig);
            _effectCounter = _effect.Duration;
        }

        public bool TryDiscard()
        {
            if (_effectCounter <= 0)
            {
                _effect?.End();
                _effect = null;
                return true;
            }

            _effectCounter--;

            return false;
        }
    }
}