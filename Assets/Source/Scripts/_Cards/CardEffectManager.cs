using UnityEngine;

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

            _effectCounter = 0;
        }

        public void Play()
        {
            _effect = _effectFactory.Create(_cardEffectConfig, SetCounter);
            //_effectCounter = _effect.Duration;
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

        private void SetCounter(int count)
        {
            Debug.Log("_effectCounter: " + _effectCounter + " -/- count: " + count);

            if (_effectCounter < count)
                _effectCounter = count;
        }
    }
}