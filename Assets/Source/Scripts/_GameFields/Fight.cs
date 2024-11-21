using GameFields.Persons;
using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GameFields
{
    internal class Fight : IFightStep, IWinnerSetter
    {
        private const int MaxTurns = 100;
        private const float DelayBeforeStartTurn = 3f;

        private readonly FightResult _fightResult;
        private readonly PersonsState _personsState;

        private int _turnNumber;

        public Fight(PersonsState personsState, FightResult fightResult)
        {
            _personsState = personsState;
            _fightResult = fightResult;

            _turnNumber = 1;

            IsComplete = false;
        }

        public bool IsComplete { get; private set; }

        private ITurnStep ActivePerson => _personsState.Active;
        private bool TurnsIsOut => _turnNumber >= MaxTurns;

        public void StartStep()
        {
            StartTurn().ToUniTask();
        }

        public void SetWinner(Person winner)
        {
            if (winner is Player)
            {
                _fightResult.SetPlayerWin();
            }
            else if (winner is EnemyAI)
            {
                _fightResult.SetEnemyWin();
            }
            else
            {
                throw new System.Exception("Unknown winner");
            }

            IsComplete = true;
        }

        private IEnumerator StartTurn()
        {
            yield return new WaitForSeconds(DelayBeforeStartTurn);

            while (IsComplete == false)
            {
                ActivePerson.StartStep();

                yield return new WaitUntil(() => ActivePerson.IsComplete);

                NextTurn();
            }
        }

        private void NextTurn()
        {
            _turnNumber++;

            ActivePerson.FinishTurn();

            if (TurnsIsOut)
            {
                _fightResult.SetDraw();
                IsComplete = true;
            }

            _personsState.Switch();
        }
    }
}