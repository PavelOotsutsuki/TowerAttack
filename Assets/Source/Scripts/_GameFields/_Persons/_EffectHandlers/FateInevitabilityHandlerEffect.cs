using Cards;

namespace GameFields.Persons.EffectHandlers
{
    public class FateInevitabilityHandlerEffect
    {
        private readonly int _startTurns;
        private readonly Card _card;

        private int _restTurns;

        public FateInevitabilityHandlerEffect(Card card, int countTurns)
        {
            _startTurns = countTurns;
            _card = card;
            _restTurns = _startTurns;
        }

        public Card Card => _card;

        public bool CanActivate() // Если ход первый, то способность не активируем
        {
            return _startTurns == _restTurns == false;
        }

        public void Next()
        {
            _restTurns--;
        }

        public bool IsReadyToDestroy()
        {
            return _restTurns <= 0;
        }
    }
}