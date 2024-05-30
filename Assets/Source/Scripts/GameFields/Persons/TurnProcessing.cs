using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFields.EndTurnButtons;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons
{
    public class TurnProcessing : ITurnStep
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

        //public void PrepareToStart()
        //{
        //    //_isComplete = false;
        //}

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

            Debug.Log("_buttonActivator.IsActive: " + _buttonActivator.IsActive);
            _isComplete = true;
        }
    }
}
