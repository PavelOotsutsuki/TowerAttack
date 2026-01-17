using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.EndTurnButtons;
using GameFields.InputSettings;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.Hands;
using Tools.InputSettings;
using UnityEngine;

namespace GameFields.Persons
{
    public class EndTurnProcessing : PersonStep, IInputLogicObject
    {
        private readonly IEndTurnButtonStateWatcher _endTurnButtonStateWatcher;
        private readonly PersonEffectsHandler _personEffectsHandler;
        //private readonly IHandBlockable _handBlockable;
        //private readonly GameFieldObjectsActivator _gameFieldObjectsActivator;

        private bool _isComplete;

        public EndTurnProcessing(IEndTurnButtonStateWatcher endTurnButtonStateWatcher,// IHandBlockable handBlockable,
            InteractionActivator interactionActivator, PersonEffectsHandler personEffectsHandler): base(interactionActivator)
        {
            _isComplete = false;
            _endTurnButtonStateWatcher = endTurnButtonStateWatcher;
            _personEffectsHandler = personEffectsHandler;
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

            _personEffectsHandler.BeforeEndTurn(BeforeEndCallback);
        }

        private void BeforeEndCallback()
        {
            _isComplete = true;
        }
    }
}