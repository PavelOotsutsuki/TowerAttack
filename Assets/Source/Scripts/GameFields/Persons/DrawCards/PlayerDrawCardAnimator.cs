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
        [SerializeField] private PlayerSimpleDrawCardAnimation _simpleDrawAnimation;
        [SerializeField] private Transform _parent;

        public override void PlayingSimpleDrawCardAnimation(Card drawnCard)
        {
            Playing(drawnCard).ToUniTask();
        }

        private IEnumerator Playing(Card drawnCard)
        {
            IsDone = false;

            //_simpleDrawAnimation.SetCard(drawnCard);
            yield return _simpleDrawAnimation.StartAnimation(drawnCard, _parent);

            Hand.AddCard(drawnCard);
            IsDone = true;
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineTransform();
        }

        [ContextMenu(nameof(DefineTransform))]
        private void DefineTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _parent, ComponentLocationTypes.InThis);
        }
    }
}
