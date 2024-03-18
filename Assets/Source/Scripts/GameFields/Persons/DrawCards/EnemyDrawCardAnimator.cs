using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class EnemyDrawCardAnimator : DrawCardAnimator
    {
        [SerializeField] private float _drawCardDelay = 0.5f;

        public override IEnumerator PlayingSimpleDrawCardAnimation(Card drawnCard)
        {
            Hand.AddCard(drawnCard);
            yield return new WaitForSeconds(_drawCardDelay);
        }
    }
}
