namespace GameFields.Persons.EffectHandlers.Curses
{
    public class CurseEffectWithoutCard
    {
        private int _countTurns;

        public CurseEffectWithoutCard(int countTurns)
        {
            _countTurns = countTurns;
        }

        public bool CanBeDestroy => _countTurns <= 0;

        public void NextTurn()
        {
            _countTurns--;
        }
    }
}