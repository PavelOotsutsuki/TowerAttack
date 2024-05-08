using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFields.StartTowerCardSelections;
using UnityEngine;

namespace GameFields
{
    internal class FightStepsController: IStateMachineStep
    {
        private StartTowerCardSelection _startTowerCardSelections;
        private Fight _fight;
        private EndFight _endFight;

        private IFightStep _currentStep;

        public FightStepsController(StartTowerCardSelection startTowerCardSelections, Fight fight, EndFight endFight)
        {
            _startTowerCardSelections = startTowerCardSelections;
            _fight = fight;
            _endFight = endFight;
        }

        public bool IsComplete => _startTowerCardSelections.IsComplete && _fight.IsComplete && _endFight.IsComplete;

        public void PrepareToStart()
        {
            _startTowerCardSelections.PrepareToStart();
            _fight.PrepareToStart();
            _endFight.PrepareToStart();
        }

        public void StartStep()
        {
            _currentStep = _fight;

            Starting().ToUniTask();
        }

        private IEnumerator Starting()
        {
            //_startTowerCardSelections.Activate();

            //yield return new WaitUntil(() => _startTowerCardSelections.IsComplete);

            //_fight.StartStep();
            //yield return new WaitUntil(() => _fight.IsComplete);

            //_endFight.StartStep(_fight.EndFightResults);

            while (IsComplete == false)
            {
                _currentStep.StartStep();
                yield return new WaitUntil(() => _currentStep.IsComplete);

                NextStep();
            }
        }

        private void NextStep()
        {
            switch (_currentStep)
            {
                case StartTowerCardSelection:
                    _currentStep = _fight;
                    break;
                case Fight:
                    _currentStep = _endFight;
                    break;
                case EndFight:
                    break;
                default:
                    throw new ArgumentNullException("Invalid FightStep");
            }
        }
    }
}
