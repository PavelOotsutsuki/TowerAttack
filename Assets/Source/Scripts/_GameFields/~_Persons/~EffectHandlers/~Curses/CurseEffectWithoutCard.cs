using Servers;

namespace GameFields.Persons.EffectHandlers.Curses
{
    public class CurseEffectWithoutCard
    {
        private readonly FightProcessDBManager _fightProcessDBManager;
        private readonly bool _isPlayersEffect; 

        private int _countTurns;

        public CurseEffectWithoutCard(int countTurns, FightProcessDBManager fightProcessDBManager, bool isPlayersEffect)
        {
            _countTurns = countTurns;
            _fightProcessDBManager = fightProcessDBManager;
            _isPlayersEffect = isPlayersEffect;
        }

        public bool CanBeDestroy => _countTurns <= 0;

        public void NextTurn()
        {
            _countTurns--;
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, _isPlayersEffect, null, $"LEFT ({_countTurns}) TURNS", GetType().Name);
        }
    }
}