using GameFields.Persons;
using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;
using GameFields.Signals;
using GameFields.EndFights;
using System;
using GameFields.Seats;
using Tools;
using GameFields.FightMenues;

namespace GameFields
{
    internal class Fight : IFightStep//, IWinnerSetter
    {
        private const int MaxTurns = 500;
        private const float DelayBeforeStartTurn = 3f;

        private readonly FightResult _fightResult;
        private readonly PersonsState _personsState;
        private readonly SignalBus _bus;

        private readonly SeatPool _seatPool;
        private readonly IActivatable _soundRootActivatable;
        private readonly IActivatable _fightButtonsActivator;

        private static int _turnNumber;

        public Fight(PersonsState personsState, FightResult fightResult, SignalBus bus, SeatPool seatPool
            , IActivatable soundRootActivatable, IActivatable fightButtonsActivator)
        {
            _personsState = personsState;
            _fightResult = fightResult;

            _turnNumber = 1;

            IsComplete = false;

            _seatPool = seatPool;
            _bus = bus;
            _bus.Subscribe<PersonWinSignal>(SetWinner);

            _soundRootActivatable = soundRootActivatable;
            _fightButtonsActivator = fightButtonsActivator;
        }

        ~Fight()
        {
            _bus.Unsubscribe<PersonWinSignal>(SetWinner);
        }

        public static int TurnNumber => _turnNumber;
        public bool IsComplete { get; private set; }

        private ITurnStep ActivePerson => _personsState.Active;
        private bool TurnsIsOut => _turnNumber >= MaxTurns;

        public void StartStep()
        {
            //GC.Collect();
            _soundRootActivatable.Activate();
            _fightButtonsActivator.Activate();

            StartTurn().ToUniTask();
        }

        private void SetWinner(PersonWinSignal signal)
        {
            IPersonObject loser = signal.Loser;

            if (loser is IPlayerObject)
            {
                _fightResult.SetEnemyWin();
            }
            else if (loser is IEnemyAIObject)
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
            //_seatPool.Collect();

            ActivePerson.FinishTurn();

            if (TurnsIsOut)
            {
                _fightResult.SetDraw();
                IsComplete = true;
            }

            //if (_personsState.Deactive is Player)
            //{
            //    Debug.Log("Player: " + _personsState.Deactive.LastEffect.ToString());
            //    Debug.Log("Enemy: " + _personsState.Active.LastEffect.ToString());
            //}
            //else
            //{
            //    Debug.Log("Player: " + _personsState.Active.LastEffect.ToString());
            //    Debug.Log("Enemy: " + _personsState.Deactive.LastEffect.ToString());
            //}

            _personsState.Switch();
        }
    }
}