using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFields.StartTowerCardSelections;
using UnityEngine;

namespace GameFields
{
    internal class FightStepsController//: IStateMachineStep
    {
        //private StartTowerCardSelection _startTowerCardSelections;
        //private Fight _fight;
        //private EndFight _endFight;

        private IFightStep[] _fightSteps;

        private IFightStep _currentStep;
        private int _currentStepIndex;

        private bool _isComplete;

        public FightStepsController(StartTowerCardSelection startTowerCardSelections, Fight fight, EndFight endFight)
        {
            //_startTowerCardSelections = startTowerCardSelections;
            //_fight = fight;
            //_endFight = endFight;

            _isComplete = false;

            _fightSteps = new IFightStep[]
            {
                startTowerCardSelections,
                fight,
                endFight
            };

            _currentStepIndex = 1;
        }

        //public bool IsComplete => _startTowerCardSelections.IsComplete && _fight.IsComplete && _endFight.IsComplete;

        //public void PrepareToStart()
        //{
        //    //_startTowerCardSelections.PrepareToStart();
        //    //_fight.PrepareToStart();
        //    //_endFight.PrepareToStart();

        //    foreach (IFightStep step in _fightSteps)
        //    {
        //        step.PrepareToStart();
        //    }
        //}

        public void StartStep()
        {
            _currentStep = _fightSteps[_currentStepIndex];

            Starting().ToUniTask();
        }

        private IEnumerator Starting()
        {
            //_startTowerCardSelections.Activate();

            //yield return new WaitUntil(() => _startTowerCardSelections.IsComplete);

            //_fight.StartStep();
            //yield return new WaitUntil(() => _fight.IsComplete);

            //_endFight.StartStep(_fight.EndFightResults);

            while (_isComplete == false)
            {
                _currentStep.StartStep();
                yield return new WaitUntil(() => _currentStep.IsComplete);

                NextStep();
            }
        }

        private void NextStep()
        {
            _currentStepIndex++;

            if(_currentStepIndex < 0)
            {
                throw new ArgumentOutOfRangeException("Invalid fight step index");
            }

            if (_currentStepIndex < _fightSteps.Length)
            {
                _currentStep = _fightSteps[_currentStepIndex];
            }
            else
            {
                _isComplete = true;
            }
        }
    }
}
