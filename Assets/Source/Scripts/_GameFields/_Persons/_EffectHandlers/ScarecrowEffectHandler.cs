using Cards;
using GameFields.Persons.Tables;

namespace GameFields.Persons.EffectHandlers
{
    public class ScarecrowEffectHandler
    {
        private readonly IDiscardManager _discardManager;

        private int _counter;
        private Card _card;

        public ScarecrowEffectHandler(IDiscardManager discardManager)
        {
            _counter = 0;

            _discardManager = discardManager;
        }

        public void Activate(int countTurns, Card card)
        {
            _counter = countTurns;
            _card = card;
        }

        public bool TryUse()
        {
            if (_counter <= 0)
                return false;

            int currentCount = _counter;

            _counter--;

            if (_counter <= 0)
            {
                _counter = 0;

                if (_discardManager.HasCard(_card))
                    _discardManager.Discard(_card);
            }

            return currentCount > 0;
        }
    }
}