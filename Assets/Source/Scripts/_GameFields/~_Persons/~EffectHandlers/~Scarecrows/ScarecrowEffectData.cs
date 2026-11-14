using Cards;

namespace GameFields.Persons.EffectHandlers.Scarecrows
{
    internal class ScarecrowEffectData
    {
        private readonly Card _card;

        private int _duration;

        public ScarecrowEffectData(Card card, int duration) 
        {
            _card = card;
            _duration = duration;
        }

        public bool NeedDelete => _duration <= 0;
        public int Duration => _duration;
        public Card Card => _card;

        public void Use()
        {
            _duration--;
        }
    }
}