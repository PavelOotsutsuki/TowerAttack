using Cards.Sounds;

namespace Cards.Effects
{
    internal class CardEffectManager
    {
        private readonly IEffectFactory _effectFactory;
        private readonly CardEffectConfig _cardEffectConfig;
        private readonly CardSoundLogic _cardSoundLogic;

        public CardEffectManager(CardEffectConfig cardEffectConfig, IEffectFactory effectFactory, CardSoundLogic cardSoundLogic)
        {
            _cardEffectConfig = cardEffectConfig;
            _effectFactory = effectFactory;
            _cardSoundLogic = cardSoundLogic;
        }

        public void Play(Card card)
        {
            CardEffectConfigPair effectConfigPair = new CardEffectConfigPair(card, _cardEffectConfig, _cardSoundLogic);
            _effectFactory.Create(effectConfigPair);
        }
    }
}