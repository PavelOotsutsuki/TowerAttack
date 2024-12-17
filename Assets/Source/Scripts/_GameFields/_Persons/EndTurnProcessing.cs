using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.EndTurnButtons;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons
{
    public class EndTurnProcessing : PersonStep
    {
        private readonly IEndTurnButtonStateWatcher _endTurnButtonStateWatcher;
        //private readonly IHandBlockable _handBlockable;
        //private readonly GameFieldObjectsActivator _gameFieldObjectsActivator;

        private bool _isComplete;

        public EndTurnProcessing(IEndTurnButtonStateWatcher endTurnButtonStateWatcher,// IHandBlockable handBlockable,
            GameFieldObjectsActivator gameFieldObjectsActivator): base(gameFieldObjectsActivator)
        {
            _isComplete = false;
            _endTurnButtonStateWatcher = endTurnButtonStateWatcher;
            //_handBlockable = handBlockable;
            //_gameFieldObjectsActivator = gameFieldObjectsActivator;
        }

        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            _isComplete = false;
            //_gameFieldObjectsActivator.Activate();
            //_handBlockable.Unblock();

            WaitingEndTurnButtonClick().ToUniTask();
        }

        private IEnumerator WaitingEndTurnButtonClick()
        {
            yield return new WaitUntil(() => _endTurnButtonStateWatcher.EndTurnClicked == false);

            _isComplete = true;
        }
    }
}