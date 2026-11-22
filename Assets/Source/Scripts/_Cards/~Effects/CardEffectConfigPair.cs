using Cards.Sounds;

namespace Cards.Effects
{
    public class CardEffectConfigPair
    {
        private readonly Card _card;
        private readonly ICardEffectConfig _cardEffectConfig;
        private readonly CardSoundLogic _cardSoundLogic;

        public CardEffectConfigPair(Card card, ICardEffectConfig cardEffectConfig, CardSoundLogic cardSoundLogic)
        {
            _card = card;
            _cardEffectConfig = cardEffectConfig;
            _cardSoundLogic = cardSoundLogic;
        }

        public Card Card => _card;
        public ICardEffectConfig CardEffectConfig => _cardEffectConfig;
        public CardEffectData CardEffectData => new CardEffectData(_card, _cardEffectConfig.Duration, _cardSoundLogic);
    }
}