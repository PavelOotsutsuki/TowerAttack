using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.Reflection;

namespace Servers
{
    public class FightProcessDBManager
    {
        private readonly DBRoot _dBRoot;
        private readonly CancellationToken _gameFieldToken;

        private readonly Queue<FightProcessDBActionData> _queueFightProcessActions;

        public FightProcessDBManager(DBRoot dBRoot, CancellationToken gameFieldToken)
        {
            _dBRoot = dBRoot;
            _gameFieldToken = gameFieldToken;

            _queueFightProcessActions = new Queue<FightProcessDBActionData>();

            StartProcess().Forget();
        }

        public void WriteFightProcessAction(int turnNumber, bool? isPlayersAction, string action_target, string action_type, string action_subtype)
        {
            _queueFightProcessActions.Enqueue(new FightProcessDBActionData(turnNumber, isPlayersAction, action_target, action_type, action_subtype));
        }

        private async UniTask StartProcess()
        {
            try
            {
                while (_gameFieldToken.IsCancellationRequested == false)
                {
                    await UniTask.Delay(100, cancellationToken: _gameFieldToken);

                    while (_queueFightProcessActions.Count > 0)
                    {
                        FightProcessDBActionData currentFightProcessAction = _queueFightProcessActions.Dequeue();

                        await _dBRoot.WriteFightProcessAction(currentFightProcessAction.TurnNumber, currentFightProcessAction.IsPlayersAction, currentFightProcessAction.Action_target,
                            currentFightProcessAction.Action_type, currentFightProcessAction.Action_subtype, _gameFieldToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }
    }
}