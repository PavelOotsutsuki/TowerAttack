namespace GameFields.Persons.EffectHandlers
{
    public class SkipTurnEffectHandler
    {
        private int _counter;

        public SkipTurnEffectHandler()
        {
            _counter = 0;
        }

        public bool IsActive => _counter > 0;

        public void Activate(int countTurns)
        {
            _counter = countTurns;
        }

        public void OnEndTurn()
        {
            if (_counter > 0)
                _counter--;
        }
    }
}