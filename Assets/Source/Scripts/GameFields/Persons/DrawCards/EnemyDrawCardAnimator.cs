using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class EnemyDrawCardAnimator : DrawCardAnimator
    {
        public override bool IsDone => true;

        public override void PlayingSimpleDrawCardAnimation(Card drawnCard)
        {
            Hand.AddCard(drawnCard);
        }
    }
}
