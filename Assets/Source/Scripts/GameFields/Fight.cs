using GameFields.Persons;
using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GameFields
{
    internal class Fight : IEndTurnHandler, IFightStep
    {
        private const int MaxTurns = 100;

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
        private Person ActivePerson => _personsState.Active;

        private bool TurnsIsOut => _turnNumber >= MaxTurns;

        public void OnEndTurn()
        {
            _turnNumber++;

            _personsState.Active.FinishTurn();
            
            if (TurnsIsOut)
            {
                _fightResult.SetDraw();
                IsComplete = true;
            }
            
            _personsState.Switch();
            StartTurn();
        }

        public void StartStep()
        {
            StartTurn();
        }

        private void StartTurn()
        {
            ActivePerson.StartStep();
            TurnProcessing().ToUniTask();
        }

        private IEnumerator TurnProcessing()
        {
            yield return new WaitUntil(() => ActivePerson.IsComplete);

            OnEndTurn();
        }
    }
}
