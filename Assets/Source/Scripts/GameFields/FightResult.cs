using System;

namespace GameFields
{
    public class FightResult : IReadonlyFightResult
    {
        private EndFightResults _endFightResult;
        private bool _isEmpty;

        public FightResult()
        {
            _isEmpty = true;
            _endFightResult = EndFightResults.Draw;
        }

        public EndFightResults Result
        {
            get
            {
                if (_isEmpty)
                {
                    throw new ArgumentNullException("Fight Result is empty");
                }

                return _endFightResult;
            }
        }

        public void SetPlayerWin()
        {
            SetResult(EndFightResults.PlayerWin);
        }

        public void SetEnemyWin()
        {
            SetResult(EndFightResults.EnemyWin);
        }

        public void SetDraw()
        {
            SetResult(EndFightResults.Draw);
        }

        private void SetResult(EndFightResults result)
        {
            if (_isEmpty == false)
            {
                throw new Exception("Fight Result is already setted");
            }

            _isEmpty = false;
            _endFightResult = result;
        }
    }
}
