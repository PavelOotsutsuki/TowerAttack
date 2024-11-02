using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.EndTurnButtons;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons
{
    public class TurnProcessing : IPersonStep
    {
        private readonly IButtonActivator _buttonActivator;
        private readonly IHandBlockable _handBlockable;

        private bool _isComplete;

        public TurnProcessing(IButtonActivator buttonActivator, IHandBlockable handBlockable)
        {
            _isComplete = false;
            _buttonActivator = buttonActivator;
            _handBlockable = handBlockable;
        }

        public bool IsComplete => _isComplete;

        public void StartStep()
        {
            _isComplete = false;
            _buttonActivator.SetActiveSide();
            _handBlockable.Unblock();

            WaitingEndTurnButtonClick().ToUniTask();
        }

        private IEnumerator WaitingEndTurnButtonClick()
        {
            yield return new WaitUntil(() => _buttonActivator.IsActive == false);

            _isComplete = true;
        }
    }
}