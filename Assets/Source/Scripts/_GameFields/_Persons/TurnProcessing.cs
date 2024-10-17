using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.EndTurnButtons;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons
{
    public class TurnProcessing : IPersonStep
    {
        private bool _isComplete;
        private IButtonActivator _buttonActivator;
        private IBlockable _handBlockable;

        public TurnProcessing(IButtonActivator buttonActivator, IBlockable handBlockable)
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
