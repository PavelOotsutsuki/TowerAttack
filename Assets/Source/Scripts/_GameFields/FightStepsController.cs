using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFields.EndFights;
using GameFields.StartFights;
using UnityEngine;

namespace GameFields
{
    internal class FightStepsController
    {
        private readonly Queue<IFightStep> _fightSteps;
        private readonly CancellationToken _gameFieldToken;

        private IFightStep _currentStep;
        private bool _isComplete;

        public FightStepsController(StartFight startFight, Fight fight, EndFight endFight, CancellationToken gameFieldToken)
        {
            _isComplete = false;
            _gameFieldToken = gameFieldToken;

            _fightSteps = new Queue<IFightStep>();

            _fightSteps.Enqueue(startFight);
            _fightSteps.Enqueue(fight);
            _fightSteps.Enqueue(endFight);
        }

        public void StartStep()
        {
            _currentStep = _fightSteps.Dequeue();

            Starting(_gameFieldToken).Forget();
        }

        private async UniTask Starting(CancellationToken token)
        {
            try
            {
                while (_isComplete == false)
                {
                    _currentStep.StartStep();
                    await UniTask.WaitUntil(() => _currentStep.IsComplete, cancellationToken: token);

                    NextStep();
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private void NextStep()
        {
            if (_fightSteps.Count > 0)
            {
                _currentStep = _fightSteps.Dequeue();
            }
            else
            {
                _isComplete = true;
            }
        }

        //public void Update()
        //{
        //    if (_isComplete)
        //    {
        //        return;
        //    }

        //    if (_currentStep.IsComplete)
        //    {
        //        NextStep();
        //    }
        //}

        //public void NextStep()
        //{
        //    if (_fightSteps.Count > 0)
        //    {
        //        _currentStep = _fightSteps.Dequeue();
        //        _currentStep.StartStep();
        //    }
        //    else
        //    {
        //        _isComplete = true;
        //    }
        //}
    }
}