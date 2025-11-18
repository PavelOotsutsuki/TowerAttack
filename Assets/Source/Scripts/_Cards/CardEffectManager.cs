namespace Cards
{
    internal class CardEffectManager
    {
        private readonly IEffectFactory _effectFactory;
        private readonly CardEffectConfig _cardEffectConfig;

        public CardEffectManager(CardEffectConfig cardEffectConfig, IEffectFactory effectFactory)
        {
            _cardEffectConfig = cardEffectConfig;
            _effectFactory = effectFactory;
        }

        public void Play(Card card)
        {
            CardEffectConfigPair effectConfigPair = new CardEffectConfigPair(card, _cardEffectConfig);
            _effectFactory.Create(effectConfigPair);
        }
    }
}