using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.PersonAnimators
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private PlayerDrawCardAnimator _drawCardAnimator;

        private IHand _hand;
        //private Transform _parent;

        public void Init(IHand hand, Transform parent)
        {
            _hand = hand;
            //_parent = parent;

            _drawCardAnimator = new PlayerDrawCardAnimator(parent);
        }

        public void StartDrawCardAnimation(Card drawnCard)
        {
            PlayingDrawCardAnimation(drawnCard).ToUniTask();
        }

        private IEnumerator PlayingDrawCardAnimation(Card drawnCard)
        {
            float fullDelay = _drawCardAnimator.GetFullDelay();
            _drawCardAnimator.StartDrawCardAnimation(drawnCard);

            yield return new WaitForSeconds(fullDelay);

            _hand.AddCard(drawnCard);
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
