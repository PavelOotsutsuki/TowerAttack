using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFields.EndTurnButtons;
using GameFields.InputSettings;
using GameFields.Persons.Hands;
using Tools;
using Tools.InputSettings;
using UnityEngine;

namespace GameFields.Persons
{
    internal class TurnProcessing : PersonStep, IInputLogicObject
    {
        private readonly SkipTurnChecker _skipTurnChecker;

        private bool _isComplete;

        public TurnProcessing(InteractionActivator interactionActivator, SkipTurnChecker skipTurnChecker, CancellationToken turnToken) : base(interactionActivator, turnToken)
        {
            _skipTurnChecker = skipTurnChecker;

            _isComplete = false;
        }

        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            _skipTurnChecker.Activate();

            _isComplete = false;

            WaitingEndTurnButtonClick().Forget();
        }

        public void Completed()
        {
            _isComplete = true;
        }

        private async UniTask WaitingEndTurnButtonClick()
        {
            //yield return new WaitUntil(() => _buttonActivator.EndTurnClicked == false);
            await UniTask.WaitUntil(() => _skipTurnChecker.CanSkip || _isComplete == true, cancellationToken: Token);

            _isComplete = true;
        }
    }
}