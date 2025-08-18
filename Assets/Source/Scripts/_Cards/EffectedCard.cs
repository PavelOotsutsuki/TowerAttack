using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Cards
{
    public class EffectedCard: ICompletable
    {
        //private readonly Card _card;

        private bool _isComplete;

        //public EffectedCard(Card card)
        public EffectedCard()
        {
            //_card = card;

            _isComplete = false;
        }

        public bool IsComplete => _isComplete;

        public void EndEffect()
        {
            _isComplete = true;
        }

    }
}
