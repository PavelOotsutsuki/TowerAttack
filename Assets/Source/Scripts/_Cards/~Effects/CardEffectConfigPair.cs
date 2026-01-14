namespace Cards.Effects
{
    public class CardEffectConfigPair
    {
        private readonly Card _card;
        private readonly CardEffectConfig _cardEffectConfig;

        public CardEffectConfigPair(Card card, CardEffectConfig cardEffectConfig)
        {
            _card = card;
            _cardEffectConfig = cardEffectConfig;
        }

        public Card Card => _card;
        public CardEffectConfig CardEffectConfig => _cardEffectConfig;
        public CardEffectData CardEffectData => new CardEffectData(_card, _cardEffectConfig.Duration);
    }
}