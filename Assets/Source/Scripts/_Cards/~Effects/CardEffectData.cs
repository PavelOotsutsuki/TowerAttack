using Cards.Sounds;

namespace Cards.Effects
{
    public class CardEffectData
    {
        private readonly Card _card;
        private readonly int _duration;
        private readonly CardSoundLogic _cardSoundLogic;

        public CardEffectData(Card card, int duration, CardSoundLogic cardSoundLogic)
        {
            _card = card;
            _duration = duration;
            _cardSoundLogic = cardSoundLogic;
        }

        public Card Card => _card;
        public int Duration => _duration;
        public CardSoundLogic CardSoundLogic => _cardSoundLogic;
    }
}