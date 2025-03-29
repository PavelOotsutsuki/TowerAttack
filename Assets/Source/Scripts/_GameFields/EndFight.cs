using System;
using UnityEngine;

namespace GameFields
{
    public class EndFight: IFightStep
    {
        private readonly IReadonlyFightResult _fightResult;

        private bool _isComplete;

        public EndFight(IReadonlyFightResult fightResult)
        {
            _isComplete = false;

            _fightResult = fightResult;
        }

        public bool IsComplete => _isComplete;

        public void StartStep()
        {
            switch (_fightResult.Result)
            {
                case EndFightResults.PlayerWin:
                    ActivatePlayerWinActions();
                    break;
                case EndFightResults.EnemyWin:
                    ActivateEnemyWinActions();
                    break;
                case EndFightResults.Draw:
                    ActivateDrawActions();
                    break;
                default:
                    throw new ArgumentNullException("Invalid EndTurnResult");
            }

            _isComplete = true;
        }

        private void ActivatePlayerWinActions()
        {
            Debug.Log("Player win!");
        }

        private void ActivateEnemyWinActions()
        {
            Debug.Log("Enemy win!");
        }

        private void ActivateDrawActions()
        {
            Debug.Log("Draw!");
        }
    }
}