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
        private readonly IReadOnlyHand _hand;

        private bool _isComplete;

        public TurnProcessing(InteractionActivator interactionActivator, IReadOnlyHand hand): base(interactionActivator)
        {
            _hand = hand;

            _isComplete = false;
        }

        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            int startCountCards = _hand.CountCards;

            _isComplete = false;

            WaitingEndTurnButtonClick(startCountCards).ToUniTask();
        }

        public void Completed()
        {
            _isComplete = true;
        }

        private IEnumerator WaitingEndTurnButtonClick(int startCountCards)
        {
            //yield return new WaitUntil(() => _buttonActivator.EndTurnClicked == false);
            yield return new WaitUntil(() => startCountCards == 0 || _hand.IsSlimeEffectCountZero || _isComplete == true);

            _isComplete = true;
        }
    }
}