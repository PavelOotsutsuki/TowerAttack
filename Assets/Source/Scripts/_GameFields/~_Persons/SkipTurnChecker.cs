using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Persons
{
    public class SkipTurnChecker
    {
        private readonly ITurnSkipper _slimeEffectHandler;
        private readonly ITurnSkipper _hand;

        private bool _canSkip;

        public SkipTurnChecker(ITurnSkipper slimeEffectHandler, ITurnSkipper hand)
        {
            _slimeEffectHandler = slimeEffectHandler;
            _hand = hand;
        }

        public bool CanSkip => _slimeEffectHandler.CanSkip || _canSkip;

        public void Activate()
        {
            _canSkip = false;

            if (_hand.CanSkip)
                _canSkip = true;


        }
    }
}
