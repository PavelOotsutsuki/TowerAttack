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

        private Queue<IFightStep> _fightSteps;

        private IFightStep _currentStep;

        private bool _isComplete;

        public FightStepsController(StartTowerCardSelection startTowerCardSelections, Fight fight, EndFight endFight)
        {
            //_startTowerCardSelections = startTowerCardSelections;
            //_fight = fight;
            //_endFight = endFight;

            _isComplete = false;

            _fightSteps = new Queue<IFightStep>();

            //_fightSteps.Enqueue(startTowerCardSelections);
            _fightSteps.Enqueue(fight);
            _fightSteps.Enqueue(endFight);
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
            _currentStep = _fightSteps.Dequeue();

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
            if (_fightSteps.Count > 0)
            {
                _currentStep = _fightSteps.Dequeue();
            }
            else
            {
                _isComplete = true;
            }
        }
    }
}
