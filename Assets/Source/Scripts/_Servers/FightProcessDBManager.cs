using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.Reflection;
using Tools;

namespace Servers
{
    public class FightProcessDBManager : ICompletable
    {
        private readonly DBRoot _dBRoot;
        private readonly CancellationToken _gameFieldToken;

        private readonly Queue<FightProcessDBActionData> _queueFightProcessActions;
        private readonly List<FightProcessDBActionData> _datasInProcess;

        // private readonly List<FightProcessDBActionData> _batchList;
        private int _countBatchInProcess;
        private int _currentTurn;

        private int _logCounter;

        public FightProcessDBManager(DBRoot dBRoot, CancellationToken gameFieldToken)
        {
            _dBRoot = dBRoot;
            _gameFieldToken = gameFieldToken;
            _logCounter = 1;
            _currentTurn = -1;
            _countBatchInProcess = 0;

            _queueFightProcessActions = new Queue<FightProcessDBActionData>();
            _datasInProcess = new List<FightProcessDBActionData>();
            // _batchList = new List<FightProcessDBActionData>();

            StartProcess().Forget();
        }

        public bool IsComplete => _queueFightProcessActions.Count == 0 && _datasInProcess.Count == 0 && _countBatchInProcess == 0;
        // public bool IsComplete => _turnData.Count == 0;
        public int LogCounter => _logCounter;

        public void WriteFightProcessAction(int turnNumber, bool? isPlayersAction, string action_target, string action_type, string action_subtype)
        {
            EnqueueQueue(new FightProcessDBActionData(turnNumber, isPlayersAction, action_target, action_type, action_subtype, _logCounter++));
            //_queueFightProcessActions.Enqueue(new FightProcessDBActionData(turnNumber, isPlayersAction, action_target, action_type, action_subtype, _logCounter++));
        }

        // public void WriteFightProcessAction(int turnNumber, bool? isPlayersAction, string action_target, string action_type, string action_subtype)
        // {
        //     AddInTurnList(new FightProcessDBActionData(turnNumber, isPlayersAction, action_target, action_type, action_subtype, _logCounter++));
        //     //_queueFightProcessActions.Enqueue(new FightProcessDBActionData(turnNumber, isPlayersAction, action_target, action_type, action_subtype, _logCounter++));
        // }

        private async UniTask StartProcess()
        {
            try
            {
                while (_gameFieldToken.IsCancellationRequested == false)
                {
                    await UniTask.Delay(100, cancellationToken: _gameFieldToken);

                    while (_queueFightProcessActions.Count > 0)
                    {
                        FightProcessDBActionData currentFightProcessAction = DequeueQueue();
                        AddList(currentFightProcessAction);

                        await WriteFightProcessActionParallel(currentFightProcessAction, _gameFieldToken);

                        //await _dBRoot.WriteFightProcessAction(currentFightProcessAction.Counter, currentFightProcessAction.TurnNumber,
                        //    currentFightProcessAction.IsPlayersAction, currentFightProcessAction.Action_target,
                        //    currentFightProcessAction.Action_type, currentFightProcessAction.Action_subtype, _gameFieldToken);

                        //_dBRoot.WriteFightProcessAction(currentFightProcessAction.Counter, currentFightProcessAction.TurnNumber, currentFightProcessAction.IsPlayersAction, currentFightProcessAction.Action_target,
                        //    currentFightProcessAction.Action_type, currentFightProcessAction.Action_subtype, _gameFieldToken).Forget();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private async UniTask WriteFightProcessActionParallel(FightProcessDBActionData currentFightProcessAction, CancellationToken token)
        {
            try
            {
                // Тут цифарка - сколько процессов может идти параллельно максимум
                if (_datasInProcess.Count >= 1)
                {
                    await WriteFightProcessAction(currentFightProcessAction, token);
                }
                else
                {
                    WriteFightProcessAction(currentFightProcessAction, token).Forget();

                    await UniTask.NextFrame(cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private async UniTask WriteFightProcessAction(FightProcessDBActionData currentFightProcessAction, CancellationToken token)
        {
            await _dBRoot.WriteFightProcessAction(currentFightProcessAction.Counter, currentFightProcessAction.TurnNumber, currentFightProcessAction.IsPlayersAction, currentFightProcessAction.Action_target,
                currentFightProcessAction.Action_type, currentFightProcessAction.Action_subtype, token);

            RemoveList(currentFightProcessAction);
        }

        private async UniTask WriteFightProcessActionBatch(List<FightProcessDBActionData> batchList, CancellationToken token)
        {
            _countBatchInProcess++;

            try
            {
                Debug.Log(batchList.Count);
                await _dBRoot.WriteFightProcessActionBatch(batchList, token);
            }
            catch //(Exception ex)
            {
                Debug.LogError("УТЕРЯНы ЛОГИ ДАННЫХ BATCH. Вернем обратно в Queue!!!");

                foreach (FightProcessDBActionData fightProcessDBActionData in batchList)
                {
                    EnqueueQueue(fightProcessDBActionData);
                }
            }
            finally
            {

                _countBatchInProcess--;
            }
        }

        private void EnqueueQueue(FightProcessDBActionData fightProcessDBActionData)
        {
            if (_currentTurn < fightProcessDBActionData.TurnNumber)
            {
                List<FightProcessDBActionData> batchList = new List<FightProcessDBActionData>();

                while (_queueFightProcessActions.Count > 0)
                {
                    batchList.Add(_queueFightProcessActions.Dequeue());
                }

                WriteFightProcessActionBatch(batchList, _gameFieldToken).Forget();

                _currentTurn = fightProcessDBActionData.TurnNumber;
            }

            _queueFightProcessActions.Enqueue(fightProcessDBActionData);

            Log("Добавление ОЧЕРЕДИ");
        }

        private FightProcessDBActionData DequeueQueue()
        {
            Log("Удаление ОЧЕРЕДИ");

            return _queueFightProcessActions.Dequeue();
        }

        private void AddList(FightProcessDBActionData fightProcessDBActionData)
        {
            _datasInProcess.Add(fightProcessDBActionData);

            Log("Добавление LIST");
        }

        private void RemoveList(FightProcessDBActionData fightProcessDBActionData)
        {
            _datasInProcess.Remove(fightProcessDBActionData);

            Log("Удаление LIST");
        }

        private void Log(string startLogText)
        {
            Debug.Log($"{startLogText}. ОЧЕРЕДЬ: {_queueFightProcessActions.Count} LIST: {_datasInProcess.Count} currentTurn: {_currentTurn} _countBatchInProcess: {_countBatchInProcess}");
        }

        // private void AddInTurnList(FightProcessDBActionData fightProcessDBActionData)
        // {


        //     _batchList.Add(fightProcessDBActionData);

        //     Log("Добавление в _turnData");
        // }
    }
}