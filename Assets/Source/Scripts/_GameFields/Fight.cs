using GameFields.Persons.Common;
using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;
using GameFields.Signals;
using GameFields.EndFights;

namespace GameFields
{
    internal class Fight : IFightStep//, IWinnerSetter
    {
        private const int MaxTurns = 100;
        private const float DelayBeforeStartTurn = 3f;

        private readonly FightResult _fightResult;
        private readonly PersonsState _personsState;
        private readonly SignalBus _bus;

        private int _turnNumber;

        public Fight(PersonsState personsState, FightResult fightResult, SignalBus bus)
        {
            _personsState = personsState;
            _fightResult = fightResult;

            _turnNumber = 1;

            IsComplete = false;

            _bus = bus;
            _bus.Subscribe<PersonWinSignal>(SetWinner);
        }

        ~Fight()
        {
            _bus.Unsubscribe<PersonWinSignal>(SetWinner);
        }

        public bool IsComplete { get; private set; }

        private ITurnStep ActivePerson => _personsState.Active;
        private bool TurnsIsOut => _turnNumber >= MaxTurns;

        public void StartStep()
        {
            StartTurn().ToUniTask();
        }

        private void SetWinner(PersonWinSignal signal)
        {
            IPersonObject towerAttackedType = signal.TowerAttackedType;

            if (towerAttackedType is IEnemyAIObject)
            {
                _fightResult.SetEnemyWin();
            }
            else if (towerAttackedType is IPlayerObject)
            {
                _fightResult.SetPlayerWin();
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