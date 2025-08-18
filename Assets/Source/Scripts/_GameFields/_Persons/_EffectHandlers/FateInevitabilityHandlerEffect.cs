namespace GameFields.Persons.EffectHandlers
{
    public class FateInevitabilityHandlerEffect
    {
        private readonly int _startTurns;

        private int _restTurns;

        public FateInevitabilityHandlerEffect(int countTurns)
        {
            _startTurns = countTurns;
            _restTurns = _startTurns;
        }

        public bool CanActivate()
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