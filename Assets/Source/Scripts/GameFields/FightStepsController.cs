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
            //_startTowerCardSelections.Activate();

            //yield return new WaitUntil(() => _startTowerCardSelections.IsComplete);

            _fight.Start();
            yield return new WaitUntil(() => _fight.IsComplete);

            _endFight.Start(_fight.EndFightResults);
        }
    }
}
