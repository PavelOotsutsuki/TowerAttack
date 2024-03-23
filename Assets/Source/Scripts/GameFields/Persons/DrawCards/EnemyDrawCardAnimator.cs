using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class EnemyDrawCardAnimator : DrawCardAnimator
    {
        [SerializeField] private float _delay = 0.5f;

        public override void PlayingSimpleDrawCardAnimation(Card drawnCard)
        {
            Playing(drawnCard).ToUniTask();
        }

        private IEnumerator Playing(Card drawnCard)
        {
            IsDone = false;
            Hand.AddCard(drawnCard);

            yield return new WaitForSeconds(_delay);

            IsDone = true;
        }

    }
}
