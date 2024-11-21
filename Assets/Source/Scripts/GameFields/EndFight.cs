using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields
{
    public class EndFight: IFightStep
    {
        private IReadonlyFightResult _fightResult;
        private bool _isComplete;

        public bool IsComplete => _isComplete;

        public EndFight(IReadonlyFightResult fightResult)
        {
            _fightResult = fightResult;
            _isComplete = false;
        }

        //public void PrepareToStart()
        //{
        //    _isComplete = false;
        //}

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
