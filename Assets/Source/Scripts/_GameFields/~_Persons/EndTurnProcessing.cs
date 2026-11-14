using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFields.EndTurnButtons;
using GameFields.InputSettings;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.Hands;
using Tools.InputSettings;
using UnityEngine;

namespace GameFields.Persons
{
    internal class EndTurnProcessing : PersonStep, IInputLogicObject
    {
        private readonly IEndTurnButtonStateWatcher _endTurnButtonStateWatcher;
        private readonly PersonEffectsHandler _personEffectsHandler;
        //private readonly IHandBlockable _handBlockable;
        //private readonly GameFieldObjectsActivator _gameFieldObjectsActivator;

        private bool _isComplete;

        public EndTurnProcessing(IEndTurnButtonStateWatcher endTurnButtonStateWatcher,// IHandBlockable handBlockable,
            InteractionActivator interactionActivator, PersonEffectsHandler personEffectsHandler, CancellationToken token): base(interactionActivator, token)
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

            WaitingEndTurnButtonClick().Forget();
        }

        private async UniTask WaitingEndTurnButtonClick()
        {
            await UniTask.WaitUntil(() => _endTurnButtonStateWatcher.EndTurnClicked == false, cancellationToken: Token);

            _personEffectsHandler.BeforeEndTurn(BeforeEndCallback, Token);
        }

        private void BeforeEndCallback()
        {
            _isComplete = true;
        }
    }
}