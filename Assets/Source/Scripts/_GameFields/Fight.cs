using GameFields.Persons;
using Cysharp.Threading.Tasks;
using Zenject;
using GameFields.Signals;
using GameFields.EndFights;
using System;
using GameFields.Seats;
using Tools;
using System.Threading;
using System.Reflection;
using UnityEngine;
using Servers;

namespace GameFields
{
    internal class Fight : IFightStep//, IWinnerSetter
    {
        private const int MaxTurns = 100;
        private const float DelayBeforeStartTurn = 3f;

        private readonly FightResult _fightResult;
        private readonly PersonsState _personsState;
        private readonly SignalBus _bus;

        private readonly SeatPool _seatPool;
        private readonly IActivatable _soundRootActivatable;
        private readonly IActivatable _fightButtonsActivator;
        private readonly CancellationToken _fightToken;
        private readonly TurnToken _turnToken;
        private readonly DBRoot _dBRoot;

        private static int _turnNumber;

        public Fight(PersonsState personsState, FightResult fightResult, SignalBus bus, SeatPool seatPool,
            IActivatable soundRootActivatable, IActivatable fightButtonsActivator, CancellationToken fightToken,
            TurnToken turnToken, DBRoot dBRoot)
        {
            _personsState = personsState;
            _fightResult = fightResult;

            _turnNumber = 0;

            IsComplete = false;

            _seatPool = seatPool;
            _bus = bus;
            _bus.Subscribe<PersonWinSignal>(SetWinner);

            _soundRootActivatable = soundRootActivatable;
            _fightButtonsActivator = fightButtonsActivator;
            _fightToken = fightToken;
            _turnToken = turnToken;
            _dBRoot = dBRoot;
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
            _turnNumber++;

            _soundRootActivatable.Activate();
            _fightButtonsActivator.Activate();

            StartTurn(_fightToken).Forget();
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
                throw new Exception("Unknown winner");
            }

            IsComplete = true;
        }

        private async UniTask StartTurn(CancellationToken token)
        {
            try
            {
                await UniTask.WaitForSeconds(DelayBeforeStartTurn, cancellationToken: token);
                CancellationTokenSource turnCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);
                _turnToken.SetTokenSource(turnCTS);

                while (IsComplete == false)
                {
                    ActivePerson.StartStep();

                    await UniTask.WaitUntil(() => ActivePerson.IsComplete, cancellationToken: token);

                    NextTurn();
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private void NextTurn()
        {
            _turnNumber++;
            //_seatPool.Collect();

            ActivePerson.FinishTurn();

            if (TurnsIsOut)
            {
                SettingDraw(_fightToken).Forget();
            }
            else
            {
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

        private async UniTask SettingDraw(CancellationToken token)
        {
            await _dBRoot.FinishFightWithBot(null, token);

            _fightResult.SetDraw();
            IsComplete = true;
        }
    }
}