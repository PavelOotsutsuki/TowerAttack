using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields
{
    public class EndFight
    {
        public EndFight()
        {

        }

        public void Start(EndFightResults endFightResults)
        {
            switch (endFightResults)
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
                    Debug.Log("Error");
                    break;
            }
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
