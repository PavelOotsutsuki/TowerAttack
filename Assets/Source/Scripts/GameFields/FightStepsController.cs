using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFields.StartTowerCardSelections;
using UnityEngine;

namespace GameFields
{
    internal class FightStepsController
    {
        private StartTowerCardSelection _startTowerCardSelections;
        private Fight _fight;
        private EndFight _endFight;

        public FightStepsController(StartTowerCardSelection startTowerCardSelections, Fight fight, EndFight endFight)
        {
            _startTowerCardSelections = startTowerCardSelections;
            _fight = fight;
            _endFight = endFight;
        }

        public void StartFightSteps()
        {
            Starting().ToUniTask();
        }

        private IEnumerator Starting()
        {
            //yield break;
            _startTowerCardSelections.Activate();
            Debug.Log("1. _startTowerCardSelections.IsComplete" + _startTowerCardSelections.IsComplete);
            Debug.Log("1. _fight.IsComplete" + _fight.IsComplete);

            yield return new WaitUntil(() => _startTowerCardSelections.IsComplete);
            Debug.Log("2. _startTowerCardSelections.IsComplete" + _startTowerCardSelections.IsComplete);
            Debug.Log("2. _fight.IsComplete" + _fight.IsComplete);

            _fight.Start();
            yield return new WaitUntil(() => _fight.IsComplete);
            Debug.Log("3. _startTowerCardSelections.IsComplete" + _startTowerCardSelections.IsComplete);
            Debug.Log("3. _fight.IsComplete" + _fight.IsComplete);

            _endFight.Start(_fight.EndFightResults);
        }
    }
}
