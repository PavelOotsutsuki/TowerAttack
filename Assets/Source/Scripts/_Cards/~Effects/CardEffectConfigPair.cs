using Cards.Sounds;

namespace Cards.Effects
{
    public class CardEffectConfigPair
    {
        private readonly Card _card;
        private readonly CardEffectConfig _cardEffectConfig;
        private readonly CardSoundLogic _cardSoundLogic;

        public CardEffectConfigPair(Card card, CardEffectConfig cardEffectConfig, CardSoundLogic cardSoundLogic)
        {
            _card = card;
            _cardEffectConfig = cardEffectConfig;
            _cardSoundLogic = cardSoundLogic;
        }

        public Card Card => _card;
        public CardEffectConfig CardEffectConfig => _cardEffectConfig;
        public CardEffectData CardEffectData => new CardEffectData(_card, _cardEffectConfig.Duration, _cardSoundLogic);
    }
}