using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.EndTurnButtons;
using GameFields.InputSettings;
using GameFields.Persons.Hands;
using Tools;
using UnityEngine;

namespace GameFields.Persons
{
    public class TurnProcessing : PersonStep, IInputLogicObject
    {
        private readonly SkipTurnChecker _skipTurnChecker;

        private bool _isComplete;

        public TurnProcessing(InteractionActivator interactionActivator, SkipTurnChecker skipTurnChecker) : base(interactionActivator)
        {
            _skipTurnChecker = skipTurnChecker;

            _isComplete = false;
        }

        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            _skipTurnChecker.Activate();

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
            yield return new WaitUntil(() => _skipTurnChecker.CanSkip || _isComplete == true);

            _isComplete = true;
        }
    }
}