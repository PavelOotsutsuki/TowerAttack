using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Hands;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.PersonAnimators
{
    public class PlayerAnimator : PersonAnimator
    {
        [SerializeField] private PlayerDrawCardAnimator _drawCardAnimator;
        [SerializeField] private CanvasScaler _canvasScaler;

        private IHand _hand;

        public void Init(IHand hand)
        {
            _hand = hand;

            _drawCardAnimator.Init(_hand, _canvasScaler);
        }

        public void StartDrawCardAnimation(Card drawnCard)
        {
            _drawCardAnimator.StartDrawCardAnimation(drawnCard);
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
