using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields
{
    public class TowerSeat_2 : MonoBehaviour
    {
        private const bool IsFrontCardSide = false;

        [SerializeField] private Transform _transform;
        //[SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Min(0f)] private float _duration;

        private Card _card;

        public void Init()
        {
            ResetCard();
        }

        public void ResetCard()
        {
            _card = null;
        }

        public void GetCard(Card card)
        {
            SetCard(card, _duration);
        }

        public void SetCard(Card card, float duration)
        {
            _card = card;
            _card.BindSeat(_transform, IsFrontCardSide, duration);
        }

        public bool IsCardEqual(Card card)
        {
            return _card == card;
        }

        //private void Activate()
        //{
        //    _canvasGroup.blocksRaycasts = true;
        //}

        //private void Deactivate()
        //{
        //    _canvasGroup.blocksRaycasts = false;
        //}

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineTransform();
        }

        [ContextMenu(nameof(DefineTransform))]
        private void DefineTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
    }
}
