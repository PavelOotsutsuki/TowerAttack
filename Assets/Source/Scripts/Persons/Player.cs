using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using Cards;
using Hands;

namespace Persons
{
    public class Player : Person
    {
        [SerializeField] private DrawCardAnimator _drawCardAnimator;

        protected override void DrawCard(Card drawnCard, Hand hand)
        {
            _drawCardAnimator.Init(hand, drawnCard);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineDrawCardAnimator();
        }

        [ContextMenu(nameof(DefineDrawCardAnimator))]
        private void DefineDrawCardAnimator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _drawCardAnimator, ComponentLocationTypes.InThis);
        }
    }
}
