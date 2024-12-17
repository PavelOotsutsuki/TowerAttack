using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.EndTurnButtons;
using GameFields.Persons.Hands;
using Tools;
using UnityEngine;

namespace GameFields.Persons
{
    public class TurnProcessing : PersonStep
    {
        private bool _isComplete;

        public TurnProcessing(GameFieldObjectsActivator gameFieldObjectsActivator): base(gameFieldObjectsActivator)
        {
            _isComplete = false;
        }

        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            _isComplete = false;

            WaitingEndTurnButtonClick().ToUniTask();
        }

        public void Completed()
        {
            _isComplete = true;
        }

        private IEnumerator WaitingEndTurnButtonClick()
        {
            //yield return new WaitUntil(() => _buttonActivator.EndTurnClicked == false);
            yield return new WaitUntil(() => _isComplete);

            _isComplete = true;
        }
    }
}