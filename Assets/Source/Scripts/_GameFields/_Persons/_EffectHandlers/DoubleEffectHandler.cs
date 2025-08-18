namespace GameFields.Persons.EffectHandlers
{
    public class DoubleEffectHandler
    {
        private int _counter;

        public DoubleEffectHandler()
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