using Cards;
using Cards.Effects;
using GameFields.Effects;

namespace GameFields.Persons
{
    public class PersonEffect: IReadOnlyPersonEffect
    {
        private readonly CardEffectConfigPair _cardEffectConfigPair;
        private readonly Effect _effect;
        private readonly EffectDuration _effectDuration;

        public PersonEffect(Effect effect, EffectDuration effectDuration, CardEffectConfigPair cardEffectConfigPair)
        {
            _cardEffectConfigPair = cardEffectConfigPair;
            _effect = effect;

            _effectDuration = effectDuration;
        }

        public Card Card => _cardEffectConfigPair.Card;
        public CardEffectConfig CardEffectConfig => _cardEffectConfigPair.CardEffectConfig;
        public Effect Effect => _effect;

        public void Discard()
        {
            _effectDuration.Discard();

            TryDiscard();
        }

        public void DecreaseCounter()
        {
            _effectDuration.Decrease();
        }

        public bool TryDiscard()
        {
            if (_effectDuration.CanDiscard)
            {
                _effect?.End();
                return true;
            }

            DecreaseCounter();
            return false;
        }
    }
}