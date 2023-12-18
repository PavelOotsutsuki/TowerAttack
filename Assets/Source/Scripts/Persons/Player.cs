using UnityEngine;
using UnityEngine.UI;
using Tools;
using Cards;
using Hands;

namespace Persons
{
    public class Player : Person
    {
        [SerializeField] private DrawCardAnimator _drawCardAnimator;
        [SerializeField] private CanvasScaler _canvasScaler;

        protected override void DrawCard(Card drawnCard, Hand hand)
        {
            _drawCardAnimator.Init(hand, drawnCard, _canvasScaler);
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
