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
    [Serializable]
    public class PlayerDrawCardAnimator
    {
        [SerializeField] private PlayerSimpleDrawCardAnimator _simpleDrawAnimator;

        private IHand _hand;
        private Transform _parent;

        public void Init(IHand hand, Transform parent)
        {
            _hand = hand;
            _parent = parent;
        }

        public IEnumerator PlayingSimpleDrawCardAnimation(Card drawnCard)
        {
            yield return _simpleDrawAnimator.StartDrawCardAnimation(drawnCard, _parent);

            _hand.AddCard(drawnCard);
        }
    }
}
