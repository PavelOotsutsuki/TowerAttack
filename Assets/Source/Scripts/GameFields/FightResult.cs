namespace GameFields
{
    public class FightResult
    {
        private EndFightResults _endFightResult;

        public EndFightResults Result => _endFightResult;

        public FightResult()
        {
            _endFightResult = EndFightResults.None;
        }

        public void SetPlayerWin()
        {
            _endFightResult = EndFightResults.PlayerWin;
        }

        public void SetEnemyWin()
        {
            _endFightResult = EndFightResults.EnemyWin;
        }

        public void SetDraw()
        {
            _endFightResult = EndFightResults.Draw;
        }
    }
}
