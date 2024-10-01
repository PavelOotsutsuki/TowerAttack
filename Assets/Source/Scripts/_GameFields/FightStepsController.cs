using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFields.StartFights;
using UnityEngine;

namespace GameFields
{
    internal class FightStepsController
    {
        private Queue<IFightStep> _fightSteps;

        private IFightStep _currentStep;

        private bool _isComplete;

        public FightStepsController(StartFight startFight, Fight fight, EndFight endFight)
        {
            _isComplete = false;

            _fightSteps = new Queue<IFightStep>();

            _fightSteps.Enqueue(startFight);
            _fightSteps.Enqueue(fight);
            _fightSteps.Enqueue(endFight);
        }

        public void StartStep()
        {
            _currentStep = _fightSteps.Dequeue();

            Starting().ToUniTask();
        }

        private IEnumerator Starting()
        {
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
