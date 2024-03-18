using System.Collections;
using System;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.DrawCards
{
    public class PlayerDrawCardAnimator: DrawCardAnimator
    {
        [SerializeField] private PlayerSimpleDrawCardAnimator _simpleDrawAnimator;

        public override IEnumerator PlayingSimpleDrawCardAnimation(Card drawnCard)
        {
            yield return _simpleDrawAnimator.StartDrawCardAnimation(drawnCard, Parent);

            Hand.AddCard(drawnCard);
        }
    }
}
