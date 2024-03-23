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
        [SerializeField] private Transform _parent;

        public override void PlayingSimpleDrawCardAnimation(Card drawnCard)
        {
            Playing(drawnCard).ToUniTask();
        }

        private IEnumerator Playing(Card drawnCard)
        {
            IsDone = false;

            yield return _simpleDrawAnimator.StartDrawCardAnimation(drawnCard, _parent);

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
