namespace Cards
{
    public class CardEffectData
    {
        private readonly Card _card;
        private readonly int _duration;

        public CardEffectData(Card card, int duration)
        {
            _card = card;
            _duration = duration;
        }

        public Card Card => _card;
        public int Duration => _duration;
    }
}